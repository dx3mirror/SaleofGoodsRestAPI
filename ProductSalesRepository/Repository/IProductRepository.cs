
using ProductSalesEntity.Entity;

namespace ProductSalesRepository.Repository
{
    public interface IProductRepository
           {
                Task<Product?> GetByIdAsync(int productId);
                Task<IEnumerable<Product>> GetAllAsync();
                Task AddAsync(Product product);
                Task UpdateAsync(Product product);
                Task DeleteAsync(int productId);
           }
}
