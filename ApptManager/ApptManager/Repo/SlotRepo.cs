using ApptManager.Models;
using ApptManager.Models.Data.WebApi.Models.Data;
using ApptManager.Repo.Services;
using Dapper;
using System.Data;

namespace ApptManager.Repo
{
    public class SlotRepo : ISlotRepo
    {
        private readonly DapperDBContext _dbContext;

        public SlotRepo(DapperDBContext dbContext)
        {
            _dbContext = dbContext;
        }

        public async Task<Slot> GetById(int id)
        {
            using var conn = _dbContext.CreateConnection();
            var sql = "SELECT * FROM Slots WHERE Id = @Id";
            return await conn.QueryFirstOrDefaultAsync<Slot>(sql, new { Id = id });
        }

        public async Task GenerateSlots(int taxProfessionalId, DateTime startTime, DateTime endTime)
        {
            var slots = new List<Slot>();

            while (startTime < endTime)
            {
                slots.Add(new Slot
                {
                    TaxProfessionalId = taxProfessionalId,
                    StartTime = startTime,
                    EndTime = startTime.AddHours(1),
                    IsBooked = false,
                    CreatedOn = DateTime.UtcNow
                });

                startTime = startTime.AddHours(1);
            }

            using var connection = _dbContext.CreateConnection();

            foreach (var slot in slots)
            {
                string query = @"
                    INSERT INTO Slots (TaxProfessionalId, StartTime, EndTime, IsBooked, CreatedOn)
                    VALUES (@TaxProfessionalId, @StartTime, @EndTime, @IsBooked, @CreatedOn)";
                await connection.ExecuteAsync(query, slot);
            }
        }

        public async Task<List<Slot>> GetSlotsByTaxPro(int taxProfessionalId)
        {
            string query = "SELECT * FROM Slots WHERE TaxProfessionalId = @TaxProfessionalId";

            using var connection = _dbContext.CreateConnection();
            var slots = await connection.QueryAsync<Slot>(query, new { TaxProfessionalId = taxProfessionalId });

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

            using var connection = _dbContext.CreateConnection();
            await connection.ExecuteAsync(query, slot);

            return "Slot updated successfully.";
        }

        public async Task<string> DeleteSlot(int id)
        {
            string query = "DELETE FROM Slots WHERE Id = @Id";

            using var connection = _dbContext.CreateConnection();
            await connection.ExecuteAsync(query, new { Id = id });

            return "Slot deleted successfully.";
        }

        public async Task<Slot> GetslotByIdAsync(int id)
        {
            using var conn = _dbContext.CreateConnection();
            var sql = "SELECT * FROM Slots WHERE Id = @Id";
            return await conn.QueryFirstOrDefaultAsync<Slot>(sql, new { Id = id });
        }

    }
}