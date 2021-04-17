using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace FW.WebApi.Controllers.Base
{
    [Route("[controller]")]
    [Authorize]
    public class AuthorizeController : ControllerBase
    {
    }
}
