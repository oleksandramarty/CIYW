using CommonModule.Core.Exceptions;
using CommonModule.Core.Mediatr;
using CommonModule.Interfaces;
using Expenses.Domain;
using Expenses.Domain.Models.Projects;
using Microsoft.EntityFrameworkCore;

namespace Expenses.Mediatr.Mediatr;

public class MediatrExpensesBase: MediatrAuthBase
{
    private readonly IEntityValidator<ExpensesDataContext> entityValidator;
    private readonly IReadGenericRepository<Guid, UserProjectEntity, ExpensesDataContext> userProjectRepository;
    
    public MediatrExpensesBase(
        ICurrentUserRepository currentUserRepository,
        IEntityValidator<ExpensesDataContext> entityValidator,
        IReadGenericRepository<Guid, UserProjectEntity, ExpensesDataContext> userProjectRepository
        ) : base(currentUserRepository)
    {
        this.entityValidator = entityValidator;
        this.userProjectRepository = userProjectRepository;
    }
    
    public async Task CheckUserProjectByIdAsync(Guid userProjectId, CancellationToken cancellationToken)
    {
        Guid userId = await this.GetCurrentUserIdAsync();
        
        UserProjectEntity userProjectEntity =
            await this.userProjectRepository.GetAsync(
                up => up.Id == userProjectId, 
                cancellationToken,
                up => up.Include(a => a.AllowedUsers).Include(b => b.Balances));
        this.entityValidator.IsEntityExist(userProjectEntity);
        
        if (userProjectEntity.CreatedUserId != userId && userProjectEntity.AllowedUsers.All(au => au.UserId != userId))
        {
            throw new ForbiddenException();
        }
    }

    public async Task<UserProjectEntity> GetUserProjectByIdAsync(Guid userProjectId, CancellationToken cancellationToken)
    {
        Guid userId = await this.GetCurrentUserIdAsync();
        
        UserProjectEntity userProjectEntity =
            await this.userProjectRepository.GetAsync(
                up => up.Id == userProjectId, 
                cancellationToken,
                up => up.Include(a => a.AllowedUsers).Include(b => b.Balances));
        this.entityValidator.IsEntityExist(userProjectEntity);
        
        if (userProjectEntity.CreatedUserId != userId && userProjectEntity.AllowedUsers.All(au => au.UserId != userId))
        {
            throw new ForbiddenException();
        }

        return userProjectEntity;
    }
}