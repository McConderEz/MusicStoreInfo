using MusicStoreInfo.DAL.Repositories;
using MusicStoreInfo.Domain.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MusicStoreInfo.Services.Services.GenreService
{
    public class GenreService : IGenreService
    {
        private readonly IGenreRepository _repository;

        public GenreService(IGenreRepository repository)
        {
            _repository = repository;
        }

        public async Task CreateAsync(Genre model)
        {
            await _repository.Add(model);
        }

        public async Task<List<Genre>?> GetAllAsync()
        {
            var result = await _repository.Get();
            return result;
        }

        public async Task EditAsync(int id, Genre model)
        {
            var genre = _repository.GetById(id);

            if (genre == null)
                throw new InvalidOperationException("Данный объект не найден в коллекции");

            await _repository.Update(id, model.Name);
        }

        public async Task<Genre?> DetailsAsync(int id)
        {
            var genre = await _repository.GetById(id);

            if (genre == null)
                throw new InvalidOperationException("Данный объект не найден в коллекции");

            return genre;
        }

        public async Task DeleteAsync(int id)
        {
            await _repository.Delete(id);
        }

        public async Task<Genre?> GetByIdAsync(int id)
        {
            var genre = await _repository.GetById(id);
            return genre;
        }
    }
}
