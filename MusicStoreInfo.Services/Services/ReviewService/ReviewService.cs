using Microsoft.EntityFrameworkCore;
using MusicStoreInfo.DAL.Repositories;
using MusicStoreInfo.Domain.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MusicStoreInfo.Services.Services.ReviewService
{
    public class ReviewService : IReviewService
    {
        private readonly IReviewRepository _reviewRepository;

        public ReviewService(IReviewRepository reviewRepository)
        {
            _reviewRepository = reviewRepository;
        }

        public async Task<List<Review>> GetAsync()
        {
            return await _reviewRepository.Get();
        }

        public async Task<Review?> GetByIdAsync(int id)
        {
            return await _reviewRepository.GetById(id);
        }

        public async Task Add(Review review)
        {
            await _reviewRepository.Add(review);
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
