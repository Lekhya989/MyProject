using ApptManager.Models;

namespace ApptManager.Services
{
   
        public interface ISlotService
        {
            Task GenerateSlots(int taxProfessionalId, DateTime start, DateTime end);
            Task<List<Slot>> GetSlotsByTaxPro(int taxProfessionalId);
            Task<string> UpdateSlot(Slot slot);
            Task<string> DeleteSlot(int id);

            Task<Slot> GetSlotByIdAsync(int slotId);
        }
    
}
