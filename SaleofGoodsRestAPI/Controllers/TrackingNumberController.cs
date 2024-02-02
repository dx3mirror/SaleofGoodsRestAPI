using Microsoft.AspNetCore.Mvc;
using ProductSalesEntity.Entity;
using ProductSalesRepository.Repository;

namespace SaleofGoodsRestAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class TrackingNumberController : ControllerBase
    {
        private readonly ITrackingNumberRepository _trackingNumberRepository;

        public TrackingNumberController(ITrackingNumberRepository trackingNumberRepository)
        {
            _trackingNumberRepository = trackingNumberRepository ?? throw new ArgumentNullException(nameof(trackingNumberRepository));
        }

        [HttpGet]
        public async Task<ActionResult<IEnumerable<TrackingNumber>>> GetAllTrackingNumbers()
        {
            var trackingNumbers = await _trackingNumberRepository.GetAllAsync();
            return Ok(trackingNumbers);
        }

        [HttpGet("{trackingNumberId}")]
        public async Task<ActionResult<TrackingNumber>> GetTrackingNumberById(int trackingNumberId)
        {
            var trackingNumber = await _trackingNumberRepository.GetByIdAsync(trackingNumberId);

            if (trackingNumber == null)
            {
                return NotFound();
            }

            return Ok(trackingNumber);
        }

        [HttpPost]
        public async Task<ActionResult<TrackingNumber>> CreateTrackingNumber([FromBody] TrackingNumber trackingNumber)
        {
            await _trackingNumberRepository.AddAsync(trackingNumber);
            return CreatedAtAction(nameof(GetTrackingNumberById), new { trackingNumberId = trackingNumber.TrackingNumberId }, trackingNumber);
        }

        [HttpPut("{trackingNumberId}")]
        public async Task<IActionResult> UpdateTrackingNumber(int trackingNumberId, [FromBody] TrackingNumber trackingNumber)
        {
            if (trackingNumberId != trackingNumber.TrackingNumberId)
            {
                return BadRequest();
            }

            await _trackingNumberRepository.UpdateAsync(trackingNumber);
            return NoContent();
        }

        [HttpDelete("{trackingNumberId}")]
        public async Task<IActionResult> DeleteTrackingNumber(int trackingNumberId)
        {
            await _trackingNumberRepository.DeleteAsync(trackingNumberId);
            return NoContent();
        }
    }
}
