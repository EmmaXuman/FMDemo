using AutoMapper;
using FW.Common.IDCode;
using FW.Compoment.Jwt.UserClaim;
using FW.DbContexts;
using FW.UintOfWork.UnitOfWork;
using Microsoft.Extensions.Localization;

namespace FW.Services
{
    public interface IBaseService
    {
    }

    public class BaseService : IBaseService
    {
        public readonly IUnitOfWork<MSDbContext> _unitOfWork;
        public readonly IMapper _mapper;
        public readonly IdWorker _idWorker;
        public readonly IClaimsAccessor _claimAccessor;
        public readonly IStringLocalizer _localizer;

        public BaseService( IUnitOfWork<MSDbContext> unitOfWork, IMapper mapper, IdWorker idWorker,IClaimsAccessor claimsAccessor , IStringLocalizer localizer )
        {
            _unitOfWork = unitOfWork;
            _mapper = mapper;
            _idWorker = idWorker;
            _claimAccessor = claimsAccessor;
            _localizer = localizer;
        }
    }
}
