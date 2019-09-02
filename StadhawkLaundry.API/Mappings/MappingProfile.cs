using AutoMapper;
using StadhawkLaundry.API.Data;
using StadhawkLaundry.DataModel.Models;
using StadhawkLaundry.ViewModel;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace StadhawkLaundry.API.Mappings
{
    public class MappingProfile : Profile
    {
        public MappingProfile()
        {
            CreateMap<ServicesViewModel, TblService>()
               .ForMember(dest => dest.Name, opt => opt.MapFrom(src => src.Name));
            CreateMap<TblService, ServicesViewModel>()
                .ForMember(dest => dest.Name, opt => opt.MapFrom(src => src.Name));


            CreateMap<CategoryViewModel, TblCategory>()
                .ForMember(dest => dest.Name, opt => opt.MapFrom(src => src.Name))
                 .ForMember(dest => dest.ServiceId, opt => opt.MapFrom(src => src.ServiceId));

            CreateMap<TblCategory, CategoryViewModel>()
                .ForMember(dest => dest.Name, opt => opt.MapFrom(src => src.Name))
                .ForMember(dest => dest.ServiceId, opt => opt.MapFrom(src => src.ServiceId));


            CreateMap<SubcategoryViewModel, TblSubcategory>()
                .ForMember(dest => dest.Name, opt => opt.MapFrom(src => src.Name))
                .ForMember(dest => dest.Price, opt => opt.MapFrom(src => src.Price))
            .ForMember(dest => dest.CategoryId, opt => opt.MapFrom(src => src.CategoryId));
            CreateMap<TblSubcategory, SubcategoryViewModel>()
                .ForMember(dest => dest.Name, opt => opt.MapFrom(src => src.Name))
                .ForMember(dest => dest.Price, opt => opt.MapFrom(src => src.Price))
                .ForMember(dest => dest.CategoryId, opt => opt.MapFrom(src => src.CategoryId));



            CreateMap<ItemViewModel, TblItem>()
                .ForMember(dest => dest.Name, opt => opt.MapFrom(src => src.Name))
                .ForMember(dest => dest.ServiceId, opt => opt.MapFrom(src => src.SubcategoryId));

            CreateMap<TblItem, ItemViewModel>()
                .ForMember(dest => dest.Name, opt => opt.MapFrom(src => src.Name))
                .ForMember(dest => dest.SubcategoryId, opt => opt.MapFrom(src => src.ServiceId));


            //CreateMap<AspNetUserRoles, RoleViewModel>()
            //    .ForMember(dest => dest.RoleId, opt => opt.MapFrom(src => src.RoleId))
            //    .ForMember(dest => dest.Status, opt => opt.MapFrom(src => src.Status));

            CreateMap<UsersViewModel, ApplicationUser>()
                .ForMember(dest => dest.Email, opt => opt.MapFrom(src => src.EmailId))
                .ForMember(dest => dest.PhoneNumber, opt => opt.MapFrom(src => src.Contactno));
        }
    }
}
