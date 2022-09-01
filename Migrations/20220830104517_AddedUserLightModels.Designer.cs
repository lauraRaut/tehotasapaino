﻿// <auto-generated />
using System;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore.Metadata;
using Microsoft.EntityFrameworkCore.Migrations;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;
using Tehotasapaino.Models;

namespace Tehotasapaino.Migrations
{
    [DbContext(typeof(TehotasapainoContext))]
    [Migration("20220830104517_AddedUserLightModels")]
    partial class AddedUserLightModels
    {
        protected override void BuildTargetModel(ModelBuilder modelBuilder)
        {
#pragma warning disable 612, 618
            modelBuilder
                .HasAnnotation("Relational:MaxIdentifierLength", 128)
                .HasAnnotation("ProductVersion", "5.0.17")
                .HasAnnotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn);

            modelBuilder.Entity("Tehotasapaino.Models.UserAlertLightInformation", b =>
                {
                    b.Property<int>("UserAlertLightInformationId")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int")
                        .HasAnnotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn);

                    b.Property<int>("AlertPriceLevel")
                        .HasColumnType("int");

                    b.Property<string>("LightAlertHexColor")
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("LightBeforeAlertHexColor")
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("LightGUID")
                        .HasColumnType("nvarchar(max)");

                    b.Property<int>("UserInformationId")
                        .HasColumnType("int");

                    b.HasKey("UserAlertLightInformationId");

                    b.HasIndex("UserInformationId")
                        .IsUnique();

                    b.ToTable("UserAlertLightInformation");
                });

            modelBuilder.Entity("Tehotasapaino.Models.UserElectricityConsumptionData", b =>
                {
                    b.Property<int>("UserElectricityConsumptionDataId")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int")
                        .HasAnnotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn);

                    b.Property<decimal>("AverageConsumptionkWh")
                        .HasColumnType("decimal(18,2)");

                    b.Property<int>("Hour")
                        .HasColumnType("int");

                    b.Property<int>("UserInformationId")
                        .HasColumnType("int");

                    b.Property<int>("WeekDay")
                        .HasColumnType("int");

                    b.Property<int>("WeekNum")
                        .HasColumnType("int");

                    b.HasKey("UserElectricityConsumptionDataId");

                    b.HasIndex("UserInformationId");

                    b.ToTable("UserConsumptionData");
                });

            modelBuilder.Entity("Tehotasapaino.Models.UserExternalAPIToken", b =>
                {
                    b.Property<int>("UserExternalAPITokenId")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int")
                        .HasAnnotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn);

                    b.Property<string>("Access_token")
                        .HasColumnType("nvarchar(max)");

                    b.Property<DateTime>("CreatedDate")
                        .HasColumnType("datetime2");

                    b.Property<int>("Expires_in")
                        .HasColumnType("int");

                    b.Property<string>("ProviderName")
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("Refresh_token")
                        .HasColumnType("nvarchar(max)");

                    b.Property<int>("UserInformationId")
                        .HasColumnType("int");

                    b.Property<string>("UserNameProvider")
                        .HasColumnType("nvarchar(max)");

                    b.HasKey("UserExternalAPITokenId");

                    b.HasIndex("UserInformationId");

                    b.ToTable("UserExternalAPITokens");
                });

            modelBuilder.Entity("Tehotasapaino.Models.UserInformation", b =>
                {
                    b.Property<int>("UserInformationId")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int")
                        .HasAnnotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn);

                    b.Property<string>("Email")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.Property<bool>("HasUploadedData")
                        .HasColumnType("bit");

                    b.HasKey("UserInformationId");

                    b.ToTable("UserData");
                });

            modelBuilder.Entity("Tehotasapaino.Models.UserAlertLightInformation", b =>
                {
                    b.HasOne("Tehotasapaino.Models.UserInformation", "UserInformation")
                        .WithOne("UserAlertLightInformation")
                        .HasForeignKey("Tehotasapaino.Models.UserAlertLightInformation", "UserInformationId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.Navigation("UserInformation");
                });

            modelBuilder.Entity("Tehotasapaino.Models.UserElectricityConsumptionData", b =>
                {
                    b.HasOne("Tehotasapaino.Models.UserInformation", "UserInformation")
                        .WithMany("UserElectricityConsumptionDatas")
                        .HasForeignKey("UserInformationId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.Navigation("UserInformation");
                });

            modelBuilder.Entity("Tehotasapaino.Models.UserExternalAPIToken", b =>
                {
                    b.HasOne("Tehotasapaino.Models.UserInformation", "UserInformation")
                        .WithMany("UserExternalAPITokens")
                        .HasForeignKey("UserInformationId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.Navigation("UserInformation");
                });

            modelBuilder.Entity("Tehotasapaino.Models.UserInformation", b =>
                {
                    b.Navigation("UserAlertLightInformation");

                    b.Navigation("UserElectricityConsumptionDatas");

                    b.Navigation("UserExternalAPITokens");
                });
#pragma warning restore 612, 618
        }
    }
}
