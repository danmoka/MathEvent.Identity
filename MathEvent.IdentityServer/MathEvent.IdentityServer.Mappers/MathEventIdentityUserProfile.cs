using AutoMapper;
using MathEvent.IdentityServer.Entities;
using MathEvent.IdentityServer.Models.User;
using Microsoft.AspNetCore.Identity;
using Newtonsoft.Json;

namespace MathEvent.IdentityServer.Mappers
{
    public class MathEventIdentityUserProfile : Profile
    {
        public MathEventIdentityUserProfile()
        {
            CreateMap<MathEventIdentityUserCreateModel, MathEventIdentityUser>();
            CreateMap<MathEventIdentityUserUpdateModel, MathEventIdentityUser>();
            CreateMap<MathEventIdentityUser, MathEventIdentityUserReadModel>()
                .ForMember(dest => dest.Roles, opt => opt.MapFrom<RolesResolver>());
            CreateMap<MathEventIdentityUserReadModel, MathEventIdentityUserUpdateModel>();
        }

        /// <summary>
        /// Класс, описывающий маппинг id события под управлением на transfer объект события
        /// </summary>
        public class RolesResolver : IValueResolver<MathEventIdentityUser, MathEventIdentityUserReadModel, string>
        {
            private readonly UserManager<MathEventIdentityUser> _userManager;
            private readonly IMapper _mapper;

            public RolesResolver(UserManager<MathEventIdentityUser> userManager, IMapper mapper)
            {
                _userManager = userManager;
                _mapper = mapper;
            }

            public string Resolve(MathEventIdentityUser source, MathEventIdentityUserReadModel destination, string destMember, ResolutionContext context)
            {
                var roles = _userManager.GetRolesAsync(source).Result;
                return JsonConvert.SerializeObject(roles);
            }
        }
    }
}
