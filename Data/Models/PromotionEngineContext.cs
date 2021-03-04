using System;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata;

namespace Infrastructure.Models
{
    public partial class PromotionEngineContext : DbContext
    {
        public PromotionEngineContext()
        {
        }

        public PromotionEngineContext(DbContextOptions<PromotionEngineContext> options)
            : base(options)
        {
        }

        public virtual DbSet<Account> Account { get; set; }
        public virtual DbSet<Action> Action { get; set; }
        public virtual DbSet<Brand> Brand { get; set; }
        public virtual DbSet<Channel> Channel { get; set; }
        public virtual DbSet<ConditionGroup> ConditionGroup { get; set; }
        public virtual DbSet<ConditionRule> ConditionRule { get; set; }
        public virtual DbSet<Holiday> Holiday { get; set; }
        public virtual DbSet<Membership> Membership { get; set; }
        public virtual DbSet<MembershipAction> MembershipAction { get; set; }
        public virtual DbSet<MembershipCondition> MembershipCondition { get; set; }
        public virtual DbSet<OrderCondition> OrderCondition { get; set; }
        public virtual DbSet<ProductCondition> ProductCondition { get; set; }
        public virtual DbSet<Promotion> Promotion { get; set; }
        public virtual DbSet<PromotionChannelMapping> PromotionChannelMapping { get; set; }
        public virtual DbSet<PromotionStoreMapping> PromotionStoreMapping { get; set; }
        public virtual DbSet<PromotionTier> PromotionTier { get; set; }
        public virtual DbSet<RoleEntity> Role { get; set; }
        public virtual DbSet<Store> Store { get; set; }
        public virtual DbSet<Voucher> Voucher { get; set; }
        public virtual DbSet<VoucherGroup> VoucherGroup { get; set; }

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            if (!optionsBuilder.IsConfigured)
            {
#warning To protect potentially sensitive information in your connection string, you should move it out of source code. See http://go.microsoft.com/fwlink/?LinkId=723263 for guidance on storing connection strings.
                optionsBuilder.UseSqlServer("Server=tcp:promotionengine.database.windows.net,1433;Initial Catalog=PromotionEngine;Persist Security Info=False;User ID=adm;Password=Abcd1234;MultipleActiveResultSets=False;Encrypt=True;TrustServerCertificate=False;Connection Timeout=30;");
            }
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<Account>(entity =>
            {
                entity.HasKey(e => e.Username)
                    .HasName("PK_Account_1");

                entity.Property(e => e.Username)
                    .HasMaxLength(25)
                    .IsUnicode(false);

                entity.Property(e => e.Email)
                    .HasMaxLength(62)
                    .IsUnicode(false);

                entity.Property(e => e.FirstName).HasMaxLength(50);

                entity.Property(e => e.ImgUrl)
                    .HasMaxLength(2048)
                    .IsUnicode(false);

                entity.Property(e => e.InsDate)
                    .HasColumnType("datetime")
                    .HasDefaultValueSql("(getdate())");

                entity.Property(e => e.LastName).HasMaxLength(50);

                entity.Property(e => e.Password)
                    .HasMaxLength(60)
                    .IsUnicode(false);

                entity.Property(e => e.PhoneNumber)
                    .HasMaxLength(11)
                    .IsUnicode(false);

                entity.Property(e => e.UpdDate)
                    .HasColumnType("datetime")
                    .HasDefaultValueSql("(getdate())");

                entity.HasOne(d => d.Role)
                    .WithMany(p => p.Account)
                    .HasForeignKey(d => d.RoleId)
                    .HasConstraintName("FK_Account_Role");
            });

