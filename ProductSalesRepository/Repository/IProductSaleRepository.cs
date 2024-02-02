using ProductSalesEntity.Entity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ProductSalesRepository.Repository
{
    public interface IProductSaleRepository
    {
        Task<decimal> GetAverageProductPriceAsync();
        Task<decimal> GetTotalSalesAmountAsync();
        Task<IEnumerable<(int ProductId, int TotalQuantitySold)>> GetTopProductsBySalesAsync(int topCount);
        Task<double> GetAverageQuantityPerSaleAsync();
        Task<Product?> GetProductWithMaxPriceAsync();
        Task<Product> GetProductWithMinPriceAsync();
        Task<Dictionary<DateTime, decimal>> GetDailySalesAsync();
        Task<Dictionary<int, decimal>> GetMonthlySalesAsync();
        Task<IEnumerable<Product>> GetUnsoldProductsAsync();
        Task<int> GetTotalQuantitySoldAsync();
        Task<double> GetAverageSalesPerDayAsync();
        Task<Dictionary<int, decimal>> GetTotalCostPerProductAsync();
        Task<Product?> GetMostProfitableProductAsync();
        Task<Dictionary<int, int>> GetSalesCountPerProductAsync();
        Task<decimal> GetAverageSalePriceAsync();
        Task<IEnumerable<Product?>> GetProductsSoldMoreThanAsync(int quantity);
        Task<Dictionary<DateTime, int>> GetDaysWithHighestSalesAsync();
        Task<Dictionary<int, double>> GetAverageSalesPerMonthAsync();
        Task<IEnumerable<Sale>> GetSalesInLastMonthAsync();
        Task<decimal> GetAverageOrderValueAsync();
    }

}
