using BusinessObjects;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using System;
using System.Collections.Generic;

namespace Dao;

public partial class MetroTicketContext : DbContext
{
    public MetroTicketContext()
    {
    }

    public MetroTicketContext(DbContextOptions<MetroTicketContext> options)
        : base(options)
    {
    }

    public virtual DbSet<CustomerType> CustomerTypes { get; set; }

    public virtual DbSet<Ticket> Tickets { get; set; }

    public virtual DbSet<TicketPrice> TicketPrices { get; set; }

    public virtual DbSet<TicketType> TicketTypes { get; set; }

    public virtual DbSet<User> Users { get; set; }

    protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
#warning To protect potentially sensitive information in your connection string, you should move it out of source code. You can avoid scaffolding the connection string by using the Name= syntax to read it from configuration - see https://go.microsoft.com/fwlink/?linkid=2131148. For more guidance on storing connection strings, see https://go.microsoft.com/fwlink/?LinkId=723263.
        => optionsBuilder.UseSqlServer(GetConnectionString());

    private string GetConnectionString()
    {
        IConfiguration config = new ConfigurationBuilder()
             .SetBasePath(Directory.GetCurrentDirectory())
                    .AddJsonFile("appsettings.json", true, true)
                    .Build();
        var strConn = config["ConnectionStrings:DefaultConnection"];

        return strConn;
    }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<CustomerType>(entity =>
        {
            entity.HasKey(e => e.CustomerTypeId).HasName("PK__Customer__958B614CDEFA9FF8");

            entity.Property(e => e.CustomerTypeId)
                .ValueGeneratedNever()
                .HasColumnName("CustomerTypeID");
            entity.Property(e => e.TypeName)
                .HasMaxLength(50)
                .IsUnicode(false);
        });

        modelBuilder.Entity<Ticket>(entity =>
        {
            entity.HasKey(e => e.TicketId).HasName("PK__Tickets__712CC627428C3C05");

            entity.Property(e => e.TicketId)
                .HasMaxLength(36)
                .IsUnicode(false)
                .IsFixedLength()
                .HasColumnName("TicketID");
            entity.Property(e => e.PurchasedAt).HasColumnType("datetime");
            entity.Property(e => e.Status)
                .HasMaxLength(20)
                .IsUnicode(false);
            entity.Property(e => e.TicketPriceId).HasColumnName("TicketPriceID");

            entity.HasOne(d => d.TicketPrice).WithMany(p => p.Tickets)
                .HasForeignKey(d => d.TicketPriceId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK__Tickets__TicketP__571DF1D5");
        });

        modelBuilder.Entity<TicketPrice>(entity =>
        {
            entity.HasKey(e => e.TicketPriceId).HasName("PK__TicketPr__BE7DED9C294251D0");

            entity.HasIndex(e => new { e.CustomerTypeId, e.TicketTypeId }, "UQ__TicketPr__9346090866CCFDA6").IsUnique();

            entity.Property(e => e.TicketPriceId)
                .ValueGeneratedNever()
                .HasColumnName("TicketPriceID");
            entity.Property(e => e.CustomerTypeId).HasColumnName("CustomerTypeID");
            entity.Property(e => e.TicketTypeId).HasColumnName("TicketTypeID");

            entity.HasOne(d => d.CustomerType).WithMany(p => p.TicketPrices)
                .HasForeignKey(d => d.CustomerTypeId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK__TicketPri__Custo__4F7CD00D");

            entity.HasOne(d => d.TicketType).WithMany(p => p.TicketPrices)
                .HasForeignKey(d => d.TicketTypeId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK__TicketPri__Ticke__4E88ABD4");
        });

        modelBuilder.Entity<TicketType>(entity =>
        {
            entity.HasKey(e => e.TicketTypeId).HasName("PK__TicketTy__6CD684514E4E5F7D");

            entity.Property(e => e.TicketTypeId)
                .ValueGeneratedNever()
                .HasColumnName("TicketTypeID");
            entity.Property(e => e.TicketTypeName)
                .HasMaxLength(50)
                .IsUnicode(false);
        });

        modelBuilder.Entity<User>(entity =>
        {
            entity.HasKey(e => e.UserId).HasName("PK__Users__1788CCAC7C39319B");

            entity.HasIndex(e => e.Email, "UQ__Users__A9D105341B476472").IsUnique();

            entity.Property(e => e.UserId)
                .ValueGeneratedNever()
                .HasColumnName("UserID");
            entity.Property(e => e.Email)
                .HasMaxLength(100)
                .IsUnicode(false);
            entity.Property(e => e.Password)
                .HasMaxLength(100)
                .IsUnicode(false);
            entity.Property(e => e.RoleId).HasColumnName("RoleID");
            entity.Property(e => e.Username)
                .HasMaxLength(50)
                .IsUnicode(false);
        });

        OnModelCreatingPartial(modelBuilder);
    }

    partial void OnModelCreatingPartial(ModelBuilder modelBuilder);
}
