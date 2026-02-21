using FemvedBackend.Application.Exceptions;
using FemvedBackend.Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.ChangeTracking;

namespace FemvedBackend.Infrastructure.Persistence;

public class AppDbContext : DbContext
{
    public AppDbContext(DbContextOptions<AppDbContext> options)
        : base(options)
    {
    }

    public DbSet<User> Users => Set<User>();
    public DbSet<Role> Roles => Set<Role>();
    public DbSet<Product> Products => Set<Product>();
    public DbSet<ProductType> ProductTypes => Set<ProductType>();
    public DbSet<ProductVariant> ProductVariants => Set<ProductVariant>();
    public DbSet<Expert> Experts => Set<Expert>();
    public DbSet<ExpertProduct> ExpertProducts => Set<ExpertProduct>();
    public DbSet<global::FemvedBackend.Domain.Entities.Domain> Domains => Set<global::FemvedBackend.Domain.Entities.Domain>();
    public DbSet<Category> Categories => Set<Category>();
    public DbSet<Program> Programs => Set<Program>();
    public DbSet<ProgramPricing> ProgramPricing => Set<ProgramPricing>();
    public DbSet<RecordedContent> RecordedContents => Set<RecordedContent>();
    public DbSet<Event> Events => Set<Event>();
    public DbSet<Order> Orders => Set<Order>();
    public DbSet<OrderItem> OrderItems => Set<OrderItem>();
    public DbSet<Payment> Payments => Set<Payment>();
    public DbSet<PaymentGateway> PaymentGateways => Set<PaymentGateway>();
    public DbSet<Refund> Refunds => Set<Refund>();
    public DbSet<Coupon> Coupons => Set<Coupon>();
    public DbSet<CouponUsage> CouponUsages => Set<CouponUsage>();
    public DbSet<UserProductAccess> UserProductAccesses => Set<UserProductAccess>();
    public DbSet<RefreshToken> RefreshTokens => Set<RefreshToken>();
    public DbSet<Notification> Notifications => Set<Notification>();
    public DbSet<NotificationTemplate> NotificationTemplates => Set<NotificationTemplate>();

    public override int SaveChanges()
    {
        ApplyAuditDefaults();
        ValidateExpertProductRules();
        return base.SaveChanges();
    }

    public override async Task<int> SaveChangesAsync(CancellationToken cancellationToken = default)
    {
        ApplyAuditDefaults();
        await ValidateExpertProductRulesAsync(cancellationToken);
        return await base.SaveChangesAsync(cancellationToken);
    }

    private void ApplyAuditDefaults()
    {
        var utcNow = DateTime.UtcNow;

        foreach (var entry in ChangeTracker.Entries())
        {
            if (entry.State == EntityState.Added)
            {
                SetDateTimeProperty(entry, "CreatedAt", utcNow);
                SetDateTimeProperty(entry, "UpdatedAt", utcNow);
                SetBooleanProperty(entry, "IsActive", true);
                SetBooleanProperty(entry, "WhatsappOptIn", false);
            }
            else if (entry.State == EntityState.Modified)
            {
                SetDateTimeProperty(entry, "UpdatedAt", utcNow);

                if (entry.Metadata.FindProperty("CreatedAt") is not null)
                {
                    entry.Property("CreatedAt").IsModified = false;
                }
            }
        }
    }

    private static void SetDateTimeProperty(EntityEntry entry, string propertyName, DateTime utcNow)
    {
        var property = entry.Metadata.FindProperty(propertyName);
        if (property is null)
        {
            return;
        }

        var entryProperty = entry.Property(propertyName);
        if (property.ClrType == typeof(DateTimeOffset) || property.ClrType == typeof(DateTimeOffset?))
        {
            entryProperty.CurrentValue = DateTimeOffset.UtcNow;
            return;
        }

        if (property.ClrType == typeof(DateTime) || property.ClrType == typeof(DateTime?))
        {
            entryProperty.CurrentValue = utcNow;
        }
    }

    private static void SetBooleanProperty(EntityEntry entry, string propertyName, bool value)
    {
        var property = entry.Metadata.FindProperty(propertyName);
        if (property is null)
        {
            return;
        }

        if (property.ClrType == typeof(bool) || property.ClrType == typeof(bool?))
        {
            entry.Property(propertyName).CurrentValue = value;
        }
    }

