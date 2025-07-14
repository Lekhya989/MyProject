using ApptManager.Models;
using ApptManager.Models.Data.WebApi.Models.Data;
using Dapper;
using Microsoft.EntityFrameworkCore;
using System.Data;

namespace ApptManager.Repo
{
    public class BookingRepo : IBookingRepo
    {
        private readonly DapperDBContext _dbContext;

        public BookingRepo(DapperDBContext dbContext)
        {
            _dbContext = dbContext;
        }

        public async Task<IEnumerable<BookingDetailsDto>> GetBookingDetailsByUserIdAsync(int userId)
        {
            using var conn = _dbContext.CreateConnection();

            var sql = @"
        SELECT 
            b.Id, b.BookedOn,
            s.StartTime, s.EndTime,b.IsApproved,
            (tp.FirstName + ' ' + tp.LastName) AS TaxProfessionalName
        FROM Bookings b
        JOIN Slots s ON b.SlotId = s.Id
        JOIN TaxProfessionals tp ON s.TaxProfessionalId = tp.Id
        WHERE b.UserId = @UserId";

            
                var result = await conn.QueryAsync<BookingDetailsDto>(sql, new { UserId = userId });
                Console.WriteLine("Fetched bookings: " + result.Count());
                return result;
            
        }

        public async Task<IEnumerable<BookingDetailsDto>> GetAllBookingsAsync()
        {
            using var conn = _dbContext.CreateConnection();

            var sql = @"
        SELECT 
            b.Id, b.UserId, b.SlotId, b.BookedOn, b.IsApproved, 
            u.FirstName + ' ' + u.LastName AS UserName,
            s.StartTime, s.EndTime,
            CONCAT(tp.FirstName, ' ', tp.LastName) AS TaxProfessionalName
        FROM Bookings b
        JOIN Users u ON b.UserId = u.Id
        JOIN Slots s ON b.SlotId = s.Id
        JOIN TaxProfessionals tp ON s.TaxProfessionalId = tp.Id";

            return await conn.QueryAsync<BookingDetailsDto>(sql);
        }


        public async Task<int> CreateBookingAsync(Bookings booking)
        {
            using var conn = _dbContext.CreateConnection();
            conn.Open();
            using var transaction = conn.BeginTransaction();

            try
            {
                var insertSql = @"INSERT INTO Bookings (UserId, SlotId, BookedOn)
                          VALUES (@UserId, @SlotId, @BookedOn)";
                await conn.ExecuteAsync(insertSql, booking, transaction);

                var updateSlotSql = @"UPDATE Slots SET IsBooked = 1 WHERE Id = @SlotId";
                await conn.ExecuteAsync(updateSlotSql, new { booking.SlotId }, transaction);

                transaction.Commit();
                return 1;
            }
            catch (Exception ex)
            {
                transaction.Rollback();
                Console.WriteLine("Booking creation failed:");
                Console.WriteLine("Message: " + ex.Message);
                Console.WriteLine("StackTrace: " + ex.StackTrace);
                throw;
            }
        }

        public async Task<int> DeleteBookingAsync(int id)
        {
            using var conn = _dbContext.CreateConnection();
            var sql = "DELETE FROM Bookings WHERE Id = @Id";
            return await conn.ExecuteAsync(sql, new { Id = id });
        }

        public Task<IEnumerable<BookingDetailsDto>> GetBookingsByUserIdAsync(int userId)
        {
            return GetBookingDetailsByUserIdAsync(userId);
        }

        public async Task<Bookings> GetBookingByIdAsync(int id)
        {
            using var conn = _dbContext.CreateConnection();
            var sql = "SELECT * FROM Bookings WHERE Id = @Id";
            return await conn.QueryFirstOrDefaultAsync<Bookings>(sql, new { Id = id });

        }

        public async Task<bool> UpdateBookingAsync(Bookings booking)
        {
            using var conn = _dbContext.CreateConnection();
            var sql = @"UPDATE Bookings 
                SET SlotId = @SlotId, 
                    UserId = @UserId, 
                    BookedOn = @BookedOn, 
                    IsApproved = @IsApproved 
                WHERE Id = @Id";

            var rowsAffected = await conn.ExecuteAsync(sql, booking);
            return rowsAffected > 0;
        }

        public async Task<IEnumerable<BookingDetailsDto>> GetUpcomingBookingsAsync(int? userId = null)
        {
            using var conn = _dbContext.CreateConnection();
            var now = DateTime.UtcNow;

            var sql = @"
        SELECT 
            b.Id, b.UserId, b.SlotId, b.BookedOn, b.IsApproved,
            u.FirstName + ' ' + u.LastName AS UserName,
            s.StartTime, s.EndTime,
            CONCAT(tp.FirstName, ' ', tp.LastName) AS TaxProfessionalName
        FROM Bookings b
        JOIN Users u ON b.UserId = u.Id
        JOIN Slots s ON b.SlotId = s.Id
        JOIN TaxProfessionals tp ON s.TaxProfessionalId = tp.Id
        WHERE s.StartTime > @Now AND s.EndTime>@Now";

            if (userId.HasValue)
            {
                sql += " AND b.UserId = @UserId";
            }

            return await conn.QueryAsync<BookingDetailsDto>(sql, new { Now = now, UserId = userId });
        }


    }
}