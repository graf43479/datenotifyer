using DateNotifier.Entities;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Text;

namespace DateNotifier
{
    public class ApplicationContext : DbContext
    {

        public ApplicationContext(DbContextOptions<ApplicationContext> options) : base(options) 
        {
            Database.EnsureCreated();
        }

       

        public DbSet<EventDate> EventDates { get; set; }
        public DbSet<EventType> EventTypes { get; set; }
        public DbSet<ExceptionDetail> ExceptionDetails { get; set; }
        public DbSet<Person> Persons { get; set; }


          protected override void OnModelCreating(ModelBuilder modelBuilder)
          {
            //конфиги
            //modelBuilder.Conventions.Remove<PluralizingTableNameConvention>();

            //modelBuilder.Entity<IdentityUserLogin>().HasKey<string>(l => l.UserId);
            //modelBuilder.Entity<IdentityRole>().HasKey<string>(r => r.Id);
            //modelBuilder.Entity<IdentityUserRole>().HasKey(r => new { r.RoleId, r.UserId });

            //modelBuilder.Configurations.Add(new ProductConfig());
            //modelBuilder.Configurations.Add(new ExceptionDetailConfig());
            base.OnModelCreating(modelBuilder);


          }
          
    }
}
