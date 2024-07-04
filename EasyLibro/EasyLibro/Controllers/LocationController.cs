
using Buisness_Logic_Layer.DTOs;
using Buisness_Logic_Layer.Interfaces;
using Microsoft.AspNetCore.Mvc;

namespace EasyLibro.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class LocationController : ControllerBase

    {
        //Create ILocationService Field
        private readonly ILocationService _locationService;

        //Constructor
        public LocationController(ILocationService locationService)
        {
            _locationService = locationService;
        }

      
    
        [HttpPost("GetAllLocation")]
        public async Task<IActionResult> GetAllLocation(LocationDto location)
        {
            return await _locationService.GetAllLocation(location.CupboardName);
        }

        [HttpPost("SearchResources")]
        public async Task<IActionResult> SearchResources(SearchbookcupDto request)
        {
            return await _locationService.SearchResources(request);
        }

        [HttpPost("AddLocation")]
        public async Task<IActionResult> AddLocation(AddLocationDto location)
        {
            return await _locationService.AddLocation(location);
        }

        [HttpDelete("DeleteLocation")]
        public async Task<IActionResult> DeleteCupboard(int cupboardid)
        {
            return await _locationService.DeleteCupboard(cupboardid);
        }
    }
}
