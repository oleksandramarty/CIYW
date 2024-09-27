using AutoMapper;
using CommonModule.Core.Exceptions;
using CommonModule.Core.Mediatr;
using CommonModule.Interfaces;
using CommonModule.Shared.Constants;
using Expenses.Domain;
using Expenses.Domain.Models.Balances;
using Expenses.Domain.Models.Projects;
using Expenses.Mediatr.Mediatr.Projects.Commands;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace Expenses.Mediatr.Mediatr.Projects.Handlers;

public class CreateUserProjectCommandHandler: MediatrAuthBase, IRequestHandler<CreateUserProjectCommand>
{
    private readonly IMapper mapper;
    private readonly IGenericRepository<Guid, UserProject, ExpensesDataContext> userProjectRepository;
    
    public CreateUserProjectCommandHandler(
        IAuthRepository authRepository,
        IMapper mapper,
        IGenericRepository<Guid, UserProject, ExpensesDataContext> userProjectRepository): base(authRepository)
    {
        this.mapper = mapper;
        this.userProjectRepository = userProjectRepository;
    }
    
    public async Task Handle(CreateUserProjectCommand command, CancellationToken cancellationToken)
    {
        Guid userId = await this.GetCurrentUserIdAsync();
        int count = await this.userProjectRepository.GetQueryable(
            p => p.CreatedUserId == userId).CountAsync(cancellationToken);

        if (count > 5)
        {
            throw new BusinessException(ErrorMessages.UserProjectLimitExceeded, 409);
        }
        
        UserProject userProject = this.mapper.Map<UserProject>(command);
        
        userProject.Id = Guid.NewGuid();
        userProject.CreatedUserId = userId;

        userProject.Balances = new List<Balance>
        {
            new Balance
            {
                Id = Guid.NewGuid(),
                Amount = 0,
                Created = DateTime.UtcNow,
                CurrencyId = command.CurrencyId,
                UserProjectId = userProject.Id
            }
        };
        
        await this.userProjectRepository.AddAsync(userProject, cancellationToken);
    }
}