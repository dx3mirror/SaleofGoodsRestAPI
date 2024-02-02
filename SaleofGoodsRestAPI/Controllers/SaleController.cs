using Microsoft.AspNetCore.Mvc;
using ProductSalesEntity.Entity;
using ProductSalesRepository.Repository;


namespace SaleofGoodsRestAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class SaleController : ControllerBase
    {
        private readonly ISaleRepository _saleRepository;

        public SaleController(ISaleRepository saleRepository) => _saleRepository = saleRepository;
        

        [HttpGet]
        public async Task<ActionResult<IEnumerable<Sale>>> GetAllSales()
        {
            var sales = await _saleRepository.GetAllAsync();
            return Ok(sales);
        }

        [HttpGet("{saleId}")]
        public async Task<ActionResult<Sale>> GetSaleById(int saleId)
        {
            var sale = await _saleRepository.GetByIdAsync(saleId);

            if (sale == null)
            {
                return NotFound();
            }

            return Ok(sale);
        }

        [HttpPost]
        public async Task<ActionResult<Sale>> CreateSale([FromBody] Sale sale)
        {
            await _saleRepository.AddAsync(sale);
            return CreatedAtAction(nameof(GetSaleById), new { saleId = sale.SaleId }, sale);
        }

        [HttpPut("{saleId}")]
        public async Task<IActionResult> UpdateSale(int saleId, [FromBody] Sale sale)
        {
            if (saleId != sale.SaleId)
            {
                return BadRequest();
            }

            await _saleRepository.UpdateAsync(sale);
            return NoContent();
        }

        [HttpDelete("{saleId}")]
        public async Task<IActionResult> DeleteSale(int saleId)
        {
            await _saleRepository.DeleteAsync(saleId);
            return NoContent();
        }
    }
}
