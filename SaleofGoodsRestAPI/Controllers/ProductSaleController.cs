using Microsoft.AspNetCore.Mvc;
using ProductSalesRepository.Repository;
using ProductSalesEntity.Entity;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace SaleofGoodsRestAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ProductSaleController : ControllerBase
    {
        private readonly IProductSaleRepository _productSaleRepository;

        public ProductSaleController(IProductSaleRepository productSaleRepository)
        {
            _productSaleRepository = productSaleRepository;
        }

        [HttpGet("averageProductPrice")]
        public async Task<ActionResult<decimal>> GetAverageProductPriceAsync()
        {
            var result = await _productSaleRepository.GetAverageProductPriceAsync();
            return Ok(result);
        }

        [HttpGet("topProductsBySales/{topCount}")]
        public async Task<ActionResult<IEnumerable<(int ProductId, int TotalQuantitySold)>>> GetTopProductsBySalesAsync(int topCount)
        {
            var result = await _productSaleRepository.GetTopProductsBySalesAsync(topCount);
            return Ok(result);
        }

        [HttpGet("averageQuantityPerSale")]
        public async Task<ActionResult<double>> GetAverageQuantityPerSaleAsync()
        {
            var result = await _productSaleRepository.GetAverageQuantityPerSaleAsync();
            return Ok(result);
        }

        [HttpGet("productWithMaxPrice")]
        public async Task<ActionResult<Product>> GetProductWithMaxPriceAsync()
        {
            var result = await _productSaleRepository.GetProductWithMaxPriceAsync();
            return Ok(result);
        }

        [HttpGet("productWithMinPrice")]
        public async Task<ActionResult<Product>> GetProductWithMinPriceAsync()
        {
            var result = await _productSaleRepository.GetProductWithMinPriceAsync();
            return Ok(result);
        }

        [HttpGet("dailySales")]
        public async Task<ActionResult<Dictionary<DateTime, decimal>>> GetDailySalesAsync()
        {
            var result = await _productSaleRepository.GetDailySalesAsync();
            return Ok(result);
        }

        [HttpGet("monthlySales")]
        public async Task<ActionResult<Dictionary<int, decimal>>> GetMonthlySalesAsync()
        {
            var result = await _productSaleRepository.GetMonthlySalesAsync();
            return Ok(result);
        }

        [HttpGet("unsoldProducts")]
        public async Task<ActionResult<IEnumerable<Product>>> GetUnsoldProductsAsync()
        {
            var result = await _productSaleRepository.GetUnsoldProductsAsync();
            return Ok(result);
        }

        [HttpGet("totalQuantitySold")]
        public async Task<ActionResult<int>> GetTotalQuantitySoldAsync()
        {
            var result = await _productSaleRepository.GetTotalQuantitySoldAsync();
            return Ok(result);
        }

        [HttpGet("averageSalesPerDay")]
        public async Task<ActionResult<double>> GetAverageSalesPerDayAsync()
        {
            var result = await _productSaleRepository.GetAverageSalesPerDayAsync();
            return Ok(result);
        }

        [HttpGet("totalCostPerProduct")]
        public async Task<ActionResult<Dictionary<int, decimal>>> GetTotalCostPerProductAsync()
        {
            var result = await _productSaleRepository.GetTotalCostPerProductAsync();
            return Ok(result);
        }

        [HttpGet("mostProfitableProduct")]
        public async Task<ActionResult<Product?>> GetMostProfitableProductAsync()
        {
            var result = await _productSaleRepository.GetMostProfitableProductAsync();
            return Ok(result);
        }

        [HttpGet("salesCountPerProduct")]
        public async Task<ActionResult<Dictionary<int, int>>> GetSalesCountPerProductAsync()
        {
            var result = await _productSaleRepository.GetSalesCountPerProductAsync();
            return Ok(result);
        }

        [HttpGet("averageSalePrice")]
        public async Task<ActionResult<decimal>> GetAverageSalePriceAsync()
        {
            var result = await _productSaleRepository.GetAverageSalePriceAsync();
            return Ok(result);
        }

        [HttpGet("productsSoldMoreThan/{quantity}")]
        public async Task<ActionResult<IEnumerable<Product?>>> GetProductsSoldMoreThanAsync(int quantity)
        {
            var result = await _productSaleRepository.GetProductsSoldMoreThanAsync(quantity);
            return Ok(result);
        }

        [HttpGet("daysWithHighestSales")]
        public async Task<ActionResult<Dictionary<DateTime, int>>> GetDaysWithHighestSalesAsync()
        {
            var result = await _productSaleRepository.GetDaysWithHighestSalesAsync();
            return Ok(result);
        }

        [HttpGet("averageSalesPerMonth")]
        public async Task<ActionResult<Dictionary<int, double>>> GetAverageSalesPerMonthAsync()
        {
            var result = await _productSaleRepository.GetAverageSalesPerMonthAsync();
            return Ok(result);
        }

        [HttpGet("salesInLastMonth")]
        public async Task<ActionResult<IEnumerable<Sale>>> GetSalesInLastMonthAsync()
        {
            var result = await _productSaleRepository.GetSalesInLastMonthAsync();
            return Ok(result);
        }

        [HttpGet("averageOrderValue")]
        public async Task<ActionResult<decimal>> GetAverageOrderValueAsync()
        {
            var result = await _productSaleRepository.GetAverageOrderValueAsync();
            return Ok(result);
        }
    }
}
