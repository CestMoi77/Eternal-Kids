using EternalKids.Domain.Entities;
using Microsoft.EntityFrameworkCore;

namespace EternalKids.Infrastructure.Data;

public class EternalKidsContext(DbContextOptions<EternalKidsContext> options) : DbContext(options)
{
    public DbSet<Cart> Carts => Set<Cart>();
    public DbSet<CartItem> CartItems => Set<CartItem>();
    public DbSet<Order> Orders => Set<Order>();
    public DbSet<OrderItem> OrderItems => Set<OrderItem>();
    public DbSet<Payment> Payments => Set<Payment>();
    public DbSet<PaymentEvent> PaymentEvents => Set<PaymentEvent>();
    public DbSet<ChatSession> ChatSessions => Set<ChatSession>();
    public DbSet<ChatMessage> ChatMessages => Set<ChatMessage>();
    public DbSet<AiRequestLog> AiRequestLogs => Set<AiRequestLog>();
    public DbSet<PriceRule> PriceRules => Set<PriceRule>();
    public DbSet<PackageDefinition> PackageDefinitions => Set<PackageDefinition>();
    public DbSet<SiteContentBlock> SiteContentBlocks => Set<SiteContentBlock>();

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        base.OnModelCreating(modelBuilder);

        modelBuilder.Entity<Cart>(entity =>
        {
            entity.ToTable("EternalKids_Cart");
            entity.HasKey(x => x.Id);
            entity.Property(x => x.AnonymousTokenHash).HasMaxLength(64).IsRequired();
            entity.Property(x => x.Currency).HasMaxLength(3).IsRequired();
        });

        modelBuilder.Entity<CartItem>(entity =>
        {
            entity.ToTable("EternalKids_CartItem");
            entity.HasKey(x => x.Id);
            entity.Property(x => x.UnitPrice).HasPrecision(18, 2);
            entity.Property(x => x.LineTotal).HasPrecision(18, 2);
            entity.Property(x => x.OptionsJson).HasColumnType("nvarchar(max)");
            entity.HasOne(x => x.Cart).WithMany(x => x.Items).HasForeignKey(x => x.CartId);
        });

        modelBuilder.Entity<Order>(entity =>
        {
            entity.ToTable("EternalKids_Order");
            entity.HasKey(x => x.Id);
            entity.Property(x => x.OrderNumber).HasMaxLength(64).IsRequired();
            entity.Property(x => x.EventType).HasMaxLength(100);
            entity.Property(x => x.Subtotal).HasPrecision(18, 2);
            entity.Property(x => x.DiscountTotal).HasPrecision(18, 2);
            entity.Property(x => x.GrandTotal).HasPrecision(18, 2);
            entity.Property(x => x.DepositRate).HasPrecision(5, 2);
            entity.Property(x => x.DepositAmount).HasPrecision(18, 2);
            entity.Property(x => x.CouponCodeApplied).HasMaxLength(50);
            entity.Property(x => x.RowVersion).IsRowVersion();
            entity.HasIndex(x => x.OrderNumber).IsUnique();
        });

        modelBuilder.Entity<OrderItem>(entity =>
        {
            entity.ToTable("EternalKids_OrderItem");
            entity.HasKey(x => x.Id);
            entity.Property(x => x.DescriptionSnapshot).HasMaxLength(500).IsRequired();
            entity.Property(x => x.UnitPrice).HasPrecision(18, 2);
            entity.Property(x => x.LineTotal).HasPrecision(18, 2);
            entity.Property(x => x.OptionsJson).HasColumnType("nvarchar(max)");
            entity.Property(x => x.FulfillmentStatus).HasMaxLength(32);
            entity.HasOne(x => x.Order).WithMany(x => x.Items).HasForeignKey(x => x.OrderId);
        });

        modelBuilder.Entity<Payment>(entity =>
        {
            entity.ToTable("EternalKids_Payment");
            entity.HasKey(x => x.Id);
            entity.Property(x => x.Provider).HasMaxLength(50).IsRequired();
            entity.Property(x => x.ProviderPaymentId).HasMaxLength(100).IsRequired();
            entity.Property(x => x.ProviderCheckoutUrl).HasMaxLength(2048).IsRequired();
            entity.Property(x => x.Amount).HasPrecision(18, 2);
            entity.Property(x => x.Currency).HasMaxLength(3).IsRequired();
            entity.HasIndex(x => x.ProviderPaymentId).IsUnique();
            entity.HasOne(x => x.Order).WithMany(x => x.Payments).HasForeignKey(x => x.OrderId);
        });

