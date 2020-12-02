using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using CsScore.Services;
using CsScore.Services.Interfaces;
using Microsoft.AspNetCore.Hosting;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Options;

namespace CsScore.Models.Context
{
    public sealed class DatabaseContext : DbContext
    {
        public DbSet<User> User { get; set; }

        public DbSet<Group> Group { get; set; }

        public DbSet<Type> Type { get; set; }

        public DbSet<Project> Project { get; set; }

        public DatabaseContext(DbContextOptions<DatabaseContext> options, IOptions<UserSetting> userSetting) :
            base(options)
        {
            Database.EnsureCreated();

            var save = false;
            var initType = Type.Include(t => t.Users).FirstOrDefault(t => t.HasDashboardAccess);
            if (initType == null)
            {
                initType = new Type
                {
                    AvailableSubmit = 0,
                    PointPerSubmit = 0,
                    Name = "Super User",
                    HasDashboardAccess = true,
                    Users = new List<User>(),
                };

                Type.Add(initType);
                save = true;
            }

            User initUser = null;
            if (!initType.Users.Any())
            {
                initUser = new User
                {
                    Id = "000000000",
                    Type = initType,
                    FirstName = "Super",
                    LastName = "User",
                    Password = userSetting.Value.SuperUserPassword,
                };

                initType.Users.Add(initUser);
                save = true;
            }

            if (save) {
                SaveChanges();
            }

            Entry(initType).State = EntityState.Detached;

            if (initUser != null)
            {
                Entry(initUser).State = EntityState.Detached;
            }

            SaveChanges();

        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<Project>()
                .Property(p => p.SubmitDate)
                .HasDefaultValueSql("getdate()");

            modelBuilder.Entity<Project>()
                .Property(p => p.UpdatedDate)
                .HasDefaultValueSql("getdate()");

            modelBuilder.Entity<Score>()
                .Property(s => s.SubmitDate)
                .HasDefaultValueSql("getdate()");

            modelBuilder.Entity<Group>()
                .Property(g => g.Id)
                .ValueGeneratedNever();

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
                .HasForeignKey<Project>(g => g.GroupId);

            modelBuilder.Entity<Score>()
                .HasOne(sc => sc.FromUser)
                .WithMany(u => u.ScoreRecords);

            modelBuilder.Entity<Score>()
                .HasOne(sc => sc.ToProject)
                .WithMany(p => p.ScoreRecords);
        }
    }
}
