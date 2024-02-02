
using Microsoft.EntityFrameworkCore;
using ProductSalesEntity.Entity;

namespace ProductSalesRepository.Repository
{
    public class ProductRepository : IProductRepository
    {
        private readonly ProductSalesContext _context;

        public ProductRepository(ProductSalesContext context) => _context = context;

        public async Task<Product?> GetByIdAsync(int productId) => await _context.Products
                 .Include(p => p.Sales)
                 .AsNoTracking() // Добавьте это, чтобы гарантировать отсутствие проблем с отслеживанием объектов
                 .FirstOrDefaultAsync(p => p.ProductId == productId);


        public async Task<IEnumerable<Product>> GetAllAsync() => await _context.Products
                .Include(p => p.Sales)
                .ToListAsync();
        

        public async Task AddAsync(Product product)
        {
            _context.Products.Add(product);
            await _context.SaveChangesAsync();
        }

        public async Task UpdateAsync(Product product)
        {
            _context.Products.Update(product);
            await _context.SaveChangesAsync();
        }

        public async Task DeleteAsync(int productId)
        {
            var product = await _context.Products.FindAsync(productId);
            if (product != null)
            {
                _context.Products.Remove(product);
                await _context.SaveChangesAsync();
            }
        }
    }
}
