﻿// <auto-generated />
using System;
using FleetManagement.Repository;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore.Metadata;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;

#nullable disable

namespace FleetManagement.Repository.Migrations
{
    [DbContext(typeof(DataContext))]
    partial class DataContextModelSnapshot : ModelSnapshot
    {
        protected override void BuildModel(ModelBuilder modelBuilder)
        {
#pragma warning disable 612, 618
            modelBuilder
                .HasAnnotation("ProductVersion", "6.0.3")
                .HasAnnotation("Relational:MaxIdentifierLength", 128);

            SqlServerModelBuilderExtensions.UseIdentityColumns(modelBuilder, 1L, 1);

            modelBuilder.Entity("FleetManagement.Core.Entities.Bag", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int");

                    SqlServerPropertyBuilderExtensions.UseIdentityColumn(b.Property<int>("Id"), 1L, 1);

                    b.Property<string>("Barcode")
                        .IsRequired()
                        .HasMaxLength(11)
                        .HasColumnType("nvarchar(11)");

                    b.Property<DateTime>("CreatedDate")
                        .HasColumnType("datetime2");

                    b.Property<int>("DeliveryPointId")
                        .HasColumnType("int");

                    b.Property<int>("State")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int")
                        .HasDefaultValue(1);

                    b.Property<DateTime?>("UpdatedDate")
                        .HasColumnType("datetime2");

                    b.HasKey("Id");

                    b.HasIndex("Barcode")
                        .IsUnique();

                    b.HasIndex("DeliveryPointId");

                    b.ToTable("Bags", (string)null);
                });

            modelBuilder.Entity("FleetManagement.Core.Entities.DeliveryPoint", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int");

                    SqlServerPropertyBuilderExtensions.UseIdentityColumn(b.Property<int>("Id"), 1L, 1);

                    b.Property<DateTime>("CreatedDate")
                        .HasColumnType("datetime2");

                    b.Property<int>("Type")
                        .HasColumnType("int");

                    b.Property<int>("Value")
                        .HasColumnType("int");

                    b.HasKey("Id");

                    b.HasIndex("Value")
                        .IsUnique();

                    b.ToTable("DeliveryPoints", (string)null);
                });

            modelBuilder.Entity("FleetManagement.Core.Entities.FleetTransaction", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int");

                    SqlServerPropertyBuilderExtensions.UseIdentityColumn(b.Property<int>("Id"), 1L, 1);

                    b.Property<string>("Barcode")
                        .IsRequired()
                        .HasMaxLength(11)
                        .HasColumnType("nvarchar(11)");

                    b.Property<DateTime>("CreatedDate")
                        .HasColumnType("datetime2");

                    b.Property<int?>("DeliveryPointId")
                        .HasColumnType("int");

                    b.Property<string>("Message")
                        .HasMaxLength(255)
                        .HasColumnType("nvarchar(255)");

                    b.Property<int?>("State")
                        .IsRequired()
                        .HasColumnType("int");

                    b.Property<Guid>("TransactionId")
                        .HasColumnType("uniqueidentifier");

                    b.Property<int>("VehicleId")
                        .HasColumnType("int");

                    b.HasKey("Id");

                    b.HasIndex("DeliveryPointId");

                    b.HasIndex("VehicleId");

                    b.ToTable("FleetTransactions", (string)null);
                });

            modelBuilder.Entity("FleetManagement.Core.Entities.Package", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int");

                    SqlServerPropertyBuilderExtensions.UseIdentityColumn(b.Property<int>("Id"), 1L, 1);

                    b.Property<int?>("BagId")
                        .HasColumnType("int");

                    b.Property<string>("Barcode")
                        .IsRequired()
                        .HasMaxLength(11)
                        .HasColumnType("nvarchar(11)");

                    b.Property<DateTime>("CreatedDate")
                        .HasColumnType("datetime2");

                    b.Property<int>("DeliveryPointId")
                        .HasColumnType("int");

                    b.Property<int>("State")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int")
                        .HasDefaultValue(1);

                    b.Property<DateTime?>("UpdatedDate")
                        .HasColumnType("datetime2");

                    b.Property<int>("VolumetricWeight")
                        .HasColumnType("int");

                    b.HasKey("Id");

                    b.HasIndex("BagId");

                    b.HasIndex("Barcode")
                        .IsUnique();

                    b.HasIndex("DeliveryPointId");

                    b.ToTable("Packages", (string)null);
                });

            modelBuilder.Entity("FleetManagement.Core.Entities.Vehicle", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int");

                    SqlServerPropertyBuilderExtensions.UseIdentityColumn(b.Property<int>("Id"), 1L, 1);

                    b.Property<DateTime>("CreatedDate")
                        .HasColumnType("datetime2");

                    b.Property<string>("Plate")
                        .IsRequired()
                        .HasMaxLength(11)
                        .HasColumnType("nvarchar(11)");

                    b.HasKey("Id");

                    b.HasIndex("Plate")
                        .IsUnique();

                    b.ToTable("Vehicles", (string)null);
                });

            modelBuilder.Entity("FleetManagement.Core.Entities.Bag", b =>
                {
                    b.HasOne("FleetManagement.Core.Entities.DeliveryPoint", null)
                        .WithMany("Bags")
                        .HasForeignKey("DeliveryPointId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();
                });

            modelBuilder.Entity("FleetManagement.Core.Entities.FleetTransaction", b =>
                {
                    b.HasOne("FleetManagement.Core.Entities.DeliveryPoint", null)
                        .WithMany("FleetTransactions")
                        .HasForeignKey("DeliveryPointId");

                    b.HasOne("FleetManagement.Core.Entities.Vehicle", null)
                        .WithMany("FleetTransactions")
                        .HasForeignKey("VehicleId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();
                });

            modelBuilder.Entity("FleetManagement.Core.Entities.Package", b =>
                {
                    b.HasOne("FleetManagement.Core.Entities.Bag", null)
                        .WithMany("Packages")
                        .HasForeignKey("BagId");

                    b.HasOne("FleetManagement.Core.Entities.DeliveryPoint", null)
                        .WithMany("Packages")
                        .HasForeignKey("DeliveryPointId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();
                });

            modelBuilder.Entity("FleetManagement.Core.Entities.Bag", b =>
                {
                    b.Navigation("Packages");
                });

            modelBuilder.Entity("FleetManagement.Core.Entities.DeliveryPoint", b =>
                {
                    b.Navigation("Bags");

                    b.Navigation("FleetTransactions");

                    b.Navigation("Packages");
                });

            modelBuilder.Entity("FleetManagement.Core.Entities.Vehicle", b =>
                {
                    b.Navigation("FleetTransactions");
                });
#pragma warning restore 612, 618
        }
    }
}
