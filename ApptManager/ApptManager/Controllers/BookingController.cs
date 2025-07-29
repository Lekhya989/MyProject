using ApptManager.DTOs;
using ApptManager.Models;
using ApptManager.Repo.Services;
using ApptManager.Services;
using AutoMapper;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Serilog;
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
        private readonly IMapper _mapper;

        public BookingController(
            IBookingService bookingService,
            ISlotService slotService,
            IUserService userService,
            IMapper mapper)
        {
            _bookingService = bookingService;
            _userService = userService;
            _slotService = slotService;
            _mapper = mapper;
        }

        [HttpGet("My")]
        public async Task<IActionResult> GetByBookings()
        {
            var userId = GetCurrentUserId();
            if (userId == null)
                return Unauthorized();

            var bookings = await _bookingService.GetBookingsByUserIdAsync(userId.Value);
            var dto = _mapper.Map<IEnumerable<BookingDetailsDto>>(bookings);
            Log.Information("Fetched bookings");
            return Ok(dto);
        }

        [HttpGet("all")]
        public async Task<IActionResult> GetAllBookings()
        {
            var bookings = await _bookingService.GetAllBookingsAsync();
            var dto = _mapper.Map<IEnumerable<BookingDetailsDto>>(bookings);
            Log.Information("Fetched all bookings");
            return Ok(dto);
        }

        [HttpPost]
        public async Task<IActionResult> CreateBooking([FromBody] CreateBookingDto dto)
        {
            var userId = GetCurrentUserId();
            if (userId == null)
                return Unauthorized();

            var user = await _userService.GetById(userId.Value);
            if (user == null) return NotFound("User not found");

            var slot = await _slotService.GetSlotByIdAsync(dto.SlotId);
            if (slot == null) return NotFound("Slot not found");

            // Map User and Slot to their corresponding DTOs
            var userDto = _mapper.Map<UserResponseDto>(user);
            var slotDto = _mapper.Map<SlotDto>(slot);

            try
            {
                var result = await _bookingService.CreateBookingAsync(dto, userDto, slotDto);
                Log.Information("Created bookings");

                return result > 0
                    ? Ok(new { message = "Booking successful" })
                    : StatusCode(500, "Booking failed");
            }
            catch (Exception ex)
            {
                Log.Error("Booking error: {Message}", ex.Message);
                return StatusCode(500, "Booking failed: " + ex.Message);
            }
        }


        [HttpPost("approve/{id}")]
        public async Task<IActionResult> ApproveBooking(int id)
        {
            var success = await _bookingService.ApproveBookingAsync(id);
            if (!success)
                return BadRequest("Approval failed or booking already approved.");

            Log.Information("Approved bookings");
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
            var dto = _mapper.Map<IEnumerable<BookingDetailsDto>>(result);
            Log.Information("Showing upcoming bookings");
            return Ok(dto);
        }

        [HttpGet("Pending")]
        public async Task<IActionResult> GetPendingBookings()
        {
            var bookings = await _bookingService.GetAllBookingsAsync();
            var pending = bookings.Where(b => !b.IsApproved);
            var dto = _mapper.Map<IEnumerable<BookingDetailsDto>>(pending);

            Log.Information("Get pending booking confirmations");
            return Ok(dto);
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> CancelBooking(int id)
        {
            var result = await _bookingService.DeleteBookingAsync(id);
            Log.Information("Deleted booking by id");

            return result > 0
                ? Ok(new { message = "Booking deleted" })
                : NotFound("Booking not found");
        }

        private int? GetCurrentUserId()
        {
            var userIdClaim = User.Claims.FirstOrDefault(c =>
                c.Type == ClaimTypes.NameIdentifier ||
                c.Type.EndsWith("nameidentifier"));

            return int.TryParse(userIdClaim?.Value, out var id) ? id : null;
        }
    }
}
