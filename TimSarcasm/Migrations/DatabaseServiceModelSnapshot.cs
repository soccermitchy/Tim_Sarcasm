﻿// <auto-generated />
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;
using TimSarcasm.Services;

namespace TimSarcasm.Migrations
{
    [DbContext(typeof(DatabaseService))]
    partial class DatabaseServiceModelSnapshot : ModelSnapshot
    {
        protected override void BuildModel(ModelBuilder modelBuilder)
        {
#pragma warning disable 612, 618
            modelBuilder
                .HasAnnotation("ProductVersion", "3.0.0");

            modelBuilder.Entity("TimSarcasm.Models.ServerProperties", b =>
                {
                    b.Property<uint>("ServerId")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("INTEGER");

                    b.Property<uint>("LogChannelId")
                        .HasColumnType("INTEGER");

                    b.Property<uint>("SpamRoleId")
                        .HasColumnType("INTEGER");

                    b.Property<uint>("TempVoiceCategoryId")
                        .HasColumnType("INTEGER");

                    b.Property<uint>("TempVoiceCreateChannelId")
                        .HasColumnType("INTEGER");

                    b.HasKey("ServerId");

                    b.ToTable("ServerProperties");
                });
#pragma warning restore 612, 618
        }
    }
}
