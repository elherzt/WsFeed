﻿// <auto-generated />
using System;
using DataLayer;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore.Migrations;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;

#nullable disable

namespace DataLayer.Migrations
{
    [DbContext(typeof(AppDbContext))]
    [Migration("20240901002959_SeedAgain")]
    partial class SeedAgain
    {
        /// <inheritdoc />
        protected override void BuildTargetModel(ModelBuilder modelBuilder)
        {
#pragma warning disable 612, 618
            modelBuilder.HasAnnotation("ProductVersion", "8.0.8");

            modelBuilder.Entity("DataLayer.Models.Feed", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("INTEGER");

                    b.Property<string>("Description")
                        .IsRequired()
                        .HasColumnType("TEXT");

                    b.Property<bool>("IsPrivate")
                        .HasColumnType("INTEGER");

                    b.Property<string>("Name")
                        .IsRequired()
                        .HasColumnType("TEXT");

                    b.Property<int>("UserId")
                        .HasColumnType("INTEGER");

                    b.HasKey("Id");

                    b.HasIndex("UserId");

                    b.ToTable("Feeds");

                    b.HasData(
                        new
                        {
                            Id = 1,
                            Description = "All about sports",
                            IsPrivate = false,
                            Name = "Sports",
                            UserId = 1
                        });
                });

            modelBuilder.Entity("DataLayer.Models.Topic", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("INTEGER");

                    b.Property<string>("Description")
                        .IsRequired()
                        .HasColumnType("TEXT");

                    b.Property<int>("FeedId")
                        .HasColumnType("INTEGER");

                    b.Property<string>("Name")
                        .IsRequired()
                        .HasColumnType("TEXT");

                    b.HasKey("Id");

                    b.HasIndex("FeedId");

                    b.ToTable("Topics");

                    b.HasData(
                        new
                        {
                            Id = 1,
                            Description = "Swimming News",
                            FeedId = 1,
                            Name = "Swimming"
                        },
                        new
                        {
                            Id = 2,
                            Description = "Cycling News",
                            FeedId = 1,
                            Name = "Cycling"
                        },
                        new
                        {
                            Id = 3,
                            Description = "Tennis News",
                            FeedId = 1,
                            Name = "Tennis"
                        },
                        new
                        {
                            Id = 4,
                            Description = "Boxing News",
                            FeedId = 1,
                            Name = "Boxing"
                        },
                        new
                        {
                            Id = 5,
                            Description = "Shooting News",
                            FeedId = 1,
                            Name = "Shooting"
                        },
                        new
                        {
                            Id = 6,
                            Description = "Equestrian News",
                            FeedId = 1,
                            Name = "Equestrian"
                        },
                        new
                        {
                            Id = 7,
                            Description = "Jumping News",
                            FeedId = 1,
                            Name = "Jumping"
                        },
                        new
                        {
                            Id = 8,
                            Description = "Sailing News",
                            FeedId = 1,
                            Name = "Sailing"
                        },
                        new
                        {
                            Id = 9,
                            Description = "Rhythmic News",
                            FeedId = 1,
                            Name = "Rhythmic"
                        },
                        new
                        {
                            Id = 10,
                            Description = "Gymnastics News",
                            FeedId = 1,
                            Name = "Gymnastics"
                        });
                });

            modelBuilder.Entity("DataLayer.Models.User", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("INTEGER");

                    b.Property<DateTime>("CreatedDate")
                        .HasColumnType("TEXT");

                    b.Property<DateTime?>("LastLoginDate")
                        .HasColumnType("TEXT");

                    b.Property<string>("Mail")
                        .IsRequired()
                        .HasColumnType("TEXT");

                    b.Property<string>("Name")
                        .IsRequired()
                        .HasColumnType("TEXT");

                    b.Property<string>("Password")
                        .IsRequired()
                        .HasColumnType("TEXT");

                    b.HasKey("Id");

                    b.ToTable("Users");

                    b.HasData(
                        new
                        {
                            Id = 1,
                            CreatedDate = new DateTime(2024, 9, 1, 0, 29, 59, 377, DateTimeKind.Utc).AddTicks(9460),
                            LastLoginDate = new DateTime(2024, 9, 1, 0, 29, 59, 377, DateTimeKind.Utc).AddTicks(9461),
                            Mail = "kiosko@example.com",
                            Name = "Pedro Paramo",
                            Password = "$2a$11$rxziVW5vnCWEh74E0RfTR.ybxTCWX4LHiUUPN3amac7ZuZproWiOK"
                        });
                });

            modelBuilder.Entity("DataLayer.Models.UserFeed", b =>
                {
                    b.Property<int>("UserId")
                        .HasColumnType("INTEGER");

                    b.Property<int>("FeedId")
                        .HasColumnType("INTEGER");

                    b.HasKey("UserId", "FeedId");

                    b.HasIndex("FeedId");

                    b.ToTable("UserFeeds");
                });

            modelBuilder.Entity("DataLayer.Models.Feed", b =>
                {
                    b.HasOne("DataLayer.Models.User", "User")
                        .WithMany("Feeds")
                        .HasForeignKey("UserId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.Navigation("User");
                });

            modelBuilder.Entity("DataLayer.Models.Topic", b =>
                {
                    b.HasOne("DataLayer.Models.Feed", "Feed")
                        .WithMany("Topics")
                        .HasForeignKey("FeedId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.Navigation("Feed");
                });

            modelBuilder.Entity("DataLayer.Models.UserFeed", b =>
                {
                    b.HasOne("DataLayer.Models.Feed", "Feed")
                        .WithMany("FollowingUsers")
                        .HasForeignKey("FeedId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.HasOne("DataLayer.Models.User", "User")
                        .WithMany("FollowedFeeds")
                        .HasForeignKey("UserId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.Navigation("Feed");

                    b.Navigation("User");
                });

            modelBuilder.Entity("DataLayer.Models.Feed", b =>
                {
                    b.Navigation("FollowingUsers");

                    b.Navigation("Topics");
                });

            modelBuilder.Entity("DataLayer.Models.User", b =>
                {
                    b.Navigation("Feeds");

                    b.Navigation("FollowedFeeds");
                });
#pragma warning restore 612, 618
        }
    }
}