    private void ValidateExpertProductRules()
    {
        var entries = ChangeTracker.Entries<ExpertProduct>()
            .Where(entry => entry.State is EntityState.Added or EntityState.Modified)
            .Select(entry => entry.Entity)
            .ToList();

        foreach (var expertProduct in entries)
        {
            EnsureNoDuplicateDuration(expertProduct);
            EnsureFinalPriceMatchesDiscount(expertProduct);
        }
    }

    private async Task ValidateExpertProductRulesAsync(CancellationToken cancellationToken)
    {
        var entries = ChangeTracker.Entries<ExpertProduct>()
            .Where(entry => entry.State is EntityState.Added or EntityState.Modified)
            .Select(entry => entry.Entity)
            .ToList();

        foreach (var expertProduct in entries)
        {
            await EnsureNoDuplicateDurationAsync(expertProduct, cancellationToken);
            EnsureFinalPriceMatchesDiscount(expertProduct);
        }
    }

    private void EnsureNoDuplicateDuration(ExpertProduct expertProduct)
    {
        var duplicateExists = ExpertProducts.Any(existing =>
            existing.Id != expertProduct.Id
            && existing.ProductId == expertProduct.ProductId
            && existing.ExpertId == expertProduct.ExpertId
            && existing.DurationWeeks == expertProduct.DurationWeeks);

        if (duplicateExists)
        {
            throw new ValidationException("durationWeeks", "Duplicate duration weeks for this product.");
        }
    }

    private async Task EnsureNoDuplicateDurationAsync(ExpertProduct expertProduct, CancellationToken cancellationToken)
    {
        var duplicateExists = await ExpertProducts.AnyAsync(existing =>
            existing.Id != expertProduct.Id
            && existing.ProductId == expertProduct.ProductId
            && existing.ExpertId == expertProduct.ExpertId
            && existing.DurationWeeks == expertProduct.DurationWeeks, cancellationToken);

        if (duplicateExists)
        {
            throw new ValidationException("durationWeeks", "Duplicate duration weeks for this product.");
        }
    }

    private static void EnsureFinalPriceMatchesDiscount(ExpertProduct expertProduct)
    {
        var expectedFinalPrice = expertProduct.OriginalPrice
            - (expertProduct.OriginalPrice * expertProduct.DiscountPercentage / 100m);

        if (expertProduct.FinalPrice != expectedFinalPrice)
        {
            throw new ValidationException("finalPrice", "Final price does not match discount calculation.");
        }
    }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.ApplyConfigurationsFromAssembly(typeof(AppDbContext).Assembly);

        modelBuilder.Entity<User>(builder =>
        {
            builder.ToTable("users");
            builder.HasKey(user => user.Id);

            builder.Property(user => user.Id).HasColumnName("id");
            builder.Property(user => user.Email).HasColumnName("email");
            builder.Property(user => user.PasswordHash).HasColumnName("password_hash");
            builder.Property(user => user.FirstName).HasColumnName("first_name");
            builder.Property(user => user.LastName).HasColumnName("last_name");
            builder.Property(user => user.CountryCode).HasColumnName("country_code");
            builder.Property(user => user.MobileNumber).HasColumnName("mobile_number");
            builder.Property(user => user.IsMobileVerified).HasColumnName("is_mobile_verified");
            builder.Property(user => user.IsEmailVerified).HasColumnName("is_email_verified");
            builder.Property(user => user.RoleId).HasColumnName("role_id");

            builder.Property<bool>("WhatsappOptIn").HasColumnName("whatsapp_opt_in");
            builder.Property<bool>("IsActive").HasColumnName("is_active");
            builder.Property<DateTimeOffset>("CreatedAt").HasColumnName("created_at");
            builder.Property<DateTimeOffset>("UpdatedAt").HasColumnName("updated_at");

            builder.Ignore(user => user.PasswordResetTokenHash);
            builder.Ignore(user => user.PasswordResetTokenExpiresAt);
        });

