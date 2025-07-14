// Services/BookingService.cs
using ApptManager.Models;
using ApptManager.Models.Data.WebApi.Models.Data;
using ApptManager.Repo;
using ApptManager.Repo.Services;
using Dapper;
using Microsoft.EntityFrameworkCore;

namespace ApptManager.Services
{
    public class BookingService : IBookingService
    {
        private readonly IBookingRepo _bookingRepo;
        private readonly IMailService _mailService;
        private readonly IUserRepo _userRepo;
        private readonly ISlotRepo _slotRepo;
        private readonly DapperDBContext _dBContext;
       

        public BookingService(IBookingRepo bookingRepo, IMailService mailService, IUserRepo userRepo, DapperDBContext dbContext,
    ISlotRepo slotRepo)
        {
            _bookingRepo = bookingRepo;
            _mailService = mailService;
            _userRepo = userRepo;
            _slotRepo = slotRepo;
            _dBContext = dbContext;
        }

        public async Task<IEnumerable<BookingDetailsDto>> GetBookingsByUserIdAsync(int userId)
        {
            return await _bookingRepo.GetBookingsByUserIdAsync(userId);
        }

        public async Task<int> CreateBookingAsync(Bookings booking, UserObj user, Slot slot)
        {
            booking.BookedOn = DateTime.UtcNow;
            var result =await _bookingRepo.CreateBookingAsync(booking);
            if (result > 0)
            {
                try
                {
                    Console.WriteLine(" Preparing to send user email...");
                    var userMail = new MailRequest
                    {
                        ToEmail = user.Email,
                        Subject = "Booking Confirmation - Tax Pros",
                        Body = $"Hi {user.FirstName},<br/><br/>Your slot with Tax Professional is confirmed from <b>{slot.StartTime:hh:mm tt}</b> to <b>{slot.EndTime:hh:mm tt}</b>.<br/><br/>Thank you,<br/>Tax Pros Team"
                    };
                    await _mailService.SendEmailAsync(userMail);

                    Console.WriteLine(" User email sent!");

                    var adminMail = new MailRequest
                    {
                        ToEmail = "lekhyachowdary9@gmail.com", 
                        Subject = "New Slot Booking",
                        Body = $"User <b>{user.FirstName} {user.LastName}</b> has booked a slot with Tax Professional ID <b>{slot.TaxProfessionalId}</b> from <b>{slot.StartTime:hh:mm tt}</b> to <b>{slot.EndTime:hh:mm tt}</b>."
                    };
                    await _mailService.SendEmailAsync(adminMail);
                    Console.WriteLine("Admin email sent!");
                }
                catch (Exception emailEx)
                {
                    Console.WriteLine(" Email sending failed: " + emailEx.Message);
                }
            }
            return result;
        }


        public async Task<IEnumerable<BookingDetailsDto>> GetAllBookingsAsync()
        {
            return await _bookingRepo.GetAllBookingsAsync();
        }
        public async Task<int> DeleteBookingAsync(int id)
        {
            return await _bookingRepo.DeleteBookingAsync(id);
        }

        public async Task<Bookings> GetBookingByIdAsync(int id)
        {
            return await _bookingRepo.GetBookingByIdAsync(id);
        }

        public async Task<IEnumerable<BookingDetailsDto>> GetUpcomingBookingsAsync(int? userId = null)
        {
            return await _bookingRepo.GetUpcomingBookingsAsync(userId);
        }


        public async Task<bool> UpdateBookingAsync(Bookings booking)
        {
            var sql = "UPDATE Bookings SET IsApproved = @IsApproved WHERE Id - @Id";
            using var conn = _dBContext.CreateConnection();
            var result = await conn.ExecuteAsync(sql, booking);
            return true;
        }

        public async Task<bool> ApproveBookingAsync(int bookingId)
        {
            var booking = await _bookingRepo.GetBookingByIdAsync(bookingId);
            if (booking == null || booking.IsApproved) return false;

            booking.IsApproved = true;
            var updateResult = await _bookingRepo.UpdateBookingAsync(booking);
            if (updateResult)
            {
                var user = await _userRepo.GetbyId(booking.UserId);
                var slot = await _slotRepo.GetById(booking.SlotId);

                var email = new MailRequest
                {
                    ToEmail = user.Email,
                    Subject = "Booking Approved - Tax Pros",
                    Body = $"Hello {user.FirstName},<br/><br/>" +
                   $"Your booking for the slot from <b>{slot.StartTime:hh:mm tt}</b> to <b>{slot.EndTime:hh:mm tt}</b> has been approved.<br/><br/>" +
                   $"Thank you,<br/>Tax Pros Team"
                };

                await _mailService.SendEmailAsync(email);
            }
            return updateResult;
        }


        public async Task SendRemainderEmailAsync()
        {
            var allUpcomingBookings = await _bookingRepo.GetUpcomingBookingsAsync();

            var now = DateTime.UtcNow;
            var inOneHour = now.AddHours(1);

            var bookingsToRemind = allUpcomingBookings
                .Where(b => b.StartTime > now && b.StartTime <= inOneHour && b.IsApproved)
                .ToList();

            foreach (var booking in bookingsToRemind)
            {
                var user = await _userRepo.GetbyId(booking.UserId);
                if (user == null) continue;

                var email = new MailRequest
                {
                    ToEmail = user.Email,
                    Subject = "Reminder: Upcoming Appointment",
                    Body = $@"
                Hi {user.FirstName},<br/><br/>
                This is a reminder for your upcoming booking scheduled at 
                <b>{booking.StartTime:hh:mm tt}</b> to <b>{booking.EndTime:hh:mm tt}</b> today.<br/><br/>
                Regards,<br/>Tax Pros Team"
                };

                try
                {
                    await _mailService.SendEmailAsync(email);
                }
                catch (Exception ex)
                {
                    Console.WriteLine($"Failed to send reminder email to {user.Email}: {ex.Message}");
                }
            }
        }

    }
}
