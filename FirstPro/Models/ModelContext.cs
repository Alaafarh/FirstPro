using System;
using System.Collections.Generic;
using Microsoft.EntityFrameworkCore;

namespace FirstPro.Models;

public partial class ModelContext : DbContext
{
    public ModelContext()
    {
    }

    public ModelContext(DbContextOptions<ModelContext> options)
        : base(options)
    {
    }

    public virtual DbSet<Aboutu> Aboutus { get; set; }

    public virtual DbSet<Category> Categories { get; set; }

    public virtual DbSet<Checkout> Checkouts { get; set; }

    public virtual DbSet<Contact> Contacts { get; set; }

    public virtual DbSet<Home> Homes { get; set; }

    public virtual DbSet<Orderrecipe> Orderrecipes { get; set; }

    public virtual DbSet<Payment> Payments { get; set; }

    public virtual DbSet<Recipe> Recipes { get; set; }

    public virtual DbSet<Role> Roles { get; set; }

    public virtual DbSet<Testimonial> Testimonials { get; set; }

    public virtual DbSet<User> Users { get; set; }

    public virtual DbSet<Userlogin> Userlogins { get; set; }

    public virtual DbSet<Wishlist> Wishlists { get; set; }

    protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        => optionsBuilder.UseOracle("USER ID=C##MVCPRO;PASSWORD=Test321;DATA SOURCE=localhost:1521/xe");

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder
            .HasDefaultSchema("C##MVCPRO")
            .UseCollation("USING_NLS_COMP");

        modelBuilder.Entity<Aboutu>(entity =>
        {
            entity.HasKey(e => e.Aboutus).HasName("SYS_C008465");

            entity.ToTable("ABOUTUS");

            entity.Property(e => e.Aboutus)
                .ValueGeneratedOnAdd()
                .HasColumnType("NUMBER(38)")
                .HasColumnName("ABOUTUS");
            entity.Property(e => e.About)
                .HasMaxLength(100)
                .IsUnicode(false)
                .HasColumnName("ABOUT");
            entity.Property(e => e.Alltext)
                .HasMaxLength(100)
                .IsUnicode(false)
                .HasColumnName("ALLTEXT");
            entity.Property(e => e.Imageback)
                .HasMaxLength(200)
                .IsUnicode(false)
                .HasColumnName("IMAGEBACK");
            entity.Property(e => e.Imageform)
                .HasMaxLength(200)
                .IsUnicode(false)
                .HasColumnName("IMAGEFORM");
            entity.Property(e => e.Textinsideimage)
                .HasMaxLength(100)
                .IsUnicode(false)
                .HasColumnName("TEXTINSIDEIMAGE");
        });

        modelBuilder.Entity<Category>(entity =>
        {
            entity.HasKey(e => e.Categoryid).HasName("SYS_C008449");

            entity.ToTable("CATEGORY");

            entity.Property(e => e.Categoryid)
                .ValueGeneratedOnAdd()
                .HasColumnType("NUMBER(38)")
                .HasColumnName("CATEGORYID");
            entity.Property(e => e.Categoryname)
                .HasMaxLength(20)
                .IsUnicode(false)
                .HasColumnName("CATEGORYNAME");
            entity.Property(e => e.Imagepath)
                .HasMaxLength(200)
                .IsUnicode(false)
                .HasColumnName("IMAGEPATH");
        });

        modelBuilder.Entity<Checkout>(entity =>
        {
            entity.HasKey(e => new { e.Checkoutid, e.Userid }).HasName("SYS_C008473");

            entity.ToTable("CHECKOUT");

            entity.Property(e => e.Checkoutid)
                .ValueGeneratedOnAdd()
                .HasColumnType("NUMBER(38)")
                .HasColumnName("CHECKOUTID");
            entity.Property(e => e.Userid)
                .HasColumnType("NUMBER(38)")
                .HasColumnName("USERID");
            entity.Property(e => e.Fname)
                .HasMaxLength(20)
                .IsUnicode(false)
                .HasColumnName("FNAME");
            entity.Property(e => e.Lname)
                .HasMaxLength(20)
                .IsUnicode(false)
                .HasColumnName("LNAME");
            entity.Property(e => e.Phonenum)
                .HasMaxLength(20)
                .IsUnicode(false)
                .HasColumnName("PHONENUM");

            entity.HasOne(d => d.User).WithMany(p => p.Checkouts)
                .HasForeignKey(d => d.Userid)
                .OnDelete(DeleteBehavior.SetNull)
                .HasConstraintName("USER_PAYMENT");
        });

