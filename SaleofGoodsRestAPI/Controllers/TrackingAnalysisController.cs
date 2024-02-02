using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using ProductSalesEntity.Entity;
using ProductSalesRepository.Repository;

namespace YourNamespace.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class TrackingAnalysisController : ControllerBase
    {
        private readonly ITrackingAnalysisRepository _repository;

        public TrackingAnalysisController(ITrackingAnalysisRepository repository)
        {
            _repository = repository;
        }

        [HttpGet("above-average-quantity")]
        public async Task<IEnumerable<Product>> GetProductsAboveAverageQuantityAsync()
        {
            return await _repository.GetProductsAboveAverageQuantityAsync();
        }

        [HttpGet("sale-dates-above-quantity/{quantity}")]
        public async Task<IEnumerable<DateTime>> GetSaleDatesAboveQuantityAsync(int quantity)
        {
            return await _repository.GetSaleDatesAboveQuantityAsync(quantity);
        }

        [HttpGet("unsold-products-last-month")]
        public async Task<IEnumerable<Product>> GetUnsoldProductsLastMonthAsync()
        {
            return await _repository.GetUnsoldProductsLastMonthAsync();
        }

        [HttpGet("min-total-quantity-sold")]
        public async Task<IEnumerable<(Product Product, int TotalQuantitySold)>> GetProductsMinTotalQuantitySoldAsync()
        {
            return await _repository.GetProductsMinTotalQuantitySoldAsync();
        }

        [HttpGet("sold-with-tracking-numbers")]
        public async Task<IEnumerable<Product>> GetProductsSoldWithTrackingNumbersAsync()
        {
            return await _repository.GetProductsSoldWithTrackingNumbersAsync();
        }

        [HttpGet("monthly-product-sales")]
        public async Task<IEnumerable<(int ProductId, DateTime MonthStart, int TotalQuantitySold)>> GetMonthlyProductSalesAsync()
        {
            return await _repository.GetMonthlyProductSalesAsync();
        }

        [HttpGet("unsold-products-with-tracking-numbers")]
        public async Task<IEnumerable<Product>> GetUnsoldProductsWithTrackingNumbersAsync()
        {
            return await _repository.GetUnsoldProductsWithTrackingNumbersAsync();
        }

        [HttpGet("unique-product-count-per-month")]
        public async Task<IEnumerable<(DateTime MonthStart, int UniqueProductCount)>> GetUniqueProductCountPerMonthAsync()
        {
            return await _repository.GetUniqueProductCountPerMonthAsync();
        }

        [HttpGet("increased-in-sales")]
        public async Task<IEnumerable<int>> GetProductsIncreasedInSalesAsync()
        {
            return await _repository.GetProductsIncreasedInSalesAsync();
        }

        [HttpGet("total-quantity-sold-per-date")]
        public async Task<IEnumerable<(DateTime SaleDate, int TotalQuantitySold)>> GetTotalQuantitySoldPerDateAsync()
        {
            return await _repository.GetTotalQuantitySoldPerDateAsync();
        }
    }
}
