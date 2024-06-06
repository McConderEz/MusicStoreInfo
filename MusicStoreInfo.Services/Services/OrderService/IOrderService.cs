using MusicStoreInfo.Domain.Entities;

namespace MusicStoreInfo.Services.Services.OrderService
{
    public interface IOrderService
    {
        Task AddAsync(Order order);
        Task Delete(int id);
        Task<List<Order>> GetAsync();
        Task<Order?> GetByIdAsync(int id);
        Task Update(int id, string name, string image);
    }
}