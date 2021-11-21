using AutoMapper;
using MathEvent.IdentityServer.Entities;
using MathEvent.IdentityServer.Models.User;

namespace MathEvent.IdentityServer.Mappers
{
    public class MathEventIdentityUserProfile : Profile
    {
        public MathEventIdentityUserProfile()
        {
            CreateMap<MathEventIdentityUserCreateModel, MathEventIdentityUser>();
            CreateMap<MathEventIdentityUserUpdateModel, MathEventIdentityUser>();
            CreateMap<MathEventIdentityUser, MathEventIdentityUserReadModel>();
            CreateMap<MathEventIdentityUserReadModel, MathEventIdentityUserUpdateModel>();
        }
    }
}
