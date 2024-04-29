﻿// <auto-generated />
using System;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore.Metadata;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;
using MusicStoreInfo.DAL;

#nullable disable

namespace MusicStoreInfo.DAL.Migrations
{
    [DbContext(typeof(MusicStoreDbContext))]
    partial class MusicStoreDbContextModelSnapshot : ModelSnapshot
    {
        protected override void BuildModel(ModelBuilder modelBuilder)
        {
#pragma warning disable 612, 618
            modelBuilder
                .HasAnnotation("ProductVersion", "8.0.4")
                .HasAnnotation("Relational:MaxIdentifierLength", 128);

            SqlServerModelBuilderExtensions.UseIdentityColumns(modelBuilder);

            modelBuilder.Entity("GenreGroup", b =>
                {
                    b.Property<int>("GenresId")
                        .HasColumnType("int");

                    b.Property<int>("GroupsId")
                        .HasColumnType("int");

                    b.HasKey("GenresId", "GroupsId");

                    b.HasIndex("GroupsId");

                    b.ToTable("GroupGenreLink", (string)null);
                });

            modelBuilder.Entity("GroupMember", b =>
                {
                    b.Property<int>("GroupsId")
                        .HasColumnType("int");

                    b.Property<int>("MembersId")
                        .HasColumnType("int");

                    b.HasKey("GroupsId", "MembersId");

                    b.HasIndex("MembersId");

                    b.ToTable("MemberGroupLink", (string)null);
                });

            modelBuilder.Entity("MemberSpecialization", b =>
                {
                    b.Property<int>("MembersId")
                        .HasColumnType("int");

                    b.Property<int>("SpecializationsId")
                        .HasColumnType("int");

                    b.HasKey("MembersId", "SpecializationsId");

                    b.HasIndex("SpecializationsId");

                    b.ToTable("MemberSpecializationLink", (string)null);
                });

            modelBuilder.Entity("MusicStoreInfo.Domain.Entities.Album", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int");

                    SqlServerPropertyBuilderExtensions.UseIdentityColumn(b.Property<int>("Id"));

                    b.Property<int>("CompanyId")
                        .HasColumnType("int");

                    b.Property<int>("Duration")
                        .HasColumnType("int");

                    b.Property<int>("GroupId")
                        .HasColumnType("int");

                    b.Property<string>("ImagePath")
                        .HasColumnType("nvarchar(max)");

                    b.Property<int>("ListenerTypeId")
                        .HasColumnType("int");

                    b.Property<string>("Name")
                        .IsRequired()
                        .HasMaxLength(50)
                        .HasColumnType("nvarchar(50)");

                    b.Property<DateTime>("ReleaseDate")
                        .HasColumnType("datetime2");

                    b.Property<int>("SongsCount")
                        .HasColumnType("int");

                    b.HasKey("Id");

                    b.HasIndex("CompanyId");

                    b.HasIndex("GroupId");

                    b.HasIndex("ListenerTypeId");

                    b.ToTable("Albums", t =>
                        {
                            t.HasCheckConstraint("Duration", "Duration > 0");

                            t.HasCheckConstraint("ReleaseDate", "YEAR(ReleaseDate) <= YEAR(GETDATE())");

                            t.HasCheckConstraint("SongsCount", "SongsCount > 0");
                        });
                });

            modelBuilder.Entity("MusicStoreInfo.Domain.Entities.City", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int");

                    SqlServerPropertyBuilderExtensions.UseIdentityColumn(b.Property<int>("Id"));

                    b.Property<string>("Name")
                        .IsRequired()
                        .HasMaxLength(50)
                        .HasColumnType("nvarchar(50)");

                    b.HasKey("Id");

                    b.ToTable("Cities");
                });

