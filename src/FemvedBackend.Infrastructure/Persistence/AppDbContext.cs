using FemvedBackend.Domain.Entities;
using Microsoft.EntityFrameworkCore;

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

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<User>(builder =>
        {
            builder.ToTable("users");
            builder.HasKey(user => user.Id);

            builder.Property(user => user.Id).HasColumnName("id");
            builder.Property(user => user.Email).HasColumnName("email");
            builder.Property(user => user.PasswordHash).HasColumnName("password_hash");
            builder.Property(user => user.FirstName).HasColumnName("first_name");
            builder.Property(user => user.LastName).HasColumnName("last_name");
            builder.Property(user => user.Country).HasColumnName("country_code");
            builder.Property(user => user.Currency).HasColumnName("currency_code");
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

            builder.Property(role => role.Id).HasColumnName("id");
            builder.Property(role => role.Name).HasColumnName("name");

            builder.Property<string>("Description").HasColumnName("description");

            builder.Ignore(role => role.Type);
        });

        modelBuilder.Entity<ProductType>(builder =>
        {
            builder.ToTable("product_types");
            builder.HasKey(productType => productType.Id);

            builder.Property(productType => productType.Id).HasColumnName("id");
            builder.Property(productType => productType.Name).HasColumnName("name");
            builder.Property<string>("Description").HasColumnName("description");
        });

        modelBuilder.Entity<Product>(builder =>
        {
            builder.ToTable("products");
            builder.HasKey(product => product.Id);

            builder.Property(product => product.Id).HasColumnName("id");
            builder.Property(product => product.ProductTypeId).HasColumnName("product_type_id");
            builder.Property(product => product.Name).HasColumnName("title");
            builder.Property(product => product.Description).HasColumnName("description");

            builder.Property<decimal>("BasePrice").HasColumnName("base_price");
            builder.Property<string>("CurrencyCode").HasColumnName("currency_code");
            builder.Property<bool>("IsActive").HasColumnName("is_active");
            builder.Property<Guid>("CreatedBy").HasColumnName("created_by");
            builder.Property<DateTimeOffset>("CreatedAt").HasColumnName("created_at");
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

            builder.Property<string>("Specialization").HasColumnName("specialization");
            builder.Property<decimal>("Rating").HasColumnName("rating");
            builder.Property<bool>("IsVerified").HasColumnName("is_verified");

            builder.Ignore(expert => expert.Id);
            builder.Ignore(expert => expert.DisplayName);
        });

        modelBuilder.Entity<ExpertProduct>(builder =>
        {
            builder.ToTable("expert_products");
            builder.HasKey(expertProduct => expertProduct.Id);

            builder.Property(expertProduct => expertProduct.Id).HasColumnName("id");
            builder.Property(expertProduct => expertProduct.ProductId).HasColumnName("product_id");
            builder.Property(expertProduct => expertProduct.ExpertId).HasColumnName("experts_id");

            builder.Property<short>("DurationWeeks").HasColumnName("duration_weeks");
            builder.Property<decimal>("Price").HasColumnName("price");
            builder.Property<bool>("IsActive").HasColumnName("is_active");
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
