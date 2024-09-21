using CommonModule.Core.Exceptions;
using CommonModule.Interfaces;

namespace CommonModule.Core.Mediatr;

public class MediatrAuthBase
{
    private readonly IAuthRepository authRepository;
    
    public MediatrAuthBase(IAuthRepository authRepository)
    {
        this.authRepository = authRepository;
    }

    protected async Task<Guid> GetUserIdAsync()
    {
        Guid? userId = await this.authRepository.GetCurrentUserIdAsync();

        if (!userId.HasValue)
        {
            throw new EntityNotFoundException();
        }

        return userId.Value;
    }
}