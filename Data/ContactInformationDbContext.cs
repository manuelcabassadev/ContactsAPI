using System;
using System.Collections.Generic;
using ContactsAPI.Models;
using Microsoft.EntityFrameworkCore;

namespace ContactsAPI.Data;

public partial class ContactInformationDbContext : DbContext
{
    public ContactInformationDbContext(DbContextOptions<ContactInformationDbContext> options)
        : base(options)
    {
    }

    public virtual DbSet<Contact> Contacts { get; set; }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<Contact>(entity =>
        {
            entity.Property(e => e.ContactId).HasColumnName("ContactID");
            entity.Property(e => e.eMail)
                .HasMaxLength(50)
                .IsUnicode(false)
                .HasColumnName("eMail");
            entity.Property(e => e.Fax)
                .HasMaxLength(10)
                .IsUnicode(false);
            entity.Property(e => e.LastUpdateDate).HasColumnType("datetime");
            entity.Property(e => e.LastUpdateUserName)
                .HasMaxLength(20)
                .IsUnicode(false);
            entity.Property(e => e.Name)
                .HasMaxLength(50)
                .IsUnicode(false);
            entity.Property(e => e.Notes).HasColumnType("ntext");
            entity.Property(e => e.Phone)
                .HasMaxLength(10)
                .IsUnicode(false);
        });

        OnModelCreatingPartial(modelBuilder);
    }

    partial void OnModelCreatingPartial(ModelBuilder modelBuilder);
}
