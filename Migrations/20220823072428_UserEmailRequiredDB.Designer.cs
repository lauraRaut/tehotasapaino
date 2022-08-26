﻿// <auto-generated />
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore.Metadata;
using Microsoft.EntityFrameworkCore.Migrations;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;
using Tehotasapaino.Models;

namespace Tehotasapaino.Migrations
{
    [DbContext(typeof(TehotasapainoContext))]
    [Migration("20220823072428_UserEmailRequiredDB")]
    partial class UserEmailRequiredDB
    {
        protected override void BuildTargetModel(ModelBuilder modelBuilder)
        {
#pragma warning disable 612, 618
            modelBuilder
                .HasAnnotation("Relational:MaxIdentifierLength", 128)
                .HasAnnotation("ProductVersion", "5.0.17")
                .HasAnnotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn);

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
#pragma warning restore 612, 618
        }
    }
}
