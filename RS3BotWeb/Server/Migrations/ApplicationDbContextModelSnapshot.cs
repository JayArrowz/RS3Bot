﻿// <auto-generated />
using System;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore.Metadata;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;
using RS3Bot.DAL;

namespace RS3BotWeb.Server.Migrations
{
    [DbContext(typeof(ApplicationDbContext))]
    partial class ApplicationDbContextModelSnapshot : ModelSnapshot
    {
        protected override void BuildModel(ModelBuilder modelBuilder)
        {
#pragma warning disable 612, 618
            modelBuilder
                .UseIdentityColumns()
                .HasAnnotation("Relational:MaxIdentifierLength", 128)
                .HasAnnotation("ProductVersion", "5.0.1");

            modelBuilder.Entity("Microsoft.AspNetCore.Identity.IdentityRole<string>", b =>
                {
                    b.Property<string>("Id")
                        .HasColumnType("nvarchar(450)");

                    b.Property<string>("ConcurrencyStamp")
                        .IsConcurrencyToken()
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("Name")
                        .HasMaxLength(256)
                        .HasColumnType("nvarchar(256)");

                    b.Property<string>("NormalizedName")
                        .HasMaxLength(256)
                        .HasColumnType("nvarchar(256)");

                    b.HasKey("Id");

                    b.HasIndex("NormalizedName")
                        .IsUnique()
                        .HasDatabaseName("RoleNameIndex")
                        .HasFilter("[NormalizedName] IS NOT NULL");

                    b.ToTable("AspNetRoles");
                });

            modelBuilder.Entity("Microsoft.AspNetCore.Identity.IdentityRoleClaim<string>", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int")
                        .UseIdentityColumn();

                    b.Property<string>("ClaimType")
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("ClaimValue")
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("RoleId")
                        .IsRequired()
                        .HasColumnType("nvarchar(450)");

                    b.HasKey("Id");

                    b.HasIndex("RoleId");

                    b.ToTable("AspNetRoleClaims");
                });

            modelBuilder.Entity("Microsoft.AspNetCore.Identity.IdentityUserClaim<string>", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int")
                        .UseIdentityColumn();

                    b.Property<string>("ClaimType")
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("ClaimValue")
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("UserId")
                        .IsRequired()
                        .HasColumnType("nvarchar(450)");

                    b.HasKey("Id");

                    b.HasIndex("UserId");

                    b.ToTable("AspNetUserClaims");
                });

            modelBuilder.Entity("Microsoft.AspNetCore.Identity.IdentityUserLogin<string>", b =>
                {
                    b.Property<string>("LoginProvider")
                        .HasColumnType("nvarchar(450)");

                    b.Property<string>("ProviderKey")
                        .HasColumnType("nvarchar(450)");

                    b.Property<string>("ProviderDisplayName")
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("UserId")
                        .IsRequired()
                        .HasColumnType("nvarchar(450)");

                    b.HasKey("LoginProvider", "ProviderKey");

                    b.HasIndex("UserId");

                    b.ToTable("AspNetUserLogins");
                });

            modelBuilder.Entity("Microsoft.AspNetCore.Identity.IdentityUserRole<string>", b =>
                {
                    b.Property<string>("UserId")
                        .HasColumnType("nvarchar(450)");

                    b.Property<string>("RoleId")
                        .HasColumnType("nvarchar(450)");

                    b.HasKey("UserId", "RoleId");

                    b.HasIndex("RoleId");

                    b.ToTable("AspNetUserRoles");
                });

            modelBuilder.Entity("Microsoft.AspNetCore.Identity.IdentityUserToken<string>", b =>
                {
                    b.Property<string>("UserId")
                        .HasColumnType("nvarchar(450)");

                    b.Property<string>("LoginProvider")
                        .HasColumnType("nvarchar(450)");

                    b.Property<string>("Name")
                        .HasColumnType("nvarchar(450)");

                    b.Property<string>("Value")
                        .HasColumnType("nvarchar(max)");

                    b.HasKey("UserId", "LoginProvider", "Name");

                    b.ToTable("AspNetUserTokens");
                });

            modelBuilder.Entity("RS3Bot.Abstractions.Model.ApplicationUser", b =>
                {
                    b.Property<string>("Id")
                        .HasColumnType("nvarchar(450)");

                    b.Property<int>("AccessFailedCount")
                        .HasColumnType("int");

                    b.Property<string>("ConcurrencyStamp")
                        .IsConcurrencyToken()
                        .HasColumnType("nvarchar(max)");

                    b.Property<int>("CurrentTaskId")
                        .HasColumnType("int");

                    b.Property<decimal>("DiscordId")
                        .HasColumnType("decimal(20,0)");

                    b.Property<string>("Email")
                        .HasMaxLength(256)
                        .HasColumnType("nvarchar(256)");

                    b.Property<bool>("EmailConfirmed")
                        .HasColumnType("bit");

                    b.Property<bool>("LockoutEnabled")
                        .HasColumnType("bit");

                    b.Property<DateTimeOffset?>("LockoutEnd")
                        .HasColumnType("datetimeoffset");

                    b.Property<string>("Mention")
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("NormalizedEmail")
                        .HasMaxLength(256)
                        .HasColumnType("nvarchar(256)");

                    b.Property<string>("NormalizedUserName")
                        .HasMaxLength(256)
                        .HasColumnType("nvarchar(256)");

                    b.Property<string>("PasswordHash")
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("PhoneNumber")
                        .HasColumnType("nvarchar(max)");

                    b.Property<bool>("PhoneNumberConfirmed")
                        .HasColumnType("bit");

                    b.Property<string>("PlayerName")
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("SecurityStamp")
                        .HasColumnType("nvarchar(max)");

                    b.Property<bool>("TwoFactorEnabled")
                        .HasColumnType("bit");

                    b.Property<string>("UserName")
                        .HasMaxLength(256)
                        .HasColumnType("nvarchar(256)");

                    b.HasKey("Id");

                    b.HasIndex("NormalizedEmail")
                        .HasDatabaseName("EmailIndex");

                    b.HasIndex("NormalizedUserName")
                        .IsUnique()
                        .HasDatabaseName("UserNameIndex")
                        .HasFilter("[NormalizedUserName] IS NOT NULL");

                    b.ToTable("AspNetUsers");
                });

