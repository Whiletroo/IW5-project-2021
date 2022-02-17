using System;
using System.Collections.Generic;
using Microsoft.EntityFrameworkCore;
using Netizine.Enums;
using Tournament.DAL.Entities;

namespace Tournament.DAL
{
    public static class DbSeed
    {
        public static readonly PersonEntity Person1 = new PersonEntity
        {
            Id = Guid.Parse("16fc8580-707a-42a2-8ba9-191c67d591de"),
            FirstName = "John",
            LastName = "Doe",
            PhotoURL = "",
            Description = "This is a man named John Doe."
        };

        public static readonly PersonEntity Person2 = new PersonEntity
        {
            Id = Guid.Parse("393b47d4-b93f-4b2f-87ad-b905c958067a"),
            FirstName = "Jane",
            LastName = "Sue",
            PhotoURL = "",
            Description = "This is a woman named Jane Sue."
        };

        public static readonly PlaceEntity Place1 = new PlaceEntity
        {
            Id = Guid.Parse("11f3a641-0404-40cf-83b5-80e293062eb1"),
            Name = "Avenue",
            Description = "This is an event place."
        };

        public static readonly PlaceEntity Place2 = new PlaceEntity
        {
            Id = Guid.Parse("ae6d83ab-1688-4a6f-8bbb-748aeb5fc96b"),
            Name = "Boulevard",
            Description = "Another one match place."
        };

        public static readonly TeamEntity Team1 = new TeamEntity
        {
            Id = Guid.Parse("74738348-08a6-4c25-9b93-219d88c2de2d"),
            TeamName = "Team Man",
            LogoURL = "some.url/1",
            Description = "This is a team 1",
            RegistrationCountry = Country.Czechia
        };

        public static readonly TeamEntity Team2 = new TeamEntity
        {
            Id = Guid.Parse("2b4450d3-c32e-45af-80ac-8ec0cb5a6537"),
            TeamName = "Team Woman",
            LogoURL = "other.url/2",
            Description = "This is a team 2",
            RegistrationCountry = Country.Slovakia
        };

        public static readonly MatchEntity Match1 = new MatchEntity
        {
            Id = Guid.Parse("df2e39d9-f691-4f9b-8533-61c2474c23f7"),
            DateTime = new DateTime(2021, 10, 15, 17, 56, 20, 501, DateTimeKind.Local).AddTicks(2002),
            Result = Enums.Results.Draw,
            Place = Place1,
            PlaceId = Place1.Id,
            Team1 = Team1,
            Team1Id = Team1.Id,
            Team2 = Team2,
            Team2Id = Team2.Id
        };

        public static readonly MatchEntity Match2 = new MatchEntity
        {
            Id = Guid.Parse("819a4cf7-e06d-42b5-a1e9-47cceb48424d"),
            DateTime = new DateTime(2021, 10, 16, 17, 56, 20, 501, DateTimeKind.Local).AddTicks(2002),
            Result = Enums.Results.Draw,
            Place = Place2,
            PlaceId = Place2.Id,
            Team1 = Team1,
            Team1Id = Team1.Id,
            Team2 = Team2,
            Team2Id = Team2.Id
        };

        static DbSeed()
        {
            Person1.Team = Team1;
            Person1.TeamId = Team1.Id;
            Person2.Team = Team2;
            Person2.TeamId = Team2.Id;

            Team1.Persons = new List<PersonEntity> { Person1 };
            Team2.Persons = new List<PersonEntity> { Person2 };
            Place1.Matches = new List<MatchEntity> { Match1 };
            Place2.Matches = new List<MatchEntity> { Match2 };
        }

        public static void Seed(this ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<PersonEntity>().HasData(
                new PersonEntity
                {
                    Id = Person1.Id,
                    FirstName = Person1.FirstName,
                    LastName = Person1.LastName,
                    PhotoURL = Person1.PhotoURL,
                    Description = Person1.Description,
                    TeamId = Team1.Id
                },
                new PersonEntity
                {
                    Id = Person2.Id,
                    FirstName = Person2.FirstName,
                    LastName = Person2.LastName,
                    PhotoURL = Person2.PhotoURL,
                    Description = Person2.Description,
                    TeamId = Team2.Id
                });

            modelBuilder.Entity<PlaceEntity>().HasData(
                new PlaceEntity
                {
                    Id = Place1.Id,
                    Name = Place1.Name,
                    Description = Place1.Description
                },
                new PlaceEntity
                {
                    Id = Place2.Id,
                    Name = Place2.Name,
                    Description = Place2.Description
                });

            modelBuilder.Entity<TeamEntity>().HasData(
                new TeamEntity
                {
                    Id = Team1.Id,
                    TeamName = Team1.TeamName,
                    LogoURL = Team1.LogoURL,
                    Description = Team1.Description,
                    RegistrationCountry = Team1.RegistrationCountry
                },
                new TeamEntity
                {
                    Id = Team2.Id,
                    TeamName = Team2.TeamName,
                    LogoURL = Team2.LogoURL,
                    Description = Team2.Description,
                    RegistrationCountry = Team2.RegistrationCountry
                });

            modelBuilder.Entity<MatchEntity>().HasData(
                new MatchEntity
                {
                    Id = Match1.Id,
                    DateTime = Match1.DateTime,
                    Result = Match1.Result,
                    PlaceId = Place1.Id,
                    Team1Id = Team1.Id,
                    Team2Id = Team2.Id
                },
                new MatchEntity
                {
                    Id = Match2.Id,
                    DateTime = Match2.DateTime,
                    Result = Match2.Result,
                    PlaceId = Place2.Id,
                    Team1Id = Team1.Id,
                    Team2Id = Team2.Id
                });
        }
    }
}
