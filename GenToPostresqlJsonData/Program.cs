using Microsoft.EntityFrameworkCore;
using MusicStoreInfo.DAL;
using MusicStoreInfo.DAL.Contexts;
using Newtonsoft.Json;
using System.Xml;

namespace GenToPostresqlJsonData
{
    internal class Program
    {
        static void Main(string[] args)
        {
            using (var sqlServerContext = new MusicStoreDbContext())
            {
                var albums = sqlServerContext.Albums                    
                    .Include(a => a.ListenerType)
                    .Include(a => a.Group)
                        .ThenInclude(g => g.Genres)
                    .Include(a => a.Company)
                    .ToList();

                using (var postgreSqlContext = new PostgresContext())
                {
                    foreach (var item in albums)
                    {
                        var jsonData = new
                        {
                            Name = item.Name,
                            ListenerType = item.ListenerType?.Name,
                            Company = item.Company?.Name,
                            Group = new
                            {
                                GroupName = item.Group?.Name,
                                Genres = item.Group?.Genres.Select(g => g.Name).ToList()
                            },
                            Duration = item.Duration,
                            ReleaseDate = item.ReleaseDate.ToString("yyyy-MM-dd"),
                            SongsCount = item.SongsCount,
                            ImagePath = item.ImagePath
                        };

                        var jsonString = JsonConvert.SerializeObject(jsonData, Newtonsoft.Json.Formatting.Indented);

                        postgreSqlContext.JsonTables.Add(new JsonTable { Dataj = jsonString, Id = item.Id });
                    }

                    postgreSqlContext.SaveChanges();
                }
            }
        }
    }
}
