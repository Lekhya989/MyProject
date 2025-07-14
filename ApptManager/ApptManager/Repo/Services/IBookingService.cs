using ApptManager.Models;

namespace ApptManager.Services
{
    public interface IBookingService
    {
        Task<IEnumerable<BookingDetailsDto>> GetBookingsByUserIdAsync(int userId);
        Task<IEnumerable<BookingDetailsDto>> GetAllBookingsAsync();

        Task<int> CreateBookingAsync(Bookings booking, UserObj user, Slot slot);
        Task<int> DeleteBookingAsync(int id);
        Task<Bookings> GetBookingByIdAsync(int id);
        Task<bool> UpdateBookingAsync(Bookings booking);
        Task<bool> ApproveBookingAsync(int bookingId);
        Task<IEnumerable<BookingDetailsDto>> GetUpcomingBookingsAsync(int? userId = null);

        Task SendRemainderEmailAsync();
    }
}