        modelBuilder.Entity<Role>(builder =>
        {
            builder.ToTable("roles");
            builder.HasKey(role => role.Id);

            builder.Property(role => role.Name)
                .HasMaxLength(100)
                .IsRequired();

            builder.HasIndex(role => role.Name)
                .IsUnique();

            builder.HasMany(role => role.Users)
                .WithOne(user => user.Role)
                .HasForeignKey(user => user.RoleId)
                .OnDelete(DeleteBehavior.Restrict);
        });

        modelBuilder.Entity<ProductType>(builder =>
        {
            builder.ToTable("product_types");
            builder.HasKey(productType => productType.Id);

            builder.Property(productType => productType.Id).HasColumnName("id");
            builder.Property(productType => productType.Name).HasColumnName("name");
            builder.Property(productType => productType.Description).HasColumnName("description");
        });

        modelBuilder.Entity<Product>(builder =>
        {
            builder.ToTable("products");
            builder.HasKey(product => product.Id);

            builder.Property(product => product.Id).HasColumnName("id");
            builder.Property(product => product.ProductTypeId).HasColumnName("product_type_id");
            builder.Property(product => product.Title).HasColumnName("title");
            builder.Property(product => product.Description).HasColumnName("description");
            builder.Property(product => product.ImageUrl).HasColumnName("image_url");
            builder.Property(product => product.IsActive).HasColumnName("is_active");
            builder.Property(product => product.CreatedBy).HasColumnName("created_by");
            builder.Property(product => product.CreatedAt).HasColumnName("created_at");
        });

        modelBuilder.Entity<ProductVariant>(builder =>
        {
            builder.ToTable("product_variants");
            builder.HasKey(variant => variant.Id);

            builder.Property(variant => variant.Id).HasColumnName("id");
            builder.Property(variant => variant.ProductId).HasColumnName("product_id");
            builder.Property(variant => variant.Name).HasColumnName("name");
            builder.Property(variant => variant.Sku).HasColumnName("sku");
            builder.Property(variant => variant.Price).HasColumnName("price");
        });

        modelBuilder.Entity<Expert>(builder =>
        {
            builder.ToTable("experts");
            builder.HasKey(expert => expert.UserId);

            builder.Property(expert => expert.UserId).HasColumnName("user_id");
            builder.Property(expert => expert.Bio).HasColumnName("bio");
            builder.Property(expert => expert.Specialization).HasColumnName("specialization");
            builder.Property(expert => expert.Rating).HasColumnName("rating");
            builder.Property(expert => expert.IsVerified).HasColumnName("is_verified");
        });

        modelBuilder.Entity<ExpertProduct>(builder =>
        {
            builder.ToTable("expert_products");
            builder.HasKey(expertProduct => expertProduct.Id);

            builder.Property(expertProduct => expertProduct.Id).HasColumnName("id");
            builder.Property(expertProduct => expertProduct.ProductId).HasColumnName("product_id");
            builder.Property(expertProduct => expertProduct.ExpertId).HasColumnName("expert_id");
            builder.Property(expertProduct => expertProduct.DurationWeeks).HasColumnName("duration_weeks");
            builder.Property(expertProduct => expertProduct.OriginalPrice).HasColumnName("original_price");
            builder.Property(expertProduct => expertProduct.DiscountPercentage).HasColumnName("discount_percentage");
            builder.Property(expertProduct => expertProduct.FinalPrice).HasColumnName("final_price");
            builder.Property(expertProduct => expertProduct.CurrencyCode).HasColumnName("currency_code");
            builder.Property(expertProduct => expertProduct.DisplayOrder).HasColumnName("display_order");
            builder.Property(expertProduct => expertProduct.IsActive).HasColumnName("is_active");

            builder.HasIndex(expertProduct => new { expertProduct.ProductId, expertProduct.ExpertId, expertProduct.DurationWeeks })
                .IsUnique();
        });

        modelBuilder.Entity<global::FemvedBackend.Domain.Entities.Domain>(builder =>
        {
            builder.ToTable("domains");
            builder.HasKey(domain => domain.Id);

            builder.Property(domain => domain.Id).HasColumnName("id");
            builder.Property(domain => domain.Name).HasColumnName("name");
            builder.Property(domain => domain.Description).HasColumnName("description");
            builder.Property(domain => domain.IsActive).HasColumnName("is_active");
        });

