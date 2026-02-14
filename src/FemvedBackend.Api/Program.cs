using FemvedBackend.Api.Swagger;
using FemvedBackend.Api.Validation.Authentication;
using FemvedBackend.Application.Identity;
using FemvedBackend.Application.Interfaces.Identity;
using FemvedBackend.Application.Interfaces.Media;
using FemvedBackend.Application.Interfaces.Notifications;
using FemvedBackend.Application.Interfaces.Payments;
using FemvedBackend.Application.Interfaces.Repositories;
using FemvedBackend.Application.UseCases.Authentication;
using FemvedBackend.Application.UseCases.Guided.Admin;
using FemvedBackend.Application.UseCases.Products;
using FemvedBackend.Application.Validation.Authentication;
using FemvedBackend.Infrastructure.DependencyInjection;
using FemvedBackend.Infrastructure.Identity;
using FemvedBackend.Infrastructure.Media;
using FemvedBackend.Infrastructure.Notifications;
using FemvedBackend.Infrastructure.Payments;
using FemvedBackend.Infrastructure.Persistence;
using FemvedBackend.Infrastructure.Persistence.Repositories;
using FluentValidation;
using FluentValidation.AspNetCore;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using Microsoft.OpenApi.Models;
using System.Security.Claims;
using System.Reflection;
using System.Text;
using NpgsqlTypes;
using AppValidationException = FemvedBackend.Application.Exceptions.ValidationException;
using Serilog;
using Serilog.Context;
using Serilog.Sinks.PostgreSQL;
using DotNetEnv;


var envFilePath = ".env.local";
if (File.Exists(envFilePath))
{
    Env.Load(envFilePath);
}

var builder = WebApplication.CreateBuilder(args);

var defaultConn = builder.Configuration.GetConnectionString("DefaultConnection");
var serilogConn = builder.Configuration.GetConnectionString("SerilogConnection");

Console.WriteLine("DEBUG DefaultConnection: " + defaultConn);
Console.WriteLine("DEBUG SerilogConnection: " + serilogConn);

var serilogConnectionString =
    serilogConn ?? defaultConn;

if (string.IsNullOrWhiteSpace(serilogConnectionString))
{
    Console.WriteLine("DEBUG: Listing all configuration values:");
    foreach (var kv in builder.Configuration.AsEnumerable())
    {
        Console.WriteLine($"{kv.Key} = {kv.Value}");
    }

    throw new InvalidOperationException("Serilog connection string is not configured.");
}


//var serilogConnectionString = builder.Configuration.GetConnectionString("SerilogConnection")
//    ?? builder.Configuration.GetConnectionString("DefaultConnection")
//    ?? throw new InvalidOperationException("Serilog connection string is not configured.");

var columnWriters = new Dictionary<string, ColumnWriterBase>
{
    ["message"] = new RenderedMessageColumnWriter(),
    ["message_template"] = new MessageTemplateColumnWriter(),
    ["level"] = new LevelColumnWriter(true, NpgsqlDbType.Varchar),
    ["timestamp"] = new TimestampColumnWriter(NpgsqlDbType.TimestampTz),
    ["exception"] = new ExceptionColumnWriter(),
    ["properties"] = new LogEventSerializedColumnWriter(NpgsqlDbType.Jsonb),
    ["source_context"] = new SinglePropertyColumnWriter("SourceContext", PropertyWriteMethod.ToString, NpgsqlDbType.Text),
    ["request_path"] = new SinglePropertyColumnWriter("RequestPath", PropertyWriteMethod.ToString, NpgsqlDbType.Text),
    ["user_id"] = new SinglePropertyColumnWriter("UserId", PropertyWriteMethod.Raw, NpgsqlDbType.Uuid)
};

Log.Logger = new LoggerConfiguration()
    .ReadFrom.Configuration(builder.Configuration)
    .Enrich.FromLogContext()
    .Enrich.WithEnvironmentName()
    .Enrich.WithThreadId()
    .WriteTo.PostgreSQL(
        serilogConnectionString,
        "application_logs",
        columnOptions: columnWriters,
        needAutoCreateTable: false)
    .CreateLogger();

builder.Logging.AddSerilog(Log.Logger, dispose: true);

