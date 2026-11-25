using EventRegistrationAPI_JaydanWilliams.Models;
using Microsoft.EntityFrameworkCore;

namespace EventRegistrationAPI_JaydanWilliams.Data
{
    /// <summary>
    /// EF Core database context. Defines the tables and seeds starting data.
    /// </summary>
    public class AppDbContext : DbContext
    {
        public AppDbContext(DbContextOptions<AppDbContext> options) : base(options) { }

        public DbSet<Event> Events { get; set; }
        public DbSet<Registration> Registrations { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            // One Event has many Registrations; deleting an event deletes its registrations
            modelBuilder.Entity<Event>()
                .HasMany(e => e.Registrations)
                .WithOne(r => r.Event)
                .HasForeignKey(r => r.EventId)
                .OnDelete(DeleteBehavior.Cascade);

            // Seed events. Fixed dates keep past/future events stable while marking.
            modelBuilder.Entity<Event>().HasData(
                // Past event -> used to test "Registration is closed for past events."
                new Event
                {
                    EventId = 1,
                    Title = "Fall Orientation 2025",
                    Location = "Trafalgar Campus, Main Hall",
                    EventDate = new DateTime(2025, 9, 3, 10, 0, 0),
                    Capacity = 200
                },

                // Future event with spots left -> used for valid/duplicate registration tests
                new Event
                {
                    EventId = 2,
                    Title = "Intro to Cloud Computing Workshop",
                    Location = "Davis Campus, Room B210",
                    EventDate = new DateTime(2027, 2, 15, 14, 0, 0),
                    Capacity = 30
                },

                // FULL event: capacity 2 with 2 registrations -> used to test "This event is full."
                new Event
                {
                    EventId = 3,
                    Title = "Resume Review Session",
                    Location = "HMC Campus, Career Centre",
                    EventDate = new DateTime(2027, 3, 10, 13, 0, 0),
                    Capacity = 2
                },

                // Another future event with plenty of room
                new Event
                {
                    EventId = 4,
                    Title = "Spring Hackathon",
                    Location = "Trafalgar Campus, Library Commons",
                    EventDate = new DateTime(2027, 4, 20, 9, 0, 0),
                    Capacity = 100
                }
            );

            // Seed registrations (HasData needs explicit IDs and fixed values)
            modelBuilder.Entity<Registration>().HasData(
                new Registration
                {
                    RegistrationId = 1,
                    EventId = 2,
                    StudentName = "Priya Sharma",
                    Email = "priya.sharma@sheridancollege.ca",
                    RegisteredOn = new DateTime(2026, 11, 1, 9, 30, 0)
                },
                new Registration
                {
                    RegistrationId = 2,
                    EventId = 3,
                    StudentName = "Marcus Lee",
                    Email = "marcus.lee@sheridancollege.ca",
                    RegisteredOn = new DateTime(2026, 11, 2, 11, 0, 0)
                },
                new Registration
                {
                    RegistrationId = 3,
                    EventId = 3,
                    StudentName = "Sofia Alvarez",
                    Email = "sofia.alvarez@sheridancollege.ca",
                    RegisteredOn = new DateTime(2026, 11, 3, 15, 45, 0)
                }
            );
        }
    }
}