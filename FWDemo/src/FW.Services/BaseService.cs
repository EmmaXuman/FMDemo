using AutoMapper;
using FW.Common.IDCode;
using FW.DbContexts;
using FW.UintOfWork.UnitOfWork;

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

        public BaseService( IUnitOfWork<MSDbContext> unitOfWork, IMapper mapper, IdWorker idWorker )
        {
            _unitOfWork = unitOfWork;
            _mapper = mapper;
            _idWorker = idWorker;
        }
    }
}
