using AuthGateway.Domain;
using AuthGateway.Domain.Models.Users;
using AuthGateway.Mediatr.Mediatr.Auth.Commands;
using AuthGateway.Mediatr.Validators.Auth;
using AutoMapper;
using CommonModule.Core.Exceptions;
using CommonModule.Interfaces;
using CommonModule.Shared.Constants;
using CommonModule.Shared.Enums;
using CommonModule.Shared.Responses.Base;
using MediatR;

namespace AuthGateway.Mediatr.Mediatr.Auth.Handlers;

public class AuthSignUpCommandHandler: IRequestHandler<AuthSignUpCommand, BaseEntityIdResponse<Guid>>
{
    private readonly IMapper mapper;
    private readonly IEntityValidator<AuthGatewayDataContext> entityValidator;
    private readonly IJwtTokenFactory jwtTokenFactory;
    private readonly IGenericRepository<Guid, UserEntity, AuthGatewayDataContext> userRepository;
    private readonly IGenericRepository<Guid, UserRoleEntity, AuthGatewayDataContext> userRoleRepository;

    public AuthSignUpCommandHandler(
        IMapper mapper, 
        IEntityValidator<AuthGatewayDataContext> entityValidator,
        IJwtTokenFactory jwtTokenFactory,
        IGenericRepository<Guid, UserEntity, AuthGatewayDataContext> userRepository,
        IGenericRepository<Guid, UserRoleEntity, AuthGatewayDataContext> userRoleRepository)
    {
        this.mapper = mapper;
        this.entityValidator = entityValidator;
        this.jwtTokenFactory = jwtTokenFactory;
        this.userRepository = userRepository;
        this.userRoleRepository = userRoleRepository;
    }

    public async Task<BaseEntityIdResponse<Guid>> Handle(AuthSignUpCommand command, CancellationToken cancellationToken)
    {
        this.entityValidator.ValidateRequest<AuthSignUpCommand, BaseEntityIdResponse<Guid>>(command, () => new AuthSignUpCommandValidator());
        
        await this.entityValidator.ValidateExistParamAsync<UserEntity>(
            u => u.Email == command.Email, 
            ErrorMessages.EntityWithEmailAlreadyExists, 
            cancellationToken);

        UserEntity userEntity = this.mapper.Map<AuthSignUpCommand, UserEntity>(command);
        
        userEntity.Salt = this.jwtTokenFactory.GenerateSalt();
        userEntity.PasswordHash = this.jwtTokenFactory.HashPassword(command.Password, userEntity.Salt);
        
        await this.userRepository.AddAsync(userEntity, cancellationToken);
        
        await this.userRoleRepository.AddAsync(new UserRoleEntity
        {
            RoleId = (int)UserRoleEnum.User,
            UserId = userEntity.Id
        }, cancellationToken);

        return new BaseEntityIdResponse<Guid> { Id = userEntity.Id };
    }
}