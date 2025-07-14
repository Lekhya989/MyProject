using ApptManager.Models;
using Microsoft.EntityFrameworkCore;
using ApptManager.Services;

namespace ApptManager.Repo.Services
{
    public class SlotService : ISlotService
    {
        private readonly ISlotRepo _repo;
        public SlotService(ISlotRepo repo )
        {
            _repo = repo; 
        }
        public Task GenerateSlots(int taxProfessionalId, DateTime start, DateTime end)
            => _repo.GenerateSlots(taxProfessionalId, start, end);
        public Task<List<Slot>> GetSlotsByTaxPro(int id)
            => _repo.GetSlotsByTaxPro(id);
        public Task<string> UpdateSlot(Slot slot)
            => _repo.UpdateSlot(slot);
        public Task<string> DeleteSlot(int id)
            => _repo.DeleteSlot(id);
        public Task<Slot> GetSlotByIdAsync(int slotId)
        {
            return _repo.GetById(slotId);
        }
    }
}