            modelBuilder.Entity("RS3Bot.Abstractions.Model.CurrentTask", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int")
                        .UseIdentityColumn();

                    b.Property<decimal>("ChannelId")
                        .HasColumnType("decimal(20,0)");

                    b.Property<string>("CompletionMessage")
                        .HasColumnType("nvarchar(max)");

                    b.Property<bool>("Notified")
                        .HasColumnType("bit");

                    b.Property<string>("TaskName")
                        .HasColumnType("nvarchar(max)");

                    b.Property<DateTime?>("UnlockTime")
                        .HasColumnType("datetime2");

                    b.Property<string>("UserId")
                        .HasColumnType("nvarchar(450)");

                    b.HasKey("Id");

                    b.HasIndex("UserId")
                        .IsUnique()
                        .HasFilter("[UserId] IS NOT NULL");

                    b.ToTable("CurrentTasks");
                });

            modelBuilder.Entity("RS3Bot.Abstractions.Model.EquipmentItem", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int")
                        .UseIdentityColumn();

                    b.Property<int>("EquipmentSlot")
                        .HasColumnType("int");

                    b.Property<string>("EquipmentType")
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("UserId")
                        .HasColumnType("nvarchar(450)");

                    b.HasKey("Id");

                    b.HasIndex("UserId");

                    b.ToTable("EquipmentItems");
                });

            modelBuilder.Entity("RS3Bot.Abstractions.Model.ExpGain", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int")
                        .UseIdentityColumn();

                    b.Property<double>("Amount")
                        .HasColumnType("float");

                    b.Property<int>("CurrentTaskId")
                        .HasColumnType("int");

                    b.Property<int>("Skill")
                        .HasColumnType("int");

                    b.HasKey("Id");

                    b.HasIndex("CurrentTaskId");

                    b.ToTable("CurrentTaskXpGains");
                });

            modelBuilder.Entity("RS3Bot.Abstractions.Model.Skill", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int")
                        .UseIdentityColumn();

                    b.Property<int>("CurrentLevel")
                        .HasColumnType("int");

                    b.Property<double>("Experience")
                        .HasColumnType("float");

                    b.Property<int>("MaximumLevel")
                        .HasColumnType("int");

                    b.Property<int>("SkillId")
                        .HasColumnType("int");

                    b.Property<int?>("SkillSetId")
                        .HasColumnType("int");

                    b.HasKey("Id");

                    b.HasIndex("SkillSetId");

                    b.ToTable("Skills");
                });

            modelBuilder.Entity("RS3Bot.Abstractions.Model.SkillSet", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int")
                        .UseIdentityColumn();

                    b.Property<int>("Combat")
                        .HasColumnType("int");

                    b.Property<string>("UserId")
                        .HasColumnType("nvarchar(450)");

                    b.HasKey("Id");

                    b.HasIndex("UserId")
                        .IsUnique()
                        .HasFilter("[UserId] IS NOT NULL");

                    b.ToTable("SkillSets");
                });

            modelBuilder.Entity("RS3Bot.Abstractions.Model.TaskItem", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int")
                        .UseIdentityColumn();

                    b.Property<int>("CurrentTaskId")
                        .HasColumnType("int");

                    b.HasKey("Id");

                    b.HasIndex("CurrentTaskId");

                    b.ToTable("TaskItems");
                });

            modelBuilder.Entity("RS3Bot.Abstractions.Model.UserItem", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int")
                        .UseIdentityColumn();

                    b.Property<string>("UserId")
                        .HasColumnType("nvarchar(450)");

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
                                .HasColumnType("int")
                                .UseIdentityColumn();

                            b1.Property<decimal>("Amount")
                                .HasColumnType("decimal(20,0)");

                            b1.Property<int>("ItemId")
                                .HasColumnType("int");

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
                                .HasColumnType("int")
                                .UseIdentityColumn();

                            b1.Property<decimal>("Amount")
                                .HasColumnType("decimal(20,0)");

                            b1.Property<int>("ItemId")
                                .HasColumnType("int");

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
                                .HasColumnType("int")
                                .UseIdentityColumn();

                            b1.Property<decimal>("Amount")
                                .HasColumnType("decimal(20,0)");

                            b1.Property<int>("ItemId")
                                .HasColumnType("int");

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
