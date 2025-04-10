﻿using Microsoft.EntityFrameworkCore;
using UserService.Models;
namespace UserService.Data
{
    public class UserDbContext : DbContext
    {
        public UserDbContext(DbContextOptions<UserDbContext> options) : base(options) { }

        public DbSet<User> Users { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            modelBuilder.Entity<User>()
            .HasIndex(u => u.UserTag)
            .IsUnique()
            .HasFilter("[UserTag] IS NOT NULL");

            modelBuilder.Entity<Friend>()
                .HasOne
        }
    }
}
