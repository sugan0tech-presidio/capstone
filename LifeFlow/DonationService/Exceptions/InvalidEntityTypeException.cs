namespace DonationService.Exceptions;

public class InvalidEntityTypeException: Exception
{
    public InvalidEntityTypeException()
    {
    }

    public InvalidEntityTypeException(string? message) : base(message)
    {
    }
}