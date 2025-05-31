using AutoMapper;
using Billi.Backend.Domain.Entities;
using Billi.Backend.Domain.ValueObjects;

namespace Billi.Backend.Application.Mappings
{
    public class AuthMappingProfile : Profile
    {
        public AuthMappingProfile()
        {
            CreateMap<AccessToken, UserRefreshTokenEntity>()
                .ForMember(dest => dest.UserId, opt => opt.MapFrom(src => src.UserId))
                .ForMember(dest => dest.Token, opt => opt.MapFrom(src => src.RefreshToken))
                .ForMember(dest => dest.ExpiresAt, opt => opt.MapFrom(src => src.CreatedAt.AddDays(1)))
                .ForMember(dest => dest.IsUsed, opt => opt.MapFrom(_ => false))
                .ForMember(dest => dest.IsRevoked, opt => opt.MapFrom(_ => false));
        }
    }
}
