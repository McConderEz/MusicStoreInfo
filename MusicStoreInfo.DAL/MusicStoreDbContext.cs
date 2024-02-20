using Microsoft.EntityFrameworkCore;
using MusicStoreInfo.DAL.EntitiesConfigurations;
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
        //TODO: Добавить индексы
        //TODO: Добавить триггеры
        //TODO: Добавить каскадное удаление

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
            modelBuilder.ApplyConfiguration(new AlbumConfiguration());
            modelBuilder.ApplyConfiguration(new CassetteConfiguration());
            modelBuilder.ApplyConfiguration(new StoreConfiguration());
            modelBuilder.ApplyConfiguration(new MemberConfiguration());
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
        public DbSet<Store> Stores { get; set; } = null!;
        public DbSet<Album> Albums { get; set; } = null!;
        public DbSet<Cassette> Cassettes { get; set; } = null!;
        public DbSet<City> Cities { get; set; } = null!;
        public DbSet<Company> Companies { get; set; } = null!;
        public DbSet<District> Districts { get; set; } = null!;
        public DbSet<ListenerType> ListenerTypes { get; set; } = null!;
        public DbSet<OwnershipType> OwnershipTypes { get; set; } = null!;
        public DbSet<StoreAlbumLink> StoreAlbumLink { get; set; } = null!;
        public DbSet<Gender> Genders { get; set; } = null!;
        public DbSet<Genre> Genres { get; set; } = null!;
        public DbSet<Group> Groups { get; set; } = null!;
        public DbSet<Member> Members { get; set; } = null!;
        public DbSet<Specialization> Specializations { get; set; } = null!;
        public DbSet<GroupGenreLink> GroupGenreLinks { get; set; } = null!;
        public DbSet<MemberSpecializationLink> MemberSpecializationLinks { get; set; } = null!;
        public DbSet<MemberGroupLink> MemberGroupLinks { get; set; } = null!;
        #endregion
    }
}
