﻿#nullable disable
using System.Text.Json.Serialization;

namespace DonationService.Features.User;

public record UserDto
{
    public int UserId { get; set; }
    public string Email { get; init; }
    public string Name { get; init; }
    public string PhoneNumber { get; init; }
    public int? AddressId { get; set; }
    public bool IsVerified { get; set; }

    [JsonIgnore] public byte[] Password { get; set; }
    [JsonIgnore] public byte[] HashKey { get; set; }
    public int LoginAttempts { get; init; }
    public string Role { get; init; }
}