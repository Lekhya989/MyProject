using ApptManager.DTOs;
using ApptManager.Models;

namespace ApptManager.Repo
{
    public interface IBookingRepo : IGenericRepository<Bookings>
    {
        Task<IEnumerable<BookingDetailsDto>> GetBookingsByUserIdAsync(int userId);
        Task<IEnumerable<BookingDetailsDto>> GetAllBookingsAsync();
        Task<int> CreateBookingAsync(Bookings booking);
        Task<int> DeleteBookingAsync(int id);
        Task<Bookings> GetBookingByIdAsync(int id);
        Task<bool> UpdateBookingAsync(Bookings booking);
        Task<IEnumerable<BookingDetailsDto>> GetUpcomingBookingsAsync(int? userId = null);
    }
}
