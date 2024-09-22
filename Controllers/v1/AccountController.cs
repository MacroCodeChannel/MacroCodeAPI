using Asp.Versioning;
using BC.NAV;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System.Globalization;
using System.Reflection;

namespace MacroCodeAPI.Controllers.v1
{
    [ApiVersion("1.0")]
    [Route("api/v{version:apiVersion}/[controller]")]
    [ApiController]
    public class AccountController : ControllerBase
    {
        public AccountController()
        {

        }

        [HttpGet(Name = "GetToken")]
        public async Task Token()
        {
           
        }
    }
}
