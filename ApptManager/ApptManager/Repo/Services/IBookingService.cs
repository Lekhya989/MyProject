using ApptManager.DTOs;
using ApptManager.Models;

namespace ApptManager.Services
{
    public interface IBookingService
    {
        Task<IEnumerable<BookingDetailsDto>> GetBookingsByUserIdAsync(int userId);
        Task<IEnumerable<BookingDetailsDto>> GetAllBookingsAsync();

        Task<int> CreateBookingAsync(CreateBookingDto bookingDto, UserResponseDto userDto, SlotDto slotDto);
        Task<int> DeleteBookingAsync(int id);
        Task<BookingDetailsDto> GetBookingByIdAsync(int id);
        Task<bool> UpdateBookingAsync(BookingDetailsDto bookingDto);
        Task<bool> ApproveBookingAsync(int bookingId);
        Task<IEnumerable<BookingDetailsDto>> GetUpcomingBookingsAsync(int? userId = null);

        Task SendRemainderEmailAsync();
    }
}
