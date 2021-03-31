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
        public virtual DbSet<ActionProductMapping> ActionProductMapping { get; set; }
        public virtual DbSet<Brand> Brand { get; set; }
        public virtual DbSet<Channel> Channel { get; set; }
        public virtual DbSet<ConditionGroup> ConditionGroup { get; set; }
        public virtual DbSet<ConditionRule> ConditionRule { get; set; }
        public virtual DbSet<Device> Device { get; set; }
        public virtual DbSet<GameConfig> GameConfig { get; set; }
        public virtual DbSet<GameItems> GameItems { get; set; }
        public virtual DbSet<GameMaster> GameMaster { get; set; }
        public virtual DbSet<Holiday> Holiday { get; set; }
        public virtual DbSet<MemberLevel> MemberLevel { get; set; }
        public virtual DbSet<MemberLevelMapping> MemberLevelMapping { get; set; }
        public virtual DbSet<Membership> Membership { get; set; }
        public virtual DbSet<OrderCondition> OrderCondition { get; set; }
        public virtual DbSet<PostAction> PostAction { get; set; }
        public virtual DbSet<PostActionProductMapping> PostActionProductMapping { get; set; }
        public virtual DbSet<Product> Product { get; set; }
        public virtual DbSet<ProductCategory> ProductCategory { get; set; }
        public virtual DbSet<ProductCondition> ProductCondition { get; set; }
        public virtual DbSet<ProductConditionMapping> ProductConditionMapping { get; set; }
        public virtual DbSet<Promotion> Promotion { get; set; }
        public virtual DbSet<PromotionChannelMapping> PromotionChannelMapping { get; set; }
        public virtual DbSet<PromotionStoreMapping> PromotionStoreMapping { get; set; }
        public virtual DbSet<PromotionTier> PromotionTier { get; set; }
        public virtual DbSet<RoleEntity> Role { get; set; }
        public virtual DbSet<Store> Store { get; set; }
        public virtual DbSet<Transaction> Transaction { get; set; }
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
                    .IsRequired()
                    .HasMaxLength(62)
                    .IsUnicode(false);

                entity.Property(e => e.FirstName)
                    .IsRequired()
                    .HasMaxLength(50);

                entity.Property(e => e.ImgUrl)
                    .HasMaxLength(2048)
                    .IsUnicode(false);

                entity.Property(e => e.InsDate)
                    .HasColumnType("datetime")
                    .HasDefaultValueSql("(getdate())");

                entity.Property(e => e.LastName)
                    .IsRequired()
                    .HasMaxLength(50);

                entity.Property(e => e.Password)
                    .IsRequired()
                    .HasMaxLength(60)
                    .IsUnicode(false);

                entity.Property(e => e.PhoneNumber)
                    .IsRequired()
                    .HasMaxLength(11)
                    .IsUnicode(false);

                entity.Property(e => e.UpdDate)
                    .HasColumnType("datetime")
                    .HasDefaultValueSql("(getdate())");

                entity.HasOne(d => d.Role)
                    .WithMany(p => p.Account)
                    .HasForeignKey(d => d.RoleId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK_Account_Role");
            });

            modelBuilder.Entity<Action>(entity =>
            {
                entity.HasIndex(e => e.PromotionTierId)
                    .HasName("IX_Action")
                    .IsUnique();

                entity.Property(e => e.ActionId).HasDefaultValueSql("(newid())");

                entity.Property(e => e.BundlePrice).HasColumnType("decimal(10, 0)");

                entity.Property(e => e.DiscountAmount).HasColumnType("decimal(10, 0)");

                entity.Property(e => e.DiscountPercentage).HasColumnType("decimal(18, 0)");

                entity.Property(e => e.FixedPrice).HasColumnType("decimal(10, 0)");

                entity.Property(e => e.InsDate)
                    .HasColumnType("datetime")
                    .HasDefaultValueSql("(getdate())");

                entity.Property(e => e.LadderPrice).HasColumnType("decimal(10, 0)");

                entity.Property(e => e.MaxAmount).HasColumnType("decimal(10, 0)");

                entity.Property(e => e.MinPriceAfter).HasColumnType("decimal(10, 0)");

                entity.Property(e => e.Name).HasMaxLength(100);

                entity.Property(e => e.UpdDate)
                    .HasColumnType("datetime")
                    .HasDefaultValueSql("(getdate())");

                entity.HasOne(d => d.Brand)
                    .WithMany(p => p.Action)
                    .HasForeignKey(d => d.BrandId)
                    .HasConstraintName("FK_Action_Brand");

                entity.HasOne(d => d.PromotionTier)
                    .WithOne(p => p.Action)
                    .HasForeignKey<Action>(d => d.PromotionTierId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("Action_FK");
            });

            modelBuilder.Entity<ActionProductMapping>(entity =>
            {
                entity.Property(e => e.Id).ValueGeneratedNever();

                entity.Property(e => e.InsDate).HasColumnType("datetime");

                entity.Property(e => e.UpdDate).HasColumnType("datetime");

                entity.HasOne(d => d.Action)
                    .WithMany(p => p.ActionProductMapping)
                    .HasForeignKey(d => d.ActionId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK_ActionProductMapping_Action");

                entity.HasOne(d => d.Product)
                    .WithMany(p => p.ActionProductMapping)
                    .HasForeignKey(d => d.ProductId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK_ActionProductMapping_Product");
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
                    .IsRequired()
                    .HasMaxLength(30)
                    .IsUnicode(false);

                entity.Property(e => e.BrandName)
                    .IsRequired()
                    .HasMaxLength(50);

                entity.Property(e => e.CompanyName)
                    .IsRequired()
                    .HasMaxLength(50);

                entity.Property(e => e.Description).HasMaxLength(100);

                entity.Property(e => e.ImgUrl)
                    .HasMaxLength(2048)
                    .IsUnicode(false);

                entity.Property(e => e.InsDate)
                    .HasColumnType("datetime")
                    .HasDefaultValueSql("(getdate())");

                entity.Property(e => e.PhoneNumber)
                    .IsRequired()
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
                    .IsRequired()
                    .HasMaxLength(20)
                    .IsUnicode(false);

                entity.Property(e => e.ChannelName)
                    .IsRequired()
                    .HasMaxLength(50);

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

                entity.Property(e => e.InsDate)
                    .HasColumnType("datetime")
                    .HasDefaultValueSql("(getdate())");

                entity.Property(e => e.Summary).HasMaxLength(4000);

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

            modelBuilder.Entity<Device>(entity =>
            {
                entity.Property(e => e.DeviceId).HasDefaultValueSql("(newid())");

                entity.Property(e => e.Code)
                    .IsRequired()
                    .HasMaxLength(8)
                    .IsUnicode(false);

                entity.Property(e => e.InsDate)
                    .HasColumnType("datetime")
                    .HasDefaultValueSql("(getdate())");

                entity.Property(e => e.Name)
                    .IsRequired()
                    .HasMaxLength(50);

                entity.Property(e => e.UpdDate)
                    .HasColumnType("datetime")
                    .HasDefaultValueSql("(getdate())");

                entity.HasOne(d => d.Store)
                    .WithMany(p => p.Device)
                    .HasForeignKey(d => d.StoreId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK_Device_Store");
            });

            modelBuilder.Entity<GameConfig>(entity =>
            {
                entity.Property(e => e.Id).HasDefaultValueSql("(newid())");

                entity.Property(e => e.InsDate)
                    .HasColumnType("datetime")
                    .HasDefaultValueSql("(getdate())");

                entity.Property(e => e.Name)
                    .IsRequired()
                    .HasMaxLength(50);

                entity.Property(e => e.UpdDate)
                    .HasColumnType("datetime")
                    .HasDefaultValueSql("(getdate())");

                entity.HasOne(d => d.Brand)
                    .WithMany(p => p.GameConfig)
                    .HasForeignKey(d => d.BrandId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK_Game_Brand");

                entity.HasOne(d => d.GameMaster)
                    .WithMany(p => p.GameConfig)
                    .HasForeignKey(d => d.GameMasterId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK_GameConfig_GameMaster");
            });

            modelBuilder.Entity<GameItems>(entity =>
            {
                entity.Property(e => e.Id).HasDefaultValueSql("(newid())");

                entity.Property(e => e.Description).HasMaxLength(4000);

                entity.Property(e => e.DisplayText)
                    .IsRequired()
                    .HasMaxLength(30);

                entity.Property(e => e.InsDate)
                    .HasColumnType("datetime")
                    .HasDefaultValueSql("(getdate())");

                entity.Property(e => e.Priority).HasDefaultValueSql("((1))");

                entity.Property(e => e.UpdDate)
                    .HasColumnType("datetime")
                    .HasDefaultValueSql("(getdate())");

                entity.HasOne(d => d.Game)
                    .WithMany(p => p.GameItems)
                    .HasForeignKey(d => d.GameId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK_GameItems_Game");

                entity.HasOne(d => d.Promotion)
                    .WithMany(p => p.GameItems)
                    .HasForeignKey(d => d.PromotionId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK_GameItems_Promotion");
            });

            modelBuilder.Entity<GameMaster>(entity =>
            {
                entity.Property(e => e.Id).HasDefaultValueSql("(newid())");

                entity.Property(e => e.InsDate)
                    .HasColumnType("datetime")
                    .HasDefaultValueSql("(getdate())");

                entity.Property(e => e.Name)
                    .IsRequired()
                    .HasMaxLength(50);

                entity.Property(e => e.UpdDate)
                    .HasColumnType("datetime")
                    .HasDefaultValueSql("(getdate())");
            });

            modelBuilder.Entity<Holiday>(entity =>
            {
                entity.Property(e => e.HolidayId).HasDefaultValueSql("(newid())");

                entity.Property(e => e.Date).HasColumnType("date");

                entity.Property(e => e.HolidayName)
                    .IsRequired()
                    .HasMaxLength(100);

                entity.Property(e => e.InsDate)
                    .HasColumnType("datetime")
                    .HasDefaultValueSql("(getdate())");

                entity.Property(e => e.UpdDate)
                    .HasColumnType("datetime")
                    .HasDefaultValueSql("(getdate())");
            });

            modelBuilder.Entity<MemberLevel>(entity =>
            {
                entity.Property(e => e.MemberLevelId).HasDefaultValueSql("(newid())");

                entity.Property(e => e.InsDate)
                    .HasColumnType("datetime")
                    .HasDefaultValueSql("(getdate())");

                entity.Property(e => e.Name)
                    .IsRequired()
                    .HasMaxLength(50);

                entity.Property(e => e.UpdDate)
                    .HasColumnType("datetime")
                    .HasDefaultValueSql("(getdate())");

                entity.HasOne(d => d.Brand)
                    .WithMany(p => p.MemberLevel)
                    .HasForeignKey(d => d.BrandId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK_MemberLevel_Brand");
            });

            modelBuilder.Entity<MemberLevelMapping>(entity =>
            {
                entity.Property(e => e.Id).HasDefaultValueSql("(newid())");

                entity.Property(e => e.InsDate)
                    .HasColumnType("datetime")
                    .HasDefaultValueSql("(getdate())");

                entity.Property(e => e.UpdDate)
                    .HasColumnType("datetime")
                    .HasDefaultValueSql("(getdate())");

                entity.HasOne(d => d.MemberLevel)
                    .WithMany(p => p.MemberLevelMapping)
                    .HasForeignKey(d => d.MemberLevelId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK_MemberLevelMapping_MemberLevel");

                entity.HasOne(d => d.Promotion)
                    .WithMany(p => p.MemberLevelMapping)
                    .HasForeignKey(d => d.PromotionId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK_MemberLevelMapping_Promotion");
            });

            modelBuilder.Entity<Membership>(entity =>
            {
                entity.Property(e => e.MembershipId).HasDefaultValueSql("(newid())");

                entity.Property(e => e.Email)
                    .IsRequired()
                    .HasMaxLength(62)
                    .IsUnicode(false);

                entity.Property(e => e.Fullname)
                    .IsRequired()
                    .HasMaxLength(50);

                entity.Property(e => e.InsDate)
                    .HasColumnType("datetime")
                    .HasDefaultValueSql("(getdate())");

                entity.Property(e => e.MembershipCode)
                    .HasMaxLength(20)
                    .IsUnicode(false);

                entity.Property(e => e.PhoneNumber)
                    .IsRequired()
                    .HasMaxLength(11)
                    .IsUnicode(false);

                entity.Property(e => e.UpdDate)
                    .HasColumnType("datetime")
                    .HasDefaultValueSql("(getdate())");
            });

            modelBuilder.Entity<OrderCondition>(entity =>
            {
                entity.Property(e => e.OrderConditionId).HasDefaultValueSql("(newid())");

                entity.Property(e => e.Amount).HasColumnType("decimal(10, 0)");

                entity.Property(e => e.AmountOperator)
                    .IsRequired()
                    .HasMaxLength(2)
                    .IsUnicode(false)
                    .IsFixedLength();

                entity.Property(e => e.InsDate)
                    .HasColumnType("datetime")
                    .HasDefaultValueSql("(getdate())");

                entity.Property(e => e.QuantityOperator)
                    .IsRequired()
                    .HasMaxLength(2)
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

            modelBuilder.Entity<PostAction>(entity =>
            {
                entity.HasIndex(e => e.PromotionTierId)
                    .HasName("IX_MembershipAction")
                    .IsUnique();

                entity.Property(e => e.PostActionId).HasDefaultValueSql("(newid())");

                entity.Property(e => e.BonusPoint).HasColumnType("decimal(10, 2)");

                entity.Property(e => e.GiftProductCode)
                    .HasMaxLength(50)
                    .IsUnicode(false);

                entity.Property(e => e.GiftQuantity).HasColumnType("decimal(6, 0)");

                entity.Property(e => e.InsDate)
                    .HasColumnType("datetime")
                    .HasDefaultValueSql("(getdate())");

                entity.Property(e => e.Name).HasMaxLength(100);

                entity.Property(e => e.UpdDate)
                    .HasColumnType("datetime")
                    .HasDefaultValueSql("(getdate())");

                entity.HasOne(d => d.Brand)
                    .WithMany(p => p.PostAction)
                    .HasForeignKey(d => d.BrandId)
                    .HasConstraintName("FK_PostAction_Brand");

                entity.HasOne(d => d.PromotionTier)
                    .WithOne(p => p.PostAction)
                    .HasForeignKey<PostAction>(d => d.PromotionTierId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("MembershipAction_FK");
            });

            modelBuilder.Entity<PostActionProductMapping>(entity =>
            {
                entity.Property(e => e.Id).ValueGeneratedNever();

                entity.Property(e => e.InsDate).HasColumnType("datetime");

                entity.Property(e => e.UpdDate).HasColumnType("datetime");

                entity.HasOne(d => d.PostAction)
                    .WithMany(p => p.PostActionProductMapping)
                    .HasForeignKey(d => d.PostActionId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK_PostActionProductMapping_PostAction");

                entity.HasOne(d => d.Product)
                    .WithMany(p => p.PostActionProductMapping)
                    .HasForeignKey(d => d.ProductId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK_PostActionProductMapping_Product");
            });

            modelBuilder.Entity<Product>(entity =>
            {
                entity.Property(e => e.ProductId).HasDefaultValueSql("(newid())");

                entity.Property(e => e.Code)
                    .IsRequired()
                    .HasMaxLength(50)
                    .IsUnicode(false);

                entity.Property(e => e.InsDate)
                    .HasColumnType("datetime")
                    .HasDefaultValueSql("(getdate())");

                entity.Property(e => e.Name)
                    .IsRequired()
                    .HasMaxLength(80);

                entity.Property(e => e.UpdDate)
                    .HasColumnType("datetime")
                    .HasDefaultValueSql("(getdate())");

                entity.HasOne(d => d.ProductCate)
                    .WithMany(p => p.Product)
                    .HasForeignKey(d => d.ProductCateId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK_Product_ProductCategory");
            });

            modelBuilder.Entity<ProductCategory>(entity =>
            {
                entity.HasKey(e => e.ProductCateId);

                entity.Property(e => e.ProductCateId).HasDefaultValueSql("(newid())");

                entity.Property(e => e.CateId)
                    .IsRequired()
                    .HasMaxLength(30)
                    .IsUnicode(false);

                entity.Property(e => e.InsDate)
                    .HasColumnType("datetime")
                    .HasDefaultValueSql("(getdate())");

                entity.Property(e => e.Name).HasMaxLength(50);

                entity.Property(e => e.UpdDate)
                    .HasColumnType("datetime")
                    .HasDefaultValueSql("(getdate())");

                entity.HasOne(d => d.Brand)
                    .WithMany(p => p.ProductCategory)
                    .HasForeignKey(d => d.BrandId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK_ProductCategory_Brand");
            });

            modelBuilder.Entity<ProductCondition>(entity =>
            {
                entity.Property(e => e.ProductConditionId).HasDefaultValueSql("(newid())");

                entity.Property(e => e.InsDate)
                    .HasColumnType("datetime")
                    .HasDefaultValueSql("(getdate())");

                entity.Property(e => e.QuantityOperator)
                    .IsRequired()
                    .HasMaxLength(2)
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

            modelBuilder.Entity<ProductConditionMapping>(entity =>
            {
                entity.Property(e => e.Id).HasDefaultValueSql("(newid())");

                entity.Property(e => e.InsDate).HasColumnType("datetime");

                entity.Property(e => e.UpdTime).HasColumnType("datetime");

                entity.HasOne(d => d.ProductCondition)
                    .WithMany(p => p.ProductConditionMapping)
                    .HasForeignKey(d => d.ProductConditionId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK_ProductConditionMapping_ProductCondition");

                entity.HasOne(d => d.Product)
                    .WithMany(p => p.ProductConditionMapping)
                    .HasForeignKey(d => d.ProductId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK_ProductConditionMapping_Product");
            });

            modelBuilder.Entity<Promotion>(entity =>
            {
                entity.Property(e => e.PromotionId).HasDefaultValueSql("(newid())");

                entity.Property(e => e.Description).HasMaxLength(4000);

                entity.Property(e => e.EndDate).HasColumnType("datetime");

                entity.Property(e => e.ImgUrl)
                    .HasMaxLength(2048)
                    .IsUnicode(false);

                entity.Property(e => e.InsDate)
                    .HasColumnType("datetime")
                    .HasDefaultValueSql("(getdate())");

                entity.Property(e => e.PromotionCode)
                    .IsRequired()
                    .HasMaxLength(6)
                    .IsUnicode(false);

                entity.Property(e => e.PromotionName)
                    .IsRequired()
                    .HasMaxLength(100);

                entity.Property(e => e.StartDate).HasColumnType("datetime");

                entity.Property(e => e.UpdDate)
                    .HasColumnType("datetime")
                    .HasDefaultValueSql("(getdate())");

                entity.HasOne(d => d.Brand)
                    .WithMany(p => p.Promotion)
                    .HasForeignKey(d => d.BrandId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK_Promotion_Brand");
            });

            modelBuilder.Entity<PromotionChannelMapping>(entity =>
            {
                entity.HasKey(e => e.PromotionChannelId)
                    .HasName("PK_VoucherChannel");

                entity.Property(e => e.PromotionChannelId).HasDefaultValueSql("(newid())");

                entity.Property(e => e.InsDate)
                    .HasColumnType("datetime")
                    .HasDefaultValueSql("(getdate())");

                entity.Property(e => e.UpdDate)
                    .HasColumnType("datetime")
                    .HasDefaultValueSql("(getdate())");

                entity.HasOne(d => d.Channel)
                    .WithMany(p => p.PromotionChannelMapping)
                    .HasForeignKey(d => d.ChannelId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK_VoucherChannel_Channel");

                entity.HasOne(d => d.Promotion)
                    .WithMany(p => p.PromotionChannelMapping)
                    .HasForeignKey(d => d.PromotionId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
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
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK_PromotionStoreMapping_Promotion");

                entity.HasOne(d => d.Store)
                    .WithMany(p => p.PromotionStoreMapping)
                    .HasForeignKey(d => d.StoreId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
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

                entity.Property(e => e.Summary).HasMaxLength(4000);

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

                entity.HasOne(d => d.VoucherGroup)
                    .WithMany(p => p.PromotionTier)
                    .HasForeignKey(d => d.VoucherGroupId)
                    .HasConstraintName("FK_PromotionTier_VoucherGroup");
            });

            modelBuilder.Entity<RoleEntity>(entity =>
            {
                entity.Property(e => e.InsDate)
                    .HasColumnType("datetime")
                    .HasDefaultValueSql("(getdate())");

                entity.Property(e => e.Name)
                    .IsRequired()
                    .HasMaxLength(20)
                    .IsUnicode(false);

                entity.Property(e => e.UpdDate)
                    .HasColumnType("datetime")
                    .HasDefaultValueSql("(getdate())");
            });

            modelBuilder.Entity<Store>(entity =>
            {
                entity.Property(e => e.StoreId).HasDefaultValueSql("(newid())");

                entity.Property(e => e.InsDate)
                    .HasColumnType("datetime")
                    .HasDefaultValueSql("(getdate())");

                entity.Property(e => e.StoreCode)
                    .IsRequired()
                    .HasMaxLength(20)
                    .IsUnicode(false);

                entity.Property(e => e.StoreName)
                    .IsRequired()
                    .HasMaxLength(50);

                entity.Property(e => e.UpdDate)
                    .HasColumnType("datetime")
                    .HasDefaultValueSql("(getdate())");

                entity.HasOne(d => d.Brand)
                    .WithMany(p => p.Store)
                    .HasForeignKey(d => d.BrandId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK_Store_Brand");
            });

            modelBuilder.Entity<Transaction>(entity =>
            {
                entity.Property(e => e.Id).ValueGeneratedNever();

                entity.Property(e => e.InsDate).HasColumnType("datetime");

                entity.Property(e => e.TransactionJson)
                    .IsRequired()
                    .HasMaxLength(4000);

                entity.Property(e => e.UpdDate).HasColumnType("datetime");

                entity.HasOne(d => d.Brand)
                    .WithMany(p => p.Transaction)
                    .HasForeignKey(d => d.BrandId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK_Transaction_Brand");
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

                entity.HasOne(d => d.Promotion)
                    .WithMany(p => p.Voucher)
                    .HasForeignKey(d => d.PromotionId)
                    .HasConstraintName("FK_Voucher_Promotion");

                entity.HasOne(d => d.Store)
                    .WithMany(p => p.Voucher)
                    .HasForeignKey(d => d.StoreId)
                    .HasConstraintName("FK_Voucher_Store");

                entity.HasOne(d => d.VoucherGroup)
                    .WithMany(p => p.Voucher)
                    .HasForeignKey(d => d.VoucherGroupId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK_Voucher_VoucherGroup");
            });

            modelBuilder.Entity<VoucherGroup>(entity =>
            {
                entity.Property(e => e.VoucherGroupId).HasDefaultValueSql("(newid())");

                entity.Property(e => e.Charset)
                    .HasMaxLength(42)
                    .IsUnicode(false);

                entity.Property(e => e.CustomCharset)
                    .HasMaxLength(106)
                    .IsUnicode(false);

                entity.Property(e => e.InsDate)
                    .HasColumnType("datetime")
                    .HasDefaultValueSql("(getdate())");

                entity.Property(e => e.Postfix)
                    .HasMaxLength(10)
                    .IsUnicode(false);

                entity.Property(e => e.Prefix)
                    .HasMaxLength(10)
                    .IsUnicode(false);

                entity.Property(e => e.UpdDate)
                    .HasColumnType("datetime")
                    .HasDefaultValueSql("(getdate())");

                entity.Property(e => e.VoucherName)
                    .IsRequired()
                    .HasMaxLength(50);

                entity.Property(e => e.VoucherType)
                    .IsRequired()
                    .HasMaxLength(1)
                    .IsUnicode(false)
                    .IsFixedLength();

                entity.HasOne(d => d.Action)
                    .WithMany(p => p.VoucherGroup)
                    .HasForeignKey(d => d.ActionId)
                    .HasConstraintName("FK_VoucherGroup_Action");

                entity.HasOne(d => d.Brand)
                    .WithMany(p => p.VoucherGroup)
                    .HasForeignKey(d => d.BrandId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK_VoucherGroup_Brand");

                entity.HasOne(d => d.ConditionRule)
                    .WithMany(p => p.VoucherGroup)
                    .HasForeignKey(d => d.ConditionRuleId)
                    .HasConstraintName("FK_VoucherGroup_ConditionRule");

                entity.HasOne(d => d.PostAction)
                    .WithMany(p => p.VoucherGroup)
                    .HasForeignKey(d => d.PostActionId)
                    .HasConstraintName("FK_VoucherGroup_PostAction");
            });

            OnModelCreatingPartial(modelBuilder);
        }

        partial void OnModelCreatingPartial(ModelBuilder modelBuilder);
    }
}
