using DataLayer.Models;
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

            modelBuilder.Entity<UserFeed>()
                .HasOne(uf => uf.Feed)
                .WithMany(f => f.FollowingUsers)
                .HasForeignKey(uf => uf.FeedId)
                .OnDelete(DeleteBehavior.Cascade); // Cuando un Feed se elimina, se elimina su relación con el User
        }
    }
}
