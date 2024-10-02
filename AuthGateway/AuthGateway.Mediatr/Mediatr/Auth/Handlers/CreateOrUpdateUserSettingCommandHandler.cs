using AuthGateway.Domain;
using AuthGateway.Domain.Models.Users;
using AuthGateway.Mediatr.Mediatr.Auth.Commands;
using AutoMapper;
using CommonModule.Interfaces;
using MediatR;

namespace AuthGateway.Mediatr.Mediatr.Auth.Handlers;

public class CreateOrUpdateUserSettingCommandHandler: IRequestHandler<CreateOrUpdateUserSettingCommand>
{
    private readonly IMapper mapper;
    private readonly IAuthRepository authRepository;
    private readonly IEntityValidator<AuthGatewayDataContext> entityValidator;
    private readonly IGenericRepository<Guid, UserSetting, AuthGatewayDataContext> userSettingRepository;
    
    public CreateOrUpdateUserSettingCommandHandler(
        IMapper mapper,
        IAuthRepository authRepository, 
        IEntityValidator<AuthGatewayDataContext> entityValidator,
        IGenericRepository<Guid, UserSetting, AuthGatewayDataContext> userSettingRepository
        )
    {
        this.mapper = mapper;
        this.authRepository = authRepository;
        this.entityValidator = entityValidator;
        this.userSettingRepository = userSettingRepository;
    }
    
    public async Task Handle(CreateOrUpdateUserSettingCommand command, CancellationToken cancellationToken)
    {
        Guid? userId = await authRepository.GetCurrentUserIdAsync();
        this.entityValidator.ValidateExist(userId);

        if (command.Id.HasValue)
        {
            UserSetting userSetting = await userSettingRepository.GetByIdAsync(command.Id.Value, cancellationToken);
            this.entityValidator.ValidateExist<UserSetting, Guid>(userSetting, command.Id.Value);

            await this.userSettingRepository.UpdateAsync(
                this.mapper.Map(command, userSetting),
                cancellationToken
            );
            
            return;
        }
        
        await this.userSettingRepository.AddAsync(
            this.mapper.Map<UserSetting>(command),
            cancellationToken
        );
    }
}