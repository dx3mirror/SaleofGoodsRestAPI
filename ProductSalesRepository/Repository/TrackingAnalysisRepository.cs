using System.Linq;
using Microsoft.EntityFrameworkCore;
using ProductSalesEntity.Entity;

namespace ProductSalesRepository.Repository
{
    public class TrackingAnalysisRepository : ITrackingAnalysisRepository
    {
        private readonly ProductSalesContext _context;

        public TrackingAnalysisRepository(ProductSalesContext context)
        {
            _context = context;
        }
        public async Task<IEnumerable<Product>> GetProductsAboveAverageQuantityAsync()
        {
            return await _context.Products
                .Join(
                    _context.Sales
                        .GroupBy(s => s.ProductId)
                        .Select(g => new { ProductId = g.Key, AvgQuantity = g.Average(s => s.Quantity) }),
                    p => p.ProductId,
                    s => s.ProductId,
                    (p, s) => new { Product = p, AvgQuantity = s.AvgQuantity }
                )
                .Join(
                    _context.Sales,
                    ps => ps.Product.ProductId,
                    s => s.ProductId,
                    (ps, s) => new { Product = ps.Product, AvgQuantity = ps.AvgQuantity, Sale = s }
                )
                .Where(ps => ps.Sale.Quantity > ps.AvgQuantity)
                .Select(ps => ps.Product)
                .ToListAsync();
        }

        public async Task<IEnumerable<DateTime>> GetSaleDatesAboveQuantityAsync(int quantity)
        {
            return await _context.Sales
                .Where(s => s.ProductId == _context.Sales
                    .OrderByDescending(p => p.Quantity * p.Product.Price)
                    .Select(p => p.ProductId)
                    .FirstOrDefault()
                && s.Quantity > quantity)
                .Select(s => s.SaleDate.GetValueOrDefault())
                .ToListAsync();
        }

        public async Task<IEnumerable<Product>> GetUnsoldProductsLastMonthAsync()
        {
            return await _context.Products
                .GroupJoin(
                    _context.Sales.Where(s => s.SaleDate >= DateTime.Now.AddMonths(-1)),
                    p => p.ProductId,
                    s => s.ProductId,
                    (p, s) => new { Product = p, Sales = s }
                )
                .Where(ps => !ps.Sales.Any())
                .Select(ps => ps.Product)
                .ToListAsync();
        }

        public async Task<IEnumerable<(Product Product, int TotalQuantitySold)>> GetProductsMinTotalQuantitySoldAsync()
        {
            var products = await _context.Products
                .GroupJoin(
                    _context.Sales.GroupBy(s => s.ProductId)
                        .Select(g => new { ProductId = g.Key, TotalQuantitySold = g.Sum(s => s.Quantity) }),
                    p => p.ProductId,
                    s => s.ProductId,
                    (p, s) => new { Product = p, Sales = s }
                )
                .SelectMany(ps => ps.Sales.DefaultIfEmpty(),
                    (ps, s) => new { ps.Product, TotalQuantitySold = s != null ? s.TotalQuantitySold ?? 0 : 0 })
                .ToListAsync();

            var minTotalQuantitySold = products.Min(p => p.TotalQuantitySold);

            return products.Where(p => p.TotalQuantitySold == minTotalQuantitySold)
                .Select(p => (p.Product, (int)p.TotalQuantitySold));
        }

        public async Task<IEnumerable<Product>> GetProductsSoldWithTrackingNumbersAsync()
        {
            return await _context.Products
                .Join(
                    _context.Sales,
                    p => p.ProductId,
                    s => s.ProductId,
                    (p, s) => new { Product = p, Sale = s }
                )
                .Join(
                    _context.TrackingNumbers,
                    ps => ps.Sale.SaleId,
                    t => t.SaleId,
                    (ps, t) => ps.Product
                )
                .Distinct()
                .ToListAsync();
        }

