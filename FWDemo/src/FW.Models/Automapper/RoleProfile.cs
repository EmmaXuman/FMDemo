using AutoMapper;
using FW.Entities;
using FW.Models.ViewModel;
using System;
using System.Collections.Generic;
using System.Text;

namespace FW.Models.Automapper
{
    public class RoleProfile:Profile
    {
        public RoleProfile()
        {
            CreateMap<RoleViewModel, Role>();
        }
    }
}
