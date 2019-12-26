using Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Text;

namespace Domain
{
    public class ApplicationContext : IdentityDbContext<AppUser>
    {

        public ApplicationContext(DbContextOptions<ApplicationContext> options) : base(options) 
        {
           // Database.EnsureDeleted();
            Database.EnsureCreated();
        }        

        public DbSet<EventDate> EventDates { get; set; }
        public DbSet<EventType> EventTypes { get; set; }
        public DbSet<ExceptionDetail> ExceptionDetails { get; set; }
        public DbSet<ClientProfile> ClientProfiles { get; set; }
        public DbSet<Person> Persons { get; set; }
        
        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            //конфиги
            //modelBuilder.Conventions.Remove<PluralizingTableNameConvention>();

            //modelBuilder.Entity<IdentityUserLogin>().HasKey<string>(l => l.UserId);
            //modelBuilder.Entity<IdentityRole>().HasKey<string>(r => r.Id);
            //modelBuilder.Entity<IdentityUserRole>().HasKey(r => new { r.RoleId, r.UserId });

            modelBuilder.Entity<EventDate>().HasKey(x => x.EventDateID);
            modelBuilder.Entity<EventDate>().HasOne(x => x.Person).WithMany(x=>x.EventDates).HasForeignKey(x=>x.PersonID);
            modelBuilder.Entity<EventDate>().HasOne(x => x.EventType).WithMany(x=>x.EventDates).HasForeignKey(x=>x.EventTypeID);
            modelBuilder.Entity<EventDate>().Property(x => x.EventDateID).IsRequired().ValueGeneratedOnAdd();
            modelBuilder.Entity<EventDate>().Property(x => x.Date).HasColumnType("date");

            modelBuilder.Entity<EventType>().HasKey(x => x.EventTypeID);

            modelBuilder.Entity<Person>().HasKey(x => x.PersonID);
            modelBuilder.Entity<Person>().HasOne(x => x.ClientProfile).WithMany(x=>x.Persons).HasForeignKey(x=>x.ClientProfileID);
            //modelBuilder.Entity<Person>().HasOne(x=>x.AppUser).WithMany(x=>x.)

           // modelBuilder.Entity<ClientProfile>().HasKey(x => x.ClientProfileID);
            //modelBuilder.Entity<ClientProfile>().HasOne(x => x.AppUser).WithOne(x=>x.ClientProfile);

            //modelBuilder.Entity<AppUser>().HasOne(x=>x.ClientProfile).WithOne(x=>x.AppUser).HasForeignKey(x=>x.C)

            //modelBuilder.Entity<Person>().HasMany(x => x.EventDates).WithOne(x=>x.Person).HasForeignKey(x=>x.);


            modelBuilder.Entity<ExceptionDetail>().HasKey(x => x.Id);

            //modelBuilder.Configurations.Add(new ProductConfig());
            //modelBuilder.Configurations.Add(new ExceptionDetailConfig());
            base.OnModelCreating(modelBuilder);
        }
          
    }
}
