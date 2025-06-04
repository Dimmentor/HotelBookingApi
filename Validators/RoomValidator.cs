using FluentValidation;
using HotelBookingApi.Models;
using System.Text.RegularExpressions;

namespace HotelBookingApi.Validators;

public class RoomValidator : AbstractValidator<Room>
{
    public RoomValidator()
    {
        RuleFor(r => r.Number)
            .NotEmpty().WithMessage("Введите номер комнаты.")
            .Matches(@"\d").WithMessage("Номер должен содержать как минимум одну цифру.");

        RuleFor(r => r.Type)
            .NotEmpty().WithMessage("Укажите тип номера.")
            .MinimumLength(3).WithMessage("Тип номера должен содержать минимум три буквы.");
    }
}