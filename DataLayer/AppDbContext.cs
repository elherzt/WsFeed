using DataLayer.Models;
using DataLayer.Utilities;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DataLayer
{
    public class AppDbContext : DbContext
    {
        public DbSet<User> Users { get; set; }
        public DbSet<Feed> Feeds { get; set; }
        public DbSet<Topic> Topics { get; set; }
        public DbSet<UserFeed> UserFeeds { get; set; }


        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            optionsBuilder.UseSqlite("Data Source=DBWsFeeed.db");
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            // Configuración de la relación entre Feed y Topic
            modelBuilder.Entity<Feed>()
                .HasMany(f => f.Topics)
                .WithOne(t => t.Feed)
                .HasForeignKey(t => t.FeedId)
                .OnDelete(DeleteBehavior.Cascade); // Cuando un Feed se elimina, se eliminan sus Topics

            // Configuración de la relación entre User y Feed
            modelBuilder.Entity<User>()
                .HasMany(u => u.Feeds)
                .WithOne(f => f.User)
                .HasForeignKey(f => f.UserId)
                .OnDelete(DeleteBehavior.Cascade); // Cuando un User se elimina, se eliminan sus Feeds

            // Configuración de la tabla intermedia UserFeed con llave compuesta
            modelBuilder.Entity<UserFeed>()
                .HasKey(uf => new { uf.UserId, uf.FeedId }); // Llave compuesta

            modelBuilder.Entity<UserFeed>()
                .HasOne(uf => uf.User)
                .WithMany(u => u.FollowedFeeds)
                .HasForeignKey(uf => uf.UserId)
                .OnDelete(DeleteBehavior.Cascade); // Cuando un User se elimina, se elimina su relación con el Feed



            SeedData(modelBuilder);

        }

        private void SeedData(ModelBuilder modelBuilder)
        {
            var securedPassword = Cryptography.Encrypt("kiosko");

            modelBuilder.Entity<User>().HasData(
                new User
                {
                    Id = 1,
                    Name = "Pedro Paramo",
                    Mail = "kiosko@example.com",
                    Password = securedPassword,
                    CreatedDate = DateTime.UtcNow,
                    LastLoginDate = DateTime.UtcNow
                }
            );

            // Agregar un feed y sus topics
            modelBuilder.Entity<Feed>().HasData(
                new Feed
                {
                    Id = 1,
                    Name = "Sports",
                    Description = "All about sports",
                    UserId = 1
                }
            );

            modelBuilder.Entity<Topic>().HasData(
                new Topic { Id = 1, Name = "Swimming", Description = "Swimming News", FeedId = 1 },
                new Topic { Id = 2, Name = "Cycling", Description = "Cycling News", FeedId = 1 },
                new Topic { Id = 3, Name = "Tennis", Description = "Tennis News", FeedId = 1 },
                new Topic { Id = 4, Name = "Boxing", Description = "Boxing News", FeedId = 1 },
                new Topic { Id = 5, Name = "Shooting", Description = "Shooting News", FeedId = 1 },   //limit (?)
                new Topic { Id = 6, Name = "Equestrian", Description = "Equestrian News", FeedId = 1 },
                new Topic { Id = 7, Name = "Jumping", Description = "Jumping News", FeedId = 1 },
                new Topic { Id = 8, Name = "Sailing", Description = "Sailing News", FeedId = 1 },
                new Topic { Id = 9, Name = "Rhythmic", Description = "Rhythmic News", FeedId = 1 },
                new Topic { Id = 10, Name = "Gymnastics", Description = "Gymnastics News", FeedId = 1 }
            );
        }
    }
}
