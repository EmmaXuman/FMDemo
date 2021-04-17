using AutoMapper;
using FW.Compoment.Jwt.UserClaim;
using FW.Entities;
using System;
using System.Collections.Generic;
using System.Text;

namespace FW.Models.Automapper
{
    public class UserProfile: Profile
    {
        public UserProfile()
        {
            CreateMap<User, UserData>()
                .ForMember(a => a.Id, t => t.MapFrom(b => b.Id))
                .ForMember(a => a.RoleName, t => t.MapFrom(b => b.Role.Name))
                .ForMember(a => a.RoleDisplayName, t => t.MapFrom(b => b.Role.DisplayName))
                ;
        }
    }
}
