using ApptManager.DTOs;
using ApptManager.Models;
using ApptManager.Repo;
using ApptManager.UnitOfWork;
using AutoMapper;

namespace ApptManager.Services
{
    public class SlotService : ISlotService
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IMapper _mapper;

        public SlotService(IUnitOfWork unitOfWork, IMapper mapper)
        {
            _unitOfWork = unitOfWork;
            _mapper = mapper;
        }

        public async Task GenerateSlots(SlotGenerationRequestDto slotGenerationRequestDto)
        {
            await _unitOfWork.Slots.GenerateSlots(slotGenerationRequestDto);
        }

        public async Task<List<SlotDto>> GetSlotsByTaxPro(int id)
        {
            var slots = await _unitOfWork.Slots.GetSlotsByTaxPro(id);
            return _mapper.Map<List<SlotDto>>(slots);
        }

        public async Task<string> UpdateSlot(int id, SlotUpdateDto dto)
        {
            var slot = _mapper.Map<Slot>(dto);
            slot.Id = id;
            return await _unitOfWork.Slots.UpdateSlot(slot);
        }

        public Task<string> DeleteSlot(int id)
        {
            return _unitOfWork.Slots.DeleteSlot(id);
        }

        public async Task<SlotDto> GetSlotByIdAsync(int slotId)
        {
            var slot = await _unitOfWork.Slots.GetByIdAsync(slotId);
            return _mapper.Map<SlotDto>(slot);
        }
    }
}
