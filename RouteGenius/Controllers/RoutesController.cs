using Microsoft.AspNetCore.Mvc;

namespace RouteGenius.Controllers
{
    [Route("api/[controller]")]
    public class RoutesController : Controller
    {
        [HttpGet]
        public JsonResult Get()
        {
            
            
            return Json(new object());
        }
    }
}