using CommonModule.Core.Exceptions;
using CommonModule.Core.Mediatr;
using CommonModule.Interfaces;
using Expenses.Domain;
using Expenses.Domain.Models.Projects;
using Microsoft.EntityFrameworkCore;

namespace Expenses.Mediatr.Mediatr;

public class MediatrExpensesBase: MediatrAuthBase
{
    private readonly IEntityValidator<ExpensesDataContext> _entityValidator;
    private readonly IReadGenericRepository<Guid, UserProjectEntity, ExpensesDataContext> _readGenericUserProjectRepository;
    
    public MediatrExpensesBase(
        ICurrentUserRepository currentUserRepository,
        IEntityValidator<ExpensesDataContext> entityValidator,
        IReadGenericRepository<Guid, UserProjectEntity, ExpensesDataContext> readGenericUserProjectRepository
        ) : base(currentUserRepository)
    {
        _entityValidator = entityValidator;
        _readGenericUserProjectRepository = readGenericUserProjectRepository;
    }
    
    public async Task CheckUserProjectByIdAsync(Guid userProjectId, CancellationToken cancellationToken)
    {
        Guid userId = await CurrentUserIdAsync();
        
        UserProjectEntity? userProject =
            await _readGenericUserProjectRepository.Async(
                up => up.Id == userProjectId, 
                cancellationToken,
                up => up.Include(a => a.AllowedUsers).Include(b => b.Balances));
        if (userProject == null)
        {
            throw new EntityNotFoundException();
        }
        
        if (userProject.CreatedUserId != userId && userProject.AllowedUsers.All(au => au.UserId != userId))
        {
            throw new ForbiddenException();
        }
    }

    public async Task<UserProjectEntity> UserProjectByIdAsync(Guid userProjectId, CancellationToken cancellationToken)
    {
        Guid userId = await CurrentUserIdAsync();
        
        UserProjectEntity? userProject =
            await _readGenericUserProjectRepository.Async(
                up => up.Id == userProjectId, 
                cancellationToken,
                up => up.Include(a => a.AllowedUsers).Include(b => b.Balances));
        if (userProject == null)
        {
            throw new EntityNotFoundException();
        }
        
        if (userProject.CreatedUserId != userId && userProject.AllowedUsers.All(au => au.UserId != userId))
        {
            throw new ForbiddenException();
        }

        return userProject;
    }
}