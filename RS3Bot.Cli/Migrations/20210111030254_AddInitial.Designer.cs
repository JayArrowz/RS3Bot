﻿// <auto-generated />
using System;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore.Migrations;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;
using Npgsql.EntityFrameworkCore.PostgreSQL.Metadata;
using RS3Bot.DAL;

namespace RS3Bot.Cli.Migrations
{
    [DbContext(typeof(ApplicationDbContext))]
    [Migration("20210111030254_AddInitial")]
    partial class AddInitial
    {
        protected override void BuildTargetModel(ModelBuilder modelBuilder)
        {
#pragma warning disable 612, 618
            modelBuilder
                .UseIdentityByDefaultColumns()
                .HasAnnotation("Relational:MaxIdentifierLength", 63)
                .HasAnnotation("ProductVersion", "5.0.1");

            modelBuilder.Entity("Microsoft.AspNetCore.Identity.IdentityRole<string>", b =>
                {
                    b.Property<string>("Id")
                        .HasColumnType("text");

                    b.Property<string>("ConcurrencyStamp")
                        .IsConcurrencyToken()
                        .HasColumnType("text");

                    b.Property<string>("Name")
                        .HasMaxLength(256)
                        .HasColumnType("character varying(256)");

                    b.Property<string>("NormalizedName")
                        .HasMaxLength(256)
                        .HasColumnType("character varying(256)");

                    b.HasKey("Id");

                    b.HasIndex("NormalizedName")
                        .IsUnique()
                        .HasDatabaseName("RoleNameIndex");

                    b.ToTable("AspNetRoles");
                });

            modelBuilder.Entity("Microsoft.AspNetCore.Identity.IdentityRoleClaim<string>", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("integer")
                        .UseIdentityByDefaultColumn();

                    b.Property<string>("ClaimType")
                        .HasColumnType("text");

                    b.Property<string>("ClaimValue")
                        .HasColumnType("text");

                    b.Property<string>("RoleId")
                        .IsRequired()
                        .HasColumnType("text");

                    b.HasKey("Id");

                    b.HasIndex("RoleId");

                    b.ToTable("AspNetRoleClaims");
                });

            modelBuilder.Entity("Microsoft.AspNetCore.Identity.IdentityUserClaim<string>", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("integer")
                        .UseIdentityByDefaultColumn();

                    b.Property<string>("ClaimType")
                        .HasColumnType("text");

                    b.Property<string>("ClaimValue")
                        .HasColumnType("text");

                    b.Property<string>("UserId")
                        .IsRequired()
                        .HasColumnType("text");

                    b.HasKey("Id");

                    b.HasIndex("UserId");

                    b.ToTable("AspNetUserClaims");
                });

            modelBuilder.Entity("Microsoft.AspNetCore.Identity.IdentityUserLogin<string>", b =>
                {
                    b.Property<string>("LoginProvider")
                        .HasColumnType("text");

                    b.Property<string>("ProviderKey")
                        .HasColumnType("text");

                    b.Property<string>("ProviderDisplayName")
                        .HasColumnType("text");

                    b.Property<string>("UserId")
                        .IsRequired()
                        .HasColumnType("text");

                    b.HasKey("LoginProvider", "ProviderKey");

                    b.HasIndex("UserId");

                    b.ToTable("AspNetUserLogins");
                });

            modelBuilder.Entity("Microsoft.AspNetCore.Identity.IdentityUserRole<string>", b =>
                {
                    b.Property<string>("UserId")
                        .HasColumnType("text");

                    b.Property<string>("RoleId")
                        .HasColumnType("text");

                    b.HasKey("UserId", "RoleId");

                    b.HasIndex("RoleId");

                    b.ToTable("AspNetUserRoles");
                });

            modelBuilder.Entity("Microsoft.AspNetCore.Identity.IdentityUserToken<string>", b =>
                {
                    b.Property<string>("UserId")
                        .HasColumnType("text");

                    b.Property<string>("LoginProvider")
                        .HasColumnType("text");

                    b.Property<string>("Name")
                        .HasColumnType("text");

                    b.Property<string>("Value")
                        .HasColumnType("text");

                    b.HasKey("UserId", "LoginProvider", "Name");

                    b.ToTable("AspNetUserTokens");
                });

