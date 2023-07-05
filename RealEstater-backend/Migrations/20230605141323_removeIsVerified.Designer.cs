﻿// <auto-generated />
using System;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore.Metadata;
using Microsoft.EntityFrameworkCore.Migrations;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;
using RealEstater_backend.Data.Database;

#nullable disable

namespace RealEstater_backend.Migrations
{
    [DbContext(typeof(RealEstaterDbContext))]
    [Migration("20230605141323_removeIsVerified")]
    partial class removeIsVerified
    {
        /// <inheritdoc />
        protected override void BuildTargetModel(ModelBuilder modelBuilder)
        {
#pragma warning disable 612, 618
            modelBuilder
                .HasAnnotation("ProductVersion", "7.0.4")
                .HasAnnotation("Relational:MaxIdentifierLength", 128);

            SqlServerModelBuilderExtensions.UseIdentityColumns(modelBuilder);

            modelBuilder.Entity("RealEstater_backend.Data.Models.AddressModel", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int");

                    SqlServerPropertyBuilderExtensions.UseIdentityColumn(b.Property<int>("Id"));

                    b.Property<int>("CityId")
                        .HasColumnType("int");

                    b.Property<string>("Title")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.HasKey("Id");

                    b.HasIndex("CityId");

                    b.ToTable("Addresses");
                });

            modelBuilder.Entity("RealEstater_backend.Data.Models.CityModel", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int");

                    SqlServerPropertyBuilderExtensions.UseIdentityColumn(b.Property<int>("Id"));

                    b.Property<string>("Title")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.HasKey("Id");

                    b.ToTable("Cities");
                });

