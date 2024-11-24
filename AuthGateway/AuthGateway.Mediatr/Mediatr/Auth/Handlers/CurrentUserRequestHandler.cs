using System.Net;
using AuthGateway.Domain;
using AuthGateway.Domain.Models.Users;
using AuthGateway.Mediatr.Mediatr.Auth.Requests;
using AutoMapper;
using CommonModule.Core.Exceptions;
using CommonModule.Core.Extensions;
using CommonModule.Core.Mediatr;
using CommonModule.Interfaces;
using CommonModule.Shared.Constants;
using CommonModule.Shared.Responses.AuthGateway.Users;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace AuthGateway.Mediatr.Mediatr.Auth.Handlers;

public class CurrentUserRequestHandler: MediatrAuthBase, IRequestHandler<CurrentUserRequest, UserResponse>
{
    private readonly IMapper mapper;
    private readonly IGenericRepository<Guid, UserEntity, AuthGatewayDataContext> userRepository;

    public CurrentUserRequestHandler(
        ICurrentUserRepository currentUserRepository,
        IMapper mapper, 
        IGenericRepository<Guid, UserEntity, AuthGatewayDataContext> userRepository): base(currentUserRepository)
    {
        this.mapper = mapper;
        this.userRepository = userRepository;
    }
    
    public async Task<UserResponse> Handle(CurrentUserRequest request, CancellationToken cancellationToken)
    {
        Guid userId = await this.CurrentUserIdAsync();
        
        UserEntity? user = await this.userRepository.ByIdAsync(userId, cancellationToken, 
            user => 
                user
                    .Include(u => u.Roles)
                    .ThenInclude(ur => ur.Role)
                    .Include(u => u.UserSetting));
        if (user == null)
        {
            throw new EntityNotFoundException();
        }

        user.CheckInvalidStatus();
        
        UserResponse response = this.mapper.Map<UserEntity, UserResponse>(user);

        return response;
    }
}