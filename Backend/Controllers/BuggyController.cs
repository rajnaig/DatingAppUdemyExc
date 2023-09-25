using Backend.Data;
using Backend.Model;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace Backend.Controllers
{
    public class BuggyController : BaseApiController
    {
        private readonly AppDbContext _context;

        public BuggyController(AppDbContext context)
        {
            _context = context;
        }

        [Authorize]
        [HttpGet("auth")]
        public ActionResult<string> GetSecret()
        {
            return "secret text";
        }


        [HttpGet("not-found")]
        public ActionResult<AppUser> GetNotFound()
        {
            var user = _context.Users.Find("test");

            if (user == null) return NotFound();

            return user;
        }


        [HttpGet("server-error")]
        public ActionResult<string> GetServerError()
        {
            var user = _context.Users.Find("test");
            var thingToReturn = user.ToString();

            return thingToReturn;
        }


        [HttpGet("bad-request")]
        public ActionResult<string> GetBadRequest()
        {
            return BadRequest("nem jó");
        }
    }
}
