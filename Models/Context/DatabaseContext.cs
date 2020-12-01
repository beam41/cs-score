using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;

namespace CsScore.Models.Context
{
    public sealed class DatabaseContext : DbContext
    {
        public DbSet<User> User { get; set; }

        public DbSet<Group> Group { get; set; }

        public DbSet<Type> Type { get; set; }

        public DbSet<Project> Project { get; set; }

        public DatabaseContext(DbContextOptions<DatabaseContext> options) : base(options)
        {
            Database.EnsureCreated();
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<User>()
                .Property(u => u.Id)
                .ValueGeneratedNever();

            modelBuilder.Entity<User>()
                .HasOne(u => u.Group)
                .WithMany(g => g.UsersInGroup);

            modelBuilder.Entity<User>()
                .HasOne(u => u.Type)
                .WithMany(t => t.Users);

            modelBuilder.Entity<Group>()
                .HasOne(g => g.GroupProject)
                .WithOne(p => p.OwnerGroup)
                .HasForeignKey<Group>(g => g.GroupProjectRef);

            modelBuilder.Entity<Score>()
                .HasOne(sc => sc.FromUser)
                .WithMany(u => u.ScoreRecords);

            modelBuilder.Entity<Score>()
                .HasOne(sc => sc.ToProject)
                .WithMany(p => p.ScoreRecords);
        }
    }
}
