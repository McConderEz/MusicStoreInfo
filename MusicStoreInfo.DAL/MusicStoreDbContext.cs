using Microsoft.EntityFrameworkCore;
using MusicStoreInfo.Domain.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MusicStoreInfo.DAL
{
    public class MusicStoreDbContext: DbContext
    {
        private readonly StreamWriter _logStream = new StreamWriter("log.txt", true);
        private const string CONNECTION_STRING = "data source=(localdb)\\MSSQLLocalDB;Initial Catalog=musicStores;Integrated Security=True;";
        public MusicStoreDbContext()
        {
            Database.EnsureCreated();
        }

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            optionsBuilder.UseSqlServer(CONNECTION_STRING);
            optionsBuilder.LogTo(_logStream.WriteLine);
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<StoreAlbumLink>()
                .HasKey(sal => sal.Id);

            modelBuilder.Entity<StoreAlbumLink>()
                .HasOne(sal => sal.Store)
                .WithMany(s => s.Albums)
                .HasForeignKey(sal => sal.StoreId)
                .OnDelete(DeleteBehavior.Restrict);

            modelBuilder.Entity<StoreAlbumLink>()
                .HasOne(sal => sal.Album)
                .WithMany(a => a.Stores)
                .HasForeignKey(sal => sal.AlbumId)
                .OnDelete(DeleteBehavior.Restrict);
        }

        public override void Dispose()
        {
            base.Dispose();
            _logStream.Dispose();
        }

        public override async ValueTask DisposeAsync()
        {
            await base.DisposeAsync();
            await _logStream.DisposeAsync();
        }

        #region Таблицы
        public DbSet<Store> Stores { get; set; }
        public DbSet<Album> Albums { get; set; }
        public DbSet<Cassette> Cassettes { get; set; }
        public DbSet<City> Cities { get; set; }
        public DbSet<Company> Companies { get; set; }
        public DbSet<District> Districts { get; set; }
        public DbSet<ListenerType> ListenerTypes { get; set; }
        public DbSet<OwnershipType> OwnershipTypes { get; set; }
        public DbSet<StoreAlbumLink> StoreAlbumLink { get; set; }
        public DbSet<Gender> Genders { get; set; }
        public DbSet<Genre> Genres { get; set; }
        public DbSet<Group> Groups { get; set; }
        public DbSet<Member> Members { get; set; }
        public DbSet<Specialization> Specializations { get; set; }
        public DbSet<GroupGenreLink> GroupGenreLinks { get; set; }
        public DbSet<MemberSpecializationLink> MemberSpecializationLinks { get; set; }
        public DbSet<MemberGroupLink> MemberGroupLinks { get; set; }
        #endregion
    }
}