        modelBuilder.Entity<Category>(builder =>
        {
            builder.ToTable("categories");
            builder.HasKey(category => category.Id);

            builder.Property(category => category.Id).HasColumnName("id");
            builder.Property(category => category.DomainId).HasColumnName("domain_id");
            builder.Property(category => category.ParentCategoryId).HasColumnName("parent_category_id");
            builder.Property(category => category.Name).HasColumnName("name");
            builder.Property(category => category.Description).HasColumnName("description");
            builder.Property(category => category.DisplayOrder).HasColumnName("display_order");
            builder.Property(category => category.IsActive).HasColumnName("is_active");
        });

        modelBuilder.Entity<Program>(builder =>
        {
            builder.ToTable("programs");
            builder.HasKey(program => program.Id);

            builder.Property(program => program.Id).HasColumnName("id");
            builder.Property(program => program.CategoryId).HasColumnName("category_id");
            builder.Property(program => program.ExpertId).HasColumnName("expert_id");
            builder.Property(program => program.Title).HasColumnName("title");
            builder.Property(program => program.Description).HasColumnName("description");
            builder.Property(program => program.ImageUrl).HasColumnName("image_url");
            builder.Property(program => program.IsActive).HasColumnName("is_active");
            builder.Property(program => program.CreatedAt).HasColumnName("created_at");
        });

        modelBuilder.Entity<ProgramPricing>(builder =>
        {
            builder.ToTable("program_duration");
            builder.HasKey(pricing => pricing.Id);

            builder.Property(pricing => pricing.Id).HasColumnName("id");
            builder.Property(pricing => pricing.ProgramId).HasColumnName("program_id");
            builder.Property(pricing => pricing.DurationWeeks).HasColumnName("duration_weeks");
            builder.Property(pricing => pricing.OriginalPrice).HasColumnName("original_price");
            builder.Property(pricing => pricing.DiscountPercentage).HasColumnName("discount_percentage");
            builder.Property(pricing => pricing.FinalPrice).HasColumnName("final_price");
            builder.Property(pricing => pricing.CurrencyCode).HasColumnName("currency_code");
            builder.Property(pricing => pricing.IsActive).HasColumnName("is_active");
        });

        modelBuilder.Entity<RecordedContent>(builder =>
        {
            builder.ToTable("recorded_contents");
            builder.HasKey(content => content.ProductId);

            builder.Property(content => content.ProductId).HasColumnName("product_id");
            builder.Property(content => content.ContentUrl).HasColumnName("content_url");

            builder.Property<string>("TrailerUrl").HasColumnName("trailer_url");
            builder.Property<string>("ContentType").HasColumnName("content_type");
            builder.Property<bool>("Downloadable").HasColumnName("downloadable");

            builder.Ignore(content => content.Id);
            builder.Ignore(content => content.Title);
            builder.Ignore(content => content.Duration);
        });

        modelBuilder.Entity<Event>(builder =>
        {
            builder.ToTable("events");
            builder.HasKey(@event => @event.ProductId);

            builder.Property(@event => @event.ProductId).HasColumnName("product_id");
            builder.Property(@event => @event.StartsAt).HasColumnName("start_date");
            builder.Property(@event => @event.EndsAt).HasColumnName("end_date");

            builder.Property<string>("Location").HasColumnName("location");
            builder.Property<int>("Capacity").HasColumnName("capacity");
            builder.Property<string>("GalleryUrls").HasColumnName("gallery_urls");

            builder.Ignore(@event => @event.Id);
            builder.Ignore(@event => @event.Title);
            builder.Ignore(@event => @event.ExpertId);
            builder.Ignore(@event => @event.Expert);
        });

        modelBuilder.Entity<Order>(builder =>
        {
            builder.ToTable("orders");
            builder.HasKey(order => order.Id);

            builder.Property(order => order.Id).HasColumnName("id");
            builder.Property(order => order.UserId).HasColumnName("user_id");
            builder.Property(order => order.TotalAmount).HasColumnName("total_amount");
            builder.Property(order => order.Status).HasColumnName("status");

            builder.Property<string>("CurrencyCode").HasColumnName("currency_code");
            builder.Property<DateTimeOffset>("CreatedAt").HasColumnName("created_at");
        });