// Add services to the container.
builder.Services.AddControllers();
builder.Services.AddProblemDetails();
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen(options =>
{
    options.SwaggerDoc("v1", new OpenApiInfo
    {
        Title = "Femved API",
        Version = "v1",
        Description = "Femved API endpoints"
    });

    var xmlFile = $"{Assembly.GetExecutingAssembly().GetName().Name}.xml";
    var xmlPath = Path.Combine(AppContext.BaseDirectory, xmlFile);
    options.IncludeXmlComments(xmlPath);

    options.TagActionsBy(api => new[]
    {
        api.ActionDescriptor.RouteValues.TryGetValue("controller", out var controller) && !string.IsNullOrWhiteSpace(controller)
            ? controller
            : api.GroupName ?? "default"
    });
    options.DocInclusionPredicate((_, _) => true);
    options.OperationFilter<GlobalResponsesOperationFilter>();
    options.SchemaFilter<ProblemDetailsSchemaFilter>();

    options.AddSecurityDefinition("Bearer", new OpenApiSecurityScheme
    {
        Name = "Authorization",
        Type = SecuritySchemeType.Http,
        Scheme = "bearer",
        BearerFormat = "JWT",
        In = ParameterLocation.Header,
        Description = "JWT Authorization header using the Bearer scheme. Example: \"Bearer {token}\""
    });

    options.AddSecurityRequirement(new OpenApiSecurityRequirement
    {
        {
            new OpenApiSecurityScheme
            {
                Reference = new OpenApiReference
                {
                    Type = ReferenceType.SecurityScheme,
                    Id = "Bearer"
                }
            },
            Array.Empty<string>()
        }
    });
});
builder.Services.AddDbContext<AppDbContext>(options =>
    options.UseNpgsql(builder.Configuration.GetConnectionString("DefaultConnection")));
builder.Services.AddInfrastructure(builder.Configuration);

builder.Services.AddScoped<IUserRepository, UserRepository>();
builder.Services.AddScoped<IRefreshTokenRepository, RefreshTokenRepository>();
builder.Services.AddScoped<IProductRepository, ProductRepository>();
builder.Services.AddScoped<IOrderRepository, OrderRepository>();
builder.Services.AddScoped<IPaymentRepository, PaymentRepository>();
builder.Services.AddScoped<ICouponRepository, CouponRepository>();
builder.Services.AddScoped<IExpertRepository, ExpertRepository>();
builder.Services.AddScoped<IGuidedRepository, GuidedRepository>();
builder.Services.AddScoped<IGuidedAdminRepository, GuidedAdminRepository>();
builder.Services.AddScoped<IRoleRepository, RoleRepository>();
builder.Services.AddScoped<IUserProductAccessRepository, UserProductAccessRepository>();
builder.Services.AddScoped<INotificationSender, NotificationSender>();
builder.Services.AddScoped<IMediaAccessService, SignedUrlMediaAccessService>();
builder.Services.AddScoped<ICurrentUserContext, CurrentUserContext>();
builder.Services.AddScoped<ICurrentUserService, CurrentUserContext>();
builder.Services.AddScoped<ITokenService, JwtTokenService>();
builder.Services.AddScoped<IPasswordHasher, Pbkdf2PasswordHasher>();
builder.Services.AddScoped<IEmailSender, ConsoleEmailSender>();
builder.Services.AddScoped<IPaymentGatewayStrategy, StripePaymentGatewayStrategy>();
builder.Services.AddScoped<IPaymentGatewayStrategy, PayPalPaymentGatewayStrategy>();
builder.Services.AddScoped<IPaymentGatewayStrategy, RazorpayPaymentGatewayStrategy>();

builder.Services.AddScoped<SignUpHandler>();
builder.Services.AddScoped<SignInHandler>();
builder.Services.AddScoped<RefreshTokenHandler>();
builder.Services.AddScoped<ForgotPasswordHandler>();
builder.Services.AddScoped<ResetPasswordHandler>();
builder.Services.AddScoped<CreateProductHandler>();
builder.Services.AddScoped<UpdateProductHandler>();
builder.Services.AddScoped<DeleteProductHandler>();
builder.Services.AddScoped<CreateDomainHandler>();
builder.Services.AddScoped<UpdateDomainHandler>();
builder.Services.AddScoped<DeleteDomainHandler>();
builder.Services.AddScoped<CreateCategoryHandler>();
builder.Services.AddScoped<UpdateCategoryHandler>();
builder.Services.AddScoped<DeleteCategoryHandler>();
builder.Services.AddScoped<CreateProgramHandler>();
builder.Services.AddScoped<UpdateProgramHandler>();
builder.Services.AddScoped<DeleteProgramHandler>();
builder.Services.AddScoped<CreateProgramPricingHandler>();
builder.Services.AddScoped<UpdateProgramPricingHandler>();
builder.Services.AddScoped<DeleteProgramPricingHandler>();
builder.Services.AddScoped<CreateExpertHandler>();
builder.Services.AddScoped<UpdateExpertHandler>();
builder.Services.AddScoped<DeleteExpertHandler>();

