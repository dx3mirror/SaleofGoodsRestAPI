using ProductSalesEntity.Entity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ProductSalesRepository.Repository
{
    public interface ITrackingNumberRepository
    {
        Task<IEnumerable<TrackingNumber>> GetAllAsync();
        Task<TrackingNumber?> GetByIdAsync(int trackingNumberId);
        Task AddAsync(TrackingNumber trackingNumber);
        Task UpdateAsync(TrackingNumber trackingNumber);
        Task DeleteAsync(int trackingNumberId);
    }
}
