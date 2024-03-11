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
        private readonly StreamWriter _logStream = new StreamWriter("log.txt", true);
        public static string CONNECTION_STRING = "data source=(localdb)\\MSSQLLocalDB;Initial Catalog=musicStores;Integrated Security=True;";
        public MusicStoreDbContext(DbContextOptions<MusicStoreDbContext> options)
            : base(options)
        {
            Database.EnsureDeleted();
            Database.EnsureCreated();
        }

        public MusicStoreDbContext()
        {

        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);
            modelBuilder.ApplyConfigurationsFromAssembly(typeof(MusicStoreDbContext).Assembly);
        }

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            if (!optionsBuilder.IsConfigured)
            {
                optionsBuilder.UseSqlServer(CONNECTION_STRING);
            }
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
        public DbSet<Song> Songs { get; set; } = null!;
        public DbSet<City> Cities { get; set; } = null!;
        public DbSet<Company> Companies { get; set; } = null!;
        public DbSet<District> Districts { get; set; } = null!;
        public DbSet<ListenerType> ListenerTypes { get; set; } = null!;
        public DbSet<OwnershipType> OwnershipTypes { get; set; } = null!;
        public DbSet<Product> Products { get; set; } = null!;
        public DbSet<Gender> Genders { get; set; } = null!;
        public DbSet<Genre> Genres { get; set; } = null!;
        public DbSet<Group> Groups { get; set; } = null!;
        public DbSet<Member> Members { get; set; } = null!;
        public DbSet<Specialization> Specializations { get; set; } = null!;
        public DbSet<User> Users { get; set; } = null!;
        public DbSet<Role> Roles { get; set; } = null!;
        #endregion
    }
}
