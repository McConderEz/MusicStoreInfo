using MusicStoreInfo.DAL;
using MusicStoreInfo.DAL.Repositories;
using MusicStoreInfo.Domain.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MusicStoreInfo.Services.Services.OrderService
{
    public class OrderService : IOrderService
    {
        private readonly IOrderRepository _repository;

        public OrderService(IOrderRepository repository)
        {
            _repository = repository;
        }

        public async Task<List<Order>> GetAsync()
        {
            return await _repository.Get();
        }

        public async Task<Order?> GetByIdAsync(int id)
        {
            return await _repository.GetById(id);
        }

        public async Task AddAsync(Order order)
        {
            await _repository.Add(order);
        }

        public async Task Update(int id, string name, string image)
        {
            //TODO:реализовать
        }

        public async Task Delete(int id)
        {
            //TODO:реализовать
        }
    }
}
