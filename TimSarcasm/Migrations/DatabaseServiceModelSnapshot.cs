﻿// <auto-generated />
using System;
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

            modelBuilder.Entity("TimSarcasm.Models.LoggedMessage", b =>
                {
                    b.Property<ulong>("MessageId")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("INTEGER");

                    b.Property<ulong>("AuthorId")
                        .HasColumnType("INTEGER");

                    b.Property<ulong>("ChannelId")
                        .HasColumnType("INTEGER");

                    b.Property<DateTime>("EditTimestamp")
                        .HasColumnType("TEXT");

                    b.Property<string>("Message")
                        .HasColumnType("TEXT");

                    b.Property<ulong>("ServerId")
                        .HasColumnType("INTEGER");

                    b.Property<DateTime>("Timestamp")
                        .HasColumnType("TEXT");

                    b.HasKey("MessageId");

                    b.ToTable("LoggedMessages");
                });

            modelBuilder.Entity("TimSarcasm.Models.LoggedMessageAttachment", b =>
                {
                    b.Property<ulong>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("INTEGER");

                    b.Property<ulong>("MessageId")
                        .HasColumnType("INTEGER");

                    b.Property<string>("Url")
                        .HasColumnType("TEXT");

                    b.HasKey("Id");

                    b.HasIndex("MessageId");

                    b.ToTable("LoggedMessageAttachments");
                });

            modelBuilder.Entity("TimSarcasm.Models.PermissionEntry", b =>
                {
                    b.Property<ulong>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("INTEGER");

                    b.Property<ulong?>("ParentGroupId")
                        .HasColumnType("INTEGER");

                    b.Property<string>("Permission")
                        .HasColumnType("TEXT");

                    b.HasKey("Id");

                    b.HasIndex("ParentGroupId");

                    b.ToTable("PermissionEntries");
                });

            modelBuilder.Entity("TimSarcasm.Models.PermissionsGroup", b =>
                {
                    b.Property<ulong>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("INTEGER");

                    b.Property<ulong>("ChannelId")
                        .HasColumnType("INTEGER");

                    b.Property<string>("GroupName")
                        .HasColumnType("TEXT");

                    b.Property<int>("Scope")
                        .HasColumnType("INTEGER");

                    b.Property<ulong>("ServerId")
                        .HasColumnType("INTEGER");

                    b.HasKey("Id");

                    b.ToTable("PermissionGroups");
                });

            modelBuilder.Entity("TimSarcasm.Models.PermissionsGroupUser", b =>
                {
                    b.Property<ulong>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("INTEGER");

                    b.Property<ulong?>("GroupId")
                        .HasColumnType("INTEGER");

                    b.Property<ulong?>("UserId")
                        .HasColumnType("INTEGER");

                    b.HasKey("Id");

                    b.HasIndex("GroupId");

                    b.HasIndex("UserId");

                    b.ToTable("PermissionsGroupUsers");
                });

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

            modelBuilder.Entity("TimSarcasm.Models.User", b =>
                {
                    b.Property<ulong>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("INTEGER");

                    b.HasKey("Id");

                    b.ToTable("Users");
                });

            modelBuilder.Entity("TimSarcasm.Models.LoggedMessageAttachment", b =>
                {
                    b.HasOne("TimSarcasm.Models.LoggedMessage", "Message")
                        .WithMany("Attachments")
                        .HasForeignKey("MessageId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();
                });

            modelBuilder.Entity("TimSarcasm.Models.PermissionEntry", b =>
                {
                    b.HasOne("TimSarcasm.Models.PermissionsGroup", "ParentGroup")
                        .WithMany("Permissions")
                        .HasForeignKey("ParentGroupId");
                });

            modelBuilder.Entity("TimSarcasm.Models.PermissionsGroupUser", b =>
                {
                    b.HasOne("TimSarcasm.Models.PermissionsGroup", "Group")
                        .WithMany("Users")
                        .HasForeignKey("GroupId");

                    b.HasOne("TimSarcasm.Models.User", "User")
                        .WithMany("Groups")
                        .HasForeignKey("UserId");
                });
#pragma warning restore 612, 618
        }
    }
}
