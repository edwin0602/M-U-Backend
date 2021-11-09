using AutoMapper;
using RestBackend.Core.Models.Auth;
using RestBackend.Core.Models.Business;
using RestBackend.Core.Resources;

namespace RestBackend.Core.Mapping
{
    public class MappingProfile : Profile
    {
        public MappingProfile()
        {
            #region [ Users ]

            CreateMap<CreateUserResource, User>();
            CreateMap<User, UserResource>();
            CreateMap<UserResource, User>();

            #endregion

            #region [ Owner ]

            CreateMap<Owner, OwnerResource>();
            CreateMap<OwnerResource, Owner>();

            CreateMap<CreateOwnerResource, Owner>();

            #endregion

            #region [ Property ]

            CreateMap<Property, PropertyResource>()
                .ForMember(m => m.Images, opt => opt.MapFrom(u => u.PropertiesImages));
            CreateMap<PropertyResource, Property>();

            CreateMap<CreatePropertyResource, Property>();

            #endregion

            #region [ PropertyTrace ]

            CreateMap<PropertyTrace, PropertyTraceResource>();
            CreateMap<PropertyTraceResource, PropertyTrace>();

            #endregion

            #region [ PropertyImage ]

            CreateMap<PropertyImage, PropertyImageResource>();
            CreateMap<PropertyImageResource, PropertyImage>();

            CreateMap<CreatePropertyImageResource, PropertyImage>();

            #endregion
        }
    }
}