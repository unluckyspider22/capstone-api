using Microsoft.EntityFrameworkCore;

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
        public virtual DbSet<ConditionRule> ConditionRule { get; set; }
        public virtual DbSet<Holiday> Holiday { get; set; }
        public virtual DbSet<Membership> Membership { get; set; }
        public virtual DbSet<MembershipAction> MembershipAction { get; set; }
        public virtual DbSet<MembershipCondition> MembershipCondition { get; set; }
        public virtual DbSet<OrderCondition> OrderCondition { get; set; }
        public virtual DbSet<ProductCondition> ProductCondition { get; set; }
        public virtual DbSet<Promotion> Promotion { get; set; }
        public virtual DbSet<PromotionStoreMapping> PromotionStoreMapping { get; set; }
        public virtual DbSet<PromotionTier> PromotionTier { get; set; }
        public virtual DbSet<RoleEntity> Role { get; set; }
        public virtual DbSet<Store> Store { get; set; }
        public virtual DbSet<Voucher> Voucher { get; set; }
        public virtual DbSet<VoucherChannel> VoucherChannel { get; set; }
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

                entity.Property(e => e.DelFlg)
                    .HasMaxLength(1)
                    .IsUnicode(false)
                    .IsFixedLength()
                    .HasDefaultValueSql("((0))");

                entity.Property(e => e.Email)
                    .HasMaxLength(62)
                    .IsUnicode(false);

                entity.Property(e => e.Fullname).HasMaxLength(50);

                entity.Property(e => e.InsDate)
                    .HasColumnType("datetime")
                    .HasDefaultValueSql("(getdate())");

                entity.Property(e => e.Password)
                    .HasMaxLength(60)
                    .IsUnicode(false);

                entity.Property(e => e.PhoneNumber)
                    .HasMaxLength(11)
                    .IsUnicode(false);

                entity.Property(e => e.UpdDate)
                    .HasColumnType("datetime")
                    .HasDefaultValueSql("(getdate())");

                entity.HasOne(d => d.Brand)
                    .WithMany(p => p.Account)
                    .HasForeignKey(d => d.BrandId)
                    .HasConstraintName("FK_Account_Brand");

                entity.HasOne(d => d.Role)
                    .WithMany(p => p.Account)
                    .HasForeignKey(d => d.RoleId)
                    .HasConstraintName("FK_Account_Role");
            });

            modelBuilder.Entity<Action>(entity =>
            {
                entity.Property(e => e.ActionId).ValueGeneratedNever();

                entity.Property(e => e.ActionType)
                    .HasMaxLength(1)
                    .IsUnicode(false)
                    .IsFixedLength();

                entity.Property(e => e.DelFlg)
                    .HasMaxLength(1)
                    .IsUnicode(false)
                    .IsFixedLength()
                    .HasDefaultValueSql("((0))");

                entity.Property(e => e.DiscountAmount).HasColumnType("decimal(10, 2)");

                entity.Property(e => e.DiscountPercentage).HasColumnType("decimal(10, 2)");

                entity.Property(e => e.DiscountQuantity).HasColumnType("decimal(10, 0)");

                entity.Property(e => e.FixedPrice).HasColumnType("decimal(10, 2)");

                entity.Property(e => e.GroupNo)
                    .HasMaxLength(2)
                    .IsUnicode(false);

                entity.Property(e => e.InsDate)
                    .HasColumnType("datetime")
                    .HasDefaultValueSql("(getdate())");

                entity.Property(e => e.MaxAmount).HasColumnType("decimal(10, 2)");

                entity.Property(e => e.ProductCode)
                    .HasMaxLength(30)
                    .IsUnicode(false);

                entity.Property(e => e.UpdDate)
                    .HasColumnType("datetime")
                    .HasDefaultValueSql("(getdate())");
            });

            modelBuilder.Entity<Brand>(entity =>
            {
                entity.Property(e => e.BrandId).ValueGeneratedNever();

                entity.Property(e => e.Address).HasMaxLength(100);

                entity.Property(e => e.BrandCode)
                    .HasMaxLength(30)
                    .IsUnicode(false);

                entity.Property(e => e.BrandName).HasMaxLength(50);

                entity.Property(e => e.CompanyName).HasMaxLength(50);

                entity.Property(e => e.DelFlg)
                    .HasMaxLength(1)
                    .IsUnicode(false)
                    .IsFixedLength()
                    .HasDefaultValueSql("((0))");

                entity.Property(e => e.Description).HasMaxLength(100);

                entity.Property(e => e.ImgUrl)
                    .HasMaxLength(100)
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
                    .HasMaxLength(25)
                    .IsUnicode(false);

                entity.HasOne(d => d.UsernameNavigation)
                    .WithMany(p => p.BrandNavigation)
                    .HasForeignKey(d => d.Username)
                    .HasConstraintName("FK_Brand_Account");
            });

            modelBuilder.Entity<Channel>(entity =>
            {
                entity.Property(e => e.ChannelId).ValueGeneratedNever();

                entity.Property(e => e.ChannelCode)
                    .HasMaxLength(20)
                    .IsUnicode(false);

                entity.Property(e => e.ChannelName).HasMaxLength(50);

                entity.Property(e => e.DelFlg)
                    .HasMaxLength(1)
                    .IsUnicode(false)
                    .IsFixedLength()
                    .HasDefaultValueSql("((0))");

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

            modelBuilder.Entity<ConditionRule>(entity =>
            {
                entity.Property(e => e.ConditionRuleId).ValueGeneratedNever();

                entity.Property(e => e.DelFlg)
                    .HasMaxLength(1)
                    .IsUnicode(false)
                    .IsFixedLength()
                    .HasDefaultValueSql("((0))");

                entity.Property(e => e.Description).HasMaxLength(200);

                entity.Property(e => e.InsDate)
                    .HasColumnType("datetime")
                    .HasDefaultValueSql("(getdate())");

                entity.Property(e => e.RuleName).HasMaxLength(30);

                entity.Property(e => e.UpdDate)
                    .HasColumnType("datetime")
                    .HasDefaultValueSql("(getdate())");

                entity.HasOne(d => d.Brand)
                    .WithMany(p => p.ConditionRule)
                    .HasForeignKey(d => d.BrandId)
                    .HasConstraintName("FK_ConditionRule_Brand");
            });

            modelBuilder.Entity<Holiday>(entity =>
            {
                entity.Property(e => e.HolidayId).ValueGeneratedNever();

                entity.Property(e => e.Date).HasColumnType("date");

                entity.Property(e => e.DelFlg)
                    .HasMaxLength(1)
                    .IsUnicode(false)
                    .IsFixedLength()
                    .HasDefaultValueSql("((0))");

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
                entity.Property(e => e.MembershipId).ValueGeneratedNever();

                entity.Property(e => e.DelFlg)
                    .HasMaxLength(1)
                    .IsUnicode(false)
                    .IsFixedLength()
                    .HasDefaultValueSql("((0))");

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
                    .HasMaxLength(50)
                    .IsUnicode(false);

                entity.Property(e => e.UpdDate)
                    .HasColumnType("datetime")
                    .HasDefaultValueSql("(getdate())");
            });

            modelBuilder.Entity<MembershipAction>(entity =>
            {
                entity.Property(e => e.MembershipActionId).ValueGeneratedNever();

                entity.Property(e => e.ActionType)
                    .HasMaxLength(1)
                    .IsUnicode(false)
                    .IsFixedLength();

                entity.Property(e => e.BonusPoint).HasColumnType("decimal(10, 2)");

                entity.Property(e => e.DelFlg)
                    .HasMaxLength(1)
                    .IsUnicode(false)
                    .IsFixedLength()
                    .HasDefaultValueSql("((0))");

                entity.Property(e => e.GiftName).HasMaxLength(50);

                entity.Property(e => e.GiftProductCode)
                    .HasMaxLength(20)
                    .IsUnicode(false);

                entity.Property(e => e.GiftVoucherCode)
                    .HasMaxLength(20)
                    .IsUnicode(false);

                entity.Property(e => e.GroupNo)
                    .HasMaxLength(2)
                    .IsUnicode(false);

                entity.Property(e => e.InsDate)
                    .HasColumnType("datetime")
                    .HasDefaultValueSql("(getdate())");

                entity.Property(e => e.UpdDate)
                    .HasColumnType("datetime")
                    .HasDefaultValueSql("(getdate())");
            });

            modelBuilder.Entity<MembershipCondition>(entity =>
            {
                entity.Property(e => e.MembershipConditionId).ValueGeneratedNever();

                entity.Property(e => e.DelFlg)
                    .HasMaxLength(1)
                    .IsUnicode(false)
                    .IsFixedLength()
                    .HasDefaultValueSql("((0))");

                entity.Property(e => e.GroupNo)
                    .HasMaxLength(2)
                    .IsUnicode(false);

                entity.Property(e => e.InsDate)
                    .HasColumnType("datetime")
                    .HasDefaultValueSql("(getdate())");

                entity.Property(e => e.MembershipLevel).HasMaxLength(10);

                entity.Property(e => e.UpdDate)
                    .HasColumnType("datetime")
                    .HasDefaultValueSql("(getdate())");

                entity.HasOne(d => d.ConditionRule)
                    .WithMany(p => p.MembershipCondition)
                    .HasForeignKey(d => d.ConditionRuleId)
                    .HasConstraintName("FK_MembershipCondition_ConditionRule");
            });

            modelBuilder.Entity<OrderCondition>(entity =>
            {
                entity.Property(e => e.OrderConditionId).ValueGeneratedNever();

                entity.Property(e => e.DelFlg)
                    .HasMaxLength(1)
                    .IsUnicode(false)
                    .IsFixedLength()
                    .HasDefaultValueSql("((0))");

                entity.Property(e => e.GroupNo)
                    .HasMaxLength(10)
                    .IsUnicode(false);

                entity.Property(e => e.InsDate)
                    .HasColumnType("datetime")
                    .HasDefaultValueSql("(getdate())");

                entity.Property(e => e.MaxAmount).HasColumnType("decimal(10, 2)");

                entity.Property(e => e.MaxQuantity).HasColumnType("decimal(10, 2)");

                entity.Property(e => e.MinAmount).HasColumnType("decimal(10, 2)");

                entity.Property(e => e.MinQuantity).HasColumnType("decimal(10, 2)");

                entity.Property(e => e.ProductCode)
                    .HasMaxLength(20)
                    .IsUnicode(false);

                entity.Property(e => e.ProductType)
                    .HasMaxLength(1)
                    .IsUnicode(false)
                    .IsFixedLength();

                entity.Property(e => e.UpdDate)
                    .HasColumnType("datetime")
                    .HasDefaultValueSql("(getdate())");

                entity.HasOne(d => d.ConditionRule)
                    .WithMany(p => p.OrderCondition)
                    .HasForeignKey(d => d.ConditionRuleId)
                    .HasConstraintName("FK_OrderCondition_ConditionRule");
            });

            modelBuilder.Entity<ProductCondition>(entity =>
            {
                entity.Property(e => e.ProductConditionId).ValueGeneratedNever();

                entity.Property(e => e.DelFlg)
                    .HasMaxLength(1)
                    .IsUnicode(false)
                    .IsFixedLength()
                    .HasDefaultValueSql("((0))");

                entity.Property(e => e.GroupNo)
                    .HasMaxLength(2)
                    .IsUnicode(false);

                entity.Property(e => e.InsDate)
                    .HasColumnType("datetime")
                    .HasDefaultValueSql("(getdate())");

                entity.Property(e => e.ProductCode)
                    .HasMaxLength(20)
                    .IsUnicode(false);

                entity.Property(e => e.ProductConditionType)
                    .HasMaxLength(1)
                    .IsUnicode(false)
                    .IsFixedLength();

                entity.Property(e => e.ProductName).HasMaxLength(100);

                entity.Property(e => e.ProductQuantity).HasColumnType("decimal(10, 2)");

                entity.Property(e => e.ProductTag).HasMaxLength(20);

                entity.Property(e => e.ProductType)
                    .HasMaxLength(1)
                    .IsUnicode(false)
                    .IsFixedLength();

                entity.Property(e => e.UpdDate)
                    .HasColumnType("datetime")
                    .HasDefaultValueSql("(getdate())");

                entity.HasOne(d => d.ConditionRule)
                    .WithMany(p => p.ProductCondition)
                    .HasForeignKey(d => d.ConditionRuleId)
                    .HasConstraintName("FK_ProductCondition_ConditionRule");
            });

            modelBuilder.Entity<Promotion>(entity =>
            {
                entity.Property(e => e.PromotionId).ValueGeneratedNever();

                entity.Property(e => e.ApplyBy)
                    .HasMaxLength(1)
                    .IsUnicode(false)
                    .IsFixedLength();

                entity.Property(e => e.ConditionLevel)
                    .HasMaxLength(1)
                    .IsUnicode(false)
                    .IsFixedLength();

                entity.Property(e => e.DayFilter)
                    .HasMaxLength(3)
                    .IsUnicode(false);

                entity.Property(e => e.DelFlg)
                    .HasMaxLength(1)
                    .IsUnicode(false)
                    .IsFixedLength()
                    .HasDefaultValueSql("((0))");

                entity.Property(e => e.Description).HasMaxLength(200);

                entity.Property(e => e.EndDate).HasColumnType("datetime");

                entity.Property(e => e.Exclusive)
                    .HasMaxLength(1)
                    .IsUnicode(false)
                    .IsFixedLength();

                entity.Property(e => e.ForHoliday)
                    .HasMaxLength(1)
                    .IsUnicode(false)
                    .IsFixedLength();

                entity.Property(e => e.ForMembership)
                    .HasMaxLength(1)
                    .IsUnicode(false)
                    .IsFixedLength();

                entity.Property(e => e.Gender)
                    .HasMaxLength(1)
                    .IsUnicode(false)
                    .IsFixedLength();

                entity.Property(e => e.HourFilter)
                    .HasMaxLength(3)
                    .IsUnicode(false);

                entity.Property(e => e.ImgUrl)
                    .HasMaxLength(2048)
                    .IsUnicode(false);

                entity.Property(e => e.InsDate)
                    .HasColumnType("datetime")
                    .HasDefaultValueSql("(getdate())");

                entity.Property(e => e.IsActive)
                    .HasMaxLength(1)
                    .IsUnicode(false)
                    .IsFixedLength();

                entity.Property(e => e.LimitCount).HasColumnType("decimal(10, 0)");

                entity.Property(e => e.PaymentMethod)
                    .HasMaxLength(10)
                    .IsUnicode(false);

                entity.Property(e => e.PromotionCode)
                    .HasMaxLength(20)
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
                    .IsUnicode(false);

                entity.Property(e => e.SaleMode)
                    .HasMaxLength(1)
                    .IsUnicode(false)
                    .IsFixedLength();

                entity.Property(e => e.StartDate).HasColumnType("datetime");

                entity.Property(e => e.Status)
                    .HasMaxLength(1)
                    .IsUnicode(false)
                    .IsFixedLength();

                entity.Property(e => e.UpdDate)
                    .HasColumnType("datetime")
                    .HasDefaultValueSql("(getdate())");

                entity.HasOne(d => d.Brand)
                    .WithMany(p => p.Promotion)
                    .HasForeignKey(d => d.BrandId)
                    .HasConstraintName("FK_Promotion_Brand");
            });

            modelBuilder.Entity<PromotionStoreMapping>(entity =>
            {
                entity.Property(e => e.Id).ValueGeneratedNever();

                entity.Property(e => e.DelFlg)
                    .HasMaxLength(1)
                    .IsUnicode(false)
                    .IsFixedLength()
                    .HasDefaultValueSql("((0))");

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
                entity.Property(e => e.PromotionTierId).ValueGeneratedNever();

                entity.Property(e => e.DelFlg)
                    .HasMaxLength(1)
                    .IsUnicode(false)
                    .IsFixedLength()
                    .HasDefaultValueSql("((0))");

                entity.Property(e => e.InsDate)
                    .HasColumnType("datetime")
                    .HasDefaultValueSql("(getdate())");

                entity.Property(e => e.UpdDate)
                    .HasColumnType("datetime")
                    .HasDefaultValueSql("(getdate())");

                entity.HasOne(d => d.Action)
                    .WithMany(p => p.PromotionTier)
                    .HasForeignKey(d => d.ActionId)
                    .HasConstraintName("FK_PromotionTier_Action");

                entity.HasOne(d => d.ConditionRule)
                    .WithMany(p => p.PromotionTier)
                    .HasForeignKey(d => d.ConditionRuleId)
                    .HasConstraintName("FK_PromotionTier_ConditionRule");

                entity.HasOne(d => d.MembershipAction)
                    .WithMany(p => p.PromotionTier)
                    .HasForeignKey(d => d.MembershipActionId)
                    .HasConstraintName("FK_PromotionTier_MembershipAction");

                entity.HasOne(d => d.Promotion)
                    .WithMany(p => p.PromotionTier)
                    .HasForeignKey(d => d.PromotionId)
                    .HasConstraintName("FK_PromotionTier_Promotion");
            });

            modelBuilder.Entity<RoleEntity>(entity =>
            {
                entity.Property(e => e.DelFlg)
                    .HasMaxLength(1)
                    .IsUnicode(false)
                    .IsFixedLength()
                    .HasDefaultValueSql("((0))");

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
                entity.Property(e => e.StoreId).ValueGeneratedNever();

                entity.Property(e => e.DelFlg)
                    .HasMaxLength(1)
                    .IsUnicode(false)
                    .IsFixedLength()
                    .HasDefaultValueSql("((0))");

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
                entity.Property(e => e.VoucherId).ValueGeneratedNever();

                entity.Property(e => e.DelFlg)
                    .HasMaxLength(1)
                    .IsUnicode(false)
                    .IsFixedLength()
                    .HasDefaultValueSql("((0))");

                entity.Property(e => e.InsDate)
                    .HasColumnType("datetime")
                    .HasDefaultValueSql("(getdate())");

                entity.Property(e => e.RedempedDate).HasColumnType("datetime");

                entity.Property(e => e.UpdDate)
                    .HasColumnType("datetime")
                    .HasDefaultValueSql("(getdate())");

                entity.Property(e => e.UsedDate).HasColumnType("datetime");

                entity.Property(e => e.VoucherCode)
                    .HasMaxLength(50)
                    .IsUnicode(false);

                entity.HasOne(d => d.Membership)
                    .WithMany(p => p.Voucher)
                    .HasForeignKey(d => d.MembershipId)
                    .HasConstraintName("FK_Voucher_Membership");

                entity.HasOne(d => d.VoucherChannel)
                    .WithMany(p => p.Voucher)
                    .HasForeignKey(d => d.VoucherChannelId)
                    .HasConstraintName("FK_Voucher_VoucherChannel");
            });

            modelBuilder.Entity<VoucherChannel>(entity =>
            {
                entity.Property(e => e.VoucherChannelId).ValueGeneratedNever();

                entity.Property(e => e.DelFlg)
                    .HasMaxLength(1)
                    .IsUnicode(false)
                    .IsFixedLength()
                    .HasDefaultValueSql("((0))");

                entity.Property(e => e.InsDate)
                    .HasColumnType("datetime")
                    .HasDefaultValueSql("(getdate())");

                entity.Property(e => e.UpdDate)
                    .HasColumnType("datetime")
                    .HasDefaultValueSql("(getdate())");

                entity.HasOne(d => d.Channel)
                    .WithMany(p => p.VoucherChannel)
                    .HasForeignKey(d => d.ChannelId)
                    .HasConstraintName("FK_VoucherChannel_Channel");

                entity.HasOne(d => d.Promotion)
                    .WithMany(p => p.VoucherChannel)
                    .HasForeignKey(d => d.PromotionId)
                    .HasConstraintName("FK_VoucherChannel_Promotion");

                entity.HasOne(d => d.VoucherGroup)
                    .WithMany(p => p.VoucherChannel)
                    .HasForeignKey(d => d.VoucherGroupId)
                    .HasConstraintName("FK_VoucherChannel_VoucherGroup");
            });

            modelBuilder.Entity<VoucherGroup>(entity =>
            {
                entity.Property(e => e.VoucherGroupId).ValueGeneratedNever();

                entity.Property(e => e.DelFlg)
                    .HasMaxLength(1)
                    .IsUnicode(false)
                    .IsFixedLength()
                    .HasDefaultValueSql("((0))");

                entity.Property(e => e.InsDate)
                    .HasColumnType("datetime")
                    .HasDefaultValueSql("(getdate())");

                entity.Property(e => e.PublicDate).HasColumnType("datetime");

                entity.Property(e => e.Quantity).HasColumnType("decimal(10, 0)");

                entity.Property(e => e.RedempedQuantity).HasColumnType("decimal(10, 0)");

                entity.Property(e => e.Status)
                    .HasMaxLength(1)
                    .IsUnicode(false)
                    .IsFixedLength();

                entity.Property(e => e.UpdDate)
                    .HasColumnType("datetime")
                    .HasDefaultValueSql("(getdate())");

                entity.Property(e => e.UsedQuantity).HasColumnType("decimal(10, 0)");

                entity.HasOne(d => d.Brand)
                    .WithMany(p => p.VoucherGroup)
                    .HasForeignKey(d => d.BrandId)
                    .HasConstraintName("FK_VoucherGroup_Brand");
            });

            OnModelCreatingPartial(modelBuilder);
        }

        partial void OnModelCreatingPartial(ModelBuilder modelBuilder);
    }
}
