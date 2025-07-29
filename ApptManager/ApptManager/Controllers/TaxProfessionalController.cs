using ApptManager.DTOs;
using ApptManager.Models;
using ApptManager.Services;
using AutoMapper;
using Microsoft.AspNetCore.Mvc;
using Serilog;

namespace ApptManager.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class TaxProfessionalController : ControllerBase
    {
        private readonly ITaxProfessionalService _service;
        private readonly IMapper _mapper;

        public TaxProfessionalController(ITaxProfessionalService service, IMapper mapper)
        {
            _service = service;
            _mapper = mapper;
        }

        [HttpPost("create")]
        public async Task<IActionResult> Create([FromBody] CreateTaxProfessionalDto dto)
        {
            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            var taxPro = _mapper.Map<TaxProfessional>(dto);

            var result = await _service.CreateTaxProfessional(dto);
            Log.Information("TaxPro Created");

            return Ok(result);
        }

        [HttpGet("getAll")]
        public async Task<IActionResult> GetAll()
        {
            var taxPros = await _service.GetAllTaxProfessionals();
            var dtos = _mapper.Map<List<TaxProfessionalDto>>(taxPros);

            Log.Information("Fetched All TaxPros");
            return Ok(dtos);
        }

        [HttpGet("get/{id}")]
        public async Task<IActionResult> Get(int id)
        {
            var taxPro = await _service.GetTaxProfessionalById(id);
            if (taxPro == null)
                return NotFound("Tax Professional not found.");

            var dto = _mapper.Map<TaxProfessionalDto>(taxPro);

            Log.Information("Fetch taxpro by id");
            return Ok(dto);
        }

        [HttpPut("update/{id}")]
        public async Task<IActionResult> Update(int id, [FromBody] CreateTaxProfessionalDto dto)
        {
            var existing = await _service.GetTaxProfessionalById(id);
            if (existing == null)
                return NotFound("Tax Professional not found.");

            _mapper.Map(dto, existing);
            var result = await _service.UpdateTaxProfessional(id, dto);

            Log.Information("Tax Pro updated");
            return Ok(result);
        }

        [HttpDelete("delete/{id}")]
        public async Task<IActionResult> Delete(int id)
        {
            var result = await _service.RemoveTaxProfessional(id);
            Log.Information("Tax pro deleted");
            return Ok(result);
        }
    }
}