        modelBuilder.Entity<OrderItem>(builder =>
        {
            builder.ToTable("order_items");
            builder.HasKey(item => item.Id);

            builder.Property(item => item.Id).HasColumnName("id");
            builder.Property(item => item.OrderId).HasColumnName("order_id");
            builder.Property(item => item.ProductVariantId).HasColumnName("product_variant_id");
            builder.Property(item => item.Quantity).HasColumnName("quantity");
            builder.Property(item => item.UnitPrice).HasColumnName("unit_price");
        });

        modelBuilder.Entity<Payment>(builder =>
        {
            builder.ToTable("payments");
            builder.HasKey(payment => payment.Id);

            builder.Property(payment => payment.Id).HasColumnName("id");
            builder.Property(payment => payment.OrderId).HasColumnName("order_id");
            builder.Property(payment => payment.PaymentGatewayId).HasColumnName("gateway_id");
            builder.Property(payment => payment.Amount).HasColumnName("amount");
            builder.Property(payment => payment.Status).HasColumnName("status");
            builder.Property(payment => payment.PaidAt).HasColumnName("completed_at");

            builder.Property<string>("CurrencyCode").HasColumnName("currency_code");
            builder.Property<string>("GatewayOrderId").HasColumnName("gateway_order_id");
            builder.Property<string>("GatewayPaymentId").HasColumnName("gateway_payment_id");
            builder.Property<DateTimeOffset>("InitiatedAt").HasColumnName("initiated_at");
        });

        modelBuilder.Entity<PaymentGateway>(builder =>
        {
            builder.ToTable("payment_gateways");
            builder.HasKey(gateway => gateway.Id);

            builder.Property(gateway => gateway.Id).HasColumnName("id");
            builder.Property(gateway => gateway.Name).HasColumnName("name");

            builder.Ignore(gateway => gateway.Type);
            builder.Ignore(gateway => gateway.IsActive);
        });

        modelBuilder.Entity<Refund>(builder =>
        {
            builder.ToTable("refunds");
            builder.HasKey(refund => refund.Id);

            builder.Property(refund => refund.Id).HasColumnName("id");
            builder.Property(refund => refund.PaymentId).HasColumnName("payment_id");
            builder.Property(refund => refund.Amount).HasColumnName("amount");
            builder.Property(refund => refund.Status).HasColumnName("status");
            builder.Property(refund => refund.RequestedAt).HasColumnName("initiated_at");
            builder.Property(refund => refund.CompletedAt).HasColumnName("completed_at");

            builder.Property<string>("Reason").HasColumnName("reason");
            builder.Property<string>("GatewayRefundId").HasColumnName("gateway_refund_id");
        });

        modelBuilder.Entity<Coupon>(builder =>
        {
            builder.ToTable("coupons");
            builder.HasKey(coupon => coupon.Id);

            builder.Property(coupon => coupon.Id).HasColumnName("id");
            builder.Property(coupon => coupon.Code).HasColumnName("code");
            builder.Property(coupon => coupon.DiscountType).HasColumnName("discount_type");
            builder.Property(coupon => coupon.DiscountValue).HasColumnName("discount_value");
            builder.Property(coupon => coupon.ValidFrom).HasColumnName("valid_from");
            builder.Property(coupon => coupon.ValidTo).HasColumnName("valid_to");
            builder.Property(coupon => coupon.MaxUsages).HasColumnName("max_usage");

            builder.Property<string>("Description").HasColumnName("description");
            builder.Property<int>("MaxUsagePerUser").HasColumnName("max_usage_per_user");
            builder.Property<bool>("IsActive").HasColumnName("is_active");
            builder.Property<DateTimeOffset>("CreatedAt").HasColumnName("created_at");
        });

        modelBuilder.Entity<CouponUsage>(builder =>
        {
            builder.ToTable("coupon_usages");
            builder.HasKey(usage => new { usage.CouponId, usage.UserId, usage.OrderId });

            builder.Property(usage => usage.CouponId).HasColumnName("coupon_id");
            builder.Property(usage => usage.UserId).HasColumnName("user_id");
            builder.Property(usage => usage.OrderId).HasColumnName("order_id");
            builder.Property(usage => usage.UsedAt).HasColumnName("used_at");

            builder.Ignore(usage => usage.Id);
        });

