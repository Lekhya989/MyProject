using ApptManager.Models;
using ApptManager.Repo.Services;
using ApptManager.Services;
using Microsoft.AspNetCore.Mvc;

namespace ApptManager.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class SlotController : ControllerBase
    {
        private readonly ISlotService _service;

        public SlotController(ISlotService service)
        {
            _service = service;
        }

        [HttpPost("generate")]
        public async Task<IActionResult> Generate([FromBody] SlotGenerationRequest request)
        {
            await _service.GenerateSlots(request.TaxProfessionalId, request.StartTime, request.EndTime);
            return Ok("Slots generated");
        }

        [HttpGet("byTaxPro/{id}")]
        public async Task<IActionResult> GetByTaxPro(int id)
        {
            var slots = await _service.GetSlotsByTaxPro(id);
            return Ok(slots);
        }

        [HttpPut("update/{id}")]
        public async Task<IActionResult> Update(int id, [FromBody] Slot slot)
        {
            slot.Id = id;
            var result = await _service.UpdateSlot(slot);
            return Ok(result);
        }

        [HttpDelete("delete/{id}")]
        public async Task<IActionResult> Delete(int id)
        {
            var result = await _service.DeleteSlot(id);
            return Ok(result);
        }
    }
}
