﻿// <auto-generated />
using System;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;
using Npgsql.EntityFrameworkCore.PostgreSQL.Metadata;
using ZBank.Infrastructure.Persistance;

#nullable disable

namespace ZBank.Infrastructure.Migrations
{
    [DbContext(typeof(ZBankDbContext))]
    partial class ZBankDbContextModelSnapshot : ModelSnapshot
    {
        protected override void BuildModel(ModelBuilder modelBuilder)
        {
#pragma warning disable 612, 618
            modelBuilder
                .HasAnnotation("ProductVersion", "8.0.10")
                .HasAnnotation("Relational:MaxIdentifierLength", 63);

            NpgsqlModelBuilderExtensions.UseIdentityByDefaultColumns(modelBuilder);

            modelBuilder.Entity("ZBank.Domain.ActivityAggregate.Activity", b =>
                {
                    b.Property<Guid>("Id")
                        .HasColumnType("uuid");

                    b.Property<DateTime>("CreatedDateTime")
                        .HasColumnType("timestamp with time zone");

                    b.Property<string>("Description")
                        .IsRequired()
                        .HasColumnType("text");

                    b.Property<string>("Name")
                        .IsRequired()
                        .HasMaxLength(100)
                        .HasColumnType("character varying(100)");

                    b.Property<Guid>("ProjectId")
                        .HasColumnType("uuid");

                    b.Property<Guid>("TeamId")
                        .HasColumnType("uuid");

                    b.Property<DateTime>("UpdatedDateTime")
                        .HasColumnType("timestamp with time zone");

                    b.HasKey("Id");

                    b.ToTable("Activities", (string)null);
                });

            modelBuilder.Entity("ZBank.Domain.NotificationAggregate.Notification", b =>
                {
                    b.Property<Guid>("Id")
                        .HasColumnType("uuid");

                    b.Property<string>("Content")
                        .IsRequired()
                        .HasColumnType("text");

                    b.Property<DateTime>("CreatedDateTime")
                        .HasColumnType("timestamp with time zone");

                    b.Property<bool>("IsRead")
                        .HasColumnType("boolean");

                    b.Property<Guid>("NotificationReceiverId")
                        .HasColumnType("uuid");

                    b.Property<string>("NotificationType")
                        .IsRequired()
                        .HasMaxLength(13)
                        .HasColumnType("character varying(13)");

                    b.Property<DateTime>("UpdatedDateTime")
                        .HasColumnType("timestamp with time zone");

                    b.HasKey("Id");

                    b.ToTable("Notifications", (string)null);

                    b.HasDiscriminator<string>("NotificationType").HasValue("Notification");

                    b.UseTphMappingStrategy();
                });

            modelBuilder.Entity("ZBank.Domain.ProfileAggregate.Profile", b =>
                {
                    b.Property<Guid>("Id")
                        .HasColumnType("uuid");

                    b.Property<DateTime>("CreatedDateTime")
                        .HasColumnType("timestamp with time zone");

                    b.Property<string>("Name")
                        .IsRequired()
                        .HasMaxLength(100)
                        .HasColumnType("character varying(100)");

                    b.Property<Guid>("OwnerId")
                        .HasColumnType("uuid");

                    b.Property<DateTime>("UpdatedDateTime")
                        .HasColumnType("timestamp with time zone");

                    b.HasKey("Id");

                    b.ToTable("Profiles", (string)null);
                });

            modelBuilder.Entity("ZBank.Domain.ProjectAggregate.Project", b =>
                {
                    b.Property<Guid>("Id")
                        .HasColumnType("uuid");

                    b.Property<DateTime>("CreatedDateTime")
                        .HasColumnType("timestamp with time zone");

                    b.Property<string>("Name")
                        .IsRequired()
                        .HasMaxLength(100)
                        .HasColumnType("character varying(100)");

                    b.Property<Guid>("TeamId")
                        .HasColumnType("uuid");

                    b.Property<DateTime>("UpdatedDateTime")
                        .HasColumnType("timestamp with time zone");

                    b.HasKey("Id");

                    b.ToTable("Projects", (string)null);
                });

            modelBuilder.Entity("ZBank.Domain.TeamAggregate.Team", b =>
                {
                    b.Property<Guid>("Id")
                        .HasColumnType("uuid");

                    b.Property<DateTime>("CreatedDateTime")
                        .HasColumnType("timestamp with time zone");

                    b.Property<string>("Description")
                        .HasMaxLength(200)
                        .HasColumnType("character varying(200)");

                    b.Property<string>("Name")
                        .IsRequired()
                        .HasMaxLength(100)
                        .HasColumnType("character varying(100)");

                    b.Property<DateTime>("UpdatedDateTime")
                        .HasColumnType("timestamp with time zone");

                    b.HasKey("Id");

                    b.ToTable("Teams", (string)null);
                });

            modelBuilder.Entity("ZBank.Domain.UserAggregate.User", b =>
                {
                    b.Property<Guid>("Id")
                        .HasColumnType("uuid");

                    b.Property<DateTime>("CreatedDateTime")
                        .HasColumnType("timestamp with time zone");

                    b.Property<string>("Email")
                        .IsRequired()
                        .HasMaxLength(100)
                        .HasColumnType("character varying(100)");

                    b.Property<string>("FirstName")
                        .IsRequired()
                        .HasMaxLength(100)
                        .HasColumnType("character varying(100)");

                    b.Property<string>("LastName")
                        .IsRequired()
                        .HasMaxLength(200)
                        .HasColumnType("character varying(200)");

                    b.Property<string>("Password")
                        .IsRequired()
                        .HasMaxLength(100)
                        .HasColumnType("character varying(100)");

                    b.Property<DateTime>("UpdatedDateTime")
                        .HasColumnType("timestamp with time zone");

                    b.HasKey("Id");

                    b.ToTable("Users", (string)null);
                });

            modelBuilder.Entity("ZBank.Domain.WalletAggregate.Wallet", b =>
                {
                    b.Property<Guid>("Id")
                        .HasColumnType("uuid");

                    b.Property<string>("Address")
                        .IsRequired()
                        .HasMaxLength(50)
                        .HasColumnType("character varying(50)");

                    b.Property<DateTime>("CreatedDateTime")
                        .HasColumnType("timestamp with time zone");

                    b.Property<Guid>("ProfileId")
                        .HasColumnType("uuid");

                    b.Property<int>("Type")
                        .HasColumnType("integer");

                    b.Property<DateTime>("UpdatedDateTime")
                        .HasColumnType("timestamp with time zone");

                    b.HasKey("Id");

                    b.ToTable("Wallets", (string)null);
                });

            modelBuilder.Entity("ZBank.Domain.NotificationAggregate.InformationNotification", b =>
                {
                    b.HasBaseType("ZBank.Domain.NotificationAggregate.Notification");

                    b.HasDiscriminator().HasValue("Information");
                });

            modelBuilder.Entity("ZBank.Domain.NotificationAggregate.TeamInviteNotification", b =>
                {
                    b.HasBaseType("ZBank.Domain.NotificationAggregate.Notification");

                    b.Property<Guid>("TeamId")
                        .HasColumnType("uuid");

                    b.HasDiscriminator().HasValue("TeamInvite");
                });

            modelBuilder.Entity("ZBank.Domain.ActivityAggregate.Activity", b =>
                {
                    b.OwnsMany("ZBank.Domain.ActivityAggregate.Entities.ActivityLog", "ActivityLogs", b1 =>
                        {
                            b1.Property<Guid>("Id")
                                .HasColumnType("uuid");

                            b1.Property<Guid>("ActivityId")
                                .HasColumnType("uuid");

                            b1.Property<Guid>("ProfileId")
                                .HasColumnType("uuid");

                            b1.Property<DateTime>("TimeStamp")
                                .HasColumnType("timestamp with time zone");

                            b1.Property<Guid>("UserId")
                                .HasColumnType("uuid");

                            b1.Property<Guid>("WalletId")
                                .HasColumnType("uuid");

                            b1.HasKey("Id", "ActivityId");

                            b1.HasIndex("ActivityId");

                            b1.ToTable("ActivityLogs", (string)null);

                            b1.WithOwner()
                                .HasForeignKey("ActivityId");
                        });

                    b.Navigation("ActivityLogs");
                });

            modelBuilder.Entity("ZBank.Domain.NotificationAggregate.Notification", b =>
                {
                    b.OwnsOne("ZBank.Domain.NotificationAggregate.ValueObjects.NotificationSender", "NotificationSender", b1 =>
                        {
                            b1.Property<Guid>("NotificationId")
                                .HasColumnType("uuid");

                            b1.Property<string>("SenderFullName")
                                .IsRequired()
                                .HasMaxLength(200)
                                .HasColumnType("character varying(200)")
                                .HasColumnName("SenderFullName");

                            b1.Property<Guid>("SenderId")
                                .HasColumnType("uuid")
                                .HasColumnName("SenderId");

                            b1.HasKey("NotificationId");

                            b1.ToTable("Notifications");

                            b1.WithOwner()
                                .HasForeignKey("NotificationId");
                        });

                    b.Navigation("NotificationSender")
                        .IsRequired();
                });

            modelBuilder.Entity("ZBank.Domain.ProfileAggregate.Profile", b =>
                {
                    b.OwnsMany("ZBank.Domain.WalletAggregate.ValueObjects.WalletId", "WalletIds", b1 =>
                        {
                            b1.Property<int>("Id")
                                .ValueGeneratedOnAdd()
                                .HasColumnType("integer");

                            NpgsqlPropertyBuilderExtensions.UseIdentityByDefaultColumn(b1.Property<int>("Id"));

                            b1.Property<Guid>("ProfileId")
                                .HasColumnType("uuid");

                            b1.Property<Guid>("Value")
                                .HasColumnType("uuid")
                                .HasColumnName("WalletId");

                            b1.HasKey("Id");

                            b1.HasIndex("ProfileId");

                            b1.ToTable("ProfileWalletIds", (string)null);

                            b1.WithOwner()
                                .HasForeignKey("ProfileId");
                        });

                    b.Navigation("WalletIds");
                });

            modelBuilder.Entity("ZBank.Domain.TeamAggregate.Team", b =>
                {
                    b.OwnsMany("ZBank.Domain.ActivityAggregate.ValueObjects.ActivityId", "ActivityIds", b1 =>
                        {
                            b1.Property<int>("Id")
                                .ValueGeneratedOnAdd()
                                .HasColumnType("integer");

                            NpgsqlPropertyBuilderExtensions.UseIdentityByDefaultColumn(b1.Property<int>("Id"));

                            b1.Property<Guid>("TeamId")
                                .HasColumnType("uuid");

                            b1.Property<Guid>("Value")
                                .HasColumnType("uuid")
                                .HasColumnName("ActivityId");

                            b1.HasKey("Id");

                            b1.HasIndex("TeamId");

                            b1.ToTable("TeamActivityIds", (string)null);

                            b1.WithOwner()
                                .HasForeignKey("TeamId");
                        });

                    b.OwnsMany("ZBank.Domain.ProjectAggregate.ValueObjects.ProjectId", "ProjectIds", b1 =>
                        {
                            b1.Property<int>("Id")
                                .ValueGeneratedOnAdd()
                                .HasColumnType("integer");

                            NpgsqlPropertyBuilderExtensions.UseIdentityByDefaultColumn(b1.Property<int>("Id"));

                            b1.Property<Guid>("TeamId")
                                .HasColumnType("uuid");

                            b1.Property<Guid>("Value")
                                .HasColumnType("uuid")
                                .HasColumnName("ProjectId");

                            b1.HasKey("Id");

                            b1.HasIndex("TeamId");

                            b1.ToTable("TeamProjectIds", (string)null);

                            b1.WithOwner()
                                .HasForeignKey("TeamId");
                        });

                    b.OwnsMany("ZBank.Domain.UserAggregate.ValueObjects.UserId", "UserIds", b1 =>
                        {
                            b1.Property<int>("Id")
                                .ValueGeneratedOnAdd()
                                .HasColumnType("integer");

                            NpgsqlPropertyBuilderExtensions.UseIdentityByDefaultColumn(b1.Property<int>("Id"));

                            b1.Property<Guid>("TeamId")
                                .HasColumnType("uuid");

                            b1.Property<Guid>("Value")
                                .HasColumnType("uuid")
                                .HasColumnName("UserId");

                            b1.HasKey("Id");

                            b1.HasIndex("TeamId");

                            b1.ToTable("TeamUserIds", (string)null);

                            b1.WithOwner()
                                .HasForeignKey("TeamId");
                        });

                    b.Navigation("ActivityIds");

                    b.Navigation("ProjectIds");

                    b.Navigation("UserIds");
                });

            modelBuilder.Entity("ZBank.Domain.UserAggregate.User", b =>
                {
                    b.OwnsMany("ZBank.Domain.NotificationAggregate.ValueObjects.NotificationId", "NotificationIds", b1 =>
                        {
                            b1.Property<int>("Id")
                                .ValueGeneratedOnAdd()
                                .HasColumnType("integer");

                            NpgsqlPropertyBuilderExtensions.UseIdentityByDefaultColumn(b1.Property<int>("Id"));

                            b1.Property<Guid>("UserId")
                                .HasColumnType("uuid");

                            b1.Property<Guid>("Value")
                                .HasColumnType("uuid")
                                .HasColumnName("NotificationId");

                            b1.HasKey("Id");

                            b1.HasIndex("UserId");

                            b1.ToTable("UserNotificationIds", (string)null);

                            b1.WithOwner()
                                .HasForeignKey("UserId");
                        });

                    b.OwnsMany("ZBank.Domain.ProfileAggregate.ValueObjects.ProfileId", "ProfileIds", b1 =>
                        {
                            b1.Property<int>("Id")
                                .ValueGeneratedOnAdd()
                                .HasColumnType("integer");

                            NpgsqlPropertyBuilderExtensions.UseIdentityByDefaultColumn(b1.Property<int>("Id"));

                            b1.Property<Guid>("UserId")
                                .HasColumnType("uuid");

                            b1.Property<Guid>("Value")
                                .HasColumnType("uuid")
                                .HasColumnName("ProfileId");

                            b1.HasKey("Id");

                            b1.HasIndex("UserId");

                            b1.ToTable("UserProfileIds", (string)null);

                            b1.WithOwner()
                                .HasForeignKey("UserId");
                        });

                    b.OwnsMany("ZBank.Domain.TeamAggregate.ValueObjects.TeamId", "TeamIds", b1 =>
                        {
                            b1.Property<int>("Id")
                                .ValueGeneratedOnAdd()
                                .HasColumnType("integer");

                            NpgsqlPropertyBuilderExtensions.UseIdentityByDefaultColumn(b1.Property<int>("Id"));

                            b1.Property<Guid>("UserId")
                                .HasColumnType("uuid");

                            b1.Property<Guid>("Value")
                                .HasColumnType("uuid")
                                .HasColumnName("TeamId");

                            b1.HasKey("Id");

                            b1.HasIndex("UserId");

                            b1.ToTable("UserTeamIds", (string)null);

                            b1.WithOwner()
                                .HasForeignKey("UserId");
                        });

                    b.Navigation("NotificationIds");

                    b.Navigation("ProfileIds");

                    b.Navigation("TeamIds");
                });
#pragma warning restore 612, 618
        }
    }
}
