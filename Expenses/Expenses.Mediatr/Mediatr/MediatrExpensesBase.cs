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
    private readonly IReadGenericRepository<Guid, UserProject, ExpensesDataContext> userProjectRepository;
    
    public MediatrExpensesBase(
        IAuthRepository authRepository,
        IEntityValidator<ExpensesDataContext> entityValidator,
        IReadGenericRepository<Guid, UserProject, ExpensesDataContext> userProjectRepository
        ) : base(authRepository)
    {
        this.entityValidator = entityValidator;
        this.userProjectRepository = userProjectRepository;
    }
    
    public async Task CheckUserProjectByIdAsync(Guid userProjectId, CancellationToken cancellationToken)
    {
        Guid userId = await this.GetCurrentUserIdAsync();
        
        UserProject userProject =
            await this.userProjectRepository.GetAsync(
                up => up.Id == userProjectId, 
                cancellationToken,
                up => up.Include(a => a.AllowedUsers).Include(b => b.Balances));
        this.entityValidator.ValidateExist(userProject, userProjectId);
        
        if (userProject.CreatedUserId != userId && userProject.AllowedUsers.All(au => au.UserId != userId))
        {
            throw new ForbiddenException();
        }
    }

    public async Task<UserProject> GetUserProjectByIdAsync(Guid userProjectId, CancellationToken cancellationToken)
    {
        Guid userId = await this.GetCurrentUserIdAsync();
        
        UserProject userProject =
            await this.userProjectRepository.GetAsync(
                up => up.Id == userProjectId, 
                cancellationToken,
                up => up.Include(a => a.AllowedUsers).Include(b => b.Balances));
        this.entityValidator.ValidateExist(userProject, userProjectId);
        
        if (userProject.CreatedUserId != userId && userProject.AllowedUsers.All(au => au.UserId != userId))
        {
            throw new ForbiddenException();
        }

        return userProject;
    }
}