﻿using MusicStoreInfo.Domain.Entities;

namespace MusicStoreInfo.DAL.Repositories
{
    public interface IUserRepository
    {
        Task Add(User user);
        Task<List<User>> Get();
        Task<User?> GetById(int id);
        Task<User?> GetByUserName(string userName);
        Task Update(int id, string userName, string passwordHash, string? email, string? phoneNumber, string? imagePath, int roleId, int? storeId, bool isBlocked);
    }
}