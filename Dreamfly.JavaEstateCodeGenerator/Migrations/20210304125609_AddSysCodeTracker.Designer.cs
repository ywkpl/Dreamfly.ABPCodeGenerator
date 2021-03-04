﻿// <auto-generated />
using System;
using Dreamfly.JavaEstateCodeGenerator.SqliteDbModels;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore.Migrations;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;

namespace Dreamfly.JavaEstateCodeGenerator.Migrations
{
    [DbContext(typeof(SettingContext))]
    [Migration("20210304125609_AddSysCodeTracker")]
    partial class AddSysCodeTracker
    {
        protected override void BuildTargetModel(ModelBuilder modelBuilder)
        {
#pragma warning disable 612, 618
            modelBuilder
                .HasAnnotation("ProductVersion", "5.0.3");

            modelBuilder.Entity("Dreamfly.JavaEstateCodeGenerator.SqliteDbModels.CodeTrack", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("INTEGER");

                    b.Property<string>("Code")
                        .HasColumnType("TEXT");

                    b.Property<DateTime?>("LastUpDateTime")
                        .HasColumnType("TEXT");

                    b.Property<long?>("SysCodeId")
                        .HasColumnType("INTEGER");

                    b.Property<string>("Value")
                        .HasColumnType("TEXT");

                    b.HasKey("Id");

                    b.ToTable("CodeTracks");
                });

            modelBuilder.Entity("Dreamfly.JavaEstateCodeGenerator.SqliteDbModels.Entity", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("INTEGER");

                    b.Property<string>("Description")
                        .HasColumnType("TEXT");

                    b.Property<bool>("HasIHasCompany")
                        .HasColumnType("INTEGER");

                    b.Property<bool>("HasIHasTenant")
                        .HasColumnType("INTEGER");

                    b.Property<bool>("IsSync")
                        .HasColumnType("INTEGER");

                    b.Property<string>("Name")
                        .HasColumnType("TEXT");

                    b.Property<string>("TableName")
                        .HasColumnType("TEXT");

                    b.HasKey("Id");

                    b.ToTable("Entities");
                });

            modelBuilder.Entity("Dreamfly.JavaEstateCodeGenerator.SqliteDbModels.EntityItem", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("INTEGER");

                    b.Property<string>("ColumnName")
                        .HasColumnType("TEXT");

                    b.Property<string>("Description")
                        .HasColumnType("TEXT");

                    b.Property<int?>("EntityId")
                        .HasColumnType("INTEGER");

                    b.Property<int?>("Fraction")
                        .HasColumnType("INTEGER");

                    b.Property<bool>("HasTime")
                        .HasColumnType("INTEGER");

                    b.Property<bool>("InCreate")
                        .HasColumnType("INTEGER");

                    b.Property<bool>("InQuery")
                        .HasColumnType("INTEGER");

                    b.Property<bool>("InResponse")
                        .HasColumnType("INTEGER");

                    b.Property<bool>("IsRequired")
                        .HasColumnType("INTEGER");

                    b.Property<int?>("Length")
                        .HasColumnType("INTEGER");

                    b.Property<string>("Name")
                        .HasColumnType("TEXT");

                    b.Property<string>("Type")
                        .HasColumnType("TEXT");

                    b.HasKey("Id");

                    b.HasIndex("EntityId");

                    b.ToTable("EntityItems");
                });

            modelBuilder.Entity("Dreamfly.JavaEstateCodeGenerator.SqliteDbModels.EntityItem", b =>
                {
                    b.HasOne("Dreamfly.JavaEstateCodeGenerator.SqliteDbModels.Entity", null)
                        .WithMany("EntityItems")
                        .HasForeignKey("EntityId");
                });

            modelBuilder.Entity("Dreamfly.JavaEstateCodeGenerator.SqliteDbModels.Entity", b =>
                {
                    b.Navigation("EntityItems");
                });
#pragma warning restore 612, 618
        }
    }
}
