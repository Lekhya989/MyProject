using ApptManager.DTOs;
using AutoMapper;

namespace ApptManager.Mapper
{
    public class SlotMapper
    {
        private readonly IMapper _mapper;

        public SlotMapper(IMapper mapper)
        {
            _mapper = mapper;
            
        }

        public SlotGenerationRequestDto ConvertSlotGenerationRequestDtoTOSlot(Slot slot)
        {
            return _mapper.Map<SlotGenerationRequestDto>(slot);
        }

        public Slot ConvertSlotTOSlotGenerationRequestDto(SlotGenerationRequestDto slotGenerationRequestDto)
        {
            return _mapper.Map<Slot>(slotGenerationRequestDto);
        }
    }
}
