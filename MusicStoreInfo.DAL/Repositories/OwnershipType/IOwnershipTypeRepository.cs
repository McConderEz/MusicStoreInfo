﻿using MusicStoreInfo.Domain.Entities;

namespace MusicStoreInfo.DAL.Repositories
{
    public interface IOwnershipTypeRepository
    {
        Task Add(OwnershipType ownershipType);
        Task Delete(int id);
        Task<List<OwnershipType>> Get();
        Task<OwnershipType?> GetById(int id);
        Task Update(int id, string name);
    }
}