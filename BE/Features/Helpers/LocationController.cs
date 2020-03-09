using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace BE.Features.Helpers
{
    [ApiController]
    [Route("api/location")]
    public class LocationController : ControllerBase
    {
        private ILocationService _locationService;

        public LocationController(ILocationService locationService)
        {
            _locationService = locationService;
        }

        [HttpGet("city/by-keyword")]
        public IActionResult FindCityByKeyword([FromQuery(Name = "keyword")] string keyword)
        {
            var res = _locationService.GetByKeyword(keyword);
            if (res == null) return NotFound();
            return Ok(res);
        }

    }
}
