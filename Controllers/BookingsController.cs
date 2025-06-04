using HotelBookingApi.Data;
using HotelBookingApi.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using FluentValidation;

namespace HotelBookingApi.Controllers;

[ApiController]
[Route("api/[controller]")]
public class BookingsController : ControllerBase
{
    private readonly AppDbContext _context;

    public BookingsController(AppDbContext context)
    {
        _context = context;
    }

    [HttpGet]
    public async Task<IActionResult> GetBookings()
    {
        var bookings = await _context.Bookings
            .Include(b => b.Room)
            .ToListAsync();
        return Ok(bookings);
    }

    [HttpGet("available")]
    public async Task<IActionResult> GetAvailableRooms([FromQuery] DateTime from, [FromQuery] DateTime to)
    {
        var utcFrom = DateTime.SpecifyKind(from, DateTimeKind.Utc);
        var utcTo = DateTime.SpecifyKind(to, DateTimeKind.Utc);

        var availableRooms = await _context.Rooms
            .Where(r => !_context.Bookings.Any(b =>
                b.RoomId == r.Id && b.StartDate < utcTo && b.EndDate > utcFrom))
            .ToListAsync();

        return Ok(availableRooms);
    }

[HttpPost]
public async Task<IActionResult> CreateBooking([FromBody] Booking booking, [FromServices] IValidator<Booking> validator)
{
    var validationResult = await validator.ValidateAsync(booking);
    if (!validationResult.IsValid)
        return BadRequest(validationResult.Errors);

    booking.StartDate = DateTime.SpecifyKind(booking.StartDate, DateTimeKind.Utc);
    booking.EndDate = DateTime.SpecifyKind(booking.EndDate, DateTimeKind.Utc);

    var overlap = await _context.Bookings.AnyAsync(b =>
        b.RoomId == booking.RoomId &&
        b.StartDate < booking.EndDate &&
        b.EndDate > booking.StartDate);

    if (overlap)
        return BadRequest("Номер уже забронирован.");

    _context.Bookings.Add(booking);
    await _context.SaveChangesAsync();

    return CreatedAtAction(nameof(GetBooking), new { id = booking.Id }, booking);
}

    [HttpGet("{id}")]
    public async Task<IActionResult> GetBooking(int id)
    {
        var booking = await _context.Bookings
            .Include(b => b.Room)
            .FirstOrDefaultAsync(b => b.Id == id);

        if (booking == null)
            return NotFound();

        return Ok(booking);
    }

    [HttpPut("{id}")]
    public async Task<IActionResult> UpdateBooking(int id, [FromBody] Booking updated)
    {
        if (id != updated.Id)
            return BadRequest();

        var booking = await _context.Bookings.FindAsync(id);
        if (booking == null)
            return NotFound();

        booking.StartDate = DateTime.SpecifyKind(updated.StartDate, DateTimeKind.Utc);
        booking.EndDate = DateTime.SpecifyKind(updated.EndDate, DateTimeKind.Utc);
        booking.CustomerName = updated.CustomerName;

        await _context.SaveChangesAsync();
        return NoContent();
    }

    [HttpDelete("{id}")]
    public async Task<IActionResult> DeleteBooking(int id)
    {
        var booking = await _context.Bookings.FindAsync(id);
        if (booking == null)
            return NotFound();

        _context.Bookings.Remove(booking);
        await _context.SaveChangesAsync();
        return NoContent();
    }
}