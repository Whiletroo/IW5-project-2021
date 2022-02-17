using System;
using Microsoft.EntityFrameworkCore;
using Tournament.DAL.Entities;

namespace Tournament.DAL
{
    public class TournamentDbContext : DbContext
    {
        public TournamentDbContext(DbContextOptions options) : base(options)
        {
        }

        public DbSet<MatchEntity> Matches { get; set; } = null;
        public DbSet<TeamEntity> Teams { get; set; } = null;
        public DbSet<PlaceEntity> Places { get; set; } = null;
        public DbSet<PersonEntity> Persons { get; set; } = null;

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<TeamEntity>()
                .HasMany(t => t.Persons)
                .WithOne(p => p.Team)
                .HasForeignKey(pp => pp.TeamId)
                .OnDelete(DeleteBehavior.SetNull);

            modelBuilder.Entity<PlaceEntity>()
                .HasMany(p => p.Matches)
                .WithOne(m => m.Place)
                .HasForeignKey(mm => mm.PlaceId)
                .OnDelete(DeleteBehavior.Cascade);

            modelBuilder.Entity<MatchEntity>()
                .HasOne(m => m.Team1)
                .WithMany()
                .HasForeignKey(mm => mm.Team1Id)
                .OnDelete(DeleteBehavior.NoAction);

            modelBuilder.Entity<MatchEntity>()
                .HasOne(m => m.Team2)
                .WithMany()
                .HasForeignKey(mm => mm.Team2Id)
                .OnDelete(DeleteBehavior.NoAction);

            modelBuilder.Seed();

            base.OnModelCreating(modelBuilder);
        }
    }
}
