using FW.Compoment.Jwt.UserClaim;
using FW.Models.RequestModel;
using FW.WebCore.Core;
using System.Threading.Tasks;

namespace FW.Services.Account
{
    public interface IAccountService : IBaseService
    {
        Task<ExecuteResult<UserData>> Login( LoginReq req );
    }
}