            modelBuilder.Entity("RS3Bot.Abstractions.Model.ApplicationUser", b =>
                {
                    b.Property<string>("Id")
                        .HasColumnType("text");

                    b.Property<int>("AccessFailedCount")
                        .HasColumnType("integer");

                    b.Property<string>("ConcurrencyStamp")
                        .IsConcurrencyToken()
                        .HasColumnType("text");

                    b.Property<int>("CurrentTaskId")
                        .HasColumnType("integer");

                    b.Property<decimal>("DiscordId")
                        .HasColumnType("numeric(20,0)");

                    b.Property<string>("Email")
                        .HasMaxLength(256)
                        .HasColumnType("character varying(256)");

                    b.Property<bool>("EmailConfirmed")
                        .HasColumnType("boolean");

                    b.Property<bool>("LockoutEnabled")
                        .HasColumnType("boolean");

                    b.Property<DateTimeOffset?>("LockoutEnd")
                        .HasColumnType("timestamp with time zone");

                    b.Property<string>("Mention")
                        .HasColumnType("text");

                    b.Property<string>("NormalizedEmail")
                        .HasMaxLength(256)
                        .HasColumnType("character varying(256)");

                    b.Property<string>("NormalizedUserName")
                        .HasMaxLength(256)
                        .HasColumnType("character varying(256)");

                    b.Property<string>("PasswordHash")
                        .HasColumnType("text");

                    b.Property<string>("PhoneNumber")
                        .HasColumnType("text");

                    b.Property<bool>("PhoneNumberConfirmed")
                        .HasColumnType("boolean");

                    b.Property<string>("PlayerName")
                        .HasColumnType("text");

                    b.Property<string>("SecurityStamp")
                        .HasColumnType("text");

                    b.Property<bool>("TwoFactorEnabled")
                        .HasColumnType("boolean");

                    b.Property<string>("UserName")
                        .HasMaxLength(256)
                        .HasColumnType("character varying(256)");

                    b.HasKey("Id");

                    b.HasIndex("NormalizedEmail")
                        .HasDatabaseName("EmailIndex");

                    b.HasIndex("NormalizedUserName")
                        .IsUnique()
                        .HasDatabaseName("UserNameIndex");

                    b.ToTable("AspNetUsers");
                });

