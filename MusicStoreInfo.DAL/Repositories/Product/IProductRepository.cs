﻿using MusicStoreInfo.Domain.Entities;

namespace MusicStoreInfo.DAL.Repositories
{
    public interface IProductRepository
    {
        Task Add(Product product);
        Task Delete(int id);
        Task<List<Product>> Get();
        Task<Product?> GetById(int id);
        Task Update(int id, int albumId, int storeId, decimal price, int quantity, DateTime dateReceived);
        Task AddReview(Review review, int id);
    }
}