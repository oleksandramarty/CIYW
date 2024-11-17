using CommonModule.Core.Exceptions;
using CommonModule.Interfaces;

namespace CommonModule.Core.Mediatr;

public class MediatrAuthBase
{
    private readonly ICurrentUserRepository currentUserRepository;
    
    public MediatrAuthBase(ICurrentUserRepository currentUserRepository)
    {
        this.currentUserRepository = currentUserRepository;
    }

    protected async Task<Guid> CurrentUserIdAsync()
    {
        Guid? userId = await this.currentUserRepository.CurrentUserIdAsync();

        if (!userId.HasValue)
        {
            throw new EntityNotFoundException();
        }

        return userId.Value;
    }
}