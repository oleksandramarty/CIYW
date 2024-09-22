﻿// <auto-generated />
using System;
using Expenses.Domain;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore.Migrations;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;
using Npgsql.EntityFrameworkCore.PostgreSQL.Metadata;

#nullable disable

namespace Expenses.Domain.Migrations
{
    [DbContext(typeof(ExpensesDataContext))]
    [Migration("20240922033822_InitExpense")]
    partial class InitExpense
    {
        /// <inheritdoc />
        protected override void BuildTargetModel(ModelBuilder modelBuilder)
        {
#pragma warning disable 612, 618
            modelBuilder
                .HasAnnotation("ProductVersion", "9.0.0-rc.1.24451.1")
                .HasAnnotation("Relational:MaxIdentifierLength", 63);

            NpgsqlModelBuilderExtensions.UseIdentityByDefaultColumns(modelBuilder);

            modelBuilder.Entity("Expenses.Domain.Models.Categories.Category", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("integer");

                    NpgsqlPropertyBuilderExtensions.UseIdentityByDefaultColumn(b.Property<int>("Id"));

                    b.Property<string>("Color")
                        .IsRequired()
                        .HasColumnType("text");

                    b.Property<string>("Icon")
                        .IsRequired()
                        .HasColumnType("text");

                    b.Property<bool>("IsActive")
                        .HasColumnType("boolean");

                    b.Property<string>("Title")
                        .IsRequired()
                        .HasColumnType("text");

                    b.HasKey("Id");

                    b.ToTable("Categories", "Categories");
                });

            modelBuilder.Entity("Expenses.Domain.Models.Categories.UserCategory", b =>
                {
                    b.Property<Guid>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("uuid");

                    b.Property<int>("CategoryId")
                        .HasColumnType("integer");

                    b.Property<Guid>("UserId")
                        .HasColumnType("uuid");

                    b.HasKey("Id");

                    b.HasIndex("CategoryId");

                    b.ToTable("UserCategories", "Categories");
                });

            modelBuilder.Entity("Expenses.Domain.Models.Expenses.Expense", b =>
                {
                    b.Property<Guid>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("uuid");

                    b.Property<decimal>("Amount")
                        .HasColumnType("numeric");

                    b.Property<DateTime>("Created")
                        .HasColumnType("timestamp with time zone");

                    b.Property<Guid>("CreatedUserId")
                        .HasColumnType("uuid");

                    b.Property<DateTime>("Date")
                        .HasColumnType("timestamp with time zone");

                    b.Property<string>("Description")
                        .HasMaxLength(300)
                        .HasColumnType("character varying(300)");

                    b.Property<bool>("IsPositive")
                        .HasColumnType("boolean");

                    b.Property<DateTime?>("Modified")
                        .HasColumnType("timestamp with time zone");

                    b.Property<string>("Title")
                        .IsRequired()
                        .HasMaxLength(100)
                        .HasColumnType("character varying(100)");

                    b.Property<Guid>("UserCategoryId")
                        .HasColumnType("uuid");

                    b.Property<Guid>("UserProjectId")
                        .HasColumnType("uuid");

                    b.HasKey("Id");

                    b.HasIndex("UserCategoryId");

                    b.HasIndex("UserProjectId");

                    b.ToTable("Expenses", "Expenses");
                });

            modelBuilder.Entity("Expenses.Domain.Models.Projects.UserAllowedProject", b =>
                {
                    b.Property<Guid>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("uuid");

                    b.Property<bool>("IsReadOnly")
                        .HasColumnType("boolean");

                    b.Property<Guid>("UserId")
                        .HasColumnType("uuid");

                    b.Property<Guid>("UserProjectId")
                        .HasColumnType("uuid");

                    b.HasKey("Id");

                    b.HasIndex("UserProjectId");

                    b.ToTable("UserAllowedProjects", "Projects");
                });

            modelBuilder.Entity("Expenses.Domain.Models.Projects.UserProject", b =>
                {
                    b.Property<Guid>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("uuid");

                    b.Property<DateTime>("Created")
                        .HasColumnType("timestamp with time zone");

                    b.Property<Guid>("CreatedUserId")
                        .HasColumnType("uuid");

                    b.Property<bool>("IsActive")
                        .HasColumnType("boolean");

                    b.Property<DateTime?>("Modified")
                        .HasColumnType("timestamp with time zone");

                    b.Property<string>("Title")
                        .IsRequired()
                        .HasColumnType("text");

                    b.HasKey("Id");

                    b.ToTable("UserProjects", "Projects");
                });

            modelBuilder.Entity("Expenses.Domain.Models.Categories.UserCategory", b =>
                {
                    b.HasOne("Expenses.Domain.Models.Categories.Category", "Category")
                        .WithMany("UserCategories")
                        .HasForeignKey("CategoryId")
                        .OnDelete(DeleteBehavior.Restrict)
                        .IsRequired();

                    b.Navigation("Category");
                });

            modelBuilder.Entity("Expenses.Domain.Models.Expenses.Expense", b =>
                {
                    b.HasOne("Expenses.Domain.Models.Categories.UserCategory", "UserCategory")
                        .WithMany("Expenses")
                        .HasForeignKey("UserCategoryId")
                        .OnDelete(DeleteBehavior.Restrict)
                        .IsRequired();

                    b.HasOne("Expenses.Domain.Models.Projects.UserProject", "UserProject")
                        .WithMany("Expenses")
                        .HasForeignKey("UserProjectId")
                        .OnDelete(DeleteBehavior.Restrict)
                        .IsRequired();

                    b.Navigation("UserCategory");

                    b.Navigation("UserProject");
                });

            modelBuilder.Entity("Expenses.Domain.Models.Projects.UserAllowedProject", b =>
                {
                    b.HasOne("Expenses.Domain.Models.Projects.UserProject", "UserProject")
                        .WithMany("AllowedUsers")
                        .HasForeignKey("UserProjectId")
                        .OnDelete(DeleteBehavior.Restrict)
                        .IsRequired();

                    b.Navigation("UserProject");
                });

            modelBuilder.Entity("Expenses.Domain.Models.Categories.Category", b =>
                {
                    b.Navigation("UserCategories");
                });

            modelBuilder.Entity("Expenses.Domain.Models.Categories.UserCategory", b =>
                {
                    b.Navigation("Expenses");
                });

            modelBuilder.Entity("Expenses.Domain.Models.Projects.UserProject", b =>
                {
                    b.Navigation("AllowedUsers");

                    b.Navigation("Expenses");
                });
#pragma warning restore 612, 618
        }
    }
}
