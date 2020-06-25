using System;
using System.Collections.Generic;
using System.Text;
using Microsoft.EntityFrameworkCore;

namespace StaffApp.Data
{
    public class TempDB : DbContext
    {
        public DBSet<User> Users {get; set;}
        public DBSet<Repos> Repos {get; set;}
        public DBSet<Stargazers> Stargazers {get; set;}
        
        public TempDb(DbContextOptions<TempDb> options) : base(options)
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
                 x.Property(u => u.Avatar_Url).IsRequired();
                 x.Property(u => u.Location).IsRequired();
                 x.Property(u => u.Name).IsRequired();
            );

            modelBuilder.Entity<Repos>(x =>
                 x.Property(r => r.svn_Url).IsRequired();
                 x.Property(r => r.Name).IsRequired();
                 x.Property(r => r.Full_Name).IsRequired();
                 x.Property(r => r.Repo_Url).IsRequired();
                 x.Property(r => r.Description).IsRequired();
                 x.Property(r => r.User).WithMany()
                                        .HasForeignKey(r => r.UserId)
                                        .IsRequired();
            );
