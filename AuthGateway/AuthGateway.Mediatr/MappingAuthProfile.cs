using AuthGateway.Domain.Models.Users;
using AuthGateway.Mediatr.Mediatr.Auth.Commands;
using AutoMapper;
using CommonModule.Core.Extensions;
using CommonModule.Shared.Enums;
using CommonModule.Shared.Responses.AuthGateway.Users;

namespace AuthGateway.Mediatr;

public class MappingAuthProfile: Profile
{
    public MappingAuthProfile()
    {
        this.CreateMap<User, UserResponse>()
            .ForMember(dest => 
                dest.Roles, 
                opt => 
                    opt.MapFrom(src => 
                        src.Roles != null ? 
                            src.Roles.Select(r => 
                                new RoleResponse
                                {
                                    Id = r.Role.Id,
                                    Title = r.Role.Title,
                                    UserRole = r.Role.UserRole
                                }).ToList() : new List<RoleResponse>()));
        this.CreateMap<Role, RoleResponse>();
        this.CreateMap<UserSetting, UserSettingResponse>();
        
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

        this.CreateMap<CreateOrUpdateUserSettingCommand, UserSetting>()
            .ConstructUsing((src, ctx) => 
                this.CreateOrUpdateEntity<CreateOrUpdateUserSettingCommand, UserSetting, Guid>(src, ctx));
    }
}