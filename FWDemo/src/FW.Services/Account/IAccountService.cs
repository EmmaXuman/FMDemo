using FW.Compoment.Jwt.UserClaim;
using FW.Models.ViewModel;
using FW.WebCore.Core;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace FW.Services.Account
{
    public interface IAccountService : IBaseService
    {
        Task<ExecuteResult<UserData>> Login(LoginViewModel viewModel);
    }
}