        modelBuilder.Entity<Contact>(entity =>
        {
            entity.HasKey(e => e.Contact1).HasName("SYS_C008469");

            entity.ToTable("CONTACT");

            entity.Property(e => e.Contact1)
                .ValueGeneratedOnAdd()
                .HasColumnType("NUMBER(38)")
                .HasColumnName("CONTACT");
            entity.Property(e => e.Email)
                .HasMaxLength(100)
                .IsUnicode(false)
                .HasColumnName("EMAIL");
            entity.Property(e => e.Massage)
                .HasMaxLength(200)
                .IsUnicode(false)
                .HasColumnName("MASSAGE");
            entity.Property(e => e.Name)
                .HasMaxLength(20)
                .IsUnicode(false)
                .HasColumnName("NAME");
            entity.Property(e => e.Phone)
                .HasMaxLength(20)
                .IsUnicode(false)
                .HasColumnName("PHONE");
        });

        modelBuilder.Entity<Home>(entity =>
        {
            entity.HasKey(e => e.Home1).HasName("SYS_C008467");

            entity.ToTable("HOME");

            entity.Property(e => e.Home1)
                .ValueGeneratedOnAdd()
                .HasColumnType("NUMBER(38)")
                .HasColumnName("HOME");
            entity.Property(e => e.Imageslider)
                .HasMaxLength(200)
                .IsUnicode(false)
                .HasColumnName("IMAGESLIDER");
            entity.Property(e => e.Textaboutus)
                .HasMaxLength(200)
                .IsUnicode(false)
                .HasColumnName("TEXTABOUTUS");
            entity.Property(e => e.Textcontact)
                .HasMaxLength(200)
                .IsUnicode(false)
                .HasColumnName("TEXTCONTACT");
            entity.Property(e => e.Textonimage)
                .HasMaxLength(200)
                .IsUnicode(false)
                .HasColumnName("TEXTONIMAGE");
            entity.Property(e => e.Textonvedio)
                .HasMaxLength(200)
                .IsUnicode(false)
                .HasColumnName("TEXTONVEDIO");
        });

        modelBuilder.Entity<Orderrecipe>(entity =>
        {
            entity.HasKey(e => e.Orderrecipe1).HasName("SYS_C008457");

            entity.ToTable("ORDERRECIPE");

            entity.Property(e => e.Orderrecipe1)
                .ValueGeneratedOnAdd()
                .HasColumnType("NUMBER(38)")
                .HasColumnName("ORDERRECIPE");
            entity.Property(e => e.Recipeid)
                .HasColumnType("NUMBER(38)")
                .HasColumnName("RECIPEID");
            entity.Property(e => e.Shopdate)
                .HasColumnType("DATE")
                .HasColumnName("SHOPDATE");
            entity.Property(e => e.Totalprice)
                .HasColumnType("NUMBER")
                .HasColumnName("TOTALPRICE");
            entity.Property(e => e.Userid)
                .HasColumnType("NUMBER(38)")
                .HasColumnName("USERID");

            entity.HasOne(d => d.Recipe).WithMany(p => p.Orderrecipes)
                .HasForeignKey(d => d.Recipeid)
                .OnDelete(DeleteBehavior.SetNull)
                .HasConstraintName("ORDERRECIPE");

            entity.HasOne(d => d.User).WithMany(p => p.Orderrecipes)
                .HasForeignKey(d => d.Userid)
                .OnDelete(DeleteBehavior.SetNull)
                .HasConstraintName("USER_ORDERRECIPE");
        });

