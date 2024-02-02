using ProductSalesEntity.Entity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ProductSalesRepository.Repository
{
    public interface ITrackingAnalysisRepository
    {
        Task<IEnumerable<Product>> GetProductsAboveAverageQuantityAsync();
        Task<IEnumerable<DateTime>> GetSaleDatesAboveQuantityAsync(int quantity);
        Task<IEnumerable<Product>> GetUnsoldProductsLastMonthAsync();
        Task<IEnumerable<(Product Product, int TotalQuantitySold)>> GetProductsMinTotalQuantitySoldAsync();
        Task<IEnumerable<Product>> GetProductsSoldWithTrackingNumbersAsync();
        Task<IEnumerable<(int ProductId, DateTime MonthStart, int TotalQuantitySold)>> GetMonthlyProductSalesAsync();
        Task<IEnumerable<Product>> GetUnsoldProductsWithTrackingNumbersAsync();
        Task<IEnumerable<(DateTime MonthStart, int UniqueProductCount)>> GetUniqueProductCountPerMonthAsync();
        Task<IEnumerable<int>> GetProductsIncreasedInSalesAsync();
        Task<IEnumerable<(DateTime SaleDate, int TotalQuantitySold)>> GetTotalQuantitySoldPerDateAsync();
    }
}