        modelBuilder.Entity<PaymentEvent>(entity =>
        {
            entity.ToTable("EternalKids_PaymentEvent");
            entity.HasKey(x => x.Id);
            entity.Property(x => x.EventType).HasMaxLength(100).IsRequired();
            entity.Property(x => x.RawPayload).HasColumnType("nvarchar(max)").IsRequired();
            entity.HasOne(x => x.Payment).WithMany(x => x.Events).HasForeignKey(x => x.PaymentId);
        });

        modelBuilder.Entity<ChatSession>(entity =>
        {
            entity.ToTable("EternalKids_ChatSession");
            entity.HasKey(x => x.Id);
            entity.Property(x => x.Channel).HasMaxLength(20).IsRequired();
            entity.Property(x => x.AnonymousTokenHash).HasMaxLength(64).IsRequired();
            entity.Property(x => x.UtmSource).HasMaxLength(100);
            entity.Property(x => x.UtmCampaign).HasMaxLength(100);
            entity.Property(x => x.Referrer).HasMaxLength(2048);
        });

        modelBuilder.Entity<ChatMessage>(entity =>
        {
            entity.ToTable("EternalKids_ChatMessage");
            entity.HasKey(x => x.Id);
            entity.Property(x => x.Content).HasColumnType("nvarchar(max)").IsRequired();
            entity.HasOne(x => x.ChatSession).WithMany(x => x.Messages).HasForeignKey(x => x.ChatSessionId);
        });

        modelBuilder.Entity<AiRequestLog>(entity =>
        {
            entity.ToTable("EternalKids_AI_Request_Log");
            entity.HasKey(x => x.Id);
            entity.Property(x => x.Model).HasMaxLength(128).IsRequired();
            entity.Property(x => x.PromptHash).HasMaxLength(64).IsRequired();
            entity.Property(x => x.ToolCallsJson).HasColumnType("nvarchar(max)");
            entity.Property(x => x.ResponseHash).HasMaxLength(64);
            entity.Property(x => x.CostEstimate).HasPrecision(18, 4);
            entity.Property(x => x.ErrorCode).HasMaxLength(64);
            entity.Property(x => x.ErrorMessage).HasMaxLength(2048);
        });

        modelBuilder.Entity<PriceRule>(entity =>
        {
            entity.ToTable("EternalKids_PriceRule");
            entity.HasKey(x => x.Id);
            entity.Property(x => x.EventType).HasMaxLength(50).IsRequired();
            entity.Property(x => x.PackageType).HasMaxLength(50).IsRequired();
            entity.Property(x => x.Price).HasPrecision(18, 2);
            entity.HasIndex(x => new { x.EventType, x.PackageType, x.MinGuests, x.MaxGuests });
        });

        modelBuilder.Entity<PackageDefinition>(entity =>
        {
            entity.ToTable("EternalKids_PackageDefinition");
            entity.HasKey(x => x.Id);
            entity.Property(x => x.PackageType).HasMaxLength(50).IsRequired();
            entity.Property(x => x.Title).HasMaxLength(120).IsRequired();
            entity.Property(x => x.Description).HasMaxLength(500).IsRequired();
            entity.Property(x => x.FeaturesJson).HasColumnType("nvarchar(max)").IsRequired();
            entity.HasIndex(x => x.PackageType).IsUnique();
        });

        modelBuilder.Entity<SiteContentBlock>(entity =>
        {
            entity.ToTable("EternalKids_SiteContentBlock");
            entity.HasKey(x => x.Id);
            entity.Property(x => x.PageKey).HasMaxLength(50).IsRequired();
            entity.Property(x => x.SectionKey).HasMaxLength(50).IsRequired();
            entity.Property(x => x.Title).HasMaxLength(200).IsRequired();
            entity.Property(x => x.Body).HasColumnType("nvarchar(max)").IsRequired();
            entity.HasIndex(x => new { x.PageKey, x.SectionKey }).IsUnique();
        });
    }
}
