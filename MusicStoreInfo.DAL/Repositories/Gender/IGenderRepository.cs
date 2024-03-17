﻿using MusicStoreInfo.Domain.Entities;

namespace MusicStoreInfo.DAL.Repositories
{
    public interface IGenderRepository
    {
        Task Add(string name);
        Task Delete(int id);
        Task<List<Gender>> Get();
        Task<Gender?> GetById(int id);
        Task Update(int id, string name);
    }
}