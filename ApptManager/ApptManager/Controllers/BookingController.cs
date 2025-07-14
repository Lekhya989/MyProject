using ApptManager.Models;
using ApptManager.Repo.Services;
using ApptManager.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;

namespace ApptManager.Controllers
{

    [ApiController]
    [Route("api/[controller]")]
   
    
    public class BookingController : ControllerBase
    {
        private readonly IBookingService _bookingService;
        private readonly IUserService _userService;
        private readonly ISlotService _slotService;

        public BookingController(IBookingService bookingService, ISlotService slotService, IUserService userService)
        {
            _bookingService = bookingService;
            _userService = userService;
            _slotService = slotService;
        }

        [HttpGet("My")]
        public async Task<IActionResult> GetByBookings()
        {
            try
            {
                var userId = GetCurrentUserId();
                if (userId == null)
                    return Unauthorized();

                var bookings = await _bookingService.GetBookingsByUserIdAsync(userId.Value);
                return Ok(bookings);
            }
            catch (Exception ex)
            {
                Console.WriteLine("Exception in GetMyBookings: " + ex.Message);
                return StatusCode(500, "Something went wrong: " + ex.Message);
            }
        }



        [HttpGet("all")]
        public async Task<IActionResult> GetAllBookings()
        {
            var bookings = await _bookingService.GetAllBookingsAsync();
            return Ok(bookings);
        }


       
        [HttpPost]
        public async Task<IActionResult> CreateBooking([FromBody] CreateBookingDto dto)
        {
            var userId = GetCurrentUserId();
            if (userId == null)
                return Unauthorized();

            var user = await _userService.GetbyId(userId.Value);
            if (user == null) return NotFound("User not found");

            var slot = await _slotService.GetSlotByIdAsync(dto.SlotId);
            if (slot == null) return NotFound("Slot not found");

            var booking = new Bookings
            {
                SlotId = dto.SlotId,
                UserId = userId.Value,
                BookedOn = DateTime.UtcNow,
                IsApproved = false
            };

            try
            {
                var result = await _bookingService.CreateBookingAsync(booking, user, slot);

                return result > 0
                    ? Ok(new { message = "Booking successful" })
                    : StatusCode(500, "Booking failed");
            }
            catch (Exception ex)
            {
                Console.WriteLine("Booking error: " + ex.Message);
                return StatusCode(500, "Booking failed: " + ex.Message);
            }
        }



        [HttpPost("approve/{id}")]
        public async Task<IActionResult> ApproveBooking(int id)
        {
            var success = await _bookingService.ApproveBookingAsync(id);
            if (!success)
                return BadRequest("Approval failed or booking already approved.");

            return Ok("Booking approved.");
        }


        [HttpGet("Upcoming")]

        public async Task<IActionResult> GetUpcomingBookings()
        {
            var role = User.Claims.FirstOrDefault(c => c.Type.Contains("role"))?.Value;

            int? userId = null;
            if (role == "USER")
            {
                var userIdClaim = User.Claims.FirstOrDefault(c => c.Type == ClaimTypes.NameIdentifier || c.Type.EndsWith("nameidentifier"));
                if (userIdClaim != null && int.TryParse(userIdClaim.Value, out var id))
                {
                    userId = id;
                }
            }

            var result = await _bookingService.GetUpcomingBookingsAsync(userId);
            return Ok(result);
        }



        [HttpGet("Pending")]

        public async Task<IActionResult> GetPendingBookings()
        {
            var bookings = await _bookingService.GetAllBookingsAsync();
            var pending = bookings.Where(b => !b.IsApproved);
            return Ok(pending);
        }



        [HttpDelete("{id}")]
        public async Task<IActionResult> CancelBooking(int id)
        {
            var result = await _bookingService.DeleteBookingAsync(id);
            return result > 0 ? Ok(new { message = "Booking deleted" }) : NotFound();
        }

        private int? GetCurrentUserId()
        {
            var userIdClaim = User.Claims.FirstOrDefault(c => c.Type == ClaimTypes.NameIdentifier || c.Type.EndsWith("nameidentifier"));
            return int.TryParse(userIdClaim?.Value, out var id) ? id : null;
        }



    }
}
