using AuthGateway.Domain;
using AuthGateway.Domain.Models.Users;
using AuthGateway.Mediatr.Mediatr.Auth.Commands;
using AutoMapper;
using CommonModule.Interfaces;
using MediatR;

namespace AuthGateway.Mediatr.Mediatr.Auth.Handlers;

public class UpdateUserSettingCommandHandler: IRequestHandler<UpdateUserSettingCommand>
{
    private readonly IMapper mapper;
    private readonly ICurrentUserRepository currentUserRepository;
    private readonly IEntityValidator<AuthGatewayDataContext> entityValidator;
    private readonly IGenericRepository<Guid, UserSettingEntity, AuthGatewayDataContext> userSettingRepository;
    
    public UpdateUserSettingCommandHandler(
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
    
    public async Task Handle(UpdateUserSettingCommand command, CancellationToken cancellationToken)
    {
        Guid? userId = await currentUserRepository.GetCurrentUserIdAsync();
        this.entityValidator.IsEntityExist(userId);

        UserSettingEntity userSettingEntity = await userSettingRepository.GetByIdAsync(command.Id, cancellationToken);
        this.entityValidator.IsEntityExist(userSettingEntity);
            
        await this.userSettingRepository.UpdateAsync(
            this.mapper.Map(command, userSettingEntity),
            cancellationToken
        );
    }
}