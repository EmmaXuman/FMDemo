using FW.Compoment.Jwt.UserClaim;
using FW.Models.RequestModel;
using FW.Services.Account;
using FW.WebApi.Controllers.Base;
using FW.WebCore.Core;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Threading.Tasks;

namespace FW.WebApi.Controllers
{
    [ApiController]
    public class AccountController : AuthorizeController
    {
        private readonly IAccountService _accountService;

        public AccountController( IAccountService accountService )
        {
            _accountService = accountService;
        }

        [HttpPost]
        [AllowAnonymous]
        public async Task<ExecuteResult<UserData>> Login( LoginReq req )
        {
            return await _accountService.Login(req);
        }
    }
}
