﻿using System;
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
        public virtual DbSet<GameCampaign> GameCampaign { get; set; }
        public virtual DbSet<GameItems> GameItems { get; set; }
        public virtual DbSet<GameMaster> GameMaster { get; set; }
        public virtual DbSet<Gift> Gift { get; set; }
        public virtual DbSet<GiftProductMapping> GiftProductMapping { get; set; }
        public virtual DbSet<MemberLevel> MemberLevel { get; set; }
        public virtual DbSet<MemberLevelMapping> MemberLevelMapping { get; set; }
        public virtual DbSet<Membership> Membership { get; set; }
        public virtual DbSet<OrderCondition> OrderCondition { get; set; }
        public virtual DbSet<Product> Product { get; set; }
        public virtual DbSet<ProductCategory> ProductCategory { get; set; }
        public virtual DbSet<ProductCondition> ProductCondition { get; set; }
        public virtual DbSet<ProductConditionMapping> ProductConditionMapping { get; set; }
        public virtual DbSet<Promotion> Promotion { get; set; }
        public virtual DbSet<PromotionChannelMapping> PromotionChannelMapping { get; set; }
        public virtual DbSet<PromotionStoreMapping> PromotionStoreMapping { get; set; }
        public virtual DbSet<PromotionTier> PromotionTier { get; set; }
        public virtual DbSet<Role> Role { get; set; }
        public virtual DbSet<Store> Store { get; set; }
        public virtual DbSet<StoreGameCampaignMapping> StoreGameCampaignMapping { get; set; }
        public virtual DbSet<Transaction> Transaction { get; set; }
        public virtual DbSet<Voucher> Voucher { get; set; }
        public virtual DbSet<VoucherGroup> VoucherGroup { get; set; }

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            if (!optionsBuilder.IsConfigured)
            { }
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

                entity.HasOne(d => d.Brand)
                    .WithMany(p => p.Account)
                    .HasForeignKey(d => d.BrandId)
                    .HasConstraintName("FK_Account_Brand");

                entity.HasOne(d => d.Role)
                    .WithMany(p => p.Account)
                    .HasForeignKey(d => d.RoleId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK_Account_Role");
            });

            modelBuilder.Entity<Action>(entity =>
            {
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

                entity.Property(e => e.BrandId).HasDefaultValueSql("(newid())");

                entity.Property(e => e.Address).HasMaxLength(100);

                entity.Property(e => e.BrandCode)
                    .IsRequired()
                    .HasMaxLength(30)
                    .IsUnicode(false);

                entity.Property(e => e.BrandEmail)
                    .HasMaxLength(62)
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
            });

            modelBuilder.Entity<Channel>(entity =>
            {
                entity.Property(e => e.ChannelId).HasDefaultValueSql("(newid())");

                entity.Property(e => e.ApiKey)
                    .HasMaxLength(44)
                    .IsUnicode(false);

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

                entity.Property(e => e.PrivateKey)
                    .HasMaxLength(2240)
                    .IsUnicode(false);

                entity.Property(e => e.PublicKey)
                    .HasMaxLength(600)
                    .IsUnicode(false);

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

            modelBuilder.Entity<GameCampaign>(entity =>
            {
                entity.Property(e => e.Id).HasDefaultValueSql("(newid())");

                entity.Property(e => e.EndGame).HasColumnType("datetime");

                entity.Property(e => e.InsDate)
                    .HasColumnType("datetime")
                    .HasDefaultValueSql("(getdate())");

                entity.Property(e => e.Name)
                    .IsRequired()
                    .HasMaxLength(50);

                entity.Property(e => e.SecretCode)
                    .HasMaxLength(6)
                    .IsUnicode(false);

                entity.Property(e => e.StartGame).HasColumnType("datetime");

                entity.Property(e => e.UpdDate)
                    .HasColumnType("datetime")
                    .HasDefaultValueSql("(getdate())");

                entity.HasOne(d => d.Brand)
                    .WithMany(p => p.GameCampaign)
                    .HasForeignKey(d => d.BrandId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK_Game_Brand");

                entity.HasOne(d => d.GameMaster)
                    .WithMany(p => p.GameCampaign)
                    .HasForeignKey(d => d.GameMasterId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK_GameConfig_GameMaster");

                entity.HasOne(d => d.Promotion)
                    .WithMany(p => p.GameCampaign)
                    .HasForeignKey(d => d.PromotionId)
                    .HasConstraintName("FK_GameCampaign_Promotion");
            });

            modelBuilder.Entity<GameItems>(entity =>
            {
                entity.Property(e => e.Id).HasDefaultValueSql("(newid())");

                entity.Property(e => e.Description).HasMaxLength(4000);

                entity.Property(e => e.DisplayText)
                    .IsRequired()
                    .HasMaxLength(30);

                entity.Property(e => e.ImgUrl)
                    .HasMaxLength(2048)
                    .IsUnicode(false);

                entity.Property(e => e.InsDate)
                    .HasColumnType("datetime")
                    .HasDefaultValueSql("(getdate())");

                entity.Property(e => e.ItemColor)
                    .HasMaxLength(7)
                    .IsUnicode(false)
                    .IsFixedLength();

                entity.Property(e => e.Priority).HasDefaultValueSql("((1))");

                entity.Property(e => e.TextColor)
                    .HasMaxLength(7)
                    .IsUnicode(false)
                    .IsFixedLength();

                entity.Property(e => e.UpdDate)
                    .HasColumnType("datetime")
                    .HasDefaultValueSql("(getdate())");

                entity.HasOne(d => d.Game)
                    .WithMany(p => p.GameItems)
                    .HasForeignKey(d => d.GameId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK_GameItems_Game");
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

            modelBuilder.Entity<Gift>(entity =>
            {
                entity.Property(e => e.GiftId).HasDefaultValueSql("(newid())");

                entity.Property(e => e.BonusPoint).HasColumnType("decimal(10, 2)");

                entity.Property(e => e.InsDate)
                    .HasColumnType("datetime")
                    .HasDefaultValueSql("(getdate())");

                entity.Property(e => e.Name).HasMaxLength(100);

                entity.Property(e => e.UpdDate)
                    .HasColumnType("datetime")
                    .HasDefaultValueSql("(getdate())");

                entity.HasOne(d => d.Brand)
                    .WithMany(p => p.Gift)
                    .HasForeignKey(d => d.BrandId)
                    .HasConstraintName("FK_PostAction_Brand");

                entity.HasOne(d => d.GameCampaign)
                    .WithMany(p => p.Gift)
                    .HasForeignKey(d => d.GameCampaignId)
                    .HasConstraintName("FK_PostAction_GameCampaign");
            });

            modelBuilder.Entity<GiftProductMapping>(entity =>
            {
                entity.Property(e => e.Id).ValueGeneratedNever();

                entity.Property(e => e.InsDate).HasColumnType("datetime");

                entity.Property(e => e.UpdDate).HasColumnType("datetime");

                entity.HasOne(d => d.Gift)
                    .WithMany(p => p.GiftProductMapping)
                    .HasForeignKey(d => d.GiftId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK_PostActionProductMapping_PostAction");

                entity.HasOne(d => d.Product)
                    .WithMany(p => p.GiftProductMapping)
                    .HasForeignKey(d => d.ProductId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK_PostActionProductMapping_Product");
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
                    .HasMaxLength(62)
                    .IsUnicode(false);

                entity.Property(e => e.Fullname)
                    .IsRequired()
                    .HasMaxLength(50);

                entity.Property(e => e.InsDate)
                    .HasColumnType("datetime")
                    .HasDefaultValueSql("(getdate())");

                entity.Property(e => e.PhoneNumber)
                    .IsRequired()
                    .HasMaxLength(12)
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
                    .IsUnicode(false);

                entity.Property(e => e.InsDate)
                    .HasColumnType("datetime")
                    .HasDefaultValueSql("(getdate())");

                entity.Property(e => e.QuantityOperator)
                    .IsRequired()
                    .HasMaxLength(2)
                    .IsUnicode(false);

                entity.Property(e => e.UpdDate)
                    .HasColumnType("datetime")
                    .HasDefaultValueSql("(getdate())");

                entity.HasOne(d => d.ConditionGroup)
                    .WithMany(p => p.OrderCondition)
                    .HasForeignKey(d => d.ConditionGroupId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK_OrderCondition_ConditionGroup1");
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
                    .IsUnicode(false);

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

                entity.Property(e => e.InsDate)
                    .HasColumnType("datetime")
                    .HasDefaultValueSql("(getdate())");

                entity.Property(e => e.UpdDate)
                    .HasColumnType("datetime")
                    .HasDefaultValueSql("(getdate())");

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
                    .HasMaxLength(20)
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
                    .HasConstraintName("FK_PromotionChannelMapping_Promotion");
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
                entity.Property(e => e.PromotionTierId).HasDefaultValueSql("(newid())");

                entity.Property(e => e.InsDate)
                    .HasColumnType("datetime")
                    .HasDefaultValueSql("(getdate())");

                entity.Property(e => e.Summary).HasMaxLength(4000);

                entity.Property(e => e.UpdDate)
                    .HasColumnType("datetime")
                    .HasDefaultValueSql("(getdate())");

                entity.HasOne(d => d.Action)
                    .WithMany(p => p.PromotionTier)
                    .HasForeignKey(d => d.ActionId)
                    .HasConstraintName("FK_Action_PromotionTier");

                entity.HasOne(d => d.ConditionRule)
                    .WithMany(p => p.PromotionTier)
                    .HasForeignKey(d => d.ConditionRuleId)
                    .HasConstraintName("FK_PromotionTier_ConditionRule");

                entity.HasOne(d => d.Gift)
                    .WithMany(p => p.PromotionTier)
                    .HasForeignKey(d => d.GiftId)
                    .HasConstraintName("PromotionTier_FK");

                entity.HasOne(d => d.Promotion)
                    .WithMany(p => p.PromotionTier)
                    .HasForeignKey(d => d.PromotionId)
                    .HasConstraintName("FK_PromotionTier_Promotion");

                entity.HasOne(d => d.VoucherGroup)
                    .WithMany(p => p.PromotionTier)
                    .HasForeignKey(d => d.VoucherGroupId)
                    .HasConstraintName("FK_PromotionTier_VoucherGroup");
            });

            modelBuilder.Entity<Role>(entity =>
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

            modelBuilder.Entity<StoreGameCampaignMapping>(entity =>
            {
                entity.HasKey(e => e.StoreGameCampaignId)
                    .HasName("StoreGameCampaignMapping_PK");

                entity.Property(e => e.StoreGameCampaignId).HasDefaultValueSql("(newid())");

                entity.Property(e => e.InsDate)
                    .HasColumnType("datetime")
                    .HasDefaultValueSql("(getdate())");

                entity.Property(e => e.UpdDate)
                    .HasColumnType("datetime")
                    .HasDefaultValueSql("(getdate())");

                entity.HasOne(d => d.GameCampaign)
                    .WithMany(p => p.StoreGameCampaignMapping)
                    .HasForeignKey(d => d.GameCampaignId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("StoreGameCampaignMapping_FK_1");

                entity.HasOne(d => d.Store)
                    .WithMany(p => p.StoreGameCampaignMapping)
                    .HasForeignKey(d => d.StoreId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("StoreGameCampaignMapping_FK");
            });

            modelBuilder.Entity<Transaction>(entity =>
            {
                entity.Property(e => e.Id).HasDefaultValueSql("(newid())");

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

                entity.Property(e => e.OrderId)
                    .HasMaxLength(100)
                    .IsUnicode(false);

                entity.Property(e => e.RedempedDate).HasColumnType("datetime");

                entity.Property(e => e.UpdDate)
                    .HasColumnType("datetime")
                    .HasDefaultValueSql("(getdate())");

                entity.Property(e => e.UsedDate).HasColumnType("datetime");

                entity.HasOne(d => d.Channel)
                    .WithMany(p => p.Voucher)
                    .HasForeignKey(d => d.ChannelId)
                    .HasConstraintName("FK_Voucher_Channel");

                entity.HasOne(d => d.GameCampaign)
                    .WithMany(p => p.Voucher)
                    .HasForeignKey(d => d.GameCampaignId)
                    .HasConstraintName("FK_Voucher_GameCampaign");

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

                entity.Property(e => e.ImgUrl)
                    .HasMaxLength(2048)
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
                    .HasMaxLength(100);

                entity.HasOne(d => d.Action)
                    .WithMany(p => p.VoucherGroup)
                    .HasForeignKey(d => d.ActionId)
                    .HasConstraintName("FK_VoucherGroup_Action");

                entity.HasOne(d => d.Brand)
                    .WithMany(p => p.VoucherGroup)
                    .HasForeignKey(d => d.BrandId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK_VoucherGroup_Brand");

                entity.HasOne(d => d.Gift)
                    .WithMany(p => p.VoucherGroup)
                    .HasForeignKey(d => d.GiftId)
                    .HasConstraintName("FK_VoucherGroup_PostAction");
            });

            OnModelCreatingPartial(modelBuilder);
        }

        partial void OnModelCreatingPartial(ModelBuilder modelBuilder);
    }
}
