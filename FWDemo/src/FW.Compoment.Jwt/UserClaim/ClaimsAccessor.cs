using Microsoft.AspNetCore.Http;
using System;
using System.Linq;
using System.Security.Claims;

namespace FW.Compoment.Jwt.UserClaim
{
    /// <summary>
    /// 定义用户信息访问接口，开发时通过获取IClaimsAccessor接口来获取登录用户的信息
    /// </summary>
    public class ClaimsAccessor : IClaimsAccessor
    {
        private readonly IHttpContextAccessor _httpContextAccessor;

        public ClaimsAccessor( IHttpContextAccessor httpContextAccessor )
        {
            _httpContextAccessor = httpContextAccessor;
        }

        public ClaimsPrincipal UserPrincipal
        {
            get
            {
                ClaimsPrincipal user = _httpContextAccessor.HttpContext.User;
                if (user.Identity.IsAuthenticated)
                {
                    return user;
                }
                else
                {
                    throw new Exception("用户未认证");
                }
            }
        }

        public string UserName
        {
            get
            {
                return UserPrincipal.Claims.First(x => x.Type == UserClaimType.Name).Value;
            }
        }

        public long UserId
        {
            get
            {
                return long.Parse(UserPrincipal.Claims.First(x => x.Type == UserClaimType.Id).Value);
            }
        }


        public string UserAccount
        {
            get
            {
                return UserPrincipal.Claims.First(x => x.Type == UserClaimType.Account).Value;
            }
        }

        public string UserRole
        {
            get
            {
                return UserPrincipal.Claims.First(x => x.Type == UserClaimType.RoleName).Value;
            }
        }

        public string UserRoleDisplayName
        {
            get
            {
                return UserPrincipal.Claims.First(x => x.Type == UserClaimType.RoleDisplayName).Value;
            }
        }
    }
}
