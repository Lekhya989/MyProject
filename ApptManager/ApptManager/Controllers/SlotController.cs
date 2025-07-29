using ApptManager.DTOs;
using ApptManager.Models;
using ApptManager.Repo.Services;
using ApptManager.Services;
using AutoMapper;
using Microsoft.AspNetCore.Mvc;
using Serilog;

namespace ApptManager.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class SlotController : ControllerBase
    {
        private readonly ISlotService _service;
        private readonly IMapper _mapper;

        public SlotController(ISlotService service, IMapper mapper)
        {
            _service = service;
            _mapper = mapper;
        }

        [HttpPost("generate")]
        public async Task<IActionResult> Generate([FromBody] SlotGenerationRequestDto request)
        {
            await _service.GenerateSlots(request);
            Log.Information("Slot Generated");
            return Ok("Slots generated");
        }

        [HttpGet("byTaxPro/{id}")]
        public async Task<IActionResult> GetByTaxPro(int id)
        {
            var slots = await _service.GetSlotsByTaxPro(id);
            var slotDtos = _mapper.Map<List<SlotDto>>(slots);

            Log.Information("Fetched Slot by id");
            return Ok(slotDtos);
        }

        [HttpPut("update/{id}")]
        public async Task<IActionResult> Update(int id, [FromBody] SlotUpdateDto dto)
        {
            var slot = await _service.GetSlotByIdAsync(id);
            if (slot == null)
                return NotFound("Slot not found");

            _mapper.Map(dto, slot);
            var result = await _service.UpdateSlot(id, dto);

            Log.Information("Updated Slot");
            return Ok(result);
        }

        [HttpDelete("delete/{id}")]
        public async Task<IActionResult> Delete(int id)
        {
            var result = await _service.DeleteSlot(id);
            Log.Information("Deleted Slot");
            return Ok(result);
        }
    }
}
