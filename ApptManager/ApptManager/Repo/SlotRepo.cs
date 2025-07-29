using ApptManager.DTOs;
using ApptManager.Mapper;
using ApptManager.Models;
using Dapper;
using System.Data;

namespace ApptManager.Repo
{
    public class SlotRepo : GenericRepository<Slot>, ISlotRepo
    {
        private readonly IDbConnection _db;
        private readonly SlotMapper _mapper;

        public SlotRepo(IDbConnection db, SlotMapper mapper) : base(db)
        {
            _db = db;
            _mapper = mapper;
        }

        public async Task GenerateSlots(SlotGenerationRequestDto slotGenerationRequestDto)
        {
            var slots = new List<Slot>();
            var startTime = slotGenerationRequestDto.StartTime;
            var endTime = slotGenerationRequestDto.EndTime;

            while (startTime < endTime)
            {
                var baseSlot = _mapper.ConvertSlotTOSlotGenerationRequestDto(slotGenerationRequestDto);

                var slot = new Slot
                {
                    TaxProfessionalId = baseSlot.TaxProfessionalId,
                    StartTime = startTime,
                    EndTime = startTime.AddHours(1),
                    IsBooked = false,
                    CreatedOn = DateTime.UtcNow
                };

                slots.Add(slot);
                startTime = startTime.AddHours(1);
            }

            foreach (var slot in slots)
            {
                string query = @"
                    INSERT INTO Slots (TaxProfessionalId, StartTime, EndTime, IsBooked, CreatedOn)
                    VALUES (@TaxProfessionalId, @StartTime, @EndTime, @IsBooked, @CreatedOn)";

                await _db.ExecuteAsync(query, slot);
            }
        }

        public async Task<List<Slot>> GetSlotsByTaxPro(int taxProfessionalId)
        {
            string query = "SELECT * FROM Slots WHERE TaxProfessionalId = @TaxProfessionalId";
            var slots = await _db.QueryAsync<Slot>(query, new { TaxProfessionalId = taxProfessionalId });
            return slots.ToList();
        }

        public async Task<string> UpdateSlot(Slot slot)
        {
            string query = @"
                UPDATE Slots 
                SET StartTime = @StartTime,
                    EndTime = @EndTime,
                    IsBooked = @IsBooked,
                    UpdatedOn = @UpdatedOn
                WHERE Id = @Id";

            slot.UpdatedOn = DateTime.UtcNow;
            await _db.ExecuteAsync(query, slot);

            return "Slot updated successfully.";
        }

        public async Task<string> DeleteSlot(int id)
        {
            string query = "DELETE FROM Slots WHERE Id = @Id";
            await _db.ExecuteAsync(query, new { Id = id });
            return "Slot deleted successfully.";
        }

        // Optional custom method if you want a named version (redundant with GetByIdAsync from GenericRepo)
        public async Task<Slot> GetslotByIdAsync(int id)
        {
            return await _db.QueryFirstOrDefaultAsync<Slot>("SELECT * FROM Slots WHERE Id = @Id", new { Id = id });
        }
    }
}