builder.Services.AddValidatorsFromAssemblyContaining<RegisterRequestDtoValidator>();
builder.Services.AddValidatorsFromAssemblyContaining<SignUpRequestValidator>();
builder.Services.AddFluentValidationAutoValidation();
builder.Services.AddFluentValidationClientsideAdapters();

builder.Services.Configure<ApiBehaviorOptions>(options =>
{
    options.InvalidModelStateResponseFactory = context =>
    {
        var errors = context.ModelState
            .Where(entry => entry.Value?.Errors.Count > 0)
            .ToDictionary(
                entry => entry.Key,
                entry => entry.Value!.Errors.Select(error => error.ErrorMessage).ToArray());

        return new BadRequestObjectResult(new
        {
            status = StatusCodes.Status400BadRequest,
            message = "Validation failed",
            errors
        });
    };
});

builder.Services.Configure<JwtOptions>(builder.Configuration.GetSection("Jwt"));

builder.Services.AddAuthentication(JwtBearerDefaults.AuthenticationScheme)
    .AddJwtBearer(options =>
    {
        var jwtOptions = builder.Configuration.GetSection("Jwt").Get<JwtOptions>()
            ?? throw new InvalidOperationException("Jwt settings are not configured.");

        options.TokenValidationParameters = new TokenValidationParameters
        {
            ValidateIssuer = true,
            ValidateAudience = true,
            ValidateIssuerSigningKey = true,
            ValidateLifetime = true,
            ValidIssuer = jwtOptions.Issuer,
            ValidAudience = jwtOptions.Audience,
            IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(jwtOptions.SigningKey)),
            ClockSkew = TimeSpan.FromMinutes(1)
        };
    });


builder.Services.AddAuthorization(options =>
{
    options.AddPolicy(PolicyNames.AdminOnly, policy => policy.RequireRole(RoleNames.Admin));
    options.AddPolicy(PolicyNames.ExpertOnly, policy => policy.RequireRole(RoleNames.Expert));
    options.AddPolicy(PolicyNames.UserOnly, policy => policy.RequireRole(RoleNames.User));
    options.AddPolicy(PolicyNames.CanManageCatalog, policy => policy.RequireRole(RoleNames.Admin, RoleNames.Expert));
    options.AddPolicy(PolicyNames.CanViewAssignedUsers, policy => policy.RequireRole(RoleNames.Admin, RoleNames.Expert));
});

builder.Services.AddHttpContextAccessor();
builder.Services.Configure<MediaAccessOptions>(builder.Configuration.GetSection("MediaAccess"));

var app = builder.Build();

// Configure the HTTP request pipeline.
app.Use(async (context, next) =>
{
    var userIdValue = context.User?.FindFirstValue(ClaimTypes.NameIdentifier);
    var userId = Guid.TryParse(userIdValue, out var parsedUserId) ? parsedUserId : (Guid?)null;

    using (LogContext.PushProperty("RequestPath", context.Request.Path.Value ?? string.Empty))
    using (LogContext.PushProperty("UserId", userId))
    {
        await next();
    }
});

app.UseExceptionHandler(appError =>
{
    appError.Run(async context =>
    {
        var feature = context.Features.Get<Microsoft.AspNetCore.Diagnostics.IExceptionHandlerFeature>();
        if (feature?.Error is AppValidationException validationException)
        {
            context.Response.StatusCode = StatusCodes.Status400BadRequest;
            await context.Response.WriteAsJsonAsync(new
            {
                //type = "validation_error",
                //title = "Validation failed",
                status = StatusCodes.Status400BadRequest,
                message = "Validation failed",
                errors = validationException.Errors
            });
            return;
        }

        if (feature?.Error is not null)
        {
            Log.Error(feature.Error, "Unhandled exception");
        }

        context.Response.StatusCode = StatusCodes.Status500InternalServerError;
        await context.Response.WriteAsJsonAsync(new
        {
            status = StatusCodes.Status500InternalServerError,
            message = "An unexpected error occurred."
        });
    });
});

//if (app.Environment.IsDevelopment())
//{
app.UseSwagger();
app.UseSwaggerUI(options =>
{
    options.RoutePrefix = string.Empty;
    options.SwaggerEndpoint("/swagger/v1/swagger.json", "Femved API v1");
});
//}

app.UseHttpsRedirection();

app.UseAuthentication();
app.UseAuthorization();

app.MapControllers();

app.Run();
