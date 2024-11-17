using AuthGateway.Domain;
using AuthGateway.Domain.Models.Users;
using AuthGateway.Mediatr.Mediatr.Auth.Commands;
using AutoMapper;
using CommonModule.Core.Exceptions;
using CommonModule.Interfaces;
using MediatR;

namespace AuthGateway.Mediatr.Mediatr.Auth.Handlers;

public class CreateUserSettingCommandHandler: IRequestHandler<CreateUserSettingCommand>
{
    private readonly IMapper mapper;
    private readonly ICurrentUserRepository currentUserRepository;
    private readonly IEntityValidator<AuthGatewayDataContext> entityValidator;
    private readonly IGenericRepository<Guid, UserSettingEntity, AuthGatewayDataContext> userSettingRepository;
    
    public CreateUserSettingCommandHandler(
        IMapper mapper,
        ICurrentUserRepository currentUserRepository, 
        IEntityValidator<AuthGatewayDataContext> entityValidator,
        IGenericRepository<Guid, UserSettingEntity, AuthGatewayDataContext> userSettingRepository
        )
    {
        this.mapper = mapper;
        this.currentUserRepository = currentUserRepository;
        this.entityValidator = entityValidator;
        this.userSettingRepository = userSettingRepository;
    }
    
    public async Task Handle(CreateUserSettingCommand command, CancellationToken cancellationToken)
    {
        Guid? userId = await currentUserRepository.CurrentUserIdAsync();
        if (!userId.HasValue)
        {
            throw new EntityNotFoundException();
        }
        
        UserSettingEntity toAdd = this.mapper.Map<UserSettingEntity>(command);
        
        await this.userSettingRepository.AddAsync(
            toAdd,
            cancellationToken
        );
    }
}