            modelBuilder.Entity("RealEstater_backend.Data.Models.ConstructionStageModel", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int");

                    SqlServerPropertyBuilderExtensions.UseIdentityColumn(b.Property<int>("Id"));

                    b.Property<string>("Title")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.HasKey("Id");

                    b.ToTable("ConstructionStages");
                });

            modelBuilder.Entity("RealEstater_backend.Data.Models.ConstructionTypeModel", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int");

                    SqlServerPropertyBuilderExtensions.UseIdentityColumn(b.Property<int>("Id"));

                    b.Property<string>("Title")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.HasKey("Id");

                    b.ToTable("ConstructionTypes");
                });

            modelBuilder.Entity("RealEstater_backend.Data.Models.ConversationModel", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int");

                    SqlServerPropertyBuilderExtensions.UseIdentityColumn(b.Property<int>("Id"));

                    b.Property<int>("StatusId")
                        .HasColumnType("int");

                    b.Property<int>("UserOneId")
                        .HasColumnType("int");

                    b.Property<int>("UserTwoId")
                        .HasColumnType("int");

                    b.HasKey("Id");

                    b.HasIndex("StatusId");

                    b.HasIndex("UserOneId");

                    b.HasIndex("UserTwoId");

                    b.ToTable("Conversations");
                });

            modelBuilder.Entity("RealEstater_backend.Data.Models.ConversationReplyModel", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int");

                    SqlServerPropertyBuilderExtensions.UseIdentityColumn(b.Property<int>("Id"));

                    b.Property<int>("ConversationId")
                        .HasColumnType("int");

                    b.Property<bool>("NeedsReplyFromOne")
                        .HasColumnType("bit");

                    b.Property<bool>("NeedsReplyFromTwo")
                        .HasColumnType("bit");

                    b.Property<string>("Reply")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.Property<DateTime>("Time")
                        .HasColumnType("datetime2");

                    b.Property<int>("UserId")
                        .HasColumnType("int");

                    b.HasKey("Id");

                    b.HasIndex("ConversationId");

                    b.HasIndex("UserId");

                    b.ToTable("ConversationReplies");
                });

            modelBuilder.Entity("RealEstater_backend.Data.Models.ConversationStatusModel", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int");

                    SqlServerPropertyBuilderExtensions.UseIdentityColumn(b.Property<int>("Id"));

                    b.Property<string>("Title")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.HasKey("Id");

                    b.ToTable("ConversationStatuses");
                });

            modelBuilder.Entity("RealEstater_backend.Data.Models.FeatureModel", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int");

                    SqlServerPropertyBuilderExtensions.UseIdentityColumn(b.Property<int>("Id"));

                    b.Property<string>("Title")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.HasKey("Id");

                    b.ToTable("Features");
                });

            modelBuilder.Entity("RealEstater_backend.Data.Models.LandholdingFeatureModel", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int");

                    SqlServerPropertyBuilderExtensions.UseIdentityColumn(b.Property<int>("Id"));

                    b.Property<int>("FeatureId")
                        .HasColumnType("int");

                    b.Property<int>("LandholdingId")
                        .HasColumnType("int");

                    b.HasKey("Id");

                    b.HasIndex("FeatureId");

                    b.HasIndex("LandholdingId");

                    b.ToTable("LandholdingsFeatures");
                });

            modelBuilder.Entity("RealEstater_backend.Data.Models.LandholdingModel", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int");

                    SqlServerPropertyBuilderExtensions.UseIdentityColumn(b.Property<int>("Id"));

                    b.Property<decimal>("Area")
                        .HasColumnType("decimal(18,2)");

                    b.Property<int>("ConstructionStageId")
                        .HasColumnType("int");

                    b.Property<int>("ConstructionTypeId")
                        .HasColumnType("int");

                    b.Property<decimal>("Courtyard")
                        .HasColumnType("decimal(18,2)");

                    b.Property<DateTime>("CreatedAt")
                        .HasColumnType("datetime2");

                    b.Property<string>("Description")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.Property<int>("Floor")
                        .HasColumnType("int");

                    b.Property<bool>("IsActive")
                        .HasColumnType("bit");

                    b.Property<int>("LandholdingTypeId")
                        .HasColumnType("int");

                    b.Property<DateTime>("LastUpdatedAt")
                        .HasColumnType("datetime2");

                    b.Property<int>("LocationId")
                        .HasColumnType("int");

                    b.Property<int>("NumberOfFloors")
                        .HasColumnType("int");

                    b.Property<decimal>("Price")
                        .HasColumnType("decimal(18,2)");

                    b.Property<string>("Title")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.Property<int>("UserId")
                        .HasColumnType("int");

                    b.Property<int>("YearOfConstruction")
                        .HasColumnType("int");

                    b.HasKey("Id");

                    b.HasIndex("ConstructionStageId");

                    b.HasIndex("ConstructionTypeId");

                    b.HasIndex("LandholdingTypeId");

                    b.HasIndex("LocationId");

                    b.HasIndex("UserId");

                    b.ToTable("Landholdings");
                });

            modelBuilder.Entity("RealEstater_backend.Data.Models.LandholdingPictureModel", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int");

                    SqlServerPropertyBuilderExtensions.UseIdentityColumn(b.Property<int>("Id"));

                    b.Property<int>("LandholdingId")
                        .HasColumnType("int");

                    b.Property<string>("PictureUrl")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.HasKey("Id");

                    b.HasIndex("LandholdingId");

                    b.ToTable("LandholdingPictures");
                });

            modelBuilder.Entity("RealEstater_backend.Data.Models.LandholdingTypeModel", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int");

                    SqlServerPropertyBuilderExtensions.UseIdentityColumn(b.Property<int>("Id"));

                    b.Property<string>("Title")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.HasKey("Id");

                    b.ToTable("LandholdingTypes");
                });

            modelBuilder.Entity("RealEstater_backend.Data.Models.PriceHistoryModel", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int");

                    SqlServerPropertyBuilderExtensions.UseIdentityColumn(b.Property<int>("Id"));

                    b.Property<int>("LandholdingId")
                        .HasColumnType("int");

                    b.Property<decimal>("Price")
                        .HasColumnType("decimal(18,2)");

                    b.Property<DateTime>("StartDate")
                        .HasColumnType("datetime2");

                    b.HasKey("Id");

                    b.HasIndex("LandholdingId");

                    b.ToTable("PriceHistory");
                });

            modelBuilder.Entity("RealEstater_backend.Data.Models.UserModel", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int");

                    SqlServerPropertyBuilderExtensions.UseIdentityColumn(b.Property<int>("Id"));

                    b.Property<string>("Email")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("FirstName")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("LastName")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("Password")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("PhoneNumber")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("PictureURL")
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("RefreshToken")
                        .HasColumnType("nvarchar(max)");

                    b.Property<DateTime>("RefreshTokenExpiryTime")
                        .HasColumnType("datetime2");

                    b.Property<DateTime>("RegisteredAt")
                        .HasColumnType("datetime2");

                    b.Property<DateTime>("ResetPasswordExpiryTime")
                        .HasColumnType("datetime2");

                    b.Property<string>("ResetPasswordToken")
                        .HasColumnType("nvarchar(max)");

                    b.Property<int>("Role")
                        .HasColumnType("int");

                    b.Property<string>("Token")
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("WebsiteURL")
                        .HasColumnType("nvarchar(max)");

                    b.HasKey("Id");

                    b.ToTable("Users");
                });

            modelBuilder.Entity("RealEstater_backend.Data.Models.AddressModel", b =>
                {
                    b.HasOne("RealEstater_backend.Data.Models.CityModel", "City")
                        .WithMany("Addresses")
                        .HasForeignKey("CityId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.Navigation("City");
                });

            modelBuilder.Entity("RealEstater_backend.Data.Models.ConversationModel", b =>
                {
                    b.HasOne("RealEstater_backend.Data.Models.ConversationStatusModel", "Status")
                        .WithMany()
                        .HasForeignKey("StatusId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.HasOne("RealEstater_backend.Data.Models.UserModel", "UserOne")
                        .WithMany()
                        .HasForeignKey("UserOneId")
                        .OnDelete(DeleteBehavior.Restrict)
                        .IsRequired();

                    b.HasOne("RealEstater_backend.Data.Models.UserModel", "UserTwo")
                        .WithMany()
                        .HasForeignKey("UserTwoId")
                        .OnDelete(DeleteBehavior.Restrict)
                        .IsRequired();

                    b.Navigation("Status");

                    b.Navigation("UserOne");

                    b.Navigation("UserTwo");
                });

            modelBuilder.Entity("RealEstater_backend.Data.Models.ConversationReplyModel", b =>
                {
                    b.HasOne("RealEstater_backend.Data.Models.ConversationModel", "Conversation")
                        .WithMany("Replies")
                        .HasForeignKey("ConversationId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.HasOne("RealEstater_backend.Data.Models.UserModel", "User")
                        .WithMany()
                        .HasForeignKey("UserId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.Navigation("Conversation");

                    b.Navigation("User");
                });

            modelBuilder.Entity("RealEstater_backend.Data.Models.LandholdingFeatureModel", b =>
                {
                    b.HasOne("RealEstater_backend.Data.Models.FeatureModel", "Feature")
                        .WithMany("LandholdingFeatures")
                        .HasForeignKey("FeatureId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.HasOne("RealEstater_backend.Data.Models.LandholdingModel", "Landholding")
                        .WithMany("LandholdingFeatures")
                        .HasForeignKey("LandholdingId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.Navigation("Feature");

                    b.Navigation("Landholding");
                });

            modelBuilder.Entity("RealEstater_backend.Data.Models.LandholdingModel", b =>
                {
                    b.HasOne("RealEstater_backend.Data.Models.ConstructionStageModel", "ConstructionStage")
                        .WithMany()
                        .HasForeignKey("ConstructionStageId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.HasOne("RealEstater_backend.Data.Models.ConstructionTypeModel", "ConstructionType")
                        .WithMany()
                        .HasForeignKey("ConstructionTypeId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.HasOne("RealEstater_backend.Data.Models.LandholdingTypeModel", "LandholdingType")
                        .WithMany()
                        .HasForeignKey("LandholdingTypeId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.HasOne("RealEstater_backend.Data.Models.AddressModel", "Location")
                        .WithMany()
                        .HasForeignKey("LocationId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.HasOne("RealEstater_backend.Data.Models.UserModel", "User")
                        .WithMany("Landholdings")
                        .HasForeignKey("UserId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.Navigation("ConstructionStage");

                    b.Navigation("ConstructionType");

                    b.Navigation("LandholdingType");

                    b.Navigation("Location");

                    b.Navigation("User");
                });

            modelBuilder.Entity("RealEstater_backend.Data.Models.LandholdingPictureModel", b =>
                {
                    b.HasOne("RealEstater_backend.Data.Models.LandholdingModel", "Landholding")
                        .WithMany("Pictures")
                        .HasForeignKey("LandholdingId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.Navigation("Landholding");
                });

            modelBuilder.Entity("RealEstater_backend.Data.Models.PriceHistoryModel", b =>
                {
                    b.HasOne("RealEstater_backend.Data.Models.LandholdingModel", "Landholding")
                        .WithMany("HistoryPrice")
                        .HasForeignKey("LandholdingId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.Navigation("Landholding");
                });

            modelBuilder.Entity("RealEstater_backend.Data.Models.CityModel", b =>
                {
                    b.Navigation("Addresses");
                });

            modelBuilder.Entity("RealEstater_backend.Data.Models.ConversationModel", b =>
                {
                    b.Navigation("Replies");
                });

            modelBuilder.Entity("RealEstater_backend.Data.Models.FeatureModel", b =>
                {
                    b.Navigation("LandholdingFeatures");
                });

            modelBuilder.Entity("RealEstater_backend.Data.Models.LandholdingModel", b =>
                {
                    b.Navigation("HistoryPrice");

                    b.Navigation("LandholdingFeatures");

                    b.Navigation("Pictures");
                });

            modelBuilder.Entity("RealEstater_backend.Data.Models.UserModel", b =>
                {
                    b.Navigation("Landholdings");
                });
#pragma warning restore 612, 618
        }
    }
}