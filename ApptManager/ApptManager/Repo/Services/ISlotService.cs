using ApptManager.DTOs;
using ApptManager.Models;

namespace ApptManager.Services
{
   
        public interface ISlotService
        {
        Task GenerateSlots(SlotGenerationRequestDto slotGenerationRequestDto);
        Task<List<SlotDto>> GetSlotsByTaxPro(int taxProfessionalId);
            Task<string> UpdateSlot(int id, SlotUpdateDto slotUpdateDto);
            Task<string> DeleteSlot(int id);

            Task<SlotDto> GetSlotByIdAsync(int slotId);
        }
    
}
