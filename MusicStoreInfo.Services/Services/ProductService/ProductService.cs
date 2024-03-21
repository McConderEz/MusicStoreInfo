using MusicStoreInfo.DAL.Repositories;
using MusicStoreInfo.Domain.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MusicStoreInfo.Services.Services.ProductService
{
    public class ProductService : IProductService
    {
        private readonly IProductRepository _repository;

        public ProductService(IProductRepository repository)
        {
            _repository = repository;
        }

        public async Task CreateAsync(Product model)
        {
            await _repository.Add(model);
        }

        public async Task<List<Product>?> GetAllAsync()
        {
            var result = await _repository.Get();
            return result;
        }

        public async Task EditAsync(int id, Product model)
        {
            var product = _repository.GetById(id);

            if (product == null)
                throw new InvalidOperationException("Данный объект не найден в коллекции");

            await _repository.Update(id, model.AlbumId, model.StoreId, model.Price, model.Quantity, model.DateReceived);
        }

        public async Task<Product?> DetailsAsync(int id)
        {
            var product = await _repository.GetById(id);

            if (product == null)
                throw new InvalidOperationException("Данный объект не найден в коллекции");

            return product;
        }

        public async Task DeleteAsync(int id)
        {
            await _repository.Delete(id);
        }
    }
}
