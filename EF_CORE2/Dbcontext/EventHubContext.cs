using System;
using System.Collections.Generic;
using System.Text;
using EF_CORE2.Models;
using Microsoft.EntityFrameworkCore;


namespace EF_CORE2.Dbcontext
{
    public class EventHubContext : DbContext
    {
        public DbSet<Organizer> Organizers { get; set; }
        public DbSet<OrganizerProfile> OrganizerProfiles { get; set; }
        public DbSet<Event> Events { get; set; }
        public DbSet<Attendee> Attendees { get; set; }
        public DbSet<Badge> Badges { get; set; }

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            optionsBuilder.UseSqlServer(
                "Server=.;Database=EventHubDb;Trusted_Connection=True;TrustServerCertificate=True;");
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            // Separate configuration classes
            modelBuilder.ApplyConfiguration(new OrganizerProfileConfiguration());
            modelBuilder.ApplyConfiguration(new EventConfiguration());
            

            // Directly in OnModelCreating (Badge)
            modelBuilder.Entity<Badge>(builder =>
            {
                builder.HasKey(b => b.AttendeeId);

                builder.Property(b => b.BadgeNumber).IsRequired();
                builder.HasIndex(b => b.BadgeNumber).IsUnique();

                builder.Property(b => b.IssuedDate).IsRequired();

                builder.Property(b => b.Tier)
                       .IsRequired()
                       .HasConversion<string>()
                       .HasMaxLength(20);

                // One-to-One: Attendee <-> Badge
                builder.HasOne(b => b.Attendee)
                       .WithOne(a => a.Badge)
                       .HasForeignKey<Badge>(b => b.AttendeeId)
                       .OnDelete(DeleteBehavior.Cascade);
            });
        }
    }
}