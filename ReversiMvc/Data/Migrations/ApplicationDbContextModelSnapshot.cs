﻿// <auto-generated />
using System;
using System.Diagnostics.CodeAnalysis;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;
using ReversiMvc.Data;

#nullable disable

namespace ReversiMvc.Migrations;

[ExcludeFromCodeCoverage]
[DbContext(typeof(ApplicationDbContext))]
partial class ApplicationDbContextModelSnapshot : ModelSnapshot
{
    protected override void BuildModel(ModelBuilder modelBuilder)
    {
#pragma warning disable 612, 618
        modelBuilder.HasAnnotation("ProductVersion", "6.0.3");

        modelBuilder.Entity("Microsoft.AspNetCore.Identity.IdentityRole", b =>
        {
            b.Property<string>("Id")
                .HasColumnType("TEXT");

            b.Property<string>("ConcurrencyStamp")
                .IsConcurrencyToken()
                .HasColumnType("TEXT");

            b.Property<string>("Name")
                .HasMaxLength(256)
                .HasColumnType("TEXT");

            b.Property<string>("NormalizedName")
                .HasMaxLength(256)
                .HasColumnType("TEXT");

            b.HasKey("Id");

            b.HasIndex("NormalizedName")
                .IsUnique()
                .HasDatabaseName("RoleNameIndex");

            b.ToTable("AspNetRoles", (string)null);

            b.HasData(
                new
                {
                    Id = "37186721-a6e7-418f-8a56-1c3b6596264f",
                    ConcurrencyStamp = "4a373a9a-fd22-42fe-aa85-04fd88478602",
                    Name = "Admin",
                    NormalizedName = "ADMIN"
                },
                new
                {
                    Id = "3c3d7381-c495-4622-aa09-6ce8fecfb8bf",
                    ConcurrencyStamp = "8372c6e6-e7b3-428d-89f6-364754c1fac6",
                    Name = "Mediator",
                    NormalizedName = "MEDIATOR"
                });
        });

        modelBuilder.Entity("Microsoft.AspNetCore.Identity.IdentityRoleClaim<string>", b =>
        {
            b.Property<int>("Id")
                .ValueGeneratedOnAdd()
                .HasColumnType("INTEGER");

            b.Property<string>("ClaimType")
                .HasColumnType("TEXT");

            b.Property<string>("ClaimValue")
                .HasColumnType("TEXT");

            b.Property<string>("RoleId")
                .IsRequired()
                .HasColumnType("TEXT");

            b.HasKey("Id");

            b.HasIndex("RoleId");

            b.ToTable("AspNetRoleClaims", (string)null);
        });

        modelBuilder.Entity("Microsoft.AspNetCore.Identity.IdentityUser", b =>
        {
            b.Property<string>("Id")
                .HasColumnType("TEXT");

            b.Property<int>("AccessFailedCount")
                .HasColumnType("INTEGER");

            b.Property<string>("ConcurrencyStamp")
                .IsConcurrencyToken()
                .HasColumnType("TEXT");

            b.Property<string>("Email")
                .HasMaxLength(256)
                .HasColumnType("TEXT");

            b.Property<bool>("EmailConfirmed")
                .HasColumnType("INTEGER");

            b.Property<bool>("LockoutEnabled")
                .HasColumnType("INTEGER");

            b.Property<DateTimeOffset?>("LockoutEnd")
                .HasColumnType("TEXT");

            b.Property<string>("NormalizedEmail")
                .HasMaxLength(256)
                .HasColumnType("TEXT");

            b.Property<string>("NormalizedUserName")
                .HasMaxLength(256)
                .HasColumnType("TEXT");

            b.Property<string>("PasswordHash")
                .HasColumnType("TEXT");

            b.Property<string>("PhoneNumber")
                .HasColumnType("TEXT");

            b.Property<bool>("PhoneNumberConfirmed")
                .HasColumnType("INTEGER");

            b.Property<string>("SecurityStamp")
                .HasColumnType("TEXT");

            b.Property<bool>("TwoFactorEnabled")
                .HasColumnType("INTEGER");

            b.Property<string>("UserName")
                .HasMaxLength(256)
                .HasColumnType("TEXT");

            b.HasKey("Id");

            b.HasIndex("NormalizedEmail")
                .HasDatabaseName("EmailIndex");

            b.HasIndex("NormalizedUserName")
                .IsUnique()
                .HasDatabaseName("UserNameIndex");

            b.ToTable("AspNetUsers", (string)null);

            b.HasData(
                new
                {
                    Id = "a18be9c0-aa65-4af8-bd17-00bd9344e575",
                    AccessFailedCount = 0,
                    ConcurrencyStamp = "b8a34d5d-3cd9-4e86-ac43-309c950a7c07",
                    Email = "admin@nimda.com",
                    EmailConfirmed = true,
                    LockoutEnabled = false,
                    NormalizedEmail = "ADMIN@NIMDA.COM",
                    NormalizedUserName = "ADMIN@NIMDA.COM",
                    PasswordHash = "AQAAAAEAACcQAAAAEPp/6bNXA0ggnaMHhgnFpyu+EeSSZbAkEhHPKJRd/amrTgxud68CR/XhoLA41Cs9rQ==",
                    PhoneNumberConfirmed = false,
                    SecurityStamp = "",
                    TwoFactorEnabled = false,
                    UserName = "admin@nimda.com"
                });
        });

        modelBuilder.Entity("Microsoft.AspNetCore.Identity.IdentityUserClaim<string>", b =>
        {
            b.Property<int>("Id")
                .ValueGeneratedOnAdd()
                .HasColumnType("INTEGER");

            b.Property<string>("ClaimType")
                .HasColumnType("TEXT");

            b.Property<string>("ClaimValue")
                .HasColumnType("TEXT");

            b.Property<string>("UserId")
                .IsRequired()
                .HasColumnType("TEXT");

            b.HasKey("Id");

            b.HasIndex("UserId");

            b.ToTable("AspNetUserClaims", (string)null);
        });

        modelBuilder.Entity("Microsoft.AspNetCore.Identity.IdentityUserLogin<string>", b =>
        {
            b.Property<string>("LoginProvider")
                .HasMaxLength(128)
                .HasColumnType("TEXT");

            b.Property<string>("ProviderKey")
                .HasMaxLength(128)
                .HasColumnType("TEXT");

            b.Property<string>("ProviderDisplayName")
                .HasColumnType("TEXT");

            b.Property<string>("UserId")
                .IsRequired()
                .HasColumnType("TEXT");

            b.HasKey("LoginProvider", "ProviderKey");

            b.HasIndex("UserId");

            b.ToTable("AspNetUserLogins", (string)null);
        });

        modelBuilder.Entity("Microsoft.AspNetCore.Identity.IdentityUserRole<string>", b =>
        {
            b.Property<string>("UserId")
                .HasColumnType("TEXT");

            b.Property<string>("RoleId")
                .HasColumnType("TEXT");

            b.HasKey("UserId", "RoleId");

            b.HasIndex("RoleId");

            b.ToTable("AspNetUserRoles", (string)null);

            b.HasData(
                new
                {
                    UserId = "a18be9c0-aa65-4af8-bd17-00bd9344e575",
                    RoleId = "37186721-a6e7-418f-8a56-1c3b6596264f"
                });
        });

        modelBuilder.Entity("Microsoft.AspNetCore.Identity.IdentityUserToken<string>", b =>
        {
            b.Property<string>("UserId")
                .HasColumnType("TEXT");

            b.Property<string>("LoginProvider")
                .HasMaxLength(128)
                .HasColumnType("TEXT");

            b.Property<string>("Name")
                .HasMaxLength(128)
                .HasColumnType("TEXT");

            b.Property<string>("Value")
                .HasColumnType("TEXT");

            b.HasKey("UserId", "LoginProvider", "Name");

            b.ToTable("AspNetUserTokens", (string)null);
        });

        modelBuilder.Entity("Microsoft.AspNetCore.Identity.IdentityRoleClaim<string>", b =>
        {
            b.HasOne("Microsoft.AspNetCore.Identity.IdentityRole", null)
                .WithMany()
                .HasForeignKey("RoleId")
                .OnDelete(DeleteBehavior.Cascade)
                .IsRequired();
        });

        modelBuilder.Entity("Microsoft.AspNetCore.Identity.IdentityUserClaim<string>", b =>
        {
            b.HasOne("Microsoft.AspNetCore.Identity.IdentityUser", null)
                .WithMany()
                .HasForeignKey("UserId")
                .OnDelete(DeleteBehavior.Cascade)
                .IsRequired();
        });

        modelBuilder.Entity("Microsoft.AspNetCore.Identity.IdentityUserLogin<string>", b =>
        {
            b.HasOne("Microsoft.AspNetCore.Identity.IdentityUser", null)
                .WithMany()
                .HasForeignKey("UserId")
                .OnDelete(DeleteBehavior.Cascade)
                .IsRequired();
        });

        modelBuilder.Entity("Microsoft.AspNetCore.Identity.IdentityUserRole<string>", b =>
        {
            b.HasOne("Microsoft.AspNetCore.Identity.IdentityRole", null)
                .WithMany()
                .HasForeignKey("RoleId")
                .OnDelete(DeleteBehavior.Cascade)
                .IsRequired();

            b.HasOne("Microsoft.AspNetCore.Identity.IdentityUser", null)
                .WithMany()
                .HasForeignKey("UserId")
                .OnDelete(DeleteBehavior.Cascade)
                .IsRequired();
        });

        modelBuilder.Entity("Microsoft.AspNetCore.Identity.IdentityUserToken<string>", b =>
        {
            b.HasOne("Microsoft.AspNetCore.Identity.IdentityUser", null)
                .WithMany()
                .HasForeignKey("UserId")
                .OnDelete(DeleteBehavior.Cascade)
                .IsRequired();
        });
#pragma warning restore 612, 618
    }
}
