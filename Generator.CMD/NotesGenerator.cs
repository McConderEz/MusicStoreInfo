using MusicStoreInfo.DAL;
using MusicStoreInfo.Domain.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Generator.CMD
{
    public class NotesGenerator
    {
        private readonly MusicStoreDbContext _dbContext = new MusicStoreDbContext();
        private readonly Parser _parser = new Parser();

        public void GenGenres()
        {
            var list = _parser.Parse("C:\\Users\\lenka\\Source\\Repos\\MusicStoreInfo\\Generator.CMD\\Dataset\\Genre.txt");
            for(var i = 0;i < list.Count;i++)
            {
                _dbContext.Add(new Genre { Name = list[i] });
            }
            _dbContext.SaveChanges();
        }

        public void GenSpecialization()
        {
            var list = _parser.Parse("C:\\Users\\lenka\\Source\\Repos\\MusicStoreInfo\\Generator.CMD\\Dataset\\Specialization.txt");
            for(var i = 0;i < list.Count; i++)
            {
                _dbContext.Add(new Specialization { Name = list[i] });
            }
            _dbContext.SaveChanges();
        }
        
        public void GenOwnershipType()
        {
            var list = _parser.Parse("C:\\Users\\lenka\\Source\\Repos\\MusicStoreInfo\\Generator.CMD\\Dataset\\OwnershipType.txt");
            for(var i = 0;i < list.Count; i++)
            {
                _dbContext.Add(new OwnershipType { Name = list[i] });
            }
            _dbContext.SaveChanges();
        }

        public void GenListenerType()
        {
            var list = _parser.Parse("C:\\Users\\lenka\\Source\\Repos\\MusicStoreInfo\\Generator.CMD\\Dataset\\ListenerType.txt");
            for(var i = 0;i < list.Count; i++)
            {
                _dbContext.Add(new ListenerType { Name = list[i] });
            }
            _dbContext.SaveChanges();
        }

        public void GenCity()
        {
            var list = _parser.Parse("C:\\Users\\lenka\\Source\\Repos\\MusicStoreInfo\\Generator.CMD\\Dataset\\City.txt");
            for(var i = 0;i < list.Count; i++)
            {
                _dbContext.Add(new City { Name = list[i] });
            }
            _dbContext.SaveChanges();
        }

        public void GenDistrict()
        {
            var list = _parser.ParseCSV("C:\\Users\\lenka\\Source\\Repos\\MusicStoreInfo\\Generator.CMD\\Dataset\\District.csv");
            for (var i = 0; i < list.Count; i++)
            {
                var city = _dbContext.Cities.First(c => c.Name.Equals(list[i].Item1));
                for(var j = 0; j < list[i].Item2.Count(); j++)
                {
                    _dbContext.Add(new District { Name = list[i].Item2[j], CityId = city.Id });
                }
            }
            _dbContext.SaveChanges();
        }

        public void GenGender()
        {
            var list = _parser.Parse("C:\\Users\\lenka\\Source\\Repos\\MusicStoreInfo\\Generator.CMD\\Dataset\\Gender.txt");
            for (var i = 0; i < list.Count; i++)
            {
                _dbContext.Add(new Gender { Name = list[i] });
            }
            _dbContext.SaveChanges();
        }

        public void GenGroup()
        {
            var list = _parser.Parse("C:\\Users\\lenka\\Source\\Repos\\MusicStoreInfo\\Generator.CMD\\Dataset\\Group.txt");

            // Загрузим существующие группы из базы данных
            var existingGroups = _dbContext.Groups.Select(g => g.Name).ToHashSet(StringComparer.OrdinalIgnoreCase);

            // Используем HashSet для удаления дубликатов в новом списке
            var uniqueList = new HashSet<string>(list, StringComparer.OrdinalIgnoreCase);

            foreach (var groupName in uniqueList)
            {
                // Добавляем только те группы, которые не существуют в базе данных
                if (!existingGroups.Contains(groupName))
                {
                    _dbContext.Add(new Group { Name = groupName });
                }
            }

            _dbContext.SaveChanges();
        }

        public void GenGroupToGenreAttitude()
        {
            var list = _parser.ParseCSV("C:\\Users\\lenka\\Source\\Repos\\MusicStoreInfo\\Generator.CMD\\Dataset\\GroupGenre.csv");

            // Загрузим все группы и жанры в память для оптимизации
            var groups = _dbContext.Groups.ToList();
            var genres = _dbContext.Genres.ToList();

            for (var i = 0; i < list.Count; i++)
            {
                var group = groups.SingleOrDefault(c => c.Name.Equals(list[i].Item1));
                if (group != null)
                {
                    for (var j = 0; j < list[i].Item2.Count; j++)
                    {
                        var genreName = list[i].Item2[j].ToLower().Trim();

                        var genre = genres.FirstOrDefault(g => g.Name.ToLower() == genreName);
                        if (genre != null)
                        {
                            if (!group.Genres.Contains(genre))
                            {
                                group.Genres.Add(genre);
                            }
                            if (!genre.Groups.Contains(group))
                            {
                                genre.Groups.Add(group);
                            }
                        }
                    }
                }
            }
            _dbContext.SaveChanges();
        }

        public void GenMemberToGroup()
        {
            var list = _parser.ParseCSV("C:\\Users\\lenka\\Source\\Repos\\MusicStoreInfo\\Generator.CMD\\Dataset\\GroupMemberAttitude.csv");
            var groups = _dbContext.Groups.ToList();

        }
    }
}
