using System;
using System.Collections.Generic;
using System.Text;

namespace FW.Compoment.Jwt.UserClaim
{
    public interface IClaimsAccessor
    {
        string UserName { get; }
        long UserId { get; }
        string UserAccount { get; }
        string UserRole { get; }
        string UserRoleDisplayName { get; }
    }
}
