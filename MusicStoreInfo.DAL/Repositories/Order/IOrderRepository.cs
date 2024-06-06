using MusicStoreInfo.Domain.Entities;

namespace MusicStoreInfo.DAL.Repositories
{
    public interface IOrderRepository
    {
        Task Add(Order order);
        Task Delete(int id);
        Task<List<Order>> Get();
        Task<Order?> GetById(int id);
        Task Update(int id, string name, string image);
    }
}