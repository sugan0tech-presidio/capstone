﻿namespace DonationService.Address.Command;

public class UpdateAddressCommand(AddressDto address)
{
    public AddressDto Address { get; set; } = address;
}