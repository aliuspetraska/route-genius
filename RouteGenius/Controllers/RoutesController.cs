using System;
using Microsoft.AspNetCore.Mvc;
using RouteGenius.Models;
using RouteGenius.Services;

namespace RouteGenius.Controllers
{
    [Route("api/[controller]")]
    public class RoutesController : Controller
    {
        private readonly RoutesService _routesService;

        public RoutesController()
        {
            _routesService = new RoutesService();
        }
        
        [HttpGet]
        public JsonResult Get()
        {
            var parameters = new RequestParameters
            {
                StartFrom = new Coordinates
                {
                    Lat = 54.69422,
                    Lng = 25.28386
                },
                LengthInMeters = 25000,
                TravelMode = 0,
                TravelDirection = 0,
                TravelHeading = 0,
                AvoidHighways = true,
                AvoidFerries = true,
                UnitSystem = 0
            };

            var routes = _routesService.GetRoutes(parameters, 10);
            
            return Json(routes.Result);
        }
    }
}