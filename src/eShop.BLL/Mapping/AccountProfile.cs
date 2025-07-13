using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using AutoMapper;
using eShop.BLL.ViewModels.Account;
using eShop.Core.Entities;

namespace eShop.BLL.Mapping
{
    public class AccountProfile : Profile
    {
        public AccountProfile()
        {
            CreateMap<AccountDataViewModel, ApplicationUser>().ReverseMap();
            CreateMap<RegisterViewModel, ApplicationUser>().ReverseMap();
        }
    }
}
