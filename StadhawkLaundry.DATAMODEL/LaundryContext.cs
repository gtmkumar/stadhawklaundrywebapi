using Microsoft.EntityFrameworkCore;
using StadhawkLaundry.DataModel.Models;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Text;

namespace StadhawkLaundry.DataModel
{
    public partial class LaundryContext : DbContext
    {
        public LaundryContext(DbContextOptions<LaundryContext> options)
            : base(options)
        {
        }

        public virtual DbSet<AspNetRoleClaims> AspNetRoleClaims { get; set; }
        public virtual DbSet<AspNetRoles> AspNetRoles { get; set; }
        public virtual DbSet<AspNetUserClaims> AspNetUserClaims { get; set; }
        public virtual DbSet<AspNetUserLogins> AspNetUserLogins { get; set; }
        public virtual DbSet<AspNetUserRoles> AspNetUserRoles { get; set; }
        public virtual DbSet<AspNetUserTokens> AspNetUserTokens { get; set; }
        public virtual DbSet<AspNetUsers> AspNetUsers { get; set; }
        public virtual DbSet<TblAccessRight> TblAccessRight { get; set; }
        public virtual DbSet<TblAddressMaster> TblAddressMaster { get; set; }
        public virtual DbSet<TblAddressTypeMaster> TblAddressTypeMaster { get; set; }
        public virtual DbSet<TblBanner> TblBanner { get; set; }
        public virtual DbSet<TblBannerCategoryMaster> TblBannerCategoryMaster { get; set; }
        public virtual DbSet<TblCategoryMaster> TblCategoryMaster { get; set; }
        public virtual DbSet<TblCityMaster> TblCityMaster { get; set; }
        public virtual DbSet<TblEntityMaster> TblEntityMaster { get; set; }
        public virtual DbSet<TblImage> TblImage { get; set; }
        public virtual DbSet<TblItemMaster> TblItemMaster { get; set; }
        public virtual DbSet<TblNavigator> TblNavigator { get; set; }
        public virtual DbSet<TblOrder> TblOrder { get; set; }
        public virtual DbSet<TblOrderItems> TblOrderItems { get; set; }
        public virtual DbSet<TblOrderStatus> TblOrderStatus { get; set; }
        public virtual DbSet<TblPayment> TblPayment { get; set; }
        public virtual DbSet<TblPaymentStatus> TblPaymentStatus { get; set; }
        public virtual DbSet<TblPrivilegeBase> TblPrivilegeBase { get; set; }
        public virtual DbSet<TblServiceLabelMaster> TblServiceLabelMaster { get; set; }
        public virtual DbSet<TblServiceMaster> TblServiceMaster { get; set; }
        public virtual DbSet<TblStateMaster> TblStateMaster { get; set; }
        public virtual DbSet<TblStatusMaster> TblStatusMaster { get; set; }
        public virtual DbSet<TblStore> TblStore { get; set; }
        public virtual DbSet<TblStoreEmployees> TblStoreEmployees { get; set; }
        public virtual DbSet<TblStoreItems> TblStoreItems { get; set; }
        public virtual DbSet<TblStorePackagesAndCategoryMapping> TblStorePackagesAndCategoryMapping { get; set; }
        public virtual DbSet<TblStorePckages> TblStorePckages { get; set; }
        public virtual DbSet<TblSubServiceMaster> TblSubServiceMaster { get; set; }
        public virtual DbSet<TblUnitMaster> TblUnitMaster { get; set; }
        public virtual DbSet<TblUserAddress> TblUserAddress { get; set; }
        public virtual DbSet<UsersMaster> UsersMaster { get; set; }
        public virtual DbSet<NavigatorView> NavigatorViews { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.HasAnnotation("ProductVersion", "2.2.3-servicing-35854");

            modelBuilder.Entity<AspNetRoleClaims>(entity =>
            {
                entity.HasIndex(e => e.RoleId);

                entity.HasOne(d => d.Role)
                    .WithMany(p => p.AspNetRoleClaims)
                    .HasForeignKey(d => d.RoleId);
            });

            modelBuilder.Entity<AspNetRoles>(entity =>
            {
                entity.HasIndex(e => e.NormalizedName)
                    .HasName("RoleNameIndex")
                    .IsUnique()
                    .HasFilter("([NormalizedName] IS NOT NULL)");

                entity.Property(e => e.Name).HasMaxLength(256);

                entity.Property(e => e.NormalizedName).HasMaxLength(256);
            });

            modelBuilder.Entity<AspNetUserClaims>(entity =>
            {
                entity.HasIndex(e => e.UserId);

                entity.HasOne(d => d.User)
                    .WithMany(p => p.AspNetUserClaims)
                    .HasForeignKey(d => d.UserId);
            });

            modelBuilder.Entity<AspNetUserLogins>(entity =>
            {
                entity.HasKey(e => new { e.LoginProvider, e.ProviderKey });

                entity.HasIndex(e => e.UserId);

                entity.HasOne(d => d.User)
                    .WithMany(p => p.AspNetUserLogins)
                    .HasForeignKey(d => d.UserId);
            });

            modelBuilder.Entity<AspNetUserRoles>(entity =>
            {
                entity.HasKey(e => new { e.UserId, e.RoleId });

                entity.HasIndex(e => e.RoleId);

                entity.HasOne(d => d.Role)
                    .WithMany(p => p.AspNetUserRoles)
                    .HasForeignKey(d => d.RoleId);

                entity.HasOne(d => d.User)
                    .WithMany(p => p.AspNetUserRoles)
                    .HasForeignKey(d => d.UserId);
            });

            modelBuilder.Entity<AspNetUserTokens>(entity =>
            {
                entity.HasKey(e => new { e.UserId, e.LoginProvider, e.Name });

                entity.HasOne(d => d.User)
                    .WithMany(p => p.AspNetUserTokens)
                    .HasForeignKey(d => d.UserId);
            });

            modelBuilder.Entity<AspNetUsers>(entity =>
            {
                entity.HasIndex(e => e.NormalizedEmail)
                    .HasName("EmailIndex");

                entity.HasIndex(e => e.NormalizedUserName)
                    .HasName("UserNameIndex")
                    .IsUnique()
                    .HasFilter("([NormalizedUserName] IS NOT NULL)");

                entity.Property(e => e.Dob)
                    .HasColumnName("DOB")
                    .HasColumnType("datetime");

                entity.Property(e => e.Email).HasMaxLength(256);

                entity.Property(e => e.Fcmtoken).HasColumnName("FCMToken");

                entity.Property(e => e.FullName).HasMaxLength(250);

                entity.Property(e => e.NormalizedEmail).HasMaxLength(256);

                entity.Property(e => e.NormalizedUserName).HasMaxLength(256);

                entity.Property(e => e.PhoneNumber).HasMaxLength(20);

                entity.Property(e => e.UserName).HasMaxLength(256);
            });

            modelBuilder.Entity<TblAccessRight>(entity =>
            {
                entity.HasKey(e => e.AccessId);

                entity.ToTable("tblAccessRight");

                entity.Property(e => e.AccessId).HasColumnName("AccessID");

                entity.Property(e => e.Name).HasMaxLength(50);
            });

            modelBuilder.Entity<TblAddressMaster>(entity =>
            {
                entity.HasKey(e => e.AddressId);

                entity.ToTable("tblAddressMaster");

                entity.Property(e => e.AddressId).HasColumnName("Address_Id");

                entity.Property(e => e.Address1)
                    .HasMaxLength(150)
                    .IsUnicode(false);

                entity.Property(e => e.Address2)
                    .HasMaxLength(150)
                    .IsUnicode(false);

                entity.Property(e => e.CreatedDate)
                    .HasColumnType("datetime")
                    .HasDefaultValueSql("(getdate())");

                entity.Property(e => e.LandMark)
                    .HasMaxLength(150)
                    .IsUnicode(false);

                entity.Property(e => e.Latitude).HasColumnType("decimal(18, 12)");

                entity.Property(e => e.Longitude).HasColumnType("decimal(18, 12)");

                entity.Property(e => e.ModifiedDate)
                    .HasColumnType("datetime")
                    .HasDefaultValueSql("(getdate())");
            });

            modelBuilder.Entity<TblAddressTypeMaster>(entity =>
            {
                entity.HasKey(e => e.AddressTypeId);

                entity.ToTable("tblAddressTypeMaster");

                entity.Property(e => e.AddressTypeId).HasColumnName("AddressType_id");

                entity.Property(e => e.AddressTypeDescription)
                    .HasMaxLength(7)
                    .IsUnicode(false);
            });

            modelBuilder.Entity<TblBanner>(entity =>
            {
                entity.ToTable("tblBanner");

                entity.Property(e => e.CreatedDate).HasColumnType("datetime");

                entity.Property(e => e.Description).HasMaxLength(200);

                entity.Property(e => e.Image).HasMaxLength(250);

                entity.Property(e => e.ModifiedDate).HasColumnType("datetime");

                entity.Property(e => e.Name).HasMaxLength(100);

                entity.HasOne(d => d.BannerCategory)
                    .WithMany(p => p.TblBanner)
                    .HasForeignKey(d => d.BannerCategoryId)
                    .HasConstraintName("FK_tblBanner_tblBannerCategoryMaster");

                entity.HasOne(d => d.Store)
                    .WithMany(p => p.TblBanner)
                    .HasForeignKey(d => d.StoreId)
                    .HasConstraintName("FK_tblBanner_tblStore");
            });

            modelBuilder.Entity<TblBannerCategoryMaster>(entity =>
            {
                entity.ToTable("tblBannerCategoryMaster");

                entity.Property(e => e.CreatedDate).HasColumnType("datetime");

                entity.Property(e => e.ModifiedDate).HasColumnType("datetime");

                entity.Property(e => e.Name).HasMaxLength(100);
            });

            modelBuilder.Entity<TblCategoryMaster>(entity =>
            {
                entity.ToTable("tblCategoryMaster");

                entity.Property(e => e.CategoryName).HasMaxLength(50);

                entity.Property(e => e.CreatedDate).HasColumnType("datetime");

                entity.Property(e => e.Description).HasMaxLength(200);

                entity.Property(e => e.Image).HasMaxLength(250);

                entity.Property(e => e.ModifiedDate).HasColumnType("datetime");
            });

            modelBuilder.Entity<TblCityMaster>(entity =>
            {
                entity.ToTable("tblCityMaster");

                entity.Property(e => e.CreatedDate).HasColumnType("datetime");

                entity.Property(e => e.DistrictName)
                    .IsRequired()
                    .HasMaxLength(100);

                entity.Property(e => e.ModifiedDate).HasColumnType("datetime");

                entity.HasOne(d => d.State)
                    .WithMany(p => p.TblCityMaster)
                    .HasForeignKey(d => d.StateId)
                    .HasConstraintName("FK_tblDistrictMaster_tblStateMaster");
            });

            modelBuilder.Entity<TblEntityMaster>(entity =>
            {
                entity.HasKey(e => e.EntityId);

                entity.ToTable("tblEntityMaster");

                entity.Property(e => e.EntityId).HasColumnName("Entity_ID");

                entity.Property(e => e.EntityDescription)
                    .HasColumnName("Entity_Description")
                    .IsUnicode(false);

                entity.Property(e => e.EntityName)
                    .HasColumnName("Entity_Name")
                    .HasMaxLength(100)
                    .IsUnicode(false);

                entity.Property(e => e.InsertDate)
                    .HasColumnType("datetime")
                    .HasDefaultValueSql("(getdate())");

                entity.Property(e => e.LastUpdateDate).HasColumnType("datetime");
            });

            modelBuilder.Entity<TblImage>(entity =>
            {
                entity.ToTable("tblImage");

                entity.Property(e => e.Id).ValueGeneratedNever();

                entity.Property(e => e.CreatedDate).HasColumnType("datetime");

                entity.Property(e => e.ModifiedDate).HasColumnType("datetime");
            });

            modelBuilder.Entity<TblItemMaster>(entity =>
            {
                entity.ToTable("tblItemMaster");

                entity.Property(e => e.CreatedDate).HasColumnType("datetime");

                entity.Property(e => e.Description).HasMaxLength(200);

                entity.Property(e => e.Image).HasMaxLength(250);

                entity.Property(e => e.ItemName).HasMaxLength(100);

                entity.Property(e => e.ModifiedDate).HasColumnType("datetime");

                entity.HasOne(d => d.Category)
                    .WithMany(p => p.TblItemMaster)
                    .HasForeignKey(d => d.CategoryId)
                    .HasConstraintName("FK_tblItemMaster_tblCategoryMaster");
            });

            modelBuilder.Entity<TblNavigator>(entity =>
            {
                entity.HasKey(e => e.NavigatorId);

                entity.ToTable("tblNavigator");

                entity.Property(e => e.NavigatorId).HasColumnName("Navigator_Id");

                entity.Property(e => e.Class)
                    .HasMaxLength(50)
                    .IsUnicode(false);

                entity.Property(e => e.DefaultOrder).HasColumnName("Default_Order");

                entity.Property(e => e.Description)
                    .HasMaxLength(200)
                    .IsUnicode(false);

                entity.Property(e => e.DisplayText)
                    .IsRequired()
                    .HasColumnName("Display_Text")
                    .HasMaxLength(150)
                    .IsUnicode(false);

                entity.Property(e => e.Icon)
                    .HasColumnName("icon")
                    .HasMaxLength(50)
                    .IsUnicode(false);

                entity.Property(e => e.InsertDate).HasColumnType("datetime");

                entity.Property(e => e.Ishide).HasColumnName("ishide");

                entity.Property(e => e.LastUpdateDate).HasColumnType("datetime");

                entity.Property(e => e.NavLevel).HasColumnName("Nav_Level");

                entity.Property(e => e.ParantId).HasColumnName("Parant_Id");

                entity.Property(e => e.PrivilegeId).HasColumnName("Privilege_Id");

                entity.Property(e => e.Url)
                    .HasColumnName("URL")
                    .HasMaxLength(500)
                    .IsUnicode(false);
            });

            modelBuilder.Entity<TblOrder>(entity =>
            {
                entity.ToTable("tblOrder");

                entity.Property(e => e.Cgst)
                    .HasColumnName("CGST")
                    .HasColumnType("decimal(18, 2)");

                entity.Property(e => e.CouponDiscount).HasColumnType("decimal(18, 2)");

                entity.Property(e => e.CreatedDate).HasColumnType("datetime");

                entity.Property(e => e.CustomerNotes).HasMaxLength(250);

                entity.Property(e => e.DeliverNotes).HasMaxLength(250);

                entity.Property(e => e.DeliveryDate).HasColumnType("datetime");

                entity.Property(e => e.GrandTotal).HasColumnType("decimal(18, 2)");

                entity.Property(e => e.Gst).HasColumnName("GST");

                entity.Property(e => e.Igst)
                    .HasColumnName("IGST")
                    .HasColumnType("decimal(18, 2)");

                entity.Property(e => e.InvoiceNo).HasMaxLength(100);

                entity.Property(e => e.ModifiedDate).HasColumnType("datetime");

                entity.Property(e => e.OnServiceNotes).HasMaxLength(250);

                entity.Property(e => e.OrderAmount).HasColumnType("decimal(18, 3)");

                entity.Property(e => e.OrderDate).HasColumnType("datetime");

                entity.Property(e => e.PackageDiscount).HasColumnType("decimal(18, 2)");

                entity.Property(e => e.PickupDate).HasColumnType("datetime");

                entity.Property(e => e.Sgst)
                    .HasColumnName("SGST")
                    .HasColumnType("decimal(18, 2)");

                entity.Property(e => e.ShippingCharge).HasColumnType("decimal(18, 2)");

                entity.Property(e => e.TotalDiscount).HasColumnType("decimal(18, 2)");

                entity.Property(e => e.TotalIbags).HasColumnName("TotalIBags");

                entity.Property(e => e.TotalItemKg)
                    .HasColumnName("TotalItemKG")
                    .HasColumnType("decimal(18, 2)");
            });

            modelBuilder.Entity<TblOrderItems>(entity =>
            {
                entity.ToTable("tblOrderItems");

                entity.Property(e => e.CreatedDate).HasColumnType("datetime");

                entity.Property(e => e.ModifiedDate).HasColumnType("datetime");

                entity.Property(e => e.Price).HasColumnType("decimal(18, 2)");

                entity.HasOne(d => d.Order)
                    .WithMany(p => p.TblOrderItems)
                    .HasForeignKey(d => d.OrderId)
                    .HasConstraintName("FK_tblOrderItems_tblOrder");

                entity.HasOne(d => d.StoreItem)
                    .WithMany(p => p.TblOrderItems)
                    .HasForeignKey(d => d.StoreItemId)
                    .HasConstraintName("FK_tblOrderItems_tblStoreItems");
            });

            modelBuilder.Entity<TblOrderStatus>(entity =>
            {
                entity.ToTable("tblOrderStatus");

                entity.Property(e => e.CreatedDate).HasColumnType("datetime");

                entity.Property(e => e.ModifiedDate).HasColumnType("datetime");

                entity.Property(e => e.Name)
                    .HasMaxLength(50)
                    .IsUnicode(false);
            });

            modelBuilder.Entity<TblPayment>(entity =>
            {
                entity.ToTable("tblPayment");

                entity.Property(e => e.Id).ValueGeneratedNever();

                entity.Property(e => e.CreatedDate).HasColumnType("datetime");

                entity.Property(e => e.DeuAmount).HasColumnType("decimal(18, 3)");

                entity.Property(e => e.ModifiedDate).HasColumnType("datetime");

                entity.Property(e => e.PaidAmmount).HasColumnType("decimal(18, 3)");

                entity.Property(e => e.PaymentDate).HasColumnType("datetime");

                entity.Property(e => e.PaymentedAmount).HasColumnType("decimal(18, 3)");
            });

            modelBuilder.Entity<TblPaymentStatus>(entity =>
            {
                entity.ToTable("tblPaymentStatus");

                entity.Property(e => e.CreatedDate).HasColumnType("datetime");

                entity.Property(e => e.ModifiedDate).HasColumnType("datetime");

                entity.Property(e => e.Name)
                    .HasMaxLength(200)
                    .IsUnicode(false);
            });

            modelBuilder.Entity<TblPrivilegeBase>(entity =>
            {
                entity.HasKey(e => e.PrivilegeId);

                entity.ToTable("tblPrivilegeBase");

                entity.Property(e => e.PrivilegeId).ValueGeneratedNever();

                entity.Property(e => e.EntityId).HasColumnName("Entity_ID");

                entity.Property(e => e.InsertDate).HasColumnType("datetime");

                entity.Property(e => e.LastUpdateDate).HasColumnType("datetime");

                entity.Property(e => e.PrivilegeName)
                    .HasColumnName("Privilege_Name")
                    .HasMaxLength(100);
            });

            modelBuilder.Entity<TblServiceLabelMaster>(entity =>
            {
                entity.ToTable("tblServiceLabelMaster");

                entity.Property(e => e.CreatedDate).HasColumnType("datetime");

                entity.Property(e => e.LabelName).HasMaxLength(50);

                entity.Property(e => e.ModifiedDate).HasColumnType("datetime");
            });

            modelBuilder.Entity<TblServiceMaster>(entity =>
            {
                entity.ToTable("tblServiceMaster");

                entity.Property(e => e.CreatedDate).HasColumnType("datetime");

                entity.Property(e => e.Description).HasMaxLength(200);

                entity.Property(e => e.ModifiedDate).HasColumnType("datetime");

                entity.Property(e => e.ServiceImage).HasMaxLength(250);

                entity.Property(e => e.ServiceName).HasMaxLength(50);
            });

            modelBuilder.Entity<TblStateMaster>(entity =>
            {
                entity.ToTable("tblStateMaster");

                entity.Property(e => e.Country)
                    .IsRequired()
                    .HasMaxLength(100);

                entity.Property(e => e.CreateDate).HasColumnType("datetime");

                entity.Property(e => e.ModifiedDate).HasColumnType("datetime");

                entity.Property(e => e.StateName)
                    .IsRequired()
                    .HasMaxLength(100);
            });

            modelBuilder.Entity<TblStatusMaster>(entity =>
            {
                entity.ToTable("tblStatusMaster");

                entity.Property(e => e.CreatedDate).HasColumnType("datetime");

                entity.Property(e => e.Description).HasMaxLength(250);

                entity.Property(e => e.ModifiedDate).HasColumnType("datetime");

                entity.Property(e => e.StatusName)
                    .IsRequired()
                    .HasMaxLength(100);
            });

            modelBuilder.Entity<TblStore>(entity =>
            {
                entity.ToTable("tblStore");

                entity.Property(e => e.AddressLine1).HasMaxLength(200);

                entity.Property(e => e.AddressLine2).HasMaxLength(200);

                entity.Property(e => e.CityName).HasMaxLength(100);

                entity.Property(e => e.CreatedDate).HasColumnType("datetime");

                entity.Property(e => e.EmailId).HasMaxLength(100);

                entity.Property(e => e.Gstno)
                    .HasColumnName("GSTNo")
                    .HasMaxLength(20);

                entity.Property(e => e.Image).HasMaxLength(250);

                entity.Property(e => e.Latitude).HasColumnType("decimal(9, 6)");

                entity.Property(e => e.Longitude).HasColumnType("decimal(9, 6)");

                entity.Property(e => e.MobileNo)
                    .IsRequired()
                    .HasMaxLength(10);

                entity.Property(e => e.ModifiedDate).HasColumnType("datetime");

                entity.Property(e => e.PhoneNo).HasMaxLength(20);

                entity.Property(e => e.StoreCode)
                    .IsRequired()
                    .HasMaxLength(50);

                entity.Property(e => e.StoreName)
                    .IsRequired()
                    .HasMaxLength(150);

                entity.Property(e => e.ZipCode).HasMaxLength(10);

                entity.HasOne(d => d.District)
                    .WithMany(p => p.TblStore)
                    .HasForeignKey(d => d.DistrictId)
                    .HasConstraintName("FK_tblStoreDetail_tblDistrictMaster");
            });

            modelBuilder.Entity<TblStoreEmployees>(entity =>
            {
                entity.ToTable("tblStoreEmployees");

                entity.Property(e => e.CreatedDate).HasColumnType("datetime");

                entity.Property(e => e.ModifiedDate).HasColumnType("datetime");

                entity.HasOne(d => d.Store)
                    .WithMany(p => p.TblStoreEmployees)
                    .HasForeignKey(d => d.StoreId)
                    .HasConstraintName("FK_tblStoreEmployees_tblStore");
            });

            modelBuilder.Entity<TblStoreItems>(entity =>
            {
                entity.ToTable("tblStoreItems");

                entity.Property(e => e.CreatedDate).HasColumnType("datetime");

                entity.Property(e => e.ModifiedDate).HasColumnType("datetime");

                entity.Property(e => e.Price).HasColumnType("decimal(18, 2)");

                entity.HasOne(d => d.Item)
                    .WithMany(p => p.TblStoreItems)
                    .HasForeignKey(d => d.ItemId)
                    .HasConstraintName("FK_tblStoreItems_tblItemMaster");

                entity.HasOne(d => d.Service)
                    .WithMany(p => p.TblStoreItems)
                    .HasForeignKey(d => d.ServiceId)
                    .HasConstraintName("FK_tblStoreItems_tblServiceMaster");

                entity.HasOne(d => d.Store)
                    .WithMany(p => p.TblStoreItems)
                    .HasForeignKey(d => d.StoreId)
                    .HasConstraintName("FK_tblStoreItems_tblStore");

                entity.HasOne(d => d.SubService)
                    .WithMany(p => p.TblStoreItems)
                    .HasForeignKey(d => d.SubServiceId)
                    .HasConstraintName("FK_tblStoreItems_tblSubServiceMaster");

                entity.HasOne(d => d.Unit)
                    .WithMany(p => p.TblStoreItems)
                    .HasForeignKey(d => d.UnitId)
                    .HasConstraintName("FK_tblStoreItems_tblUnitMaster");
            });

            modelBuilder.Entity<TblStorePackagesAndCategoryMapping>(entity =>
            {
                entity.ToTable("tblStorePackagesAndCategoryMapping");

                entity.Property(e => e.CreatedDate).HasColumnType("datetime");

                entity.Property(e => e.Description).HasMaxLength(100);

                entity.Property(e => e.ModifiedDate).HasColumnType("datetime");

                entity.Property(e => e.Price).HasColumnType("decimal(18, 2)");

                entity.HasOne(d => d.Category)
                    .WithMany(p => p.TblStorePackagesAndCategoryMapping)
                    .HasForeignKey(d => d.CategoryId)
                    .HasConstraintName("FK_tblStoresPackageAndCategoryMapping_tblCategoryMaster");

                entity.HasOne(d => d.Package)
                    .WithMany(p => p.TblStorePackagesAndCategoryMapping)
                    .HasForeignKey(d => d.PackageId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK_tblStorePackagesAndCategoryMapping_tblStorePckages");

                entity.HasOne(d => d.Store)
                    .WithMany(p => p.TblStorePackagesAndCategoryMapping)
                    .HasForeignKey(d => d.StoreId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK_tblStorePackagesAndCategoryMapping_tblStore");
            });

            modelBuilder.Entity<TblStorePckages>(entity =>
            {
                entity.ToTable("tblStorePckages");

                entity.Property(e => e.CreatedDate).HasColumnType("datetime");

                entity.Property(e => e.ModifiedDate).HasColumnType("datetime");

                entity.Property(e => e.PackageName).HasMaxLength(100);

                entity.Property(e => e.Price).HasColumnType("decimal(18, 2)");

                entity.HasOne(d => d.Store)
                    .WithMany(p => p.TblStorePckages)
                    .HasForeignKey(d => d.StoreId)
                    .HasConstraintName("FK_tblStorePckages_tblStore");
            });

            modelBuilder.Entity<TblSubServiceMaster>(entity =>
            {
                entity.ToTable("tblSubServiceMaster");

                entity.Property(e => e.CreatedDate).HasColumnType("datetime");

                entity.Property(e => e.ModifiedDate).HasColumnType("datetime");

                entity.Property(e => e.SubServiceName).HasMaxLength(100);

                entity.HasOne(d => d.Service)
                    .WithMany(p => p.TblSubServiceMaster)
                    .HasForeignKey(d => d.ServiceId)
                    .HasConstraintName("FK_tblSubServiceId_tblServiceMaster");
            });

            modelBuilder.Entity<TblUnitMaster>(entity =>
            {
                entity.ToTable("tblUnitMaster");

                entity.Property(e => e.CreatedDate).HasColumnType("datetime");

                entity.Property(e => e.Description).HasMaxLength(50);

                entity.Property(e => e.ModifiedDate).HasColumnType("datetime");

                entity.Property(e => e.UnitName).HasMaxLength(50);
            });

            modelBuilder.Entity<TblUserAddress>(entity =>
            {
                entity.HasKey(e => e.UserAddressId);

                entity.ToTable("tblUserAddress");

                entity.Property(e => e.UserAddressId).HasColumnName("User_Address_id");

                entity.Property(e => e.AddressId).HasColumnName("Address_id");

                entity.Property(e => e.AddressTypeId).HasColumnName("AddressType_id");

                entity.Property(e => e.CreatedDate).HasColumnType("datetime");

                entity.Property(e => e.ModifiedDate).HasColumnType("datetime");

                entity.Property(e => e.UserId)
                    .HasColumnName("User_id")
                    .HasMaxLength(450);

                entity.HasOne(d => d.Address)
                    .WithMany(p => p.TblUserAddress)
                    .HasForeignKey(d => d.AddressId)
                    .HasConstraintName("FK_tblUserAddress_tblAddressMaster");

                entity.HasOne(d => d.AddressType)
                    .WithMany(p => p.TblUserAddress)
                    .HasForeignKey(d => d.AddressTypeId)
                    .HasConstraintName("FK_tblUserAddress_tblAddressTypeMaster");
            });

            modelBuilder.Entity<UsersMaster>(entity =>
            {
                entity.HasKey(e => e.UserId);

                entity.Property(e => e.UserId).ValueGeneratedNever();

                entity.Property(e => e.CreatedDate).HasColumnType("datetime");

                entity.Property(e => e.ModifiedDate).HasColumnType("datetime");

                entity.HasOne(d => d.User)
                    .WithOne(p => p.UsersMaster)
                    .HasForeignKey<UsersMaster>(d => d.UserId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK_UsersMaster_AspNetUsers");
            });
            modelBuilder.Entity<NavigatorView>(entity =>
            {
                entity.HasKey(e => e.Navigator_Id);
                entity.Property(e => e.Display_Text)
                .IsUnicode(false);
                entity.Property(e => e.URL)
                .IsUnicode(false);
                entity.Property(e => e.Description)
                    .IsUnicode(false);
                entity.Property(e => e.icon)
                    .IsUnicode(false);
                entity.Property(e => e.Class)
                    .IsUnicode(false);
            });
        }
    }
}