            modelBuilder.Entity("RS3Bot.Abstractions.Model.CurrentTask", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("integer")
                        .UseIdentityByDefaultColumn();

                    b.Property<decimal>("ChannelId")
                        .HasColumnType("numeric(20,0)");

                    b.Property<string>("Command")
                        .HasColumnType("text");

                    b.Property<string>("CompletionMessage")
                        .HasColumnType("text");

                    b.Property<decimal>("MessageId")
                        .HasColumnType("numeric(20,0)");

                    b.Property<bool>("Notified")
                        .HasColumnType("boolean");

                    b.Property<string>("TaskName")
                        .HasColumnType("text");

                    b.Property<DateTime?>("UnlockTime")
                        .HasColumnType("timestamp without time zone");

                    b.Property<string>("UserId")
                        .HasColumnType("text");

                    b.HasKey("Id");

                    b.HasIndex("UserId")
                        .IsUnique();

                    b.ToTable("CurrentTasks");
                });

            modelBuilder.Entity("RS3Bot.Abstractions.Model.EquipmentItem", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("integer")
                        .UseIdentityByDefaultColumn();

                    b.Property<int>("EquipmentSlot")
                        .HasColumnType("integer");

                    b.Property<string>("EquipmentType")
                        .HasColumnType("text");

                    b.Property<string>("UserId")
                        .HasColumnType("text");

                    b.HasKey("Id");

                    b.HasIndex("UserId");

                    b.ToTable("EquipmentItems");
                });

            modelBuilder.Entity("RS3Bot.Abstractions.Model.ExpGain", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("integer")
                        .UseIdentityByDefaultColumn();

                    b.Property<double>("Amount")
                        .HasColumnType("double precision");

                    b.Property<int>("CurrentTaskId")
                        .HasColumnType("integer");

                    b.Property<int>("Skill")
                        .HasColumnType("integer");

                    b.HasKey("Id");

                    b.HasIndex("CurrentTaskId");

                    b.ToTable("CurrentTaskXpGains");
                });

            modelBuilder.Entity("RS3Bot.Abstractions.Model.Skill", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("integer")
                        .UseIdentityByDefaultColumn();

                    b.Property<int>("CurrentLevel")
                        .HasColumnType("integer");

                    b.Property<double>("Experience")
                        .HasColumnType("double precision");

                    b.Property<int>("MaximumLevel")
                        .HasColumnType("integer");

                    b.Property<int>("SkillId")
                        .HasColumnType("integer");

                    b.Property<int?>("SkillSetId")
                        .HasColumnType("integer");

                    b.HasKey("Id");

                    b.HasIndex("SkillSetId");

                    b.ToTable("Skills");
                });

            modelBuilder.Entity("RS3Bot.Abstractions.Model.SkillSet", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("integer")
                        .UseIdentityByDefaultColumn();

                    b.Property<int>("Combat")
                        .HasColumnType("integer");

                    b.Property<string>("UserId")
                        .HasColumnType("text");

                    b.HasKey("Id");

                    b.HasIndex("UserId")
                        .IsUnique();

                    b.ToTable("SkillSets");
                });

            modelBuilder.Entity("RS3Bot.Abstractions.Model.TaskItem", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("integer")
                        .UseIdentityByDefaultColumn();

                    b.Property<int>("CurrentTaskId")
                        .HasColumnType("integer");

                    b.HasKey("Id");

                    b.HasIndex("CurrentTaskId");

                    b.ToTable("TaskItems");
                });

            modelBuilder.Entity("RS3Bot.Abstractions.Model.UserItem", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("integer")
                        .UseIdentityByDefaultColumn();

                    b.Property<string>("UserId")
                        .HasColumnType("text");

                    b.HasKey("Id");

                    b.HasIndex("UserId");

                    b.ToTable("UserItems");
                });

            modelBuilder.Entity("Microsoft.AspNetCore.Identity.IdentityRoleClaim<string>", b =>
                {
                    b.HasOne("Microsoft.AspNetCore.Identity.IdentityRole<string>", null)
                        .WithMany()
                        .HasForeignKey("RoleId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();
                });

            modelBuilder.Entity("Microsoft.AspNetCore.Identity.IdentityUserClaim<string>", b =>
                {
                    b.HasOne("RS3Bot.Abstractions.Model.ApplicationUser", null)
                        .WithMany()
                        .HasForeignKey("UserId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();
                });

            modelBuilder.Entity("Microsoft.AspNetCore.Identity.IdentityUserLogin<string>", b =>
                {
                    b.HasOne("RS3Bot.Abstractions.Model.ApplicationUser", null)
                        .WithMany()
                        .HasForeignKey("UserId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();
                });

            modelBuilder.Entity("Microsoft.AspNetCore.Identity.IdentityUserRole<string>", b =>
                {
                    b.HasOne("Microsoft.AspNetCore.Identity.IdentityRole<string>", null)
                        .WithMany()
                        .HasForeignKey("RoleId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.HasOne("RS3Bot.Abstractions.Model.ApplicationUser", null)
                        .WithMany()
                        .HasForeignKey("UserId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();
                });

            modelBuilder.Entity("Microsoft.AspNetCore.Identity.IdentityUserToken<string>", b =>
                {
                    b.HasOne("RS3Bot.Abstractions.Model.ApplicationUser", null)
                        .WithMany()
                        .HasForeignKey("UserId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();
                });

            modelBuilder.Entity("RS3Bot.Abstractions.Model.CurrentTask", b =>
                {
                    b.HasOne("RS3Bot.Abstractions.Model.ApplicationUser", "User")
                        .WithOne("CurrentTask")
                        .HasForeignKey("RS3Bot.Abstractions.Model.CurrentTask", "UserId");

                    b.Navigation("User");
                });

            modelBuilder.Entity("RS3Bot.Abstractions.Model.EquipmentItem", b =>
                {
                    b.HasOne("RS3Bot.Abstractions.Model.ApplicationUser", "User")
                        .WithMany("Equipment")
                        .HasForeignKey("UserId");

                    b.OwnsOne("RS3Bot.Abstractions.Model.Item", "Item", b1 =>
                        {
                            b1.Property<int>("EquipmentItemId")
                                .ValueGeneratedOnAdd()
                                .HasColumnType("integer")
                                .UseIdentityByDefaultColumn();

                            b1.Property<decimal>("Amount")
                                .HasColumnType("numeric(20,0)");

                            b1.Property<int>("ItemId")
                                .HasColumnType("integer");

                            b1.HasKey("EquipmentItemId");

                            b1.ToTable("EquipmentItems");

                            b1.WithOwner()
                                .HasForeignKey("EquipmentItemId");
                        });

                    b.Navigation("Item");

                    b.Navigation("User");
                });

            modelBuilder.Entity("RS3Bot.Abstractions.Model.ExpGain", b =>
                {
                    b.HasOne("RS3Bot.Abstractions.Model.CurrentTask", "CurrentTask")
                        .WithMany("ExpGains")
                        .HasForeignKey("CurrentTaskId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.Navigation("CurrentTask");
                });

            modelBuilder.Entity("RS3Bot.Abstractions.Model.Skill", b =>
                {
                    b.HasOne("RS3Bot.Abstractions.Model.SkillSet", null)
                        .WithMany("Skills")
                        .HasForeignKey("SkillSetId");
                });

            modelBuilder.Entity("RS3Bot.Abstractions.Model.SkillSet", b =>
                {
                    b.HasOne("RS3Bot.Abstractions.Model.ApplicationUser", "User")
                        .WithOne("SkillSet")
                        .HasForeignKey("RS3Bot.Abstractions.Model.SkillSet", "UserId");

                    b.Navigation("User");
                });

            modelBuilder.Entity("RS3Bot.Abstractions.Model.TaskItem", b =>
                {
                    b.HasOne("RS3Bot.Abstractions.Model.CurrentTask", "CurrentTask")
                        .WithMany("Items")
                        .HasForeignKey("CurrentTaskId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.OwnsOne("RS3Bot.Abstractions.Model.Item", "Item", b1 =>
                        {
                            b1.Property<int>("TaskItemId")
                                .ValueGeneratedOnAdd()
                                .HasColumnType("integer")
                                .UseIdentityByDefaultColumn();

                            b1.Property<decimal>("Amount")
                                .HasColumnType("numeric(20,0)");

                            b1.Property<int>("ItemId")
                                .HasColumnType("integer");

                            b1.HasKey("TaskItemId");

                            b1.ToTable("TaskItems");

                            b1.WithOwner()
                                .HasForeignKey("TaskItemId");
                        });

                    b.Navigation("CurrentTask");

                    b.Navigation("Item");
                });

            modelBuilder.Entity("RS3Bot.Abstractions.Model.UserItem", b =>
                {
                    b.HasOne("RS3Bot.Abstractions.Model.ApplicationUser", "User")
                        .WithMany("Items")
                        .HasForeignKey("UserId");

                    b.OwnsOne("RS3Bot.Abstractions.Model.Item", "Item", b1 =>
                        {
                            b1.Property<int>("UserItemId")
                                .ValueGeneratedOnAdd()
                                .HasColumnType("integer")
                                .UseIdentityByDefaultColumn();

                            b1.Property<decimal>("Amount")
                                .HasColumnType("numeric(20,0)");

                            b1.Property<int>("ItemId")
                                .HasColumnType("integer");

                            b1.HasKey("UserItemId");

                            b1.ToTable("UserItems");

                            b1.WithOwner()
                                .HasForeignKey("UserItemId");
                        });

                    b.Navigation("Item");

                    b.Navigation("User");
                });

            modelBuilder.Entity("RS3Bot.Abstractions.Model.ApplicationUser", b =>
                {
                    b.Navigation("CurrentTask")
                        .IsRequired();

                    b.Navigation("Equipment");

                    b.Navigation("Items");

                    b.Navigation("SkillSet")
                        .IsRequired();
                });

            modelBuilder.Entity("RS3Bot.Abstractions.Model.CurrentTask", b =>
                {
                    b.Navigation("ExpGains");

                    b.Navigation("Items");
                });

            modelBuilder.Entity("RS3Bot.Abstractions.Model.SkillSet", b =>
                {
                    b.Navigation("Skills");
                });
#pragma warning restore 612, 618
        }
    }
}