            modelBuilder.Entity("MusicStoreInfo.Domain.Entities.Company", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int");

                    SqlServerPropertyBuilderExtensions.UseIdentityColumn(b.Property<int>("Id"));

                    b.Property<int>("DistrictId")
                        .HasColumnType("int");

                    b.Property<string>("Name")
                        .IsRequired()
                        .HasMaxLength(50)
                        .HasColumnType("nvarchar(50)");

                    b.Property<string>("PhoneNumber")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.HasKey("Id");

                    b.HasIndex("DistrictId");

                    b.ToTable("Companies");
                });

            modelBuilder.Entity("MusicStoreInfo.Domain.Entities.District", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int");

                    SqlServerPropertyBuilderExtensions.UseIdentityColumn(b.Property<int>("Id"));

                    b.Property<int>("CityId")
                        .HasColumnType("int");

                    b.Property<string>("Name")
                        .IsRequired()
                        .HasMaxLength(200)
                        .HasColumnType("nvarchar(200)");

                    b.HasKey("Id");

                    b.HasIndex("CityId");

                    b.ToTable("Districts");
                });

            modelBuilder.Entity("MusicStoreInfo.Domain.Entities.Gender", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int");

                    SqlServerPropertyBuilderExtensions.UseIdentityColumn(b.Property<int>("Id"));

                    b.Property<string>("Name")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.HasKey("Id");

                    b.ToTable("Genders");
                });

            modelBuilder.Entity("MusicStoreInfo.Domain.Entities.Genre", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int");

                    SqlServerPropertyBuilderExtensions.UseIdentityColumn(b.Property<int>("Id"));

                    b.Property<string>("Name")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.HasKey("Id");

                    b.ToTable("Genres");
                });

            modelBuilder.Entity("MusicStoreInfo.Domain.Entities.Group", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int");

                    SqlServerPropertyBuilderExtensions.UseIdentityColumn(b.Property<int>("Id"));

                    b.Property<string>("ImagePath")
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("Name")
                        .IsRequired()
                        .HasMaxLength(50)
                        .HasColumnType("nvarchar(50)");

                    b.HasKey("Id");

                    b.HasIndex("Id");

                    b.ToTable("Groups");
                });

            modelBuilder.Entity("MusicStoreInfo.Domain.Entities.ListenerType", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int");

                    SqlServerPropertyBuilderExtensions.UseIdentityColumn(b.Property<int>("Id"));

                    b.Property<string>("Name")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.HasKey("Id");

                    b.ToTable("ListenerTypes");
                });

            modelBuilder.Entity("MusicStoreInfo.Domain.Entities.Member", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int");

                    SqlServerPropertyBuilderExtensions.UseIdentityColumn(b.Property<int>("Id"));

                    b.Property<int>("Age")
                        .HasColumnType("int");

                    b.Property<int>("GenderId")
                        .HasColumnType("int");

                    b.Property<string>("Name")
                        .IsRequired()
                        .HasMaxLength(50)
                        .HasColumnType("nvarchar(50)");

                    b.Property<string>("SecondName")
                        .IsRequired()
                        .HasMaxLength(50)
                        .HasColumnType("nvarchar(50)");

                    b.HasKey("Id");

                    b.HasIndex("GenderId");

                    b.HasIndex("Id");

                    b.ToTable("Members", t =>
                        {
                            t.HasCheckConstraint("Age", "Age > 0 AND Age < 120");
                        });
                });

            modelBuilder.Entity("MusicStoreInfo.Domain.Entities.OwnershipType", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int");

                    SqlServerPropertyBuilderExtensions.UseIdentityColumn(b.Property<int>("Id"));

                    b.Property<string>("Name")
                        .IsRequired()
                        .HasMaxLength(50)
                        .HasColumnType("nvarchar(50)");

                    b.HasKey("Id");

                    b.ToTable("OwnershipTypes");
                });

            modelBuilder.Entity("MusicStoreInfo.Domain.Entities.Product", b =>
                {
                    b.Property<int>("StoreId")
                        .HasColumnType("int");

                    b.Property<int>("AlbumId")
                        .HasColumnType("int");

                    b.Property<DateTime>("DateReceived")
                        .HasColumnType("datetime2");

                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int");

                    SqlServerPropertyBuilderExtensions.UseIdentityColumn(b.Property<int>("Id"));

                    b.Property<decimal>("Price")
                        .HasColumnType("decimal(18,2)");

                    b.Property<int>("Quantity")
                        .HasColumnType("int");

                    b.HasKey("StoreId", "AlbumId");

                    b.HasIndex("AlbumId");

                    b.HasIndex("Id");

                    b.ToTable("Product", null, t =>
                        {
                            t.HasCheckConstraint("DateReceived", "DateReceived <= GETDATE()");

                            t.HasCheckConstraint("Price", "Price > 0 AND Price <= 1000000");

                            t.HasCheckConstraint("Quantity", "Quantity >= 0");
                        });
                });

            modelBuilder.Entity("MusicStoreInfo.Domain.Entities.Role", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int");

                    SqlServerPropertyBuilderExtensions.UseIdentityColumn(b.Property<int>("Id"));

                    b.Property<string>("Name")
                        .IsRequired()
                        .HasMaxLength(50)
                        .HasColumnType("nvarchar(50)");

                    b.HasKey("Id");

                    b.ToTable("Roles");
                });

            modelBuilder.Entity("MusicStoreInfo.Domain.Entities.Song", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int");

                    SqlServerPropertyBuilderExtensions.UseIdentityColumn(b.Property<int>("Id"));

                    b.Property<int>("AlbumId")
                        .HasColumnType("int");

                    b.Property<int>("Duration")
                        .HasColumnType("int");

                    b.Property<string>("Name")
                        .IsRequired()
                        .HasMaxLength(50)
                        .HasColumnType("nvarchar(50)");

                    b.HasKey("Id");

                    b.HasIndex("AlbumId");

                    b.HasIndex("Id");

                    b.ToTable("Songs", t =>
                        {
                            t.HasCheckConstraint("Duration", "Duration > 0")
                                .HasName("Duration1");
                        });
                });

            modelBuilder.Entity("MusicStoreInfo.Domain.Entities.Specialization", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int");

                    SqlServerPropertyBuilderExtensions.UseIdentityColumn(b.Property<int>("Id"));

                    b.Property<string>("Name")
                        .IsRequired()
                        .HasMaxLength(50)
                        .HasColumnType("nvarchar(50)");

                    b.HasKey("Id");

                    b.ToTable("Specializations");
                });

            modelBuilder.Entity("MusicStoreInfo.Domain.Entities.Store", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int");

                    SqlServerPropertyBuilderExtensions.UseIdentityColumn(b.Property<int>("Id"));

                    b.Property<int>("DistrictId")
                        .HasColumnType("int");

                    b.Property<string>("Name")
                        .IsRequired()
                        .HasMaxLength(50)
                        .HasColumnType("nvarchar(50)");

                    b.Property<int>("OwnershipTypeId")
                        .HasColumnType("int");

                    b.Property<string>("PhoneNumber")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.Property<DateTime>("YearOpened")
                        .HasColumnType("datetime2");

                    b.HasKey("Id");

                    b.HasIndex("DistrictId");

                    b.HasIndex("Id");

                    b.HasIndex("OwnershipTypeId");

                    b.ToTable("Stores", t =>
                        {
                            t.HasCheckConstraint("YearOpened", "YEAR(YearOpened) <= YEAR(GETDATE())");
                        });
                });

            modelBuilder.Entity("MusicStoreInfo.Domain.Entities.User", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int");

                    SqlServerPropertyBuilderExtensions.UseIdentityColumn(b.Property<int>("Id"));

                    b.Property<string>("Email")
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("ImagePath")
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("PasswordHash")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("PhoneNumber")
                        .HasColumnType("nvarchar(max)");

                    b.Property<int>("RoleId")
                        .HasColumnType("int");

                    b.Property<string>("UserName")
                        .IsRequired()
                        .HasMaxLength(50)
                        .HasColumnType("nvarchar(50)");

                    b.HasKey("Id");

                    b.HasIndex("RoleId");

                    b.ToTable("Users");
                });

            modelBuilder.Entity("GenreGroup", b =>
                {
                    b.HasOne("MusicStoreInfo.Domain.Entities.Genre", null)
                        .WithMany()
                        .HasForeignKey("GenresId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.HasOne("MusicStoreInfo.Domain.Entities.Group", null)
                        .WithMany()
                        .HasForeignKey("GroupsId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();
                });

            modelBuilder.Entity("GroupMember", b =>
                {
                    b.HasOne("MusicStoreInfo.Domain.Entities.Group", null)
                        .WithMany()
                        .HasForeignKey("GroupsId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.HasOne("MusicStoreInfo.Domain.Entities.Member", null)
                        .WithMany()
                        .HasForeignKey("MembersId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();
                });

            modelBuilder.Entity("MemberSpecialization", b =>
                {
                    b.HasOne("MusicStoreInfo.Domain.Entities.Member", null)
                        .WithMany()
                        .HasForeignKey("MembersId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.HasOne("MusicStoreInfo.Domain.Entities.Specialization", null)
                        .WithMany()
                        .HasForeignKey("SpecializationsId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();
                });

            modelBuilder.Entity("MusicStoreInfo.Domain.Entities.Album", b =>
                {
                    b.HasOne("MusicStoreInfo.Domain.Entities.Company", "Company")
                        .WithMany("Albums")
                        .HasForeignKey("CompanyId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.HasOne("MusicStoreInfo.Domain.Entities.Group", "Group")
                        .WithMany("Albums")
                        .HasForeignKey("GroupId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.HasOne("MusicStoreInfo.Domain.Entities.ListenerType", "ListenerType")
                        .WithMany("Albums")
                        .HasForeignKey("ListenerTypeId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.Navigation("Company");

                    b.Navigation("Group");

                    b.Navigation("ListenerType");
                });

            modelBuilder.Entity("MusicStoreInfo.Domain.Entities.Company", b =>
                {
                    b.HasOne("MusicStoreInfo.Domain.Entities.District", "District")
                        .WithMany("Companies")
                        .HasForeignKey("DistrictId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.Navigation("District");
                });

            modelBuilder.Entity("MusicStoreInfo.Domain.Entities.District", b =>
                {
                    b.HasOne("MusicStoreInfo.Domain.Entities.City", "City")
                        .WithMany("Districts")
                        .HasForeignKey("CityId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.Navigation("City");
                });

            modelBuilder.Entity("MusicStoreInfo.Domain.Entities.Member", b =>
                {
                    b.HasOne("MusicStoreInfo.Domain.Entities.Gender", "Gender")
                        .WithMany("Members")
                        .HasForeignKey("GenderId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.Navigation("Gender");
                });

            modelBuilder.Entity("MusicStoreInfo.Domain.Entities.Product", b =>
                {
                    b.HasOne("MusicStoreInfo.Domain.Entities.Album", "Album")
                        .WithMany("Products")
                        .HasForeignKey("AlbumId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.HasOne("MusicStoreInfo.Domain.Entities.Store", "Store")
                        .WithMany("Products")
                        .HasForeignKey("StoreId")
                        .OnDelete(DeleteBehavior.Restrict)
                        .IsRequired();

                    b.Navigation("Album");

                    b.Navigation("Store");
                });

            modelBuilder.Entity("MusicStoreInfo.Domain.Entities.Song", b =>
                {
                    b.HasOne("MusicStoreInfo.Domain.Entities.Album", "Album")
                        .WithMany("Songs")
                        .HasForeignKey("AlbumId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.Navigation("Album");
                });

            modelBuilder.Entity("MusicStoreInfo.Domain.Entities.Store", b =>
                {
                    b.HasOne("MusicStoreInfo.Domain.Entities.District", "District")
                        .WithMany("Stores")
                        .HasForeignKey("DistrictId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.HasOne("MusicStoreInfo.Domain.Entities.OwnershipType", "OwnershipType")
                        .WithMany("Stores")
                        .HasForeignKey("OwnershipTypeId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.Navigation("District");

                    b.Navigation("OwnershipType");
                });

            modelBuilder.Entity("MusicStoreInfo.Domain.Entities.User", b =>
                {
                    b.HasOne("MusicStoreInfo.Domain.Entities.Role", "Role")
                        .WithMany("Users")
                        .HasForeignKey("RoleId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.Navigation("Role");
                });

            modelBuilder.Entity("MusicStoreInfo.Domain.Entities.Album", b =>
                {
                    b.Navigation("Products");

                    b.Navigation("Songs");
                });

            modelBuilder.Entity("MusicStoreInfo.Domain.Entities.City", b =>
                {
                    b.Navigation("Districts");
                });

            modelBuilder.Entity("MusicStoreInfo.Domain.Entities.Company", b =>
                {
                    b.Navigation("Albums");
                });

            modelBuilder.Entity("MusicStoreInfo.Domain.Entities.District", b =>
                {
                    b.Navigation("Companies");

                    b.Navigation("Stores");
                });

            modelBuilder.Entity("MusicStoreInfo.Domain.Entities.Gender", b =>
                {
                    b.Navigation("Members");
                });

            modelBuilder.Entity("MusicStoreInfo.Domain.Entities.Group", b =>
                {
                    b.Navigation("Albums");
                });

            modelBuilder.Entity("MusicStoreInfo.Domain.Entities.ListenerType", b =>
                {
                    b.Navigation("Albums");
                });

            modelBuilder.Entity("MusicStoreInfo.Domain.Entities.OwnershipType", b =>
                {
                    b.Navigation("Stores");
                });

            modelBuilder.Entity("MusicStoreInfo.Domain.Entities.Role", b =>
                {
                    b.Navigation("Users");
                });

            modelBuilder.Entity("MusicStoreInfo.Domain.Entities.Store", b =>
                {
                    b.Navigation("Products");
                });
#pragma warning restore 612, 618
        }
    }
}
