using FW.Entities;
using FW.Models.ViewModel;
using FW.WebCore.Core;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace FW.Services
{
    public interface IRoleService:IBaseService
    {
        Task<ExecuteResult<Role>> Create( RoleViewModel viewModel );
        Task<ExecuteResult> Update( RoleViewModel viewModel );
        Task<ExecuteResult> Delete( RoleViewModel viewModel );
    }
}
