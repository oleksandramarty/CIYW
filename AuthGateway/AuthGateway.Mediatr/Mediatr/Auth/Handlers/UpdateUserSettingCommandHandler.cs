using AuthGateway.Domain;
using AuthGateway.Domain.Models.Users;
using AuthGateway.Mediatr.Mediatr.Auth.Commands;
using AutoMapper;
using CommonModule.Core.Exceptions;
using CommonModule.Interfaces;
using MediatR;

namespace AuthGateway.Mediatr.Mediatr.Auth.Handlers;

public class UpdateUserSettingCommandHandler: IRequestHandler<UpdateUserSettingCommand>
{
    private readonly IMapper mapper;
    private readonly ICurrentUserRepository currentUserRepository;
    private readonly IGenericRepository<Guid, UserSettingEntity, AuthGatewayDataContext> userSettingRepository;
    
    public UpdateUserSettingCommandHandler(
        IMapper mapper,
        ICurrentUserRepository currentUserRepository,
        IGenericRepository<Guid, UserSettingEntity, AuthGatewayDataContext> userSettingRepository
        )
    {
        this.mapper = mapper;
        this.currentUserRepository = currentUserRepository;
        this.userSettingRepository = userSettingRepository;
    }
    
    public async Task Handle(UpdateUserSettingCommand command, CancellationToken cancellationToken)
    {
        Guid? userId = await currentUserRepository.CurrentUserIdAsync();
        if (!userId.HasValue)
        {
            throw new EntityNotFoundException();
        }

        UserSettingEntity? userSetting = await userSettingRepository.ByIdAsync(command.Id, cancellationToken);
        if (userSetting == null)
        {
            throw new EntityNotFoundException();
        }

        UserSettingEntity? updatedUserSettingEntity = this.mapper.Map(command, userSetting);
        if (updatedUserSettingEntity == null)
        {
            throw new InvalidOperationException("User can update only own settings");
        }
            
        await this.userSettingRepository.UpdateAsync(
            updatedUserSettingEntity,
            cancellationToken
        );
    }
}