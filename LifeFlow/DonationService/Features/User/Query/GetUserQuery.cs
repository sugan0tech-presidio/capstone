using MediatR;

namespace DonationService.Features.User.Query;

public class GetUserQuery : IRequest<UserDto>
{
    public GetUserQuery(int userId)
    {
        UserId = userId;
    }

    public int UserId { get; init; }
}