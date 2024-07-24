using AutoMapper;
using DonationService.Commons;
using DonationService.Exceptions;

namespace DonationService.UserSession;

public class UserSessionService(IBaseRepo<DonationService.UserSession.UserSession> repo, ILogger<UserSessionService> logger, IMapper mapper)
    : IUserSessionService
{
    /// <intheritdoc/>
    public async Task<List<UserSessionDto>> GetById(int sessionId)
    {
        logger.LogInformation($"Fetching session with Id: {sessionId}");
        var session = await repo.GetById(sessionId);
        return new List<UserSessionDto> { mapper.Map<UserSessionDto>(session) };
    }

    /// <intheritdoc/>
    public UserSessionDto GetByToken(string token)
    {
        logger.LogInformation($"Fetching sessions for token");
        var session = repo.GetAll().Result.First(s => s.RefreshToken.Equals(token));
        return mapper.Map<UserSessionDto>(session);
    }

    /// <intheritdoc/>
    public List<UserSessionDto> GetByUserId(int userId)
    {
        logger.LogInformation($"Fetching sessions for UserId: {userId}");
        var sessions = repo.GetAll().Result.Where(s => s.UserId == userId);
        return mapper.Map<List<UserSessionDto>>(sessions);
    }

    /// <intheritdoc/>
    public async Task<List<UserSessionDto>> GetAll()
    {
        logger.LogInformation("Fetching all sessions");
        var sessions = await repo.GetAll();
        return mapper.Map<List<UserSessionDto>>(sessions);
    }

    /// <intheritdoc/>
    public async Task<UserSessionDto> DeleteById(int id)
    {
        logger.LogInformation($"Deleting session with Id: {id}");
        var session = await repo.DeleteById(id);
        return mapper.Map<UserSessionDto>(session);
    }

    /// <intheritdoc/>
    public async Task Flush()
    {
        logger.LogInformation("Deleting all expired sessions");
        var sessions = await repo.GetAll();
        var userSessions = sessions.Where(s => s.ExpiresAt <= DateTime.Now);
        foreach (var userSession in userSessions)
            await repo.DeleteById(userSession.Id);
    }

    /// <intheritdoc/>
    public async Task<UserSessionDto> Add(UserSessionDto userSessionDto)
    {
        logger.LogInformation($"Adding new session for UserId: {userSessionDto.UserId}");
        var userSession = mapper.Map<DonationService.UserSession.UserSession>(userSessionDto);
        var session = await repo.Add(userSession);
        return mapper.Map<UserSessionDto>(session);
    }

    /// <intheritdoc/>
    public async Task<bool> IsValid(string token)
    {
        var sessions = await repo.GetAll();
        var session = sessions.Find(userSession => userSession.RefreshToken.Equals(token));
        if (session == null) throw new AuthenticationException("User Session Token not found");
        return session.IsValid;
    }

    /// <intheritdoc/>
    public async Task<UserSessionDto> Invalidate(string token)
    {
        var sessions = await repo.GetAll();
        var session = sessions.Find(userSession => userSession.RefreshToken.Equals(token));
        if (session == null) throw new AuthenticationException("User Session Token not found");
        logger.LogInformation($"Invalidating session with Id: {session.Id}");
        session.IsValid = false;
        await repo.Update(session);
        return mapper.Map<UserSessionDto>(session);
    }

    /// <intheritdoc/>
    public async Task InvalidateAllPerUser(int userId)
    {
        logger.LogInformation($"Invalidating all sessions for UserId: {userId}");
        var sessions = await repo.GetAll();
        var userSessions = sessions.Where(s => s.UserId == userId).ToList();
        foreach (var session in userSessions)
        {
            session.IsValid = false;
            await repo.Update(session);
        }
    }
}