using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace TokenAuthDemo2.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    [Authorize]
    public class CustomerController : Controller
    {
        public IActionResult Get()
        {
            return Ok(new string[] {"customer one", "customer two", "customer three"});
        }
    }
}
