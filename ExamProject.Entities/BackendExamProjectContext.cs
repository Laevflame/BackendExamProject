using System;
using System.Collections.Generic;
using Microsoft.EntityFrameworkCore;

namespace ExamProject.Entities;

public partial class BackendExamProjectContext : DbContext
{

    public BackendExamProjectContext(DbContextOptions<BackendExamProjectContext> options)
        : base(options)
    {
    }

    public virtual DbSet<BookedTicket> BookedTickets { get; set; }

    public virtual DbSet<BookedTicketsDetail> BookedTicketsDetails { get; set; }

    public virtual DbSet<PaymentMethod> PaymentMethods { get; set; }

    public virtual DbSet<Ticket> Tickets { get; set; }

    public virtual DbSet<UserAccount> UserAccounts { get; set; }

    protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
    {
        if (!optionsBuilder.IsConfigured)
        {
            optionsBuilder.UseSqlServer("Server=localhost; Initial Catalog = backendExamProject; User id=sa2; Pwd = admin; Persist Security Info=False;Trusted_Connection=true;TrustServerCertificate=True");
        }
    }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<BookedTicket>(entity =>
        {
            entity.HasKey(e => e.BookedTicketId).HasName("PK__booked_t__CC5722BF1CD454B2");

            entity.ToTable("booked_tickets");

            entity.Property(e => e.BookedTicketId)
                .HasMaxLength(50)
                .IsUnicode(false)
                .HasColumnName("booked_ticket_id");
            entity.Property(e => e.BookedTicketChangeAmount)
                .HasColumnType("decimal(10, 2)")
                .HasColumnName("booked_ticket_change_amount");
            entity.Property(e => e.BookedTicketCreatedAt)
                .HasDefaultValueSql("(getdate())")
                .HasColumnType("datetime")
                .HasColumnName("booked_ticket_created_at");
            entity.Property(e => e.BookedTicketCreatedBy)
                .HasMaxLength(255)
                .IsUnicode(false)
                .HasColumnName("booked_ticket_created_by");
            entity.Property(e => e.BookedTicketDate)
                .HasDefaultValueSql("(getdate())")
                .HasColumnType("datetime")
                .HasColumnName("booked_ticket_date");
            entity.Property(e => e.BookedTicketModifiedAt)
                .HasColumnType("datetime")
                .HasColumnName("booked_ticket_modified_at");
            entity.Property(e => e.BookedTicketModifiedBy)
                .HasMaxLength(255)
                .IsUnicode(false)
                .HasColumnName("booked_ticket_modified_by");
            entity.Property(e => e.BookedTicketPaidAmount)
                .HasColumnType("decimal(10, 2)")
                .HasColumnName("booked_ticket_paid_amount");
            entity.Property(e => e.BookedTicketTotalPrice)
                .HasColumnType("decimal(10, 2)")
                .HasColumnName("booked_ticket_total_price");
            entity.Property(e => e.PaymentMethodId).HasColumnName("payment_method_id");
            entity.Property(e => e.UserAccountId)
                .HasMaxLength(50)
                .IsUnicode(false)
                .HasColumnName("user_account_id");

            entity.HasOne(d => d.PaymentMethod).WithMany(p => p.BookedTickets)
                .HasForeignKey(d => d.PaymentMethodId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK__booked_ti__payme__74AE54BC");

            entity.HasOne(d => d.UserAccount).WithMany(p => p.BookedTickets)
                .HasForeignKey(d => d.UserAccountId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK__booked_ti__user___72C60C4A");
        });

        modelBuilder.Entity<BookedTicketsDetail>(entity =>
        {
            entity.HasKey(e => e.BookedTicketDetailId).HasName("PK__booked_t__32E02427B4CA55A9");

            entity.ToTable("booked_tickets_detail");

            entity.Property(e => e.BookedTicketDetailId)
                .HasMaxLength(50)
                .IsUnicode(false)
                .HasColumnName("booked_ticket_detail_id");
            entity.Property(e => e.BookedTicketDetailsCreatedAt)
                .HasDefaultValueSql("(getdate())")
                .HasColumnType("datetime")
                .HasColumnName("booked_ticket_details_created_at");
            entity.Property(e => e.BookedTicketDetailsCreatedBy)
                .HasMaxLength(255)
                .IsUnicode(false)
                .HasColumnName("booked_ticket_details_created_by");
            entity.Property(e => e.BookedTicketDetailsModifiedAt)
                .HasColumnType("datetime")
                .HasColumnName("booked_ticket_details_modified_at");
            entity.Property(e => e.BookedTicketDetailsModifiedBy)
                .HasMaxLength(255)
                .IsUnicode(false)
                .HasColumnName("booked_ticket_details_modified_by");
            entity.Property(e => e.BookedTicketDetailsQuantity).HasColumnName("booked_ticket_details_quantity");
            entity.Property(e => e.BookedTicketDetailsSeatNumber)
                .HasMaxLength(255)
                .IsUnicode(false)
                .HasColumnName("booked_ticket_details_seat_number");
            entity.Property(e => e.BookedTicketDetailsSubtotalPrice)
                .HasColumnType("decimal(10, 2)")
                .HasColumnName("booked_ticket_details_subtotal_price");
            entity.Property(e => e.BookedTicketId)
                .HasMaxLength(50)
                .IsUnicode(false)
                .HasColumnName("booked_ticket_id");
            entity.Property(e => e.TicketId)
                .HasMaxLength(50)
                .IsUnicode(false)
                .HasColumnName("ticket_id");

            entity.HasOne(d => d.BookedTicket).WithMany(p => p.BookedTicketsDetails)
                .HasForeignKey(d => d.BookedTicketId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK__booked_ti__booke__787EE5A0");

            entity.HasOne(d => d.Ticket).WithMany(p => p.BookedTicketsDetails)
                .HasForeignKey(d => d.TicketId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK__booked_ti__ticke__797309D9");
        });

        modelBuilder.Entity<PaymentMethod>(entity =>
        {
            entity.HasKey(e => e.PaymentMethodId).HasName("PK__payment___8A3EA9EB778B4BE9");

            entity.ToTable("payment_methods");

            entity.Property(e => e.PaymentMethodId)
                .ValueGeneratedNever()
                .HasColumnName("payment_method_id");
            entity.Property(e => e.MethodName)
                .HasMaxLength(100)
                .IsUnicode(false)
                .HasColumnName("method_name");
            entity.Property(e => e.PaymentMethodsCreatedAt)
                .HasDefaultValueSql("(getdate())")
                .HasColumnType("datetime")
                .HasColumnName("payment_methods_created_at");
            entity.Property(e => e.PaymentMethodsCreatedBy)
                .HasMaxLength(255)
                .IsUnicode(false)
                .HasColumnName("payment_methods_created_by");
            entity.Property(e => e.PaymentMethodsModifiedAt)
                .HasColumnType("datetime")
                .HasColumnName("payment_methods_modified_at");
            entity.Property(e => e.PaymentMethodsModifiedBy)
                .HasMaxLength(255)
                .IsUnicode(false)
                .HasColumnName("payment_methods_modified_by");
        });

        modelBuilder.Entity<Ticket>(entity =>
        {
            entity.HasKey(e => e.TicketId).HasName("PK__tickets__D596F96B6873A8C1");

            entity.ToTable("tickets");

            entity.HasIndex(e => e.TicketCode, "UQ__tickets__628DB75F3DCA3C62").IsUnique();

            entity.Property(e => e.TicketId)
                .HasMaxLength(50)
                .IsUnicode(false)
                .HasColumnName("ticket_id");
            entity.Property(e => e.CategoryName)
                .HasMaxLength(255)
                .IsUnicode(false)
                .HasColumnName("category_name");
            entity.Property(e => e.EventDate)
                .HasDefaultValueSql("(getdate())")
                .HasColumnType("datetime")
                .HasColumnName("event_date");
            entity.Property(e => e.TicketCode)
                .HasMaxLength(50)
                .IsUnicode(false)
                .HasColumnName("ticket_code");
            entity.Property(e => e.TicketCreatedAt)
                .HasDefaultValueSql("(getdate())")
                .HasColumnType("datetime")
                .HasColumnName("ticket_created_at");
            entity.Property(e => e.TicketCreatedBy)
                .HasMaxLength(255)
                .IsUnicode(false)
                .HasColumnName("ticket_created_by");
            entity.Property(e => e.TicketHasSeatNumber).HasColumnName("ticket_has_seat_number");
            entity.Property(e => e.TicketModifiedAt)
                .HasColumnType("datetime")
                .HasColumnName("ticket_modified_at");
            entity.Property(e => e.TicketModifiedBy)
                .HasMaxLength(255)
                .IsUnicode(false)
                .HasColumnName("ticket_modified_by");
            entity.Property(e => e.TicketName)
                .HasMaxLength(255)
                .IsUnicode(false)
                .HasColumnName("ticket_name");
            entity.Property(e => e.TicketPrice)
                .HasColumnType("decimal(10, 2)")
                .HasColumnName("ticket_price");
            entity.Property(e => e.TicketQuota).HasColumnName("ticket_quota");
            entity.Property(e => e.TicketRemainingQuota).HasColumnName("ticket_remaining_quota");
        });

        modelBuilder.Entity<UserAccount>(entity =>
        {
            entity.HasKey(e => e.UserAccountId).HasName("PK__user_acc__1918BBDAAE755D92");

            entity.ToTable("user_accounts");

            entity.HasIndex(e => e.UserAccountPhoneNumber, "UQ__user_acc__029EF2BEE1A00D35").IsUnique();

            entity.HasIndex(e => e.UserAccountEmail, "UQ__user_acc__F4E454A7ADED338A").IsUnique();

            entity.Property(e => e.UserAccountId)
                .HasMaxLength(50)
                .IsUnicode(false)
                .HasColumnName("user_account_id");
            entity.Property(e => e.UserAccountAddress)
                .HasMaxLength(255)
                .IsUnicode(false)
                .HasColumnName("user_account_address");
            entity.Property(e => e.UserAccountCreatedAt)
                .HasDefaultValueSql("(getdate())")
                .HasColumnType("datetime")
                .HasColumnName("user_account_created_at");
            entity.Property(e => e.UserAccountCreatedBy)
                .HasMaxLength(255)
                .IsUnicode(false)
                .HasColumnName("user_account_created_by");
            entity.Property(e => e.UserAccountEmail)
                .HasMaxLength(255)
                .IsUnicode(false)
                .HasColumnName("user_account_email");
            entity.Property(e => e.UserAccountModifiedAt)
                .HasColumnType("datetime")
                .HasColumnName("user_account_modified_at");
            entity.Property(e => e.UserAccountModifiedBy)
                .HasMaxLength(255)
                .IsUnicode(false)
                .HasColumnName("user_account_modified_by");
            entity.Property(e => e.UserAccountName)
                .HasMaxLength(100)
                .IsUnicode(false)
                .HasColumnName("user_account_name");
            entity.Property(e => e.UserAccountPassword)
                .HasMaxLength(255)
                .IsUnicode(false)
                .HasColumnName("user_account_password");
            entity.Property(e => e.UserAccountPhoneNumber)
                .HasMaxLength(20)
                .IsUnicode(false)
                .HasColumnName("user_account_phone_number");
        });

        OnModelCreatingPartial(modelBuilder);
    }

    partial void OnModelCreatingPartial(ModelBuilder modelBuilder);
}