        public async Task<IEnumerable<(int ProductId, DateTime MonthStart, int TotalQuantitySold)>> GetMonthlyProductSalesAsync()
        {
            var monthlyProductSales = await _context.Sales
                .GroupBy(s => new { ProductId = s.ProductId, MonthStart = EF.Functions.DateDiffDay(DateTime.MinValue, s.SaleDate.GetValueOrDefault()) / 30 })
                .Select(g => new { ProductId = g.Key.ProductId.GetValueOrDefault(), MonthStart = DateTime.MinValue.AddDays(g.Key.MonthStart * 30), TotalQuantitySold = g.Sum(s => s.Quantity) })
                .ToListAsync();

            var maxTotalQuantitySold = monthlyProductSales.GroupBy(s => s.ProductId)
                .Select(g => new { ProductId = g.Key, MaxTotalQuantitySold = g.Max(s => s.TotalQuantitySold.Value) });

            return monthlyProductSales.Join(
                maxTotalQuantitySold,
                s => s.ProductId,
                m => m.ProductId,
                (s, m) => new { s, m })
                .Where(x => x.s.TotalQuantitySold == x.m.MaxTotalQuantitySold)
                .Select(x => (x.s.ProductId, x.s.MonthStart, x.s.TotalQuantitySold.Value));
        }

        public async Task<IEnumerable<Product>> GetUnsoldProductsWithTrackingNumbersAsync()
        {
            return await _context.Products
                .GroupJoin(
                    _context.Sales,
                    p => p.ProductId,
                    s => s.ProductId,
                    (p, s) => new { Product = p, Sales = s }
                )
                .SelectMany(ps => ps.Sales.DefaultIfEmpty(),
                    (ps, s) => new { ps.Product, Sale = s })
                .GroupJoin(
                    _context.TrackingNumbers,
                    ps => ps.Sale.SaleId,
                    t => t.SaleId,
                    (ps, t) => new { ps.Product, ps.Sale, TrackingNumbers = t }
                )
                .Where(ps => !ps.TrackingNumbers.Any())
                .Select(ps => ps.Product)
                .ToListAsync();
        }

        public async Task<IEnumerable<(DateTime MonthStart, int UniqueProductCount)>> GetUniqueProductCountPerMonthAsync()
        {
            return await _context.Sales
                .GroupBy(s => new { MonthStart = new DateTime(s.SaleDate.GetValueOrDefault().Year, s.SaleDate.GetValueOrDefault().Month, 1) })
                .Select(g => new ValueTuple<DateTime, int>(
                    g.Key.MonthStart,
                    g.Select(s => s.ProductId).Distinct().Count()
                ))
                .ToListAsync();
        }



        public async Task<IEnumerable<int>> GetProductsIncreasedInSalesAsync()
        {
            var previousMonthSales = await _context.Sales
                .GroupBy(s => new { s.ProductId, MonthStart = EF.Functions.DateDiffDay(DateTime.MinValue, s.SaleDate.GetValueOrDefault()) / 30 })
                .Select(g => new
                {
                    ProductId = g.Key.ProductId.GetValueOrDefault(),
                    MonthStart = DateTime.MinValue.AddDays(g.Key.MonthStart * 30),
                    TotalQuantitySold = g.Sum(s => s.Quantity)
                })
                .Join(
                    _context.Sales
                        .GroupBy(s => new { s.ProductId, MonthStart = EF.Functions.DateDiffDay(DateTime.MinValue, s.SaleDate.GetValueOrDefault()) / 30 })
                        .Select(g => new
                        {
                            ProductId = g.Key.ProductId.GetValueOrDefault(),
                            MonthStart = DateTime.MinValue.AddDays(g.Key.MonthStart * 30),
                            TotalQuantitySold = g.Sum(s => s.Quantity)
                        }),
                    s => new { s.ProductId, s.MonthStart },
                    p => new { p.ProductId, MonthStart = p.MonthStart.AddDays(30) },
                    (s, p) => s.ProductId
                )
                .Distinct()
                .ToListAsync();

            return previousMonthSales;
        }


        public async Task<IEnumerable<(DateTime SaleDate, int TotalQuantitySold)>> GetTotalQuantitySoldPerDateAsync()
        {
            return await _context.Sales
                .GroupBy(s => s.SaleDate.GetValueOrDefault())
                .Select(g => new ValueTuple<DateTime, int>(
                    g.Key,
                    g.Sum(s => s.Quantity) ?? 0
                ))
                .OrderByDescending(x => x.Item2)
                .ToListAsync();
        }

    }
}
