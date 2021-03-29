using AutoMapper;
using FW.Common.IDCode;
using FW.Compoment.Jwt;
using FW.Compoment.Jwt.UserClaim;
using FW.DbContexts;
using FW.Models.ViewModel;
using FW.UintOfWork.UnitOfWork;
using FW.WebCore;
using FW.WebCore.Core;
using Microsoft.Extensions.Options;
using System.Threading.Tasks;

namespace FW.Services.Account
{
    public class AccountService : BaseService, IAccountService
    {
        private readonly JwtService _jwtService;
        private readonly SiteSetting _siteSetting;

        public AccountService( JwtService jwtService, IOptions<SiteSetting> options, IUnitOfWork<MSDbContext> unitOfWork, IMapper mapper, IdWorker idWorker, IClaimsAccessor claimsAccessor ) : base(unitOfWork, mapper, idWorker, claimsAccessor)
        {
            _jwtService = jwtService;
            _siteSetting = options.Value;
        }

        public async Task<ExecuteResult<UserData>> Login( LoginViewModel viewModel )
        {
            var result = await viewModel.LoginValidate(_unitOfWork, _mapper, _siteSetting);
            if (result.IsSucceed)
            {
                result.Result.Token = _jwtService.BuildToken(_jwtService.BuildClaims(result.Result));
                return new ExecuteResult<UserData>(result.Result);
            }
            else
            {
                return new ExecuteResult<UserData>(result.Message);
            }
        }
    }
}
