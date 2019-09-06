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
        public virtual DbSet<MenuMaster> MenuMaster { get; set; }
        public virtual DbSet<TblAddressMaster> TblAddressMaster { get; set; }
        public virtual DbSet<TblAddressTypeMaster> TblAddressTypeMaster { get; set; }
        public virtual DbSet<TblCategory> TblCategory { get; set; }
        public virtual DbSet<TblImage> TblImage { get; set; }
        public virtual DbSet<TblItem> TblItem { get; set; }
        public virtual DbSet<TblItemPrinceMappingByStore> TblItemPrinceMappingByStore { get; set; }
        public virtual DbSet<TblOrder> TblOrder { get; set; }
        public virtual DbSet<TblOrderDetail> TblOrderDetail { get; set; }
        public virtual DbSet<TblOrderStatus> TblOrderStatus { get; set; }
        public virtual DbSet<TblPayment> TblPayment { get; set; }
        public virtual DbSet<TblPaymentStatus> TblPaymentStatus { get; set; }
        public virtual DbSet<TblService> TblService { get; set; }
        public virtual DbSet<TblStore> TblStore { get; set; }
        public virtual DbSet<TblSubcategory> TblSubcategory { get; set; }
        public virtual DbSet<TblUserAddress> TblUserAddress { get; set; }
        public virtual DbSet<Users> Users { get; set; }
        public virtual DbSet<UserAddressDataViewModel> AddressDataModels { get; set; }
        public object UsersInRoles { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.HasAnnotation("ProductVersion", "2.2.3-servicing-35854");

            modelBuilder.Entity<AspNetRoleClaims>(entity =>
            {
                entity.HasIndex(e => e.RoleId);

                entity.Property(e => e.RoleId).IsRequired();

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

                entity.Property(e => e.Id).ValueGeneratedNever();

                entity.Property(e => e.Name).HasMaxLength(256);

                entity.Property(e => e.NormalizedName).HasMaxLength(256);
            });

            modelBuilder.Entity<AspNetUserClaims>(entity =>
            {
                entity.HasIndex(e => e.UserId);

                entity.Property(e => e.UserId).IsRequired();

                entity.HasOne(d => d.User)
                    .WithMany(p => p.AspNetUserClaims)
                    .HasForeignKey(d => d.UserId);
            });

            modelBuilder.Entity<AspNetUserLogins>(entity =>
            {
                entity.HasKey(e => new { e.LoginProvider, e.ProviderKey });

                entity.HasIndex(e => e.UserId);

                entity.Property(e => e.UserId).IsRequired();

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

                entity.Property(e => e.Id).ValueGeneratedNever();

                entity.Property(e => e.CreatedBy).HasMaxLength(100);

                entity.Property(e => e.DeviceType).HasMaxLength(1);

                entity.Property(e => e.Dob).HasColumnName("DOB");

                entity.Property(e => e.Email).HasMaxLength(256);

                entity.Property(e => e.Fcmtoken).HasColumnName("FCMToken");

                entity.Property(e => e.FullName).HasMaxLength(50);

                entity.Property(e => e.ModifiedBy).HasMaxLength(100);

                entity.Property(e => e.NormalizedEmail).HasMaxLength(256);

                entity.Property(e => e.NormalizedUserName).HasMaxLength(256);

                entity.Property(e => e.UserName).HasMaxLength(256);
            });

            modelBuilder.Entity<MenuMaster>(entity =>
            {
                entity.HasKey(e => new { e.MenuIdentity, e.MenuId, e.MenuName });

                entity.Property(e => e.MenuIdentity).ValueGeneratedOnAdd();

                entity.Property(e => e.MenuId)
                    .HasColumnName("MenuID")
                    .HasMaxLength(30)
                    .IsUnicode(false);

                entity.Property(e => e.MenuName)
                    .HasMaxLength(30)
                    .IsUnicode(false);

                entity.Property(e => e.CreatedDate).HasColumnType("datetime");

                entity.Property(e => e.MenuFileName)
                    .IsRequired()
                    .HasMaxLength(100)
                    .IsUnicode(false);

                entity.Property(e => e.MenuUrl)
                    .IsRequired()
                    .HasColumnName("MenuURL")
                    .HasMaxLength(500)
                    .IsUnicode(false);

                entity.Property(e => e.ParentMenuId)
                    .IsRequired()
                    .HasColumnName("Parent_MenuID")
                    .HasMaxLength(30)
                    .IsUnicode(false);

                entity.Property(e => e.UseYn)
                    .HasColumnName("USE_YN")
                    .HasMaxLength(1)
                    .IsUnicode(false)
                    .HasDefaultValueSql("('Y')");

                entity.Property(e => e.UserRoll)
                    .IsRequired()
                    .HasColumnName("User_Roll")
                    .HasMaxLength(256)
                    .IsUnicode(false);
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

                entity.Property(e => e.Latitude).HasColumnType("decimal(9, 6)");

                entity.Property(e => e.Longitude).HasColumnType("decimal(9, 6)");

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

            modelBuilder.Entity<TblCategory>(entity =>
            {
                entity.ToTable("tblCategory");

                entity.Property(e => e.Id).ValueGeneratedNever();

                entity.Property(e => e.CreatedDate).HasColumnType("datetime");

                entity.Property(e => e.ModifiedDate).HasColumnType("datetime");

                entity.Property(e => e.Name).HasMaxLength(200);

                entity.HasOne(d => d.Service)
                    .WithMany(p => p.TblCategory)
                    .HasForeignKey(d => d.ServiceId)
                    .HasConstraintName("FK__tblCatego__Servi__1DB06A4F");
            });

            modelBuilder.Entity<TblImage>(entity =>
            {
                entity.ToTable("tblImage");

                entity.Property(e => e.Id).ValueGeneratedNever();

                entity.Property(e => e.CreatedDate).HasColumnType("datetime");

                entity.Property(e => e.ModifiedDate).HasColumnType("datetime");

                entity.Property(e => e.Name).HasMaxLength(200);
            });

            modelBuilder.Entity<TblItem>(entity =>
            {
                entity.ToTable("tblItem");

                entity.Property(e => e.Id).ValueGeneratedNever();

                entity.Property(e => e.CreatedDate).HasColumnType("datetime");

                entity.Property(e => e.Image).HasMaxLength(255);

                entity.Property(e => e.ModifiedDate).HasColumnType("datetime");

                entity.Property(e => e.Name).HasMaxLength(200);

                entity.HasOne(d => d.Service)
                    .WithMany(p => p.TblItem)
                    .HasForeignKey(d => d.ServiceId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK__tblItem__Service__534D60F1");
            });

            modelBuilder.Entity<TblItemPrinceMappingByStore>(entity =>
            {
                entity.ToTable("tblItemPrinceMappingByStore");

                entity.Property(e => e.Id).ValueGeneratedNever();

                entity.Property(e => e.CreatedDate).HasColumnType("datetime");

                entity.Property(e => e.ModifiedDate).HasColumnType("datetime");

                entity.Property(e => e.Price).HasColumnType("decimal(18, 3)");

                entity.HasOne(d => d.Item)
                    .WithMany(p => p.TblItemPrinceMappingByStore)
                    .HasForeignKey(d => d.ItemId)
                    .HasConstraintName("FK__tblItemPr__ItemI__0E6E26BF");

                entity.HasOne(d => d.Store)
                    .WithMany(p => p.TblItemPrinceMappingByStore)
                    .HasForeignKey(d => d.StoreId)
                    .HasConstraintName("FK__tblItemPr__Store__0F624AF8");
            });

            modelBuilder.Entity<TblOrder>(entity =>
            {
                entity.ToTable("tblOrder");

                entity.Property(e => e.Id).ValueGeneratedNever();

                entity.Property(e => e.CreatedDate).HasColumnType("datetime");

                entity.Property(e => e.ModifiedDate).HasColumnType("datetime");

                entity.Property(e => e.OrderCity)
                    .HasMaxLength(200)
                    .IsUnicode(false);

                entity.Property(e => e.OrderDate).HasColumnType("datetime");

                entity.Property(e => e.OrderEmail).HasMaxLength(200);

                entity.Property(e => e.OrderPhone)
                    .HasMaxLength(20)
                    .IsUnicode(false);

                entity.Property(e => e.OrderShipName)
                    .HasMaxLength(200)
                    .IsUnicode(false);

                entity.Property(e => e.OrderSiping).HasColumnType("decimal(18, 3)");

                entity.Property(e => e.OrderState)
                    .HasMaxLength(200)
                    .IsUnicode(false);

                entity.Property(e => e.OrderTrakingNo).HasMaxLength(200);

                entity.Property(e => e.OrderZip)
                    .HasColumnName("OrderZIP")
                    .HasMaxLength(20)
                    .IsUnicode(false);

                entity.Property(e => e.OrdrGst)
                    .HasColumnName("OrdrGST")
                    .HasColumnType("decimal(2, 2)");

                entity.Property(e => e.OrdrShipAddress).IsUnicode(false);

                entity.Property(e => e.OrdrShipAddress2).IsUnicode(false);

                entity.Property(e => e.TotalAmount).HasColumnType("decimal(18, 3)");
            });

            modelBuilder.Entity<TblOrderDetail>(entity =>
            {
                entity.ToTable("tblOrderDetail");

                entity.Property(e => e.Id).ValueGeneratedNever();

                entity.Property(e => e.CreatedDate).HasColumnType("datetime");

                entity.Property(e => e.ModifiedDate).HasColumnType("datetime");

                entity.Property(e => e.Name)
                    .HasMaxLength(200)
                    .IsUnicode(false);

                entity.Property(e => e.Price).HasColumnType("decimal(18, 3)");
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

            modelBuilder.Entity<TblService>(entity =>
            {
                entity.ToTable("tblService");

                entity.Property(e => e.Id).ValueGeneratedNever();

                entity.Property(e => e.CreatedDate).HasColumnType("datetime");

                entity.Property(e => e.ModifiedDate).HasColumnType("datetime");

                entity.Property(e => e.Name).HasMaxLength(200);

                entity.Property(e => e.ServiceImage).HasMaxLength(400);
            });

            modelBuilder.Entity<TblStore>(entity =>
            {
                entity.ToTable("tblStore");

                entity.Property(e => e.Id).ValueGeneratedNever();

                entity.Property(e => e.CityName)
                    .HasMaxLength(200)
                    .IsUnicode(false);

                entity.Property(e => e.CreatedDate).HasColumnType("datetime");

                entity.Property(e => e.Image).HasMaxLength(255);

                entity.Property(e => e.Latitude).HasColumnType("decimal(30, 20)");

                entity.Property(e => e.LocationName)
                    .HasMaxLength(200)
                    .IsUnicode(false);

                entity.Property(e => e.Longitude).HasColumnType("decimal(30, 20)");

                entity.Property(e => e.ModifiedDate).HasColumnType("datetime");

                entity.Property(e => e.Name)
                    .HasMaxLength(200)
                    .IsUnicode(false);

                entity.Property(e => e.PinCode)
                    .HasMaxLength(10)
                    .IsUnicode(false);
            });

            modelBuilder.Entity<TblSubcategory>(entity =>
            {
                entity.ToTable("tblSubcategory");

                entity.Property(e => e.Id).ValueGeneratedNever();

                entity.Property(e => e.CreatedDate).HasColumnType("datetime");

                entity.Property(e => e.ModifiedDate).HasColumnType("datetime");

                entity.Property(e => e.Name).HasMaxLength(200);

                entity.Property(e => e.Price).HasColumnType("decimal(18, 9)");

                entity.HasOne(d => d.Category)
                    .WithMany(p => p.TblSubcategory)
                    .HasForeignKey(d => d.CategoryId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK__tblSubcat__Categ__1EA48E88");
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

                entity.HasOne(d => d.User)
                    .WithMany(p => p.TblUserAddress)
                    .HasForeignKey(d => d.UserId)
                    .HasConstraintName("FK_tblUserAddress_AspNetUsers1");
            });

            modelBuilder.Entity<Users>(entity =>
            {
                entity.Property(e => e.Id).ValueGeneratedNever();

                entity.Property(e => e.CreatedDate).HasColumnType("datetime");

                entity.Property(e => e.ModifiedDate).HasColumnType("datetime");

                entity.Property(e => e.UserId).HasMaxLength(450);

                entity.HasOne(d => d.User)
                    .WithMany(p => p.Users)
                    .HasForeignKey(d => d.UserId)
                    .HasConstraintName("FK_Users_Users1");
            });
        }
        public DataSet ExecuteStoreProcedure(string procedureName, params SqlParameter[] parameters)
        {
            DataSet dataSet = new DataSet();
            string conStr = Database.GetDbConnection().ConnectionString;
            SqlConnection sqlConn = new SqlConnection(conStr);
            SqlCommand cmdReport = new SqlCommand(procedureName, sqlConn);
            SqlDataAdapter daReport = new SqlDataAdapter(cmdReport);
            cmdReport.CommandTimeout = 1000;
            using (cmdReport)
            {
                cmdReport.CommandType = CommandType.StoredProcedure;
                cmdReport.Parameters.AddRange(parameters);
                daReport.Fill(dataSet);
            }

            return dataSet;
        }
    }
}
