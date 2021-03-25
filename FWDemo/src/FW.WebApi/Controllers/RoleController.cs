using FW.Models.ViewModel;
using FW.Services;
using FW.WebCore.Core;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace FW.WebApi.Controllers
{
    [Route("[controller]")]
    [ApiController]
    public class RoleController : ControllerBase
    {
        private readonly IRoleService _roleService;

        public RoleController( IRoleService roleService )
        {
            _roleService = roleService;
        }

        [HttpPost]
        public async Task<ExecuteResult> Post( RoleViewModel viewModel )
        {
            return await _roleService.Create(viewModel);
        }


        [HttpPut]
        public async Task<ExecuteResult> Put( RoleViewModel viewModel )
        {
            return await _roleService.Update(viewModel);
        }

        [HttpDelete]
        public async Task<ExecuteResult> Delete( long id )
        {
            return await _roleService.Delete(new RoleViewModel { Id = id });
        }
    }
}
