using ApptManager.Models;

namespace ApptManager.Repo
{
    public interface ISlotRepo
    {
        Task GenerateSlots(int taxProfessionalId, DateTime startTime, DateTime endTime);
        Task<List<Slot>> GetSlotsByTaxPro(int taxProfessionalId);
        Task<string> UpdateSlot(Slot slot);
        Task<string> DeleteSlot(int id);

        Task<Slot> GetById(int id); 
    }
}

