using Microsoft.EntityFrameworkCore;
using ProductSalesEntity.Entity;


namespace ProductSalesRepository.Repository
{
    public class SaleRepository : ISaleRepository
    {
        private readonly ProductSalesContext _context;

        public SaleRepository(ProductSalesContext context) => _context = context;

        public async Task<IEnumerable<Sale>> GetAllAsync() => await _context.Sales
                .Include(s => s.Product)
                .Include(s => s.TrackingNumbers)
                .AsNoTracking()
                .ToListAsync();
        

        public async Task<Sale?> GetByIdAsync(int saleId) => await _context.Sales
                .Include(s => s.Product)
                .Include(s => s.TrackingNumbers)
                .AsNoTracking()
                .FirstOrDefaultAsync(s => s.SaleId == saleId);
        

        public async Task AddAsync(Sale sale)
        {
            _context.Sales.Add(sale);
            await _context.SaveChangesAsync();
        }

        public async Task UpdateAsync(Sale sale)
        {
            _context.Sales.Update(sale);
            await _context.SaveChangesAsync();
        }

        public async Task DeleteAsync(int saleId)
        {
            var sale = await _context.Sales.FindAsync(saleId);
            if (sale != null)
            {
                _context.Sales.Remove(sale);
                await _context.SaveChangesAsync();
            }
        }
    }
}
