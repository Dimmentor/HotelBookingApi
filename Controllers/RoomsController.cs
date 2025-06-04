using FluentValidation;
using HotelBookingApi.Data;
using HotelBookingApi.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace HotelBookingApi.Controllers;

[ApiController]
[Route("api/[controller]")]
public class RoomsController : ControllerBase
{
    private readonly AppDbContext _context;
    private readonly IValidator<Room> _roomValidator;

    public RoomsController(AppDbContext context, IValidator<Room> roomValidator)
    {
        _context = context;
        _roomValidator = roomValidator;
    }

    [HttpGet]
    public async Task<ActionResult<IEnumerable<Room>>> GetRooms()
    {
        return await _context.Rooms.ToListAsync();
    }

    [HttpPost]
    public async Task<ActionResult<Room>> CreateRoom([FromBody] Room room)
    {
        var validationResult = await _roomValidator.ValidateAsync(room);
        if (!validationResult.IsValid)
            return BadRequest(validationResult.Errors);

        _context.Rooms.Add(room);
        await _context.SaveChangesAsync();
        return CreatedAtAction(nameof(GetRooms), new { id = room.Id }, room);
    }
}