        modelBuilder.Entity<Payment>(entity =>
        {
            entity.HasKey(e => new { e.Payment1, e.Userid }).HasName("SYS_C008476");

            entity.ToTable("PAYMENTS");

            entity.Property(e => e.Payment1)
                .ValueGeneratedOnAdd()
                .HasColumnType("NUMBER(38)")
                .HasColumnName("PAYMENT");
            entity.Property(e => e.Userid)
                .HasColumnType("NUMBER(38)")
                .HasColumnName("USERID");
            entity.Property(e => e.Amount)
                .HasColumnType("NUMBER(38)")
                .HasColumnName("AMOUNT");
            entity.Property(e => e.Cardnum)
                .HasMaxLength(100)
                .IsUnicode(false)
                .HasColumnName("CARDNUM");
            entity.Property(e => e.Cvv)
                .HasColumnType("NUMBER(38)")
                .HasColumnName("CVV");
            entity.Property(e => e.Expmonth)
                .HasColumnType("NUMBER(38)")
                .HasColumnName("EXPMONTH");
            entity.Property(e => e.Expyear)
                .HasColumnType("NUMBER(38)")
                .HasColumnName("EXPYEAR");
            entity.Property(e => e.Nameoncard)
                .HasMaxLength(100)
                .IsUnicode(false)
                .HasColumnName("NAMEONCARD");

            entity.HasOne(d => d.User).WithMany(p => p.Payments)
                .HasForeignKey(d => d.Userid)
                .OnDelete(DeleteBehavior.SetNull)
                .HasConstraintName("USER_PAYMENT1");
        });

        modelBuilder.Entity<Recipe>(entity =>
        {
            entity.HasKey(e => e.Recipeid).HasName("SYS_C008453");

            entity.ToTable("RECIPE");

            entity.Property(e => e.Recipeid)
                .ValueGeneratedOnAdd()
                .HasColumnType("NUMBER(38)")
                .HasColumnName("RECIPEID");
            entity.Property(e => e.Categoryid)
                .HasColumnType("NUMBER(38)")
                .HasColumnName("CATEGORYID");
            entity.Property(e => e.Creatdate)
                .HasColumnType("DATE")
                .HasColumnName("CREATDATE");
            entity.Property(e => e.Discription)
                .HasColumnType("CLOB")
                .HasColumnName("DISCRIPTION");
            entity.Property(e => e.Flag)
                .HasColumnType("NUMBER(38)")
                .HasColumnName("FLAG");
            entity.Property(e => e.Imagepath)
                .HasMaxLength(200)
                .IsUnicode(false)
                .HasColumnName("IMAGEPATH");
            entity.Property(e => e.Price)
                .HasColumnType("NUMBER(38)")
                .HasColumnName("PRICE");
            entity.Property(e => e.Recipename)
                .HasMaxLength(100)
                .IsUnicode(false)
                .HasColumnName("RECIPENAME");
            entity.Property(e => e.Userid)
                .HasColumnType("NUMBER(38)")
                .HasColumnName("USERID");

            entity.HasOne(d => d.Category).WithMany(p => p.Recipes)
                .HasForeignKey(d => d.Categoryid)
                .OnDelete(DeleteBehavior.SetNull)
                .HasConstraintName("CATEGORYID");

            entity.HasOne(d => d.User).WithMany(p => p.Recipes)
                .HasForeignKey(d => d.Userid)
                .OnDelete(DeleteBehavior.SetNull)
                .HasConstraintName("USER_RECIPE");
        });

        modelBuilder.Entity<Role>(entity =>
        {
            entity.HasKey(e => e.Roleid).HasName("SYS_C008435");

            entity.ToTable("ROLE");

            entity.Property(e => e.Roleid)
                .ValueGeneratedOnAdd()
                .HasColumnType("NUMBER(38)")
                .HasColumnName("ROLEID");
            entity.Property(e => e.Rolename)
                .HasMaxLength(20)
                .IsUnicode(false)
                .HasColumnName("ROLENAME");
        });

        modelBuilder.Entity<Testimonial>(entity =>
        {
            entity.HasKey(e => e.Testimonialid).HasName("SYS_C008446");

            entity.ToTable("TESTIMONIAL");

            entity.Property(e => e.Testimonialid)
                .ValueGeneratedOnAdd()
                .HasColumnType("NUMBER(38)")
                .HasColumnName("TESTIMONIALID");
            entity.Property(e => e.CommentUser)
                .HasColumnType("CLOB")
                .HasColumnName("COMMENT_USER");
            entity.Property(e => e.Flag)
                .HasColumnType("NUMBER(38)")
                .HasColumnName("FLAG");
            entity.Property(e => e.Userid)
                .HasColumnType("NUMBER(38)")
                .HasColumnName("USERID");

            entity.HasOne(d => d.User).WithMany(p => p.Testimonials)
                .HasForeignKey(d => d.Userid)
                .OnDelete(DeleteBehavior.SetNull)
                .HasConstraintName("USER_COMMENT");
        });

        modelBuilder.Entity<User>(entity =>
        {
            entity.HasKey(e => e.UserId).HasName("SYS_C008437");

            entity.ToTable("USERS");

            entity.Property(e => e.UserId)
                .ValueGeneratedOnAdd()
                .HasColumnType("NUMBER(38)")
                .HasColumnName("USER_ID");
            entity.Property(e => e.Adress)
                .HasMaxLength(100)
                .IsUnicode(false)
                .HasColumnName("ADRESS");
            entity.Property(e => e.Age)
                .HasColumnType("NUMBER(38)")
                .HasColumnName("AGE");
            entity.Property(e => e.Email)
                .HasColumnType("CLOB")
                .HasColumnName("EMAIL");
            entity.Property(e => e.Flagchif)
                .HasColumnType("NUMBER")
                .HasColumnName("FLAGCHIF");
            entity.Property(e => e.Fname)
                .HasMaxLength(20)
                .IsUnicode(false)
                .HasColumnName("FNAME");
            entity.Property(e => e.Imagepath)
                .HasMaxLength(200)
                .IsUnicode(false)
                .HasColumnName("IMAGEPATH");
            entity.Property(e => e.Jop)
                .HasMaxLength(20)
                .IsUnicode(false)
                .HasColumnName("JOP");
            entity.Property(e => e.Lname)
                .HasMaxLength(20)
                .IsUnicode(false)
                .HasColumnName("LNAME");
            entity.Property(e => e.Roleid)
                .HasColumnType("NUMBER(38)")
                .HasColumnName("ROLEID");

            entity.HasOne(d => d.Role).WithMany(p => p.Users)
                .HasForeignKey(d => d.Roleid)
                .OnDelete(DeleteBehavior.SetNull)
                .HasConstraintName("USER_ROLE");
        });

        modelBuilder.Entity<Userlogin>(entity =>
        {
            entity.HasKey(e => e.UserloginId).HasName("SYS_C008442");

            entity.ToTable("USERLOGIN");

            entity.Property(e => e.UserloginId)
                .ValueGeneratedOnAdd()
                .HasColumnType("NUMBER(38)")
                .HasColumnName("USERLOGIN_ID");
            entity.Property(e => e.Password)
                .HasMaxLength(100)
                .IsUnicode(false)
                .HasColumnName("PASSWORD");
            entity.Property(e => e.Roleid)
                .HasColumnType("NUMBER(38)")
                .HasColumnName("ROLEID");
            entity.Property(e => e.Userid)
                .HasColumnType("NUMBER(38)")
                .HasColumnName("USERID");
            entity.Property(e => e.Username)
                .HasMaxLength(100)
                .IsUnicode(false)
                .HasColumnName("USERNAME");

            entity.HasOne(d => d.Role).WithMany(p => p.Userlogins)
                .HasForeignKey(d => d.Roleid)
                .OnDelete(DeleteBehavior.SetNull)
                .HasConstraintName("USER_ROLELOG");

            entity.HasOne(d => d.User).WithMany(p => p.Userlogins)
                .HasForeignKey(d => d.Userid)
                .OnDelete(DeleteBehavior.SetNull)
                .HasConstraintName("USER_LOG");
        });

        modelBuilder.Entity<Wishlist>(entity =>
        {
            entity.HasKey(e => e.Wishlist1).HasName("SYS_C008461");

            entity.ToTable("WISHLIST");

            entity.Property(e => e.Wishlist1)
                .ValueGeneratedOnAdd()
                .HasColumnType("NUMBER(38)")
                .HasColumnName("WISHLIST");
            entity.Property(e => e.Recipeid)
                .HasColumnType("NUMBER(38)")
                .HasColumnName("RECIPEID");
            entity.Property(e => e.Userid)
                .HasColumnType("NUMBER(38)")
                .HasColumnName("USERID");

            entity.HasOne(d => d.Recipe).WithMany(p => p.Wishlists)
                .HasForeignKey(d => d.Recipeid)
                .OnDelete(DeleteBehavior.SetNull)
                .HasConstraintName("RECIPE_WISHLIST");

            entity.HasOne(d => d.User).WithMany(p => p.Wishlists)
                .HasForeignKey(d => d.Userid)
                .OnDelete(DeleteBehavior.SetNull)
                .HasConstraintName("USER_WISHLIST");
        });

        OnModelCreatingPartial(modelBuilder);
    }

    partial void OnModelCreatingPartial(ModelBuilder modelBuilder);
}
