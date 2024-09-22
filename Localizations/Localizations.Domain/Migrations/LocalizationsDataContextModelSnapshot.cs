﻿// <auto-generated />
using System;
using Localizations.Domain;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;
using Npgsql.EntityFrameworkCore.PostgreSQL.Metadata;

#nullable disable

namespace Localizations.Domain.Migrations
{
    [DbContext(typeof(LocalizationsDataContext))]
    partial class LocalizationsDataContextModelSnapshot : ModelSnapshot
    {
        protected override void BuildModel(ModelBuilder modelBuilder)
        {
#pragma warning disable 612, 618
            modelBuilder
                .HasAnnotation("ProductVersion", "9.0.0-rc.1.24451.1")
                .HasAnnotation("Relational:MaxIdentifierLength", 63);

            NpgsqlModelBuilderExtensions.UseIdentityByDefaultColumns(modelBuilder);

            modelBuilder.Entity("JobPathfinder.Data.Domain.Models.Locales.Locale", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("integer");

                    NpgsqlPropertyBuilderExtensions.UseIdentityByDefaultColumn(b.Property<int>("Id"));

                    b.Property<string>("Culture")
                        .IsRequired()
                        .HasColumnType("text");

                    b.Property<bool>("IsActive")
                        .HasColumnType("boolean");

                    b.Property<bool>("IsDefault")
                        .HasColumnType("boolean");

                    b.Property<string>("IsoCode")
                        .IsRequired()
                        .HasColumnType("text");

                    b.Property<int>("LocaleEnum")
                        .HasColumnType("integer");

                    b.Property<string>("Title")
                        .IsRequired()
                        .HasColumnType("text");

                    b.Property<string>("TitleEn")
                        .IsRequired()
                        .HasColumnType("text");

                    b.Property<string>("TitleEnNormalized")
                        .IsRequired()
                        .HasColumnType("text");

                    b.Property<string>("TitleNormalized")
                        .IsRequired()
                        .HasColumnType("text");

                    b.HasKey("Id");

                    b.ToTable("Locales", "Locales");
                });

            modelBuilder.Entity("JobPathfinder.Data.Domain.Models.Locales.Localization", b =>
                {
                    b.Property<Guid>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("uuid");

                    b.Property<bool>("IsPublic")
                        .HasColumnType("boolean");

                    b.Property<string>("Key")
                        .IsRequired()
                        .HasColumnType("text");

                    b.Property<int>("LocaleId")
                        .HasColumnType("integer");

                    b.Property<string>("Value")
                        .IsRequired()
                        .HasColumnType("text");

                    b.Property<string>("ValueEn")
                        .IsRequired()
                        .HasColumnType("text");

                    b.HasKey("Id");

                    b.HasIndex("LocaleId", "Key")
                        .IsUnique();

                    b.ToTable("Localizations", "Locales");
                });

            modelBuilder.Entity("JobPathfinder.Data.Domain.Models.Locales.Localization", b =>
                {
                    b.HasOne("JobPathfinder.Data.Domain.Models.Locales.Locale", "Locale")
                        .WithMany("Localizations")
                        .HasForeignKey("LocaleId")
                        .OnDelete(DeleteBehavior.Restrict)
                        .IsRequired();

                    b.Navigation("Locale");
                });

            modelBuilder.Entity("JobPathfinder.Data.Domain.Models.Locales.Locale", b =>
                {
                    b.Navigation("Localizations");
                });
#pragma warning restore 612, 618
        }
    }
}
