using System;
using System.Collections.Generic;
using System.Text;
using Microsoft.EntityFrameworkCore;
using VerveGroupTask.Models;

namespace StaffApp.Data
{
    public class TempDB : DbContext
    {
        public DbSet<User> Users {get; set;}
        public DbSet<Repos> Repos {get; set;}
        public DbSet<Stargazers> Stargazers {get; set;}
        
        public TempDB(DbContextOptions<TempDB> options) : base(options)
        {
        }

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            base.OnConfiguring(optionsBuilder);
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            modelBuilder.Entity<User>(x =>
            {
                x.Property(u => u.Avatar_Url).IsRequired();
                x.Property(u => u.Location).IsRequired();
                x.Property(u => u.Name).IsRequired();
                x.HasKey(u => u.Login);
            });

            modelBuilder.Entity<Repos>(x =>
            {
                x.Property(r => r.Svn_Url).IsRequired();
                x.Property(r => r.Name).IsRequired();
                x.Property(r => r.Full_Name).IsRequired();
                x.Property(r => r.Description).IsRequired();
                x.HasOne(r => r.Owner).WithMany()
                                      .HasForeignKey(r => r.UserLogin)
                                      .IsRequired();
            });

            modelBuilder.Entity<Stargazers>(x =>
            {
                x.Property(s => s.Login).IsRequired();
                x.HasKey(s => s.Login);
                x.HasOne(s => s.Repo).WithMany()
                                     .HasForeignKey(s => s.RepoID)
                                     .IsRequired();
            });
        }
    }
}
