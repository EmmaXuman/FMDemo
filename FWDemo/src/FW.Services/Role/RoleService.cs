using AutoMapper;
using FW.Common.IDCode;
using FW.Compoment.Jwt.UserClaim;
using FW.DbContexts;
using FW.Entities;
using FW.Models.ViewModel;
using FW.UintOfWork.UnitOfWork;
using FW.WebCore.Core;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace FW.Services
{
    public class RoleService : BaseService, IRoleService
    {
        public RoleService( IUnitOfWork<MSDbContext> unitOfWork, IMapper mapper, IdWorker idWorker,IClaimsAccessor claimsAccessor ) : base(unitOfWork, mapper, idWorker, claimsAccessor)
        {
        }

        public async Task<ExecuteResult<Role>> Create( RoleViewModel viewModel )
        {
            ExecuteResult<Role> result = new ExecuteResult<Role>();

            if (viewModel.CheckField(ExecuteType.Create, _unitOfWork) is ExecuteResult checkResult && !checkResult.IsSucceed)
            {
                return result.SetFailMessage(checkResult.Message);
            }

            using (var tran = _unitOfWork.BeginTransaction())
            {
                Role newRow = _mapper.Map<Role>(viewModel);
                newRow.Id = _idWorker.NextId();//获取一个雪花id
                newRow.Creator = _claimAccessor.UserId;
                newRow.CreateTime = DateTime.Now;
                _unitOfWork.GetRepository<Role>().Insert(newRow);
                await _unitOfWork.SaveChangesAsync();
                await tran.CommitAsync();//提交事务

                result.SetData(newRow);//添加成功，把新的实体返回回去
            }
            return result;
        }

        public async Task<ExecuteResult> Delete( RoleViewModel viewModel )
        {
            ExecuteResult result = new ExecuteResult();
            if (viewModel.CheckField(ExecuteType.Delete, _unitOfWork) is ExecuteResult checkResult && !checkResult.IsSucceed)
            {
                return checkResult;
            }
            _unitOfWork.GetRepository<Role>().Delete(viewModel.Id);
            await _unitOfWork.SaveChangesAsync();

            return result;
        }

        public async Task<ExecuteResult> Update( RoleViewModel viewModel )
        {
            ExecuteResult result = new ExecuteResult();

            if (viewModel.CheckField(ExecuteType.Update, _unitOfWork) is ExecuteResult checkResult && !checkResult.IsSucceed)
            {
                return checkResult;
            }

            var row = await _unitOfWork.GetRepository<Role>().FindAsync(viewModel.Id);

            row.Name = viewModel.Name;
            row.DisplayName = viewModel.DisplayName;
            row.Remark = viewModel.Remark;
            row.Modifier = _claimAccessor.UserId;
            row.ModifyTime = DateTime.Now;
            _unitOfWork.GetRepository<Role>().Update(row);
            await _unitOfWork.SaveChangesAsync();//提交

            return result;
        }
    }
}