            modelBuilder.Entity<Action>(entity =>
            {
                entity.HasIndex(e => e.PromotionTierId)
                    .HasName("IX_Action")
                    .IsUnique();

                entity.Property(e => e.ActionId).HasDefaultValueSql("(newid())");

                entity.Property(e => e.ActionType)
                    .IsRequired()
                    .HasMaxLength(1)
                    .IsUnicode(false)
                    .IsFixedLength();

                entity.Property(e => e.BundlePrice).HasColumnType("decimal(10, 0)");

                entity.Property(e => e.BundleQuantity).HasColumnType("decimal(6, 0)");

                entity.Property(e => e.BundleStrategy)
                    .HasMaxLength(1)
                    .IsUnicode(false)
                    .IsFixedLength();

                entity.Property(e => e.DiscountAmount).HasColumnType("decimal(10, 0)");

                entity.Property(e => e.DiscountPercentage).HasColumnType("decimal(18, 0)");

                entity.Property(e => e.DiscountQuantity).HasColumnType("decimal(6, 0)");

                entity.Property(e => e.DiscountType)
                    .IsRequired()
                    .HasMaxLength(1)
                    .IsUnicode(false)
                    .IsFixedLength();

                entity.Property(e => e.FixedPrice).HasColumnType("decimal(10, 0)");

                entity.Property(e => e.InsDate)
                    .HasColumnType("datetime")
                    .HasDefaultValueSql("(getdate())");

                entity.Property(e => e.LadderPrice).HasColumnType("decimal(10, 0)");

                entity.Property(e => e.MaxAmount).HasColumnType("decimal(10, 0)");

                entity.Property(e => e.MinPriceAfter).HasColumnType("decimal(10, 0)");

                entity.Property(e => e.OrderLadderProduct).HasColumnType("decimal(2, 0)");

                entity.Property(e => e.ParentCode)
                    .HasMaxLength(50)
                    .IsUnicode(false);

                entity.Property(e => e.ProductCode)
                    .IsRequired()
                    .HasMaxLength(200)
                    .IsUnicode(false);

                entity.Property(e => e.ProductType)
                    .IsRequired()
                    .HasMaxLength(1)
                    .IsUnicode(false)
                    .IsFixedLength();

                entity.Property(e => e.UpdDate)
                    .HasColumnType("datetime")
                    .HasDefaultValueSql("(getdate())");

                entity.HasOne(d => d.PromotionTier)
                    .WithOne(p => p.Action)
                    .HasForeignKey<Action>(d => d.PromotionTierId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("Action_FK");
            });

            modelBuilder.Entity<Brand>(entity =>
            {
                entity.HasIndex(e => e.BrandCode)
                    .HasName("Brand_UN")
                    .IsUnique();

                entity.HasIndex(e => e.Username)
                    .HasName("UNIQUE_Brand")
                    .IsUnique();

                entity.Property(e => e.BrandId).HasDefaultValueSql("(newid())");

                entity.Property(e => e.Address).HasMaxLength(100);

                entity.Property(e => e.BrandCode)
                    .HasMaxLength(30)
                    .IsUnicode(false);

                entity.Property(e => e.BrandName).HasMaxLength(50);

                entity.Property(e => e.CompanyName).HasMaxLength(50);

                entity.Property(e => e.Description).HasMaxLength(100);

                entity.Property(e => e.ImgUrl)
                    .HasMaxLength(2048)
                    .IsUnicode(false);

                entity.Property(e => e.InsDate)
                    .HasColumnType("datetime")
                    .HasDefaultValueSql("(getdate())");

                entity.Property(e => e.PhoneNumber)
                    .HasMaxLength(11)
                    .IsUnicode(false);

                entity.Property(e => e.UpdDate)
                    .HasColumnType("datetime")
                    .HasDefaultValueSql("(getdate())");

                entity.Property(e => e.Username)
                    .IsRequired()
                    .HasMaxLength(25)
                    .IsUnicode(false);

                entity.HasOne(d => d.UsernameNavigation)
                    .WithOne(p => p.Brand)
                    .HasForeignKey<Brand>(d => d.Username)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK_Brand_Account");
            });

            modelBuilder.Entity<Channel>(entity =>
            {
                entity.Property(e => e.ChannelId).HasDefaultValueSql("(newid())");

                entity.Property(e => e.ChannelCode)
                    .HasMaxLength(20)
                    .IsUnicode(false);

                entity.Property(e => e.ChannelName).HasMaxLength(50);

                entity.Property(e => e.Group).HasColumnType("numeric(6, 0)");

                entity.Property(e => e.InsDate)
                    .HasColumnType("datetime")
                    .HasDefaultValueSql("(getdate())");

                entity.Property(e => e.UpdDate)
                    .HasColumnType("datetime")
                    .HasDefaultValueSql("(getdate())");

                entity.HasOne(d => d.Brand)
                    .WithMany(p => p.Channel)
                    .HasForeignKey(d => d.BrandId)
                    .HasConstraintName("FK_Channel_Brand");
            });

            modelBuilder.Entity<ConditionGroup>(entity =>
            {
                entity.Property(e => e.ConditionGroupId).HasDefaultValueSql("(newid())");

                entity.Property(e => e.GroupNo).HasColumnType("decimal(5, 0)");

                entity.Property(e => e.InsDate)
                    .HasColumnType("datetime")
                    .HasDefaultValueSql("(getdate())");

                entity.Property(e => e.NextOperator)
                    .HasMaxLength(1)
                    .IsUnicode(false)
                    .IsFixedLength();

                entity.Property(e => e.Summary)
                    .HasMaxLength(4000)
                    .IsUnicode(false);

                entity.Property(e => e.UpdDate)
                    .HasColumnType("datetime")
                    .HasDefaultValueSql("(getdate())");

                entity.HasOne(d => d.ConditionRule)
                    .WithMany(p => p.ConditionGroup)
                    .HasForeignKey(d => d.ConditionRuleId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK_ConditionGroup_ConditionRule");
            });

            modelBuilder.Entity<ConditionRule>(entity =>
            {
                entity.Property(e => e.ConditionRuleId).HasDefaultValueSql("(newid())");

                entity.Property(e => e.Description).HasMaxLength(200);

                entity.Property(e => e.InsDate)
                    .HasColumnType("datetime")
                    .HasDefaultValueSql("(getdate())");

                entity.Property(e => e.RuleName)
                    .IsRequired()
                    .HasMaxLength(50);

                entity.Property(e => e.UpdDate)
                    .HasColumnType("datetime")
                    .HasDefaultValueSql("(getdate())");

                entity.HasOne(d => d.Brand)
                    .WithMany(p => p.ConditionRule)
                    .HasForeignKey(d => d.BrandId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK_ConditionRule_Brand");
            });

            modelBuilder.Entity<Holiday>(entity =>
            {
                entity.Property(e => e.HolidayId).HasDefaultValueSql("(newid())");

                entity.Property(e => e.Date).HasColumnType("date");

                entity.Property(e => e.HolidayName).HasMaxLength(100);

                entity.Property(e => e.InsDate)
                    .HasColumnType("datetime")
                    .HasDefaultValueSql("(getdate())");

                entity.Property(e => e.Rank)
                    .HasMaxLength(5)
                    .IsUnicode(false);

                entity.Property(e => e.UpdDate)
                    .HasColumnType("datetime")
                    .HasDefaultValueSql("(getdate())");
            });

            modelBuilder.Entity<Membership>(entity =>
            {
                entity.Property(e => e.MembershipId).HasDefaultValueSql("(newid())");

                entity.Property(e => e.Email)
                    .HasMaxLength(62)
                    .IsUnicode(false);

                entity.Property(e => e.Fullname).HasMaxLength(50);

                entity.Property(e => e.InsDate)
                    .HasColumnType("datetime")
                    .HasDefaultValueSql("(getdate())");

                entity.Property(e => e.MembershipCode)
                    .HasMaxLength(20)
                    .IsUnicode(false);

                entity.Property(e => e.PhoneNumber)
                    .HasMaxLength(11)
                    .IsUnicode(false);

                entity.Property(e => e.UpdDate)
                    .HasColumnType("datetime")
                    .HasDefaultValueSql("(getdate())");
            });

            modelBuilder.Entity<MembershipAction>(entity =>
            {
                entity.HasIndex(e => e.PromotionTierId)
                    .HasName("IX_MembershipAction")
                    .IsUnique();

                entity.Property(e => e.MembershipActionId).HasDefaultValueSql("(newid())");

                entity.Property(e => e.ActionType)
                    .IsRequired()
                    .HasMaxLength(1)
                    .IsUnicode(false)
                    .IsFixedLength();

                entity.Property(e => e.BonusPoint).HasColumnType("decimal(10, 2)");

                entity.Property(e => e.DiscountType)
                    .IsRequired()
                    .HasMaxLength(2)
                    .IsUnicode(false)
                    .IsFixedLength();

                entity.Property(e => e.GiftName).HasMaxLength(50);

                entity.Property(e => e.GiftProductCode)
                    .HasMaxLength(50)
                    .IsUnicode(false);

                entity.Property(e => e.GiftQuantity).HasColumnType("decimal(6, 0)");

                entity.Property(e => e.GiftVoucherCode)
                    .HasMaxLength(100)
                    .IsUnicode(false);

                entity.Property(e => e.InsDate)
                    .HasColumnType("datetime")
                    .HasDefaultValueSql("(getdate())");

                entity.Property(e => e.UpdDate)
                    .HasColumnType("datetime")
                    .HasDefaultValueSql("(getdate())");

                entity.HasOne(d => d.PromotionTier)
                    .WithOne(p => p.MembershipAction)
                    .HasForeignKey<MembershipAction>(d => d.PromotionTierId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("MembershipAction_FK");
            });

            modelBuilder.Entity<MembershipCondition>(entity =>
            {
                entity.Property(e => e.MembershipConditionId).HasDefaultValueSql("(newid())");

                entity.Property(e => e.InsDate)
                    .HasColumnType("datetime")
                    .HasDefaultValueSql("(getdate())");

                entity.Property(e => e.MembershipLevel).HasMaxLength(50);

                entity.Property(e => e.NextOperator)
                    .IsRequired()
                    .HasMaxLength(1)
                    .IsUnicode(false)
                    .IsFixedLength();

                entity.Property(e => e.UpdDate)
                    .HasColumnType("datetime")
                    .HasDefaultValueSql("(getdate())");

                entity.HasOne(d => d.ConditionGroup)
                    .WithMany(p => p.MembershipCondition)
                    .HasForeignKey(d => d.ConditionGroupId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK_MembershipCondition_ConditionGroup");
            });

            modelBuilder.Entity<OrderCondition>(entity =>
            {
                entity.Property(e => e.OrderConditionId).HasDefaultValueSql("(newid())");

                entity.Property(e => e.Amount).HasColumnType("decimal(10, 0)");

                entity.Property(e => e.AmountOperator)
                    .IsRequired()
                    .HasMaxLength(1)
                    .IsUnicode(false)
                    .IsFixedLength();

                entity.Property(e => e.InsDate)
                    .HasColumnType("datetime")
                    .HasDefaultValueSql("(getdate())");

                entity.Property(e => e.NextOperator)
                    .HasMaxLength(1)
                    .IsUnicode(false)
                    .IsFixedLength();

                entity.Property(e => e.Quantity).HasColumnType("decimal(6, 0)");

                entity.Property(e => e.QuantityOperator)
                    .IsRequired()
                    .HasMaxLength(1)
                    .IsUnicode(false)
                    .IsFixedLength();

                entity.Property(e => e.UpdDate)
                    .HasColumnType("datetime")
                    .HasDefaultValueSql("(getdate())");

                entity.HasOne(d => d.ConditionGroup)
                    .WithMany(p => p.OrderCondition)
                    .HasForeignKey(d => d.ConditionGroupId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK_OrderCondition_ConditionGroup1");
            });

            modelBuilder.Entity<ProductCondition>(entity =>
            {
                entity.Property(e => e.ProductConditionId).HasDefaultValueSql("(newid())");

                entity.Property(e => e.InsDate)
                    .HasColumnType("datetime")
                    .HasDefaultValueSql("(getdate())");

                entity.Property(e => e.NextOperator)
                    .IsRequired()
                    .HasMaxLength(1)
                    .IsUnicode(false)
                    .IsFixedLength();

                entity.Property(e => e.ParentCode).HasMaxLength(50);

                entity.Property(e => e.ProductCode)
                    .IsRequired()
                    .HasMaxLength(50)
                    .IsUnicode(false);

                entity.Property(e => e.ProductConditionType)
                    .IsRequired()
                    .HasMaxLength(1)
                    .IsUnicode(false)
                    .IsFixedLength();

                entity.Property(e => e.ProductName)
                    .IsRequired()
                    .HasMaxLength(100);

                entity.Property(e => e.ProductQuantity).HasColumnType("decimal(6, 0)");

                entity.Property(e => e.ProductType)
                    .IsRequired()
                    .HasMaxLength(1)
                    .IsUnicode(false)
                    .IsFixedLength();

                entity.Property(e => e.QuantityOperator)
                    .IsRequired()
                    .HasMaxLength(1)
                    .IsUnicode(false)
                    .IsFixedLength();

                entity.Property(e => e.UpdDate)
                    .HasColumnType("datetime")
                    .HasDefaultValueSql("(getdate())");

                entity.HasOne(d => d.ConditionGroup)
                    .WithMany(p => p.ProductCondition)
                    .HasForeignKey(d => d.ConditionGroupId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK_ProductCondition_ConditionGroup");
            });

            modelBuilder.Entity<Promotion>(entity =>
            {
                entity.Property(e => e.PromotionId).HasDefaultValueSql("(newid())");

                entity.Property(e => e.ActionType)
                    .HasMaxLength(1)
                    .IsUnicode(false)
                    .IsFixedLength();

                entity.Property(e => e.ApplyBy)
                    .HasMaxLength(1)
                    .IsUnicode(false)
                    .IsFixedLength();

                entity.Property(e => e.DayFilter)
                    .HasMaxLength(10)
                    .IsUnicode(false);

                entity.Property(e => e.Description).HasMaxLength(4000);

                entity.Property(e => e.DiscountType)
                    .HasMaxLength(2)
                    .IsUnicode(false);

                entity.Property(e => e.EndDate).HasColumnType("datetime");

                entity.Property(e => e.Exclusive)
                    .HasMaxLength(1)
                    .IsUnicode(false)
                    .IsFixedLength();

                entity.Property(e => e.ForHoliday)
                    .HasMaxLength(1)
                    .IsUnicode(false)
                    .IsFixedLength()
                    .HasDefaultValueSql("((0))");

                entity.Property(e => e.ForMembership)
                    .HasMaxLength(1)
                    .IsUnicode(false)
                    .IsFixedLength()
                    .HasDefaultValueSql("((0))");

                entity.Property(e => e.Gender)
                    .HasMaxLength(1)
                    .IsUnicode(false)
                    .IsFixedLength();

                entity.Property(e => e.HourFilter)
                    .HasMaxLength(10)
                    .IsUnicode(false);

                entity.Property(e => e.ImgUrl)
                    .HasMaxLength(2048)
                    .IsUnicode(false);

                entity.Property(e => e.InsDate)
                    .HasColumnType("datetime")
                    .HasDefaultValueSql("(getdate())");

                entity.Property(e => e.PaymentMethod)
                    .HasMaxLength(10)
                    .IsUnicode(false);

                entity.Property(e => e.PromotionCode)
                    .HasMaxLength(6)
                    .IsUnicode(false);

                entity.Property(e => e.PromotionLevel)
                    .HasMaxLength(1)
                    .IsUnicode(false)
                    .IsFixedLength();

                entity.Property(e => e.PromotionName).HasMaxLength(100);

                entity.Property(e => e.PromotionType)
                    .HasMaxLength(1)
                    .IsUnicode(false)
                    .IsFixedLength();

                entity.Property(e => e.Rank)
                    .HasMaxLength(3)
                    .IsUnicode(false)
                    .HasDefaultValueSql("((0))");

                entity.Property(e => e.SaleMode)
                    .HasMaxLength(1)
                    .IsUnicode(false)
                    .IsFixedLength();

                entity.Property(e => e.StartDate).HasColumnType("datetime");

                entity.Property(e => e.Status)
                    .HasMaxLength(1)
                    .IsUnicode(false)
                    .IsFixedLength()
                    .HasDefaultValueSql("((1))");

                entity.Property(e => e.UpdDate)
                    .HasColumnType("datetime")
                    .HasDefaultValueSql("(getdate())");

                entity.HasOne(d => d.Brand)
                    .WithMany(p => p.Promotion)
                    .HasForeignKey(d => d.BrandId)
                    .HasConstraintName("FK_Promotion_Brand");
            });

            modelBuilder.Entity<PromotionChannelMapping>(entity =>
            {
                entity.HasKey(e => e.VoucherChannelId)
                    .HasName("PK_VoucherChannel");

                entity.Property(e => e.VoucherChannelId).HasDefaultValueSql("(newid())");

                entity.Property(e => e.InsDate)
                    .HasColumnType("datetime")
                    .HasDefaultValueSql("(getdate())");

                entity.Property(e => e.UpdDate)
                    .HasColumnType("datetime")
                    .HasDefaultValueSql("(getdate())");

                entity.HasOne(d => d.Channel)
                    .WithMany(p => p.PromotionChannelMapping)
                    .HasForeignKey(d => d.ChannelId)
                    .HasConstraintName("FK_VoucherChannel_Channel");

                entity.HasOne(d => d.Promotion)
                    .WithMany(p => p.PromotionChannelMapping)
                    .HasForeignKey(d => d.PromotionId)
                    .HasConstraintName("FK_VoucherChannel_Promotion");
            });

            modelBuilder.Entity<PromotionStoreMapping>(entity =>
            {
                entity.Property(e => e.Id).HasDefaultValueSql("(newid())");

                entity.Property(e => e.InsDate)
                    .HasColumnType("datetime")
                    .HasDefaultValueSql("(getdate())");

                entity.Property(e => e.UpdDate)
                    .HasColumnType("datetime")
                    .HasDefaultValueSql("(getdate())");

                entity.HasOne(d => d.Promotion)
                    .WithMany(p => p.PromotionStoreMapping)
                    .HasForeignKey(d => d.PromotionId)
                    .HasConstraintName("FK_PromotionStoreMapping_Promotion");

                entity.HasOne(d => d.Store)
                    .WithMany(p => p.PromotionStoreMapping)
                    .HasForeignKey(d => d.StoreId)
                    .HasConstraintName("FK_PromotionStoreMapping_Store");
            });

            modelBuilder.Entity<PromotionTier>(entity =>
            {
                entity.HasIndex(e => e.ConditionRuleId)
                    .HasName("ConditionRule")
                    .IsUnique();

                entity.Property(e => e.PromotionTierId).HasDefaultValueSql("(newid())");

                entity.Property(e => e.InsDate)
                    .HasColumnType("datetime")
                    .HasDefaultValueSql("(getdate())");

                entity.Property(e => e.Summary)
                    .HasMaxLength(4000)
                    .IsUnicode(false);

                entity.Property(e => e.UpdDate)
                    .HasColumnType("datetime")
                    .HasDefaultValueSql("(getdate())");

                entity.HasOne(d => d.ConditionRule)
                    .WithOne(p => p.PromotionTier)
                    .HasForeignKey<PromotionTier>(d => d.ConditionRuleId)
                    .HasConstraintName("FK_PromotionTier_ConditionRule");

                entity.HasOne(d => d.Promotion)
                    .WithMany(p => p.PromotionTier)
                    .HasForeignKey(d => d.PromotionId)
                    .HasConstraintName("FK_PromotionTier_Promotion");
            });

            modelBuilder.Entity<RoleEntity>(entity =>
            {
                entity.Property(e => e.InsDate)
                    .HasColumnType("datetime")
                    .HasDefaultValueSql("(getdate())");

                entity.Property(e => e.Name)
                    .HasMaxLength(20)
                    .IsUnicode(false);

                entity.Property(e => e.UpdDate)
                    .HasColumnType("datetime")
                    .HasDefaultValueSql("(getdate())");
            });

            modelBuilder.Entity<Store>(entity =>
            {
                entity.Property(e => e.StoreId).HasDefaultValueSql("(newid())");

                entity.Property(e => e.Group).HasColumnType("numeric(6, 0)");

                entity.Property(e => e.InsDate)
                    .HasColumnType("datetime")
                    .HasDefaultValueSql("(getdate())");

                entity.Property(e => e.StoreCode)
                    .HasMaxLength(20)
                    .IsUnicode(false);

                entity.Property(e => e.StoreName).HasMaxLength(50);

                entity.Property(e => e.UpdDate)
                    .HasColumnType("datetime")
                    .HasDefaultValueSql("(getdate())");

                entity.HasOne(d => d.Brand)
                    .WithMany(p => p.Store)
                    .HasForeignKey(d => d.BrandId)
                    .HasConstraintName("FK_Store_Brand");
            });

            modelBuilder.Entity<Voucher>(entity =>
            {
                entity.HasKey(e => new { e.VoucherId, e.VoucherCode })
                    .HasName("PK_Voucher_1");

                entity.Property(e => e.VoucherId).HasDefaultValueSql("(newid())");

                entity.Property(e => e.VoucherCode)
                    .HasMaxLength(50)
                    .IsUnicode(false);

                entity.Property(e => e.InsDate)
                    .HasColumnType("datetime")
                    .HasDefaultValueSql("(getdate())");

                entity.Property(e => e.RedempedDate).HasColumnType("datetime");

                entity.Property(e => e.UpdDate)
                    .HasColumnType("datetime")
                    .HasDefaultValueSql("(getdate())");

                entity.Property(e => e.UsedDate).HasColumnType("datetime");

                entity.HasOne(d => d.Channel)
                    .WithMany(p => p.Voucher)
                    .HasForeignKey(d => d.ChannelId)
                    .HasConstraintName("FK_Voucher_Channel");

                entity.HasOne(d => d.Membership)
                    .WithMany(p => p.Voucher)
                    .HasForeignKey(d => d.MembershipId)
                    .HasConstraintName("FK_Voucher_Membership");

                entity.HasOne(d => d.VoucherGroup)
                    .WithMany(p => p.Voucher)
                    .HasForeignKey(d => d.VoucherGroupId)
                    .HasConstraintName("FK_Voucher_VoucherGroup");
            });

            modelBuilder.Entity<VoucherGroup>(entity =>
            {
                entity.HasIndex(e => e.PromotionId)
                    .HasName("IX_UNIQUE_FILTERED")
                    .IsUnique()
                    .HasFilter("([PromotionId] IS NOT NULL)");

                entity.Property(e => e.VoucherGroupId).HasDefaultValueSql("(newid())");

                entity.Property(e => e.InsDate)
                    .HasColumnType("datetime")
                    .HasDefaultValueSql("(getdate())");

                entity.Property(e => e.LimitInDayCount).HasColumnType("decimal(10, 0)");

                entity.Property(e => e.PublicDate).HasColumnType("datetime");

                entity.Property(e => e.Quantity)
                    .HasColumnType("decimal(10, 0)")
                    .HasDefaultValueSql("((1))");

                entity.Property(e => e.RedempedQuantity)
                    .HasColumnType("decimal(10, 0)")
                    .HasDefaultValueSql("((0))");

                entity.Property(e => e.UpdDate)
                    .HasColumnType("datetime")
                    .HasDefaultValueSql("(getdate())");

                entity.Property(e => e.UsedQuantity)
                    .HasColumnType("decimal(10, 0)")
                    .HasDefaultValueSql("((0))");

                entity.Property(e => e.VoucherName).HasMaxLength(50);

                entity.Property(e => e.VoucherType)
                    .HasMaxLength(1)
                    .IsUnicode(false)
                    .IsFixedLength();

                entity.HasOne(d => d.Brand)
                    .WithMany(p => p.VoucherGroup)
                    .HasForeignKey(d => d.BrandId)
                    .HasConstraintName("FK_VoucherGroup_Brand");

                entity.HasOne(d => d.Promotion)
                    .WithOne(p => p.VoucherGroup)
                    .HasForeignKey<VoucherGroup>(d => d.PromotionId)
                    .HasConstraintName("FK_VoucherGroup_Promotion");
            });

            OnModelCreatingPartial(modelBuilder);
        }

        partial void OnModelCreatingPartial(ModelBuilder modelBuilder);
    }
}
