using AuthGateway.Domain;
using AuthGateway.Domain.Models.Users;
using AuthGateway.Mediatr.Mediatr.Auth.Commands;
using AutoMapper;
using CommonModule.Interfaces;
using MediatR;

namespace AuthGateway.Mediatr.Mediatr.Auth.Handlers;

public class CreateUserSettingCommandHandler: IRequestHandler<CreateUserSettingCommand>
{
    private readonly IMapper mapper;
    private readonly IAuthRepository authRepository;
    private readonly IEntityValidator<AuthGatewayDataContext> entityValidator;
    private readonly IGenericRepository<Guid, UserSetting, AuthGatewayDataContext> userSettingRepository;
    
    public CreateUserSettingCommandHandler(
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
    
    public async Task Handle(CreateUserSettingCommand command, CancellationToken cancellationToken)
    {
        Guid? userId = await authRepository.GetCurrentUserIdAsync();
        this.entityValidator.IsEntityExist(userId);
        
        UserSetting toAdd = this.mapper.Map<UserSetting>(command);
        
        await this.userSettingRepository.AddAsync(
            toAdd,
            cancellationToken
        );
    }
}