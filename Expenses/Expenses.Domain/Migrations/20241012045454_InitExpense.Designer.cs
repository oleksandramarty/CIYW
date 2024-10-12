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
    [Migration("20241012045454_InitExpense")]
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

            modelBuilder.Entity("Expenses.Domain.Models.Balances.Balance", b =>
                {
                    b.Property<Guid>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("uuid");

                    b.Property<decimal>("Amount")
                        .HasColumnType("numeric");

                    b.Property<int>("BalanceTypeId")
                        .HasColumnType("integer");

                    b.Property<DateTime>("Created")
                        .HasColumnType("timestamp with time zone");

                    b.Property<int>("CurrencyId")
                        .HasColumnType("integer");

                    b.Property<DateTime?>("Modified")
                        .HasColumnType("timestamp with time zone");

                    b.Property<string>("Title")
                        .IsRequired()
                        .HasColumnType("text");

                    b.Property<Guid>("UserId")
                        .HasColumnType("uuid");

                    b.Property<Guid>("UserProjectId")
                        .HasColumnType("uuid");

                    b.Property<string>("Version")
                        .IsRequired()
                        .HasColumnType("text");

                    b.HasKey("Id");

                    b.HasIndex("UserProjectId");

                    b.ToTable("Balances", "Balance");
                });

            modelBuilder.Entity("Expenses.Domain.Models.Expenses.Expense", b =>
                {
                    b.Property<Guid>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("uuid");

                    b.Property<decimal>("Amount")
                        .HasColumnType("numeric");

                    b.Property<Guid>("BalanceId")
                        .HasColumnType("uuid");

                    b.Property<int>("CategoryId")
                        .HasColumnType("integer");

                    b.Property<DateTime>("Created")
                        .HasColumnType("timestamp with time zone");

                    b.Property<Guid>("CreatedUserId")
                        .HasColumnType("uuid");

                    b.Property<DateTime>("Date")
                        .HasColumnType("timestamp with time zone");

                    b.Property<string>("Description")
                        .HasMaxLength(100)
                        .HasColumnType("character varying(100)");

                    b.Property<DateTime?>("Modified")
                        .HasColumnType("timestamp with time zone");

                    b.Property<string>("Title")
                        .IsRequired()
                        .HasMaxLength(50)
                        .HasColumnType("character varying(50)");

                    b.Property<Guid>("UserProjectId")
                        .HasColumnType("uuid");

                    b.Property<string>("Version")
                        .IsRequired()
                        .HasColumnType("text");

                    b.HasKey("Id");

                    b.HasIndex("UserProjectId");

                    b.ToTable("Expenses", "Expenses");
                });

            modelBuilder.Entity("Expenses.Domain.Models.Expenses.PlannedExpense", b =>
                {
                    b.Property<Guid>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("uuid");

                    b.Property<decimal>("Amount")
                        .HasColumnType("numeric");

                    b.Property<Guid>("BalanceId")
                        .HasColumnType("uuid");

                    b.Property<int>("CategoryId")
                        .HasColumnType("integer");

                    b.Property<DateTime>("Created")
                        .HasColumnType("timestamp with time zone");

                    b.Property<string>("Description")
                        .HasMaxLength(100)
                        .HasColumnType("character varying(100)");

                    b.Property<DateTime?>("EndDate")
                        .HasColumnType("timestamp with time zone");

                    b.Property<int>("FrequencyId")
                        .HasColumnType("integer");

                    b.Property<bool>("IsActive")
                        .HasColumnType("boolean");

                    b.Property<DateTime?>("Modified")
                        .HasColumnType("timestamp with time zone");

                    b.Property<DateTime>("NextDate")
                        .HasColumnType("timestamp with time zone");

                    b.Property<DateTime>("StartDate")
                        .HasColumnType("timestamp with time zone");

                    b.Property<string>("Title")
                        .IsRequired()
                        .HasMaxLength(50)
                        .HasColumnType("character varying(50)");

                    b.Property<Guid>("UserId")
                        .HasColumnType("uuid");

                    b.Property<Guid>("UserProjectId")
                        .HasColumnType("uuid");

                    b.Property<string>("Version")
                        .IsRequired()
                        .HasColumnType("text");

                    b.HasKey("Id");

                    b.HasIndex("UserProjectId");

                    b.ToTable("PlannedExpenses", "Expenses");
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

                    b.Property<string>("Version")
                        .IsRequired()
                        .HasColumnType("text");

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

                    b.Property<string>("Version")
                        .IsRequired()
                        .HasColumnType("text");

                    b.HasKey("Id");

                    b.ToTable("UserProjects", "Projects");
                });

            modelBuilder.Entity("Expenses.Domain.Models.Balances.Balance", b =>
                {
                    b.HasOne("Expenses.Domain.Models.Projects.UserProject", "UserProject")
                        .WithMany("Balances")
                        .HasForeignKey("UserProjectId")
                        .OnDelete(DeleteBehavior.Restrict)
                        .IsRequired();

                    b.Navigation("UserProject");
                });

            modelBuilder.Entity("Expenses.Domain.Models.Expenses.Expense", b =>
                {
                    b.HasOne("Expenses.Domain.Models.Projects.UserProject", "UserProject")
                        .WithMany("Expenses")
                        .HasForeignKey("UserProjectId")
                        .OnDelete(DeleteBehavior.Restrict)
                        .IsRequired();

                    b.Navigation("UserProject");
                });

            modelBuilder.Entity("Expenses.Domain.Models.Expenses.PlannedExpense", b =>
                {
                    b.HasOne("Expenses.Domain.Models.Projects.UserProject", "UserProject")
                        .WithMany("PlannedExpenses")
                        .HasForeignKey("UserProjectId")
                        .OnDelete(DeleteBehavior.Restrict)
                        .IsRequired();

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

            modelBuilder.Entity("Expenses.Domain.Models.Projects.UserProject", b =>
                {
                    b.Navigation("AllowedUsers");

                    b.Navigation("Balances");

                    b.Navigation("Expenses");

                    b.Navigation("PlannedExpenses");
                });
#pragma warning restore 612, 618
        }
    }
}
