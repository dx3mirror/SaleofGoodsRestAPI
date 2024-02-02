using Microsoft.EntityFrameworkCore;
using ProductSalesEntity.Entity;


namespace ProductSalesRepository.Repository
{
    public class TrackingNumberRepository : ITrackingNumberRepository
    {
        private readonly ProductSalesContext _context;

        public TrackingNumberRepository(ProductSalesContext context) =>  _context = context;
        

        public async Task<IEnumerable<TrackingNumber>> GetAllAsync() => await _context.TrackingNumbers
                .Include(t => t.Sale)
                .AsNoTracking()
                .ToListAsync();
        

        public async Task<TrackingNumber?> GetByIdAsync(int trackingNumberId) => await _context.TrackingNumbers
                .Include(t => t.Sale)
                .AsNoTracking()
                .FirstOrDefaultAsync(t => t.TrackingNumberId == trackingNumberId);
        

        public async Task AddAsync(TrackingNumber trackingNumber)
        {
            _context.TrackingNumbers.Add(trackingNumber);
            await _context.SaveChangesAsync();
        }

        public async Task UpdateAsync(TrackingNumber trackingNumber)
        {
            _context.TrackingNumbers.Update(trackingNumber);
            await _context.SaveChangesAsync();
        }

        public async Task DeleteAsync(int trackingNumberId)
        {
            var trackingNumber = await _context.TrackingNumbers.FindAsync(trackingNumberId);
            if (trackingNumber != null)
            {
                _context.TrackingNumbers.Remove(trackingNumber);
                await _context.SaveChangesAsync();
            }
        }
    }
}
