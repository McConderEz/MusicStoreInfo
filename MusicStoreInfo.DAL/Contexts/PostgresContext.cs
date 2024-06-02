using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MusicStoreInfo.DAL.Contexts
{

    public class JsonTable
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int Id { get; set; }
        public string Dataj { get; set; } = string.Empty;
    }

    public class PostgresContext: DbContext
    {
        public DbSet<JsonTable> JsonTables { get; set; }

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            optionsBuilder.UseNpgsql("Host = localhost; Database = musicStores; Username = postgres; Password = 345890; ");

        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<JsonTable>()
                .ToTable("JsonTable");

            modelBuilder.Entity<JsonTable>()
                .Property(e => e.Dataj)
                .HasColumnType("jsonb");
        }
    }
}
