using System;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata;

#nullable disable

namespace 鮮蔬果季_前台.Models
{
    public partial class 鮮蔬果季Context : DbContext
    {
        public 鮮蔬果季Context()
        {
        }

        public 鮮蔬果季Context(DbContextOptions<鮮蔬果季Context> options)
            : base(options)
        {
        }

        public virtual DbSet<Announce> Announces { get; set; }
        public virtual DbSet<BlogDetail> BlogDetails { get; set; }
        public virtual DbSet<Category> Categories { get; set; }
        public virtual DbSet<CategoryDetail> CategoryDetails { get; set; }
        public virtual DbSet<City> Cities { get; set; }
        public virtual DbSet<Coupon> Coupons { get; set; }
        public virtual DbSet<CouponDetail> CouponDetails { get; set; }
        public virtual DbSet<Event> Events { get; set; }
        public virtual DbSet<EventPhotoBank> EventPhotoBanks { get; set; }
        public virtual DbSet<EventRegistration> EventRegistrations { get; set; }
        public virtual DbSet<Feedback> Feedbacks { get; set; }
        public virtual DbSet<FeedbackResponse> FeedbackResponses { get; set; }
        public virtual DbSet<HomePageCover> HomePageCovers { get; set; }
        public virtual DbSet<Member> Members { get; set; }
        public virtual DbSet<MyFavorite> MyFavorites { get; set; }
        public virtual DbSet<Order> Orders { get; set; }
        public virtual DbSet<OrderDetail> OrderDetails { get; set; }
        public virtual DbSet<PayMethod> PayMethods { get; set; }
        public virtual DbSet<PhotoBank> PhotoBanks { get; set; }
        public virtual DbSet<Product> Products { get; set; }
        public virtual DbSet<ProductOnSale> ProductOnSales { get; set; }
        public virtual DbSet<ProductPhotoBank> ProductPhotoBanks { get; set; }
        public virtual DbSet<ProductPriceChange> ProductPriceChanges { get; set; }
        public virtual DbSet<Review> Reviews { get; set; }
        public virtual DbSet<ShoppingCart> ShoppingCarts { get; set; }
        public virtual DbSet<Status> Statuses { get; set; }
        public virtual DbSet<Supplier> Suppliers { get; set; }

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
//            if (!optionsBuilder.IsConfigured)
//            {
//#warning To protect potentially sensitive information in your connection string, you should move it out of source code. You can avoid scaffolding the connection string by using the Name= syntax to read it from configuration - see https://go.microsoft.com/fwlink/?linkid=2131148. For more guidance on storing connection strings, see http://go.microsoft.com/fwlink/?LinkId=723263.
//                optionsBuilder.UseSqlServer("Data Source=114.34.127.248\\DESKTOP-FBDIF9U,20221;Initial Catalog=鮮蔬果季;Persist Security Info=True;User ID=editorteam3;Password=msit1320000");
//            }
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<Announce>(entity =>
            {
                entity.ToTable("Announce");

                entity.Property(e => e.AnnounceId).HasColumnName("AnnounceID");

                entity.Property(e => e.Description)
                    .IsRequired()
                    .HasMaxLength(50)
                    .UseCollation("SQL_Latin1_General_CP1_CI_AS");
            });

