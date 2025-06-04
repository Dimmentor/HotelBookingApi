using FluentValidation;
using HotelBookingApi.Models;
using HotelBookingApi.Data;
using Microsoft.EntityFrameworkCore;

namespace HotelBookingApi.Validators;

public class BookingValidator : AbstractValidator<Booking>
{
    private readonly AppDbContext _db;

    public BookingValidator(AppDbContext db)
    {
        _db = db;

        RuleFor(x => x.CustomerName)
            .NotEmpty()
            .MinimumLength(3)
            .WithMessage("Имя должно состоять как минимум из 3 букв.");

        RuleFor(x => x.StartDate)
            .LessThan(x => x.EndDate)
            .WithMessage("Дата заезда должена быть позже вдаты выезда.");

        RuleFor(x => x)
            .MustAsync(NoOverlappingBooking)
            .WithMessage("Номер уже забронирован на выбранную дату.");
    }

    private async Task<bool> NoOverlappingBooking(Booking booking, CancellationToken cancellationToken)
    {
        return !await _db.Bookings.AnyAsync(b =>
            b.Id != booking.Id &&
            b.RoomId == booking.RoomId &&
            b.StartDate < booking.EndDate &&
            b.EndDate > booking.StartDate,
            cancellationToken);
    }
}