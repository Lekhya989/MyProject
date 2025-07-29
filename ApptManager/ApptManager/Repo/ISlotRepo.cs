using ApptManager.DTOs;
using ApptManager.Models;

namespace ApptManager.Repo
{
    public interface ISlotRepo : IGenericRepository<Slot>
    {
        Task GenerateSlots(SlotGenerationRequestDto slotGenerationRequestDto);
        Task<List<Slot>> GetSlotsByTaxPro(int taxProfessionalId);
        Task<string> UpdateSlot(Slot slot);
        Task<string> DeleteSlot(int id);
        Task<Slot> GetslotByIdAsync(int id); // Optional if you want a named custom version
    }
}
