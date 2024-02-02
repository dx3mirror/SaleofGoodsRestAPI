using Microsoft.EntityFrameworkCore;
using ProductSalesEntity.Entity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ProductSalesRepository.Repository
{
    public class ProductSaleRepository : IProductSaleRepository
    {
        private readonly ProductSalesContext _context;

        public ProductSaleRepository(ProductSalesContext context) => _context = context;

        public async Task<decimal> GetAverageProductPriceAsync()
        {
            return await _context.Products
                .AverageAsync(p => p.Price);
        }
        public async Task<IEnumerable<(int ProductId, int TotalQuantitySold)>> GetTopProductsBySalesAsync(int topCount)
        {
            return await _context.Sales
                .GroupBy(s => s.ProductId)
                .Select(g => new { ProductId = g.Key, TotalQuantitySold = g.Sum(s => s.Quantity) })
                .OrderByDescending(result => result.TotalQuantitySold)
                .Take(topCount)
                .Select(result => new ValueTuple<int, int>(
                    result.ProductId.GetValueOrDefault(),
                    result.TotalQuantitySold.GetValueOrDefault()))
                .ToListAsync();
        }

        public async Task<decimal> GetTotalSalesAsync()
        {
            return (decimal)await _context.Sales
                .SumAsync(s => s.Quantity * s.Product.Price);
        }
        

        // Реализация метода GetTotalSalesAmountAsync
        public async Task<decimal> GetTotalSalesAmountAsync()
        {
            return (await _context.Sales
                .SumAsync(s => s.Quantity * s.Product.Price)).GetValueOrDefault();
        }

        public async Task<IEnumerable<Product?>> GetTop5ProductsBySalesAsync()
        {
            return await _context.Sales
                .GroupBy(s => s.ProductId)
                .OrderByDescending(g => g.Sum(s => s.Quantity))
                .Take(5)
                .Select(g => g.First().Product)
                .ToListAsync();
        }

        public async Task<double> GetAverageQuantityPerSaleAsync()
        {
            return await _context.Sales
                .AverageAsync(s => s.Quantity ?? 0);
        }

        public async Task<Product> GetProductWithMaxPriceAsync()
        {
            return await _context.Products
                .OrderByDescending(p => p.Price)
                .FirstAsync();
        }

        public async Task<Product> GetProductWithMinPriceAsync()
        {
            return await _context.Products
                .OrderBy(p => p.Price)
                .FirstAsync();
        }

        public async Task<Dictionary<DateTime, decimal>> GetDailySalesAsync()
        {
            return await _context.Sales
                .GroupBy(s => s.SaleDate.GetValueOrDefault().Date)
                .ToDictionaryAsync(g => g.Key, g => g.Sum(s => s.Quantity * s.Product.Price) ?? 0m);
        }

        public async Task<Dictionary<int, decimal>> GetMonthlySalesAsync()
        {
            return await _context.Sales
                .GroupBy(s => s.SaleDate.GetValueOrDefault().Month)
                .ToDictionaryAsync(g => g.Key, g => g.Sum(s => (decimal)(s.Quantity ?? 0) * s.Product.Price));
        }

        public async Task<IEnumerable<Product>> GetUnsoldProductsAsync()
        {
            return await _context.Products
                .Where(p => !p.Sales.Any())
                .ToListAsync();
        }

        public async Task<int> GetTotalQuantitySoldAsync()
        {
            return await _context.Sales
                .SumAsync(s => s.Quantity ?? 0);
        }

        public async Task<double> GetAverageSalesPerDayAsync()
        {
            return await _context.Sales
                .GroupBy(s => s.SaleDate.GetValueOrDefault().Date)
                .AverageAsync(g => g.Sum(s => s.Quantity ?? 0));
        }


        public async Task<Dictionary<int, decimal>> GetTotalCostPerProductAsync()
        {
            return await _context.Sales
                .GroupBy(s => s.ProductId.GetValueOrDefault())
                .ToDictionaryAsync(g => g.Key, g => g.Sum(s => (s.Quantity ?? 0) * s.Product.Price));
        }


        public async Task<Product?> GetMostProfitableProductAsync()
        {
            var mostProfitableProductId = await _context.Sales
                .GroupBy(s => s.ProductId)
                .OrderByDescending(g => g.Sum(s => s.Quantity * s.Product.Price))
                .Select(g => g.Key)
                .FirstOrDefaultAsync();

            if (mostProfitableProductId != null)
            {
                var mostProfitableProduct = await _context.Products
                    .Include(p => p.Sales)
                    .FirstOrDefaultAsync(p => p.ProductId == mostProfitableProductId);

                return mostProfitableProduct;
            }

            return null;
        }



        public async Task<Dictionary<int, int>> GetSalesCountPerProductAsync()
        {
            return await _context.Sales
                .GroupBy(s => s.ProductId.GetValueOrDefault())
                .ToDictionaryAsync(g => g.Key, g => g.Count());
        }


        public async Task<decimal> GetAverageSalePriceAsync()
        {
            return (decimal)await _context.Sales
                .AverageAsync(s => s.Quantity * s.Product.Price);
        }

        public async Task<IEnumerable<Product?>> GetProductsSoldMoreThanAsync(int quantity)
        {
            return await _context.Sales
                .GroupBy(s => s.ProductId)
                .Where(g => g.Sum(s => s.Quantity) > quantity)
                .Select(g => g.First().Product)
                .ToListAsync();
        }

        public async Task<Dictionary<DateTime, int>> GetDaysWithHighestSalesAsync()
        {
            return await _context.Sales
                .GroupBy(s => s.SaleDate.GetValueOrDefault().Date)
                .ToDictionaryAsync(g => g.Key, g => g.Sum(s => s.Quantity) ?? 0);
        }



        public async Task<Dictionary<int, double>> GetAverageSalesPerMonthAsync()
        {
            return await _context.Sales
                .GroupBy(s => new { ProductId = s.ProductId.GetValueOrDefault(), Month = s.SaleDate.GetValueOrDefault().Month })
                .ToDictionaryAsync(g => g.Key.ProductId, g => g.Average(s => s.Quantity.GetValueOrDefault()));
        }


        public async Task<IEnumerable<Sale>> GetSalesInLastMonthAsync()
        {
            var lastMonth = DateTime.Now.AddMonths(-1);
            return await _context.Sales
                .Where(s => s.SaleDate >= lastMonth)
                .ToListAsync();
        }

        public async Task<decimal> GetAverageOrderValueAsync()
        {
            return (decimal)await _context.Sales
                .AverageAsync(s => s.Quantity * s.Product.Price);
        }

    }
}

