using System.ComponentModel.DataAnnotations;

namespace HotelBookingApi.Models;

public class Room
{
    public int Id { get; set; }

    [Required]
    public string Number { get; set; } = string.Empty;

    [Required]
    public string Type { get; set; } = string.Empty;
}