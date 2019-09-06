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
            CreateMap<ServicesViewModel, TblServiceMaster>()
               .ForMember(dest => dest.ServiceName, opt => opt.MapFrom(src => src.Name));
            CreateMap<TblServiceMaster, ServicesViewModel>()
                .ForMember(dest => dest.Name, opt => opt.MapFrom(src => src.ServiceName));


            CreateMap<CategoryViewModel, TblCategoryMaster>()
                .ForMember(dest => dest.CategoryName, opt => opt.MapFrom(src => src.Name))
                 .ForMember(dest => dest.Id, opt => opt.MapFrom(src => src.ServiceId));

            CreateMap<TblCategoryMaster, CategoryViewModel>()
                .ForMember(dest => dest.Name, opt => opt.MapFrom(src => src.CategoryName))
                .ForMember(dest => dest.ServiceId, opt => opt.MapFrom(src => src.Id));



            CreateMap<ItemViewModel, TblItemMaster>()
                .ForMember(dest => dest.ItemName, opt => opt.MapFrom(src => src.Name))
                .ForMember(dest => dest.Id, opt => opt.MapFrom(src => src.SubcategoryId));

            CreateMap<TblItemMaster, ItemViewModel>()
                .ForMember(dest => dest.Name, opt => opt.MapFrom(src => src.ItemName))
                .ForMember(dest => dest.SubcategoryId, opt => opt.MapFrom(src => src.Id));


            CreateMap<UsersViewModel, ApplicationUser>()
                .ForMember(dest => dest.Email, opt => opt.MapFrom(src => src.EmailId))
                .ForMember(dest => dest.PhoneNumber, opt => opt.MapFrom(src => src.ContactNo));
        }
    }
}
