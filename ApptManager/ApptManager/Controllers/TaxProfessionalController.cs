using ApptManager.Models;
using ApptManager.Services;
using Microsoft.AspNetCore.Mvc;

namespace ApptManager.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class TaxProfessionalController : ControllerBase
    {
        private readonly ITaxProfessionalService _service;

        public TaxProfessionalController(ITaxProfessionalService service)
        {
            _service = service;
        }

        [HttpPost("create")]
        public async Task<IActionResult> Create([FromBody] TaxProfessional taxPro)
        {
            var result = await _service.CreateTaxProfessional(taxPro);
            return Ok(result);
        }

        [HttpGet("getAll")]
        public async Task<IActionResult> GetAll()
        {
            var result = await _service.GetAllTaxProfessionals();
            return Ok(result);
        }

        [HttpGet("get/{id}")]
        public async Task<IActionResult> Get(int id)
        {
            var result = await _service.GetTaxProfessionalById(id);
            if (result == null)
                return NotFound();
            return Ok(result);
        }

        [HttpPut("update/{id}")]
        public async Task<IActionResult> Update(int id, [FromBody] TaxProfessional taxPro)
        {
            var result = await _service.UpdateTaxProfessional(id, taxPro);
            return Ok(result);
        }

        [HttpDelete("delete/{id}")]
        public async Task<IActionResult> Delete(int id)
        {
            var result = await _service.RemoveTaxProfessional(id);
            return Ok(result);
        }
    }
}