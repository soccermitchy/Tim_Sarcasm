﻿// <auto-generated />
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore.Migrations;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;
using TimSarcasm.Services;

namespace TimSarcasm.Migrations
{
    [DbContext(typeof(DatabaseService))]
    [Migration("20191007014828_ServerPropsUintToUlong")]
    partial class ServerPropsUintToUlong
    {
        protected override void BuildTargetModel(ModelBuilder modelBuilder)
        {
#pragma warning disable 612, 618
            modelBuilder
                .HasAnnotation("ProductVersion", "3.0.0");

            modelBuilder.Entity("TimSarcasm.Models.ServerProperties", b =>
                {
                    b.Property<ulong>("ServerId")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("INTEGER");

                    b.Property<ulong>("LogChannelId")
                        .HasColumnType("INTEGER");

                    b.Property<ulong>("SpamRoleId")
                        .HasColumnType("INTEGER");

                    b.Property<ulong>("TempVoiceCategoryId")
                        .HasColumnType("INTEGER");

                    b.Property<ulong>("TempVoiceCreateChannelId")
                        .HasColumnType("INTEGER");

                    b.HasKey("ServerId");

                    b.ToTable("ServerProperties");
                });
#pragma warning restore 612, 618
        }
    }
}
