using Backend.Data;
using Backend.Helpers;
using Microsoft.AspNetCore.Mvc;

namespace Backend.Controllers
{
    [ServiceFilter(typeof(LogUserActivity))]
    [ApiController]
    [Route("[controller]")]
    public class BaseApiController : ControllerBase
    {
        public BaseApiController()
        {

        }
    }
}
