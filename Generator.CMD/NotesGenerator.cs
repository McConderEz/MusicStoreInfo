using MusicStoreInfo.DAL;
using MusicStoreInfo.Domain.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using System.ComponentModel.DataAnnotations;

namespace Generator.CMD
{
    public class NotesGenerator
    {
        private readonly MusicStoreDbContext _dbContext = new MusicStoreDbContext();
        private readonly Parser _parser = new Parser();

        private const string PATH = "C:\\Users\\lenka\\source\\repos\\MusicStoreInfo";
      
        public void GenAll()
        {
            GenGenres();
            GenSpecialization();
            GenOwnershipType();
            GenListenerType();
            GenCity();
            GenDistrict();
            GenGender();
            GenGroup();
            GenGroupToGenreAttitude();
            GenMember();
            GenMemberToGroup();
            GenMemberToSpecialization();
            GenStores();
            GenCompany();
            GenAlbum();
            GenSong();
            GenProduct();
        }

        public void GenGenres()
        {
            var list = _parser.Parse(PATH + "\\Generator.CMD\\Dataset\\Genre.txt");
            for(var i = 0;i < list.Count;i++)
            {
                _dbContext.Add(new Genre { Name = list[i] });
            }
            _dbContext.SaveChanges();
        }

        public void GenSpecialization()
        {
            var list = _parser.Parse(PATH + "\\Generator.CMD\\Dataset\\Specialization.txt");
            for(var i = 0;i < list.Count; i++)
            {
                _dbContext.Add(new Specialization { Name = list[i] });
            }
            _dbContext.SaveChanges();
        }
        
        public void GenOwnershipType()
        {
            var list = _parser.Parse(PATH + "\\Generator.CMD\\Dataset\\OwnershipType.txt");
            for(var i = 0;i < list.Count; i++)
            {
                _dbContext.Add(new OwnershipType { Name = list[i] });
            }
            _dbContext.SaveChanges();
        }

        public void GenListenerType()
        {
            var list = _parser.Parse(PATH + "\\Generator.CMD\\Dataset\\ListenerType.txt");
            for(var i = 0;i < list.Count; i++)
            {
                _dbContext.Add(new ListenerType { Name = list[i] });
            }
            _dbContext.SaveChanges();
        }

        public void GenCity()
        {
            var list = _parser.Parse(PATH + "\\Generator.CMD\\Dataset\\City.txt");
            for(var i = 0;i < list.Count; i++)
            {
                _dbContext.Add(new City { Name = list[i] });
            }
            _dbContext.SaveChanges();
        }

        public void GenDistrict()
        {
            var list = _parser.ParseCSV(PATH + "\\Generator.CMD\\Dataset\\District.csv");
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
            var list = _parser.Parse(PATH + "\\Generator.CMD\\Dataset\\Gender.txt");
            for (var i = 0; i < list.Count; i++)
            {
                _dbContext.Add(new Gender { Name = list[i] });
            }
            _dbContext.SaveChanges();
        }

        public void GenGroup()
        {
            var list = _parser.Parse(PATH + "\\Generator.CMD\\Dataset\\Group.txt");

            var existingGroups = _dbContext.Groups.Select(g => g.Name).ToHashSet(StringComparer.OrdinalIgnoreCase);

            var uniqueList = new HashSet<string>(list, StringComparer.OrdinalIgnoreCase);

            foreach (var groupName in uniqueList)
            {
                if (!existingGroups.Contains(groupName))
                {
                    _dbContext.Add(new Group { Name = groupName });
                }
            }

            _dbContext.SaveChanges();
        }

        public void GenGroupToGenreAttitude()
        {
            var list = _parser.ParseCSV(PATH + "\\Generator.CMD\\Dataset\\GroupGenre.csv");

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

        public void GenMember()
        {
            var csvMembers = _parser.ParseCSVMember(PATH + "\\Generator.CMD\\Dataset\\Member.csv");

            var existingMembers = _dbContext.Members.Include(m => m.Gender).ToList();
            try
            {
                foreach (var (name, secondName, age, gender) in csvMembers)
                {
                    if (!existingMembers.Any(m => m.Name == name && m.SecondName == secondName && m.Age == age && m.Gender.Name == gender))
                    {
                        var member = new Member
                        {
                            Name = name,
                            GenderId = _dbContext.Genders.FirstOrDefault(g => g.Name == gender).Id,
                            Age = age,
                            SecondName = secondName
                        };

                        _dbContext.Members.Add(member);
                    }
                }
                _dbContext.SaveChanges();

            }
            catch(Exception ex)
            {
                Console.WriteLine(ex.Message);
            }
        }

        public void GenMemberToGroup()
        {
            try
            {
                var list = _parser.ParseCSV(PATH + "\\Generator.CMD\\Dataset\\GroupMemberAttitude.csv");
                var groups = _dbContext.Groups.ToList();
                var members = _dbContext.Members.ToList();

                for (int i = 0; i < list.Count; i++)
                {
                    var group = groups.FirstOrDefault(g => g.Name.Equals(list[i].Item1));
                    if (group != null)
                    {
                        for (var j = 0; j < list[i].Item2.Count; j++)
                        {
                            var name = list[i].Item2[j].Replace(" ","");
                            var member = members.FirstOrDefault(m => (m.Name + m.SecondName).Replace(" ","").Equals(name));
                            if (member != null)
                            {
                                group.Members.Add(member);
                                member.Groups.Add(group);
                            }
                        }
                    }
                }

                _dbContext.SaveChanges();
            }
            catch(Exception ex)
            {
                Console.WriteLine(ex);
            }

        }

        public void GenMemberToSpecialization()
        {
            var list = _parser.ParseCSV(PATH + "\\Generator.CMD\\Dataset\\MemberToSpecialization.csv");
            var members = _dbContext.Members.ToList();
            var specializations = _dbContext.Specializations.ToList();

            for(var i = 0;i < list.Count; i++)
            {
                var name = list[i].Item1.Replace(" ", "");
                var member = members.FirstOrDefault(m => (m.Name + m.SecondName).Replace(" ", "").Equals(name));
                if(member != null)
                {
                    for(var j = 0;j < list[i].Item2.Count; j++)
                    {
                        var specialization = specializations.FirstOrDefault(s => s.Name.Equals(list[i].Item2[j]));
                        if(specialization != null)
                        {
                            member.Specializations.Add(specialization);
                            specialization.Members.Add(member);
                        }
                    }
                }
            }

            _dbContext.SaveChanges();
        }

        public static string GeneratePhoneNumber()
        {
            Random random = new Random();

            int countryCodeLength = random.Next(1, 4);
            string countryCode = GenerateRandomDigits(countryCodeLength, random);

            string mainNumber = GenerateRandomDigits(10, random);

            string phoneNumber = $"+{countryCode}{mainNumber}";

            return phoneNumber;
        }

        private static string GenerateRandomDigits(int length, Random random)
        {
            char[] digits = new char[length];
            for (int i = 0; i < length; i++)
            {
                digits[i] = (char)('0' + random.Next(0, 10));
            }

            return new string(digits);
        }

        public static DateTime GenerateRandomDate(DateTime startDate, DateTime endDate)
        {
            Random random = new Random();
            int range = (endDate - startDate).Days;
            return startDate.AddDays(random.Next(range));
        }

        public void GenStores()
        {
            var storeNames_1 = _parser.Parse(PATH + "\\Generator.CMD\\Dataset\\StoreName1.txt");
            var storeNames_2 = _parser.Parse(PATH + "\\Generator.CMD\\Dataset\\StoreName2.txt");
            var districtIds = _dbContext.Districts.Select(d => d.Id).ToList();
            var ownershipTypeIds = _dbContext.OwnershipTypes.Select(o => o.Id).ToList();
            DateTime startDate = new DateTime(1860, 1, 1);
            DateTime endDate = DateTime.Now;
            for (var i = 0;i < 500; i++)
            {
                _dbContext.Stores.Add(new Store
                {
                    Name = storeNames_1[new Random().Next(0, storeNames_1.Count - 1)] + " " + storeNames_1[new Random().Next(0, storeNames_2.Count - 1)],
                    DistrictId = districtIds[new Random().Next(0, districtIds.Count - 1)],
                    OwnershipTypeId = ownershipTypeIds[new Random().Next(0, ownershipTypeIds.Count - 1)],
                    YearOpened = GenerateRandomDate(startDate, endDate),
                    PhoneNumber = GeneratePhoneNumber()
                });
            }

            _dbContext.SaveChanges();
        }

        public void GenCompany()
        {
            var list = _parser.Parse(PATH + "\\Generator.CMD\\Dataset\\Company.txt");
            var districtIds = _dbContext.Districts.Select(d => d.Id).ToList();

            for(var i = 0;i < list.Count; i++)
            {
                _dbContext.Companies.Add(new Company
                {
                    Name = list[i],
                    DistrictId = districtIds[new Random().Next(0, districtIds.Count - 1)],
                    PhoneNumber = GeneratePhoneNumber()
                });
            }

            _dbContext.SaveChanges();
        }

        public void GenAlbum()
        {
            var list = _parser.ParseCSV(PATH + "\\Generator.CMD\\Dataset\\Albums.csv");
            var groups = _dbContext.Groups.Include(g => g.Albums).ToList();
            var companyIds = _dbContext.Companies.Select(c => c.Id).ToList();
            var listenerTypeIds = _dbContext.ListenerTypes.Select(l => l.Id).ToList();
            DateTime startDate = new DateTime(1950, 1, 1);
            DateTime endDate = DateTime.Now;

            for (var i = 0;i < list.Count; i++)
            {
                var group = groups.FirstOrDefault(g => g.Name.Equals(list[i].Item1));
                if(group != null)
                {
                    for(var j = 0;j < list[i].Item2.Count; j++)
                    {
                        
                        if(group.Albums.FirstOrDefault(a => a.Name.Equals(list[i].Item2[j])) == null)
                        {
                            var album = new Album
                            {
                                Name = list[i].Item2[j],
                                CompanyId = companyIds[new Random().Next(0, companyIds.Count - 1)],
                                GroupId = group.Id,
                                Duration = 0,
                                SongsCount = 0,
                                ListenerTypeId = listenerTypeIds[new Random().Next(0, listenerTypeIds.Count - 1)],
                                ReleaseDate = GenerateRandomDate(startDate, endDate)                         
                            };
                            _dbContext.Albums.Add(album);
                            group.Albums.Add(album);
                        }
                    }
                }
            }

            _dbContext.SaveChanges();
        }

        public void GenSong()
        {
            var list = _parser.ParseCSV(PATH + "\\Generator.CMD\\Dataset\\AlbumToSongs.csv");

            var albums = _dbContext.Albums.Include(a => a.Songs).ToList();           
            var songs = _dbContext.Songs.ToList();

            for(var i = 0;i < list.Count; i++)
            {
                var album = albums.FirstOrDefault(a => a.Name.Equals(list[i].Item1));
                if(album != null)
                {
                    for(var j = 0;j < list[i].Item2.Count; j++)
                    {
                        var song = new Song
                        {
                            Name = list[i].Item2[j],
                            Duration = new Random().Next(2, 8),
                            AlbumId = album.Id
                        };

                        _dbContext.Songs.Add(song);
                        album.Songs.Add(song);
                    }
                }
            }

            _dbContext.SaveChanges();
        }

        public void GenProduct()
        {
            var albumIds = _dbContext.Albums.Select(a => a.Id).ToList();
            var storeIds = _dbContext.Stores.Select(s => s.Id).ToList();
            DateTime startDate = new DateTime(1990, 1, 1);
            DateTime endDate = DateTime.Now;

            Random random = new Random();

            // Используем HashSet для отслеживания уникальных пар StoreId и AlbumId
            HashSet<(int AlbumId, int StoreId)> existingProductKeys = _dbContext.Products
                .Select(p => new { p.AlbumId, p.StoreId })
                .AsEnumerable()
                .Select(p => (p.AlbumId, p.StoreId))
                .ToHashSet();

            for (var i = 0; i < 6000; i++)
            {
                var Aid = albumIds[random.Next(0, albumIds.Count)];
                var Sid = storeIds[random.Next(0, storeIds.Count)];

                // Проверяем, существует ли уже пара (AlbumId, StoreId)
                if (!existingProductKeys.Contains((Aid, Sid)))
                {
                    _dbContext.Products.Add(new Product
                    {
                        AlbumId = Aid,
                        StoreId = Sid,
                        Price = random.Next(1000, 100000),
                        Quantity = random.Next(1, 100),
                        DateReceived = GenerateRandomDate(startDate, endDate)
                    });
                    existingProductKeys.Add((Aid, Sid)); // Добавляем новую пару в HashSet
                }
            }

            _dbContext.SaveChanges();
        }

    }

    
}