        modelBuilder.Entity<UserProductAccess>(builder =>
        {
            builder.ToTable("user_product_access");
            builder.HasKey(access => new { access.UserId, access.ProductId });

            builder.Property(access => access.UserId).HasColumnName("user_id");
            builder.Property(access => access.ProductId).HasColumnName("product_id");
            builder.Property(access => access.GrantedAt).HasColumnName("start_date");
            builder.Property(access => access.ExpiresAt).HasColumnName("end_date");

            builder.Property<string>("Status").HasColumnName("status");

            builder.Ignore(access => access.Id);
        });

        modelBuilder.Entity<RefreshToken>(builder =>
        {
            builder.ToTable("refresh_tokens");
            builder.HasKey(token => token.Id);

            builder.Property(token => token.Id).HasColumnName("id");
            builder.Property(token => token.UserId).HasColumnName("user_id");
            builder.Property(token => token.TokenHash).HasColumnName("token_hash");
            builder.Property(token => token.ExpiresAt).HasColumnName("expires_at");
            builder.Property(token => token.CreatedAt).HasColumnName("created_at");
            builder.Property(token => token.RevokedAt).HasColumnName("revoked_at");
            builder.Property(token => token.ReplacedByTokenHash).HasColumnName("replaced_by_token_hash");
        });

        modelBuilder.Entity<Notification>(builder =>
        {
            builder.ToTable("notifications");
            builder.HasKey(notification => notification.Id);

            builder.Property(notification => notification.Id).HasColumnName("id");
            builder.Property(notification => notification.UserId).HasColumnName("user_id");
            builder.Property(notification => notification.NotificationTemplateId).HasColumnName("notification_template_id");
            builder.Property(notification => notification.Title).HasColumnName("title");
            builder.Property(notification => notification.Message).HasColumnName("message");
            builder.Property(notification => notification.Status).HasColumnName("status");
            builder.Property(notification => notification.CreatedAt).HasColumnName("created_at");
            builder.Property(notification => notification.ReadAt).HasColumnName("read_at");
        });

        modelBuilder.Entity<NotificationTemplate>(builder =>
        {
            builder.ToTable("notification_templates");
            builder.HasKey(template => template.Id);

            builder.Property(template => template.Id).HasColumnName("id");
            builder.Property(template => template.Name).HasColumnName("name");
            builder.Property(template => template.TitleTemplate).HasColumnName("subject");
            builder.Property(template => template.BodyTemplate).HasColumnName("body");
            builder.Property(template => template.Channel).HasColumnName("channel");

            builder.Property<bool>("IsActive").HasColumnName("is_active");
        });

        modelBuilder.SharedTypeEntity<Dictionary<string, object>>("CouponProduct", builder =>
        {
            builder.ToTable("coupon_products");
            builder.Property<Guid>("CouponId").HasColumnName("coupon_id");
            builder.Property<Guid>("ProductId").HasColumnName("product_id");

            builder.HasKey("CouponId", "ProductId");
        });

        modelBuilder.SharedTypeEntity<Dictionary<string, object>>("PaymentMetadata", builder =>
        {
            builder.ToTable("payment_metadata");
            builder.Property<Guid>("PaymentId").HasColumnName("payment_id");
            builder.Property<string>("RawResponse").HasColumnName("raw_response");

            builder.HasKey("PaymentId");
        });

        modelBuilder.SharedTypeEntity<Dictionary<string, object>>("Policy", builder =>
        {
            builder.ToTable("policies");
            builder.Property<int>("Id").HasColumnName("id");
            builder.Property<string>("Name").HasColumnName("name");

            builder.HasKey("Id");
        });

        modelBuilder.SharedTypeEntity<Dictionary<string, object>>("RolePolicy", builder =>
        {
            builder.ToTable("role_policies");
            builder.Property<short>("RoleId").HasColumnName("role_id");
            builder.Property<int>("PolicyId").HasColumnName("policy_id");

            builder.HasKey("RoleId", "PolicyId");
        });
    }
}
