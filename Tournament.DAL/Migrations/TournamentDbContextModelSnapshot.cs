// <auto-generated />
using System;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore.Metadata;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;
using Tournament.DAL;

#nullable disable

namespace Tournament.DAL.Migrations
{
    [DbContext(typeof(TournamentDbContext))]
    partial class TournamentDbContextModelSnapshot : ModelSnapshot
    {
        protected override void BuildModel(ModelBuilder modelBuilder)
        {
#pragma warning disable 612, 618
            modelBuilder
                .HasAnnotation("ProductVersion", "6.0.0")
                .HasAnnotation("Relational:MaxIdentifierLength", 128);

            SqlServerModelBuilderExtensions.UseIdentityColumns(modelBuilder, 1L, 1);

            modelBuilder.Entity("Tournament.DAL.Entities.MatchEntity", b =>
                {
                    b.Property<Guid>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("uniqueidentifier");

                    b.Property<DateTime>("DateTime")
                        .HasColumnType("datetime2");

                    b.Property<Guid>("PlaceId")
                        .HasColumnType("uniqueidentifier");

                    b.Property<int>("Result")
                        .HasColumnType("int");

                    b.Property<Guid?>("Team1Id")
                        .HasColumnType("uniqueidentifier");

                    b.Property<Guid?>("Team2Id")
                        .HasColumnType("uniqueidentifier");

                    b.HasKey("Id");

                    b.HasIndex("PlaceId");

                    b.HasIndex("Team1Id");

                    b.HasIndex("Team2Id");

                    b.ToTable("Matches");

                    b.HasData(
                        new
                        {
                            Id = new Guid("df2e39d9-f691-4f9b-8533-61c2474c23f7"),
                            DateTime = new DateTime(2021, 10, 15, 17, 56, 20, 501, DateTimeKind.Local).AddTicks(2002),
                            PlaceId = new Guid("11f3a641-0404-40cf-83b5-80e293062eb1"),
                            Result = 3,
                            Team1Id = new Guid("74738348-08a6-4c25-9b93-219d88c2de2d"),
                            Team2Id = new Guid("2b4450d3-c32e-45af-80ac-8ec0cb5a6537")
                        },
                        new
                        {
                            Id = new Guid("819a4cf7-e06d-42b5-a1e9-47cceb48424d"),
                            DateTime = new DateTime(2021, 10, 16, 17, 56, 20, 501, DateTimeKind.Local).AddTicks(2002),
                            PlaceId = new Guid("ae6d83ab-1688-4a6f-8bbb-748aeb5fc96b"),
                            Result = 3,
                            Team1Id = new Guid("74738348-08a6-4c25-9b93-219d88c2de2d"),
                            Team2Id = new Guid("2b4450d3-c32e-45af-80ac-8ec0cb5a6537")
                        });
                });

            modelBuilder.Entity("Tournament.DAL.Entities.PersonEntity", b =>
                {
                    b.Property<Guid>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("uniqueidentifier");

                    b.Property<string>("Description")
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("FirstName")
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("LastName")
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("PhotoURL")
                        .HasColumnType("nvarchar(max)");

                    b.Property<Guid?>("TeamId")
                        .HasColumnType("uniqueidentifier");

                    b.HasKey("Id");

                    b.HasIndex("TeamId");

                    b.ToTable("Persons");

                    b.HasData(
                        new
                        {
                            Id = new Guid("16fc8580-707a-42a2-8ba9-191c67d591de"),
                            Description = "This is a man named John Doe.",
                            FirstName = "John",
                            LastName = "Doe",
                            PhotoURL = "",
                            TeamId = new Guid("74738348-08a6-4c25-9b93-219d88c2de2d")
                        },
                        new
                        {
                            Id = new Guid("393b47d4-b93f-4b2f-87ad-b905c958067a"),
                            Description = "This is a woman named Jane Sue.",
                            FirstName = "Jane",
                            LastName = "Sue",
                            PhotoURL = "",
                            TeamId = new Guid("2b4450d3-c32e-45af-80ac-8ec0cb5a6537")
                        });
                });

            modelBuilder.Entity("Tournament.DAL.Entities.PlaceEntity", b =>
                {
                    b.Property<Guid>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("uniqueidentifier");

                    b.Property<string>("Description")
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("Name")
                        .HasColumnType("nvarchar(max)");

                    b.HasKey("Id");

                    b.ToTable("Places");

                    b.HasData(
                        new
                        {
                            Id = new Guid("11f3a641-0404-40cf-83b5-80e293062eb1"),
                            Description = "This is an event place.",
                            Name = "Avenue"
                        },
                        new
                        {
                            Id = new Guid("ae6d83ab-1688-4a6f-8bbb-748aeb5fc96b"),
                            Description = "Another one match place.",
                            Name = "Boulevard"
                        });
                });

            modelBuilder.Entity("Tournament.DAL.Entities.TeamEntity", b =>
                {
                    b.Property<Guid>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("uniqueidentifier");

                    b.Property<string>("Description")
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("LogoURL")
                        .HasColumnType("nvarchar(max)");

                    b.Property<int>("RegistrationCountry")
                        .HasColumnType("int");

                    b.Property<string>("TeamName")
                        .HasColumnType("nvarchar(max)");

                    b.HasKey("Id");

                    b.ToTable("Teams");

                    b.HasData(
                        new
                        {
                            Id = new Guid("74738348-08a6-4c25-9b93-219d88c2de2d"),
                            Description = "This is a team 1",
                            LogoURL = "some.url/1",
                            RegistrationCountry = 60,
                            TeamName = "Team Man"
                        },
                        new
                        {
                            Id = new Guid("2b4450d3-c32e-45af-80ac-8ec0cb5a6537"),
                            Description = "This is a team 2",
                            LogoURL = "other.url/2",
                            RegistrationCountry = 203,
                            TeamName = "Team Woman"
                        });
                });

            modelBuilder.Entity("Tournament.DAL.Entities.MatchEntity", b =>
                {
                    b.HasOne("Tournament.DAL.Entities.PlaceEntity", "Place")
                        .WithMany("Matches")
                        .HasForeignKey("PlaceId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.HasOne("Tournament.DAL.Entities.TeamEntity", "Team1")
                        .WithMany()
                        .HasForeignKey("Team1Id")
                        .OnDelete(DeleteBehavior.NoAction);

                    b.HasOne("Tournament.DAL.Entities.TeamEntity", "Team2")
                        .WithMany()
                        .HasForeignKey("Team2Id")
                        .OnDelete(DeleteBehavior.NoAction);

                    b.Navigation("Place");

                    b.Navigation("Team1");

                    b.Navigation("Team2");
                });

            modelBuilder.Entity("Tournament.DAL.Entities.PersonEntity", b =>
                {
                    b.HasOne("Tournament.DAL.Entities.TeamEntity", "Team")
                        .WithMany("Persons")
                        .HasForeignKey("TeamId")
                        .OnDelete(DeleteBehavior.SetNull);

                    b.Navigation("Team");
                });

            modelBuilder.Entity("Tournament.DAL.Entities.PlaceEntity", b =>
                {
                    b.Navigation("Matches");
                });

            modelBuilder.Entity("Tournament.DAL.Entities.TeamEntity", b =>
                {
                    b.Navigation("Persons");
                });
#pragma warning restore 612, 618
        }
    }
}
