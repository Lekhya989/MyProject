using ApptManager.DTOs;
using ApptManager.Models;
using ApptManager.Models.Data.WebApi.Models.Data;
using ApptManager.Repo.Services;
using ApptManager.Services;
using ApptManager.UnitOfWork;
using AutoMapper;

public class BookingService : IBookingService
{
    private readonly IUnitOfWork _unitOfWork;
    private readonly IMailService _mailService;
    private readonly DapperDBContext _dBContext;
    private readonly IMapper _mapper;

    public BookingService(
        IUnitOfWork unitOfWork,
        IMailService mailService,
        DapperDBContext dbContext,
        IMapper mapper)
    {
        _unitOfWork = unitOfWork;
        _mailService = mailService;
        _dBContext = dbContext;
        _mapper = mapper;
    }

    public Task<IEnumerable<BookingDetailsDto>> GetBookingsByUserIdAsync(int userId)
        => _unitOfWork.Bookings.GetBookingsByUserIdAsync(userId);

    public async Task<int> CreateBookingAsync(CreateBookingDto bookingDto, UserResponseDto userDto, SlotDto slotDto)
    {
        var booking = new Bookings
        {
            SlotId = bookingDto.SlotId,
            UserId = userDto.Id,
            BookedOn = DateTime.UtcNow,
            IsApproved = false
        };

        var result = await _unitOfWork.Bookings.CreateBookingAsync(booking);
        if (result > 0)
        {
            try
            {
                await _mailService.SendEmailAsync(new MailRequestDto
                {
                    ToEmail = userDto.Email,
                    Subject = "Booking Confirmation - Tax Pros",
                    Body = $"Hi {userDto.FirstName},<br/><br/>Your slot with Tax Professional is confirmed from <b>{slotDto.StartTime:hh:mm tt}</b> to <b>{slotDto.EndTime:hh:mm tt}</b>.<br/><br/>Thank you,<br/>Tax Pros Team"
                });

                await _mailService.SendEmailAsync(new MailRequestDto
                {
                    ToEmail = "lekhyachowdary9@gmail.com",
                    Subject = "New Slot Booking",
                    Body = $"User <b>{userDto.FirstName} {userDto.LastName}</b> has booked a slot with Tax Professional ID <b>{slotDto.TaxProfessionalId}</b> from <b>{slotDto.StartTime:hh:mm tt}</b> to <b>{slotDto.EndTime:hh:mm tt}</b>."
                });
            }
            catch (Exception ex)
            {
                Console.WriteLine("Email sending failed: " + ex.Message);
            }
        }

        return result;
    }

    public Task<IEnumerable<BookingDetailsDto>> GetAllBookingsAsync()
        => _unitOfWork.Bookings.GetAllBookingsAsync();

    public Task<int> DeleteBookingAsync(int id)
        => _unitOfWork.Bookings.DeleteBookingAsync(id);

    public async Task<BookingDetailsDto> GetBookingByIdAsync(int id)
    {
        var booking = await _unitOfWork.Bookings.GetBookingByIdAsync(id);
        return _mapper.Map<BookingDetailsDto>(booking);
    }

    public Task<IEnumerable<BookingDetailsDto>> GetUpcomingBookingsAsync(int? userId = null)
        => _unitOfWork.Bookings.GetUpcomingBookingsAsync(userId);

    public async Task<bool> UpdateBookingAsync(BookingDetailsDto bookingDto)
    {
        var booking = _mapper.Map<Bookings>(bookingDto);
        return await _unitOfWork.Bookings.UpdateBookingAsync(booking);
    }

    public async Task<bool> ApproveBookingAsync(int bookingId)
    {
        var booking = await _unitOfWork.Bookings.GetBookingByIdAsync(bookingId);
        if (booking == null || booking.IsApproved) return false;

        booking.IsApproved = true;
        var updateResult = await _unitOfWork.Bookings.UpdateBookingAsync(booking);

        if (updateResult)
        {
            var user = await _unitOfWork.Users.GetByIdAsync(booking.UserId);
            var slot = await _unitOfWork.Slots.GetByIdAsync(booking.SlotId);

            var email = new MailRequestDto
            {
                ToEmail = user.Email,
                Subject = "Booking Approved - Tax Pros",
                Body = $"Hello {user.FirstName},<br/><br/>Your booking for the slot from <b>{slot.StartTime:hh:mm tt}</b> to <b>{slot.EndTime:hh:mm tt}</b> has been approved.<br/><br/>Thank you,<br/>Tax Pros Team"
            };

            await _mailService.SendEmailAsync(email);
        }

        return updateResult;
    }

    public async Task SendRemainderEmailAsync()
    {
        var upcomingBookings = await _unitOfWork.Bookings.GetUpcomingBookingsAsync();

        var now = DateTime.UtcNow;
        var inOneHour = now.AddHours(1);

        var bookingsToRemind = upcomingBookings
            .Where(b => b.StartTime > now && b.StartTime <= inOneHour && b.IsApproved)
            .ToList();

        foreach (var booking in bookingsToRemind)
        {
            var user = await _unitOfWork.Users.GetByIdAsync(booking.UserId);
            if (user == null) continue;

            try
            {
                await _mailService.SendEmailAsync(new MailRequestDto
                {
                    ToEmail = user.Email,
                    Subject = "Reminder: Upcoming Appointment",
                    Body = $@"
                        Hi {user.FirstName},<br/><br/>
                        This is a reminder for your upcoming booking scheduled at 
                        <b>{booking.StartTime:hh:mm tt}</b> to <b>{booking.EndTime:hh:mm tt}</b> today.<br/><br/>
                        Regards,<br/>Tax Pros Team"
                });
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Failed to send reminder to {user.Email}: {ex.Message}");
            }
        }
    }
}
