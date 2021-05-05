﻿// <auto-generated />
using System;
using CashRegister.Data;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore.Migrations;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;

namespace CashRegister.Migrations
{
    [DbContext(typeof(CashRegisterDataContext))]
    [Migration("20210505195830_init")]
    partial class init
    {
        protected override void BuildTargetModel(ModelBuilder modelBuilder)
        {
#pragma warning disable 612, 618
            modelBuilder
                .HasAnnotation("Relational:MaxIdentifierLength", 64)
                .HasAnnotation("ProductVersion", "5.0.5");

            modelBuilder.Entity("CashRegister.SharedResources.Product", b =>
                {
                    b.Property<int>("ID")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int");

                    b.Property<string>("ProductName")
                        .IsRequired()
                        .HasColumnType("longtext CHARACTER SET utf8mb4");

                    b.Property<decimal>("UnitPrice")
                        .HasColumnType("numeric(6,2)");

                    b.HasKey("ID");

                    b.ToTable("Products");
                });

            modelBuilder.Entity("CashRegister.SharedResources.Receipt", b =>
                {
                    b.Property<int>("ID")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int");

                    b.Property<DateTime>("ReceiptTimestamp")
                        .HasColumnType("datetime(6)");

                    b.Property<decimal>("TotalPrice")
                        .HasColumnType("numeric(6,2)");

                    b.HasKey("ID");

                    b.ToTable("Receipts");
                });

            modelBuilder.Entity("CashRegister.SharedResources.ReceiptLine", b =>
                {
                    b.Property<int>("ID")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int");

                    b.Property<int>("Amount")
                        .HasColumnType("int");

                    b.Property<int?>("ProductID")
                        .HasColumnType("int");

                    b.Property<int?>("ReceiptID")
                        .HasColumnType("int");

                    b.Property<decimal>("TotalPrice")
                        .HasColumnType("numeric(6,2)");

                    b.HasKey("ID");

                    b.HasIndex("ProductID");

                    b.HasIndex("ReceiptID");

                    b.ToTable("ReceiptLines");
                });

            modelBuilder.Entity("CashRegister.SharedResources.ReceiptLine", b =>
                {
                    b.HasOne("CashRegister.SharedResources.Product", "Product")
                        .WithMany()
                        .HasForeignKey("ProductID");

                    b.HasOne("CashRegister.SharedResources.Receipt", null)
                        .WithMany("ReceiptLines")
                        .HasForeignKey("ReceiptID");

                    b.Navigation("Product");
                });

            modelBuilder.Entity("CashRegister.SharedResources.Receipt", b =>
                {
                    b.Navigation("ReceiptLines");
                });
#pragma warning restore 612, 618
        }
    }
}