            modelBuilder.Entity<BlogDetail>(entity =>
            {
                entity.HasNoKey();

                entity.Property(e => e.BlogDetailId)
                    .ValueGeneratedOnAdd()
                    .HasColumnName("BlogDetailID");

                entity.Property(e => e.Label)
                    .HasMaxLength(10)
                    .UseCollation("SQL_Latin1_General_CP1_CI_AS");

                entity.Property(e => e.LabelId).HasColumnName("LabelID");

                entity.Property(e => e.Maintext)
                    .HasMaxLength(500)
                    .UseCollation("SQL_Latin1_General_CP1_CI_AS");

                entity.Property(e => e.PhotoPath).UseCollation("SQL_Latin1_General_CP1_CI_AS");

                entity.Property(e => e.PublishedDate).HasColumnType("datetime");

                entity.Property(e => e.Subtitle)
                    .HasMaxLength(50)
                    .UseCollation("SQL_Latin1_General_CP1_CI_AS");

                entity.Property(e => e.SupplierId).HasColumnName("SupplierID");

                entity.Property(e => e.Title)
                    .HasMaxLength(50)
                    .UseCollation("SQL_Latin1_General_CP1_CI_AS");

                entity.HasOne(d => d.Supplier)
                    .WithMany()
                    .HasForeignKey(d => d.SupplierId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK_BlogDetails_Suppliers");
            });

            modelBuilder.Entity<Category>(entity =>
            {
                entity.Property(e => e.CategoryId).HasColumnName("CategoryID");

                entity.Property(e => e.CategoryName)
                    .IsRequired()
                    .HasMaxLength(10)
                    .UseCollation("SQL_Latin1_General_CP1_CI_AS");

                entity.Property(e => e.FatherCategoryId).HasColumnName("FatherCategoryID");

                entity.HasOne(d => d.FatherCategory)
                    .WithMany(p => p.InverseFatherCategory)
                    .HasForeignKey(d => d.FatherCategoryId)
                    .HasConstraintName("FK_Categories_Categories");
            });

            modelBuilder.Entity<CategoryDetail>(entity =>
            {
                entity.Property(e => e.CategoryDetailId).HasColumnName("CategoryDetailID");

                entity.Property(e => e.CategoryId).HasColumnName("CategoryID");

                entity.Property(e => e.ProductId).HasColumnName("ProductID");

                entity.HasOne(d => d.Category)
                    .WithMany(p => p.CategoryDetails)
                    .HasForeignKey(d => d.CategoryId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK_CategoryDetails_Categories");

                entity.HasOne(d => d.Product)
                    .WithMany(p => p.CategoryDetails)
                    .HasForeignKey(d => d.ProductId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK_CategoryDetails_Products");
            });

            modelBuilder.Entity<City>(entity =>
            {
                entity.Property(e => e.CityId).HasColumnName("CityID");

                entity.Property(e => e.CityName)
                    .IsRequired()
                    .HasMaxLength(10)
                    .UseCollation("SQL_Latin1_General_CP1_CI_AS");
            });

            modelBuilder.Entity<Coupon>(entity =>
            {
                entity.Property(e => e.CouponId).HasColumnName("CouponID");

                entity.Property(e => e.CouponDescription)
                    .IsRequired()
                    .HasMaxLength(50)
                    .UseCollation("SQL_Latin1_General_CP1_CI_AS");

                entity.Property(e => e.CouponEndDate).HasColumnType("datetime");

                entity.Property(e => e.CouponName)
                    .IsRequired()
                    .HasMaxLength(30)
                    .UseCollation("SQL_Latin1_General_CP1_CI_AS");

                entity.Property(e => e.CouponStartDate).HasColumnType("datetime");

                entity.Property(e => e.DiscountCondition).HasDefaultValueSql("((0))");
            });

            modelBuilder.Entity<CouponDetail>(entity =>
            {
                entity.HasKey(e => e.CouponDatilId);

                entity.Property(e => e.CouponDatilId).HasColumnName("CouponDatilID");

                entity.Property(e => e.CouponId).HasColumnName("CouponID");

                entity.Property(e => e.MemberId).HasColumnName("MemberID");

                entity.HasOne(d => d.Coupon)
                    .WithMany(p => p.CouponDetails)
                    .HasForeignKey(d => d.CouponId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK_CouponDetails_Coupons");

                entity.HasOne(d => d.Member)
                    .WithMany(p => p.CouponDetails)
                    .HasForeignKey(d => d.MemberId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK_CouponDetails_Members");
            });

            modelBuilder.Entity<Event>(entity =>
            {
                entity.Property(e => e.EventId).HasColumnName("EventID");

                entity.Property(e => e.CategoryId).HasColumnName("CategoryID");

                entity.Property(e => e.EventDescription).UseCollation("SQL_Latin1_General_CP1_CI_AS");

                entity.Property(e => e.EventEndDate).HasColumnType("datetime");

                entity.Property(e => e.EventLocation)
                    .HasMaxLength(50)
                    .UseCollation("SQL_Latin1_General_CP1_CI_AS");

                entity.Property(e => e.EventName)
                    .HasMaxLength(50)
                    .UseCollation("SQL_Latin1_General_CP1_CI_AS");

                entity.Property(e => e.EventStartDate).HasColumnType("datetime");

                entity.Property(e => e.SupplierId).HasColumnName("SupplierID");

                entity.HasOne(d => d.Category)
                    .WithMany(p => p.Events)
                    .HasForeignKey(d => d.CategoryId)
                    .HasConstraintName("FK_Events_Categories");

                entity.HasOne(d => d.Supplier)
                    .WithMany(p => p.Events)
                    .HasForeignKey(d => d.SupplierId)
                    .HasConstraintName("FK_Events_Suppliers");
            });

            modelBuilder.Entity<EventPhotoBank>(entity =>
            {
                entity.HasKey(e => e.EventPhotoId);

                entity.ToTable("EventPhotoBank");

                entity.Property(e => e.EventPhotoId).HasColumnName("EventPhotoID");

                entity.Property(e => e.EventId).HasColumnName("EventID");

                entity.Property(e => e.PhotoPath)
                    .IsRequired()
                    .UseCollation("SQL_Latin1_General_CP1_CI_AS");

                entity.HasOne(d => d.Event)
                    .WithMany(p => p.EventPhotoBanks)
                    .HasForeignKey(d => d.EventId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK_EventPhotoBank_Events");
            });

            modelBuilder.Entity<EventRegistration>(entity =>
            {
                entity.ToTable("EventRegistration");

                entity.Property(e => e.EventRegistrationId).HasColumnName("EventRegistrationID");

                entity.Property(e => e.ContactEmail)
                    .HasMaxLength(50)
                    .UseCollation("SQL_Latin1_General_CP1_CI_AS");

                entity.Property(e => e.ContactMobile)
                    .HasMaxLength(10)
                    .UseCollation("SQL_Latin1_General_CP1_CI_AS");

                entity.Property(e => e.ContactName)
                    .HasMaxLength(50)
                    .UseCollation("SQL_Latin1_General_CP1_CI_AS");

                entity.Property(e => e.EventId).HasColumnName("EventID");

                entity.Property(e => e.FoodPreference)
                    .HasMaxLength(10)
                    .UseCollation("SQL_Latin1_General_CP1_CI_AS");

                entity.Property(e => e.MemberId).HasColumnName("MemberID");

                entity.Property(e => e.Remark)
                    .HasMaxLength(80)
                    .UseCollation("SQL_Latin1_General_CP1_CI_AS");

                entity.Property(e => e.SubmitDate).HasColumnType("datetime");

                entity.HasOne(d => d.Event)
                    .WithMany(p => p.EventRegistrations)
                    .HasForeignKey(d => d.EventId)
                    .HasConstraintName("FK_EventRegistration_Events");

                entity.HasOne(d => d.Member)
                    .WithMany(p => p.EventRegistrations)
                    .HasForeignKey(d => d.MemberId)
                    .HasConstraintName("FK_EventRegistration_Members");
            });

            modelBuilder.Entity<Feedback>(entity =>
            {
                entity.Property(e => e.FeedbackName)
                    .IsRequired()
                    .HasMaxLength(50)
                    .UseCollation("SQL_Latin1_General_CP1_CI_AS");
            });

            modelBuilder.Entity<FeedbackResponse>(entity =>
            {
                entity.ToTable("FeedbackResponse");

                entity.Property(e => e.FeedbackResponseId).HasColumnName("FeedbackResponseID");

                entity.Property(e => e.FeedbackComment)
                    .HasMaxLength(100)
                    .UseCollation("SQL_Latin1_General_CP1_CI_AS");

                entity.Property(e => e.OrderDetailId).HasColumnName("OrderDetailID");
            });

            modelBuilder.Entity<HomePageCover>(entity =>
            {
                entity.HasKey(e => e.CoverId);

                entity.ToTable("HomePageCover");

                entity.Property(e => e.CoverId).HasColumnName("CoverID");

                entity.Property(e => e.CoverDescription)
                    .HasMaxLength(50)
                    .UseCollation("SQL_Latin1_General_CP1_CI_AS");

                entity.Property(e => e.CoverPhotoPath)
                    .IsRequired()
                    .UseCollation("SQL_Latin1_General_CP1_CI_AS");

                entity.Property(e => e.CoverTitle1)
                    .IsRequired()
                    .HasMaxLength(20)
                    .HasDefaultValueSql("(N'歡迎來到鮮蔬果季')")
                    .UseCollation("SQL_Latin1_General_CP1_CI_AS");

                entity.Property(e => e.CoverTitle2)
                    .HasMaxLength(20)
                    .UseCollation("SQL_Latin1_General_CP1_CI_AS");
            });

            modelBuilder.Entity<Member>(entity =>
            {
                entity.Property(e => e.MemberId).HasColumnName("MemberID");

                entity.Property(e => e.BirthDate).HasColumnType("date");

                entity.Property(e => e.CityId).HasColumnName("CityID");

                entity.Property(e => e.Gender)
                    .HasMaxLength(2)
                    .IsFixedLength(true)
                    .UseCollation("SQL_Latin1_General_CP1_CI_AS");

                entity.Property(e => e.MemberAddress)
                    .IsRequired()
                    .HasMaxLength(30)
                    .UseCollation("SQL_Latin1_General_CP1_CI_AS");

                entity.Property(e => e.MemberName)
                    .IsRequired()
                    .HasMaxLength(10)
                    .UseCollation("SQL_Latin1_General_CP1_CI_AS");

                entity.Property(e => e.MemberPhotoPass)
                    .HasDefaultValueSql("('inihead.png')")
                    .UseCollation("SQL_Latin1_General_CP1_CI_AS");

                entity.Property(e => e.Mobile)
                    .IsRequired()
                    .HasMaxLength(10)
                    .UseCollation("SQL_Latin1_General_CP1_CI_AS");

                entity.Property(e => e.Password)
                    .IsRequired()
                    .HasMaxLength(10)
                    .UseCollation("SQL_Latin1_General_CP1_CI_AS");

                entity.Property(e => e.RegisteredDate).HasColumnType("datetime");

                entity.Property(e => e.UserId)
                    .IsRequired()
                    .HasMaxLength(30)
                    .HasColumnName("UserID")
                    .UseCollation("SQL_Latin1_General_CP1_CI_AS");

                entity.HasOne(d => d.City)
                    .WithMany(p => p.Members)
                    .HasForeignKey(d => d.CityId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK_Members_Cities");
            });

            modelBuilder.Entity<MyFavorite>(entity =>
            {
                entity.ToTable("MyFavorite");

                entity.Property(e => e.MyFavoriteId).HasColumnName("MyFavoriteID");

                entity.Property(e => e.MemberId).HasColumnName("MemberID");

                entity.Property(e => e.ProductId).HasColumnName("ProductID");

                entity.Property(e => e.SupplierId).HasColumnName("SupplierID");

                entity.HasOne(d => d.Member)
                    .WithMany(p => p.MyFavorites)
                    .HasForeignKey(d => d.MemberId)
                    .HasConstraintName("FK_MyFavorite_Members");

                entity.HasOne(d => d.Product)
                    .WithMany(p => p.MyFavorites)
                    .HasForeignKey(d => d.ProductId)
                    .HasConstraintName("FK_MyFavorite_Products");

                entity.HasOne(d => d.Supplier)
                    .WithMany(p => p.MyFavorites)
                    .HasForeignKey(d => d.SupplierId)
                    .HasConstraintName("FK_MyFavorite_Suppliers");
            });

            modelBuilder.Entity<Order>(entity =>
            {
                entity.Property(e => e.OrderId).HasColumnName("OrderID");

                entity.Property(e => e.CouponId).HasColumnName("CouponID");

                entity.Property(e => e.MemberId).HasColumnName("MemberID");

                entity.Property(e => e.OrderDate).HasColumnType("datetime");

                entity.Property(e => e.PayMethodId).HasColumnName("PayMethodID");

                entity.Property(e => e.ShippedDate).HasColumnType("datetime");

                entity.Property(e => e.ShippedTo)
                    .IsRequired()
                    .UseCollation("SQL_Latin1_General_CP1_CI_AS");

                entity.Property(e => e.StatusId).HasColumnName("StatusID");

                entity.HasOne(d => d.Coupon)
                    .WithMany(p => p.Orders)
                    .HasForeignKey(d => d.CouponId)
                    .HasConstraintName("FK_Orders_Coupons1");

                entity.HasOne(d => d.Member)
                    .WithMany(p => p.Orders)
                    .HasForeignKey(d => d.MemberId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK_Orders_Members");

                entity.HasOne(d => d.Status)
                    .WithMany(p => p.Orders)
                    .HasForeignKey(d => d.StatusId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK_Orders_ShoppingStatus");
            });

            modelBuilder.Entity<OrderDetail>(entity =>
            {
                entity.Property(e => e.OrderDetailId).HasColumnName("OrderDetailID");

                entity.Property(e => e.OrderId).HasColumnName("OrderID");

                entity.Property(e => e.ProductId).HasColumnName("ProductID");

                entity.HasOne(d => d.Order)
                    .WithMany(p => p.OrderDetails)
                    .HasForeignKey(d => d.OrderId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK_OrderDetails_Orders");

                entity.HasOne(d => d.Product)
                    .WithMany(p => p.OrderDetails)
                    .HasForeignKey(d => d.ProductId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK_OrderDetails_Products");
            });

            modelBuilder.Entity<PayMethod>(entity =>
            {
                entity.ToTable("PayMethod");

                entity.Property(e => e.PayMethodId).HasColumnName("PayMethodID");

                entity.Property(e => e.Method)
                    .IsRequired()
                    .HasMaxLength(20)
                    .UseCollation("SQL_Latin1_General_CP1_CI_AS");
            });

            modelBuilder.Entity<PhotoBank>(entity =>
            {
                entity.HasKey(e => e.PhotoId);

                entity.ToTable("PhotoBank");

                entity.Property(e => e.PhotoId).HasColumnName("PhotoID");

                entity.Property(e => e.Photo).IsRequired();

                entity.Property(e => e.ProductId).HasColumnName("ProductID");

                entity.HasOne(d => d.Product)
                    .WithMany(p => p.PhotoBanks)
                    .HasForeignKey(d => d.ProductId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK_PhotoBank_Products");
            });

            modelBuilder.Entity<Product>(entity =>
            {
                entity.Property(e => e.ProductId).HasColumnName("ProductID");

                entity.Property(e => e.ProduceDate).HasColumnType("datetime");

                entity.Property(e => e.ProductDescription)
                    .HasMaxLength(50)
                    .UseCollation("SQL_Latin1_General_CP1_CI_AS");

                entity.Property(e => e.ProductName)
                    .IsRequired()
                    .HasMaxLength(10)
                    .UseCollation("SQL_Latin1_General_CP1_CI_AS");

                entity.Property(e => e.ProductQrcode).HasColumnName("ProductQRCode");

                entity.Property(e => e.ProductSize)
                    .IsRequired()
                    .HasMaxLength(10)
                    .UseCollation("SQL_Latin1_General_CP1_CI_AS");

                entity.Property(e => e.SupplierId).HasColumnName("SupplierID");

                entity.HasOne(d => d.Supplier)
                    .WithMany(p => p.Products)
                    .HasForeignKey(d => d.SupplierId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK_Products_Suppliers");
            });

            modelBuilder.Entity<ProductOnSale>(entity =>
            {
                entity.HasKey(e => e.SaleId);

                entity.ToTable("ProductOnSale");

                entity.Property(e => e.SaleId).HasColumnName("SaleID");

                entity.Property(e => e.EndDate).HasColumnType("datetime");

                entity.Property(e => e.ProductId).HasColumnName("ProductID");

                entity.Property(e => e.SaleDescription)
                    .HasMaxLength(200)
                    .UseCollation("SQL_Latin1_General_CP1_CI_AS");

                entity.Property(e => e.SaleTitle)
                    .IsRequired()
                    .HasMaxLength(50)
                    .UseCollation("SQL_Latin1_General_CP1_CI_AS");

                entity.Property(e => e.StartDate).HasColumnType("datetime");

                entity.HasOne(d => d.Product)
                    .WithMany(p => p.ProductOnSales)
                    .HasForeignKey(d => d.ProductId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK_ProductOnSale_Products");
            });

            modelBuilder.Entity<ProductPhotoBank>(entity =>
            {
                entity.HasKey(e => e.ProductPhotoId);

                entity.ToTable("ProductPhotoBank");

                entity.Property(e => e.ProductPhotoId).HasColumnName("ProductPhotoID");

                entity.Property(e => e.PhotoPath)
                    .IsRequired()
                    .UseCollation("SQL_Latin1_General_CP1_CI_AS");

                entity.Property(e => e.ProductId).HasColumnName("ProductID");

                entity.HasOne(d => d.Product)
                    .WithMany(p => p.ProductPhotoBanks)
                    .HasForeignKey(d => d.ProductId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK_ProductPhotoBank_Products");
            });

            modelBuilder.Entity<ProductPriceChange>(entity =>
            {
                entity.ToTable("ProductPriceChange");

                entity.Property(e => e.ProductPriceChangeId).HasColumnName("ProductPriceChangeID");

                entity.Property(e => e.DateChanged).HasColumnType("datetime");

                entity.Property(e => e.ProductId).HasColumnName("ProductID");

                entity.HasOne(d => d.Product)
                    .WithMany(p => p.ProductPriceChanges)
                    .HasForeignKey(d => d.ProductId)
                    .HasConstraintName("FK_ProductPriceChange_Products");
            });

            modelBuilder.Entity<Review>(entity =>
            {
                entity.Property(e => e.ReviewId).HasColumnName("ReviewID");

                entity.Property(e => e.Comments)
                    .HasMaxLength(50)
                    .UseCollation("SQL_Latin1_General_CP1_CI_AS");

                entity.Property(e => e.OrderDetailId).HasColumnName("OrderDetailID");

                entity.Property(e => e.ReviewDate).HasColumnType("datetime");

                entity.HasOne(d => d.OrderDetail)
                    .WithMany(p => p.Reviews)
                    .HasForeignKey(d => d.OrderDetailId)
                    .HasConstraintName("FK_Reviews_OrderDetails");
            });

            modelBuilder.Entity<ShoppingCart>(entity =>
            {
                entity.ToTable("ShoppingCart");

                entity.Property(e => e.ShoppingCartId).HasColumnName("ShoppingCartID");

                entity.Property(e => e.MemberId).HasColumnName("MemberID");

                entity.Property(e => e.ProductId).HasColumnName("ProductID");

                entity.Property(e => e.StatusId).HasColumnName("StatusID");

                entity.HasOne(d => d.Member)
                    .WithMany(p => p.ShoppingCarts)
                    .HasForeignKey(d => d.MemberId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK_ShoppingCart_Members");

                entity.HasOne(d => d.Product)
                    .WithMany(p => p.ShoppingCarts)
                    .HasForeignKey(d => d.ProductId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK_ShoppingCart_Products");

                entity.HasOne(d => d.Status)
                    .WithMany(p => p.ShoppingCarts)
                    .HasForeignKey(d => d.StatusId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK_ShoppingCart_ShoppingStatus");
            });

            modelBuilder.Entity<Status>(entity =>
            {
                entity.ToTable("Status");

                entity.Property(e => e.StatusId).HasColumnName("StatusID");

                entity.Property(e => e.StatusName)
                    .IsRequired()
                    .HasMaxLength(10)
                    .IsFixedLength(true)
                    .UseCollation("SQL_Latin1_General_CP1_CI_AS");
            });

            modelBuilder.Entity<Supplier>(entity =>
            {
                entity.Property(e => e.SupplierId).HasColumnName("SupplierID");

                entity.Property(e => e.BusinessOwner)
                    .IsRequired()
                    .HasMaxLength(10)
                    .UseCollation("SQL_Latin1_General_CP1_CI_AS");

                entity.Property(e => e.CityId).HasColumnName("CityID");

                entity.Property(e => e.Mobile)
                    .HasMaxLength(30)
                    .UseCollation("SQL_Latin1_General_CP1_CI_AS");

                entity.Property(e => e.SupplierAccount)
                    .HasMaxLength(50)
                    .UseCollation("SQL_Latin1_General_CP1_CI_AS");

                entity.Property(e => e.SupplierAddress)
                    .IsRequired()
                    .HasMaxLength(50)
                    .UseCollation("SQL_Latin1_General_CP1_CI_AS");

                entity.Property(e => e.SupplierName)
                    .IsRequired()
                    .HasMaxLength(30)
                    .UseCollation("SQL_Latin1_General_CP1_CI_AS");

                entity.Property(e => e.SupplierPassword)
                    .HasMaxLength(50)
                    .UseCollation("SQL_Latin1_General_CP1_CI_AS");

                entity.Property(e => e.SupplierProfile).UseCollation("SQL_Latin1_General_CP1_CI_AS");

                entity.HasOne(d => d.City)
                    .WithMany(p => p.Suppliers)
                    .HasForeignKey(d => d.CityId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK_Suppliers_Cities");
            });

            OnModelCreatingPartial(modelBuilder);
        }

        partial void OnModelCreatingPartial(ModelBuilder modelBuilder);
    }
}
