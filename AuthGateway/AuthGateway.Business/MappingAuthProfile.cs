using AuthGateway.Domain.Models.Users;
using AuthGateway.Mediatr.Mediatr.Auth.Commands;
using AutoMapper;
using CommonModule.Shared.Enums;
using CommonModule.Shared.Responses.Users;

namespace AuthGateway.ClientApi;

public class MappingAuthProfile: Profile
{
    public MappingAuthProfile()
    {
        this.CreateMap<User, UserResponse>();
        this.CreateMap<Role, RoleResponse>();
        this.CreateMap<UserRole, UserRoleResponse>();
        
        this.CreateMap<AuthSignUpCommand, User>()
            .AfterMap((src, dest) =>
            {
                dest.Id = Guid.NewGuid();
                dest.Created = DateTime.UtcNow;
                dest.LoginNormalized = src.Login.ToUpper();
                dest.EmailNormalized = src.Email.ToUpper();
                dest.IsActive = true;
                dest.IsTemporaryPassword = true;
                dest.AuthType = UserAuthMethodEnum.Base;
            });
    }
}