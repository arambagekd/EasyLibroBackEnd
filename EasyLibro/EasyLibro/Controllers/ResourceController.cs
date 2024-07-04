
using Buisness_Logic_Layer.DTOs;
using Buisness_Logic_Layer.Interfaces;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace EasyLibro.Controllers
{
  //  [Authorize]
    [Route("api/[controller]")]
    [ApiController]
    public class ResourceController : ControllerBase
    {
             //Create IResourceService Field
             private readonly IResourceService _resourceService;

        public ResourceController(IResourceService resourceService)
        {
            _resourceService = resourceService;
        }

        [HttpPost("AddResource")]
        public async Task<IActionResult> AddResource(AddBookRequestDto book)
        {
            var x = HttpContext;
            return await _resourceService.AddResource(book,x);
        }

        [HttpPut("EditResource")]
        public async Task<IActionResult> EditResource(AddBookRequestDto book)
        {
            return await _resourceService.EditResource(book);
        }

        [HttpGet("DeleteResource")]
        public async Task<IActionResult> DeleteResource(string isbn)
        {
            return await _resourceService.DeleteResource(isbn);
        }

        [HttpPost("AbouteResource")]
        public async Task<IActionResult> AboutResource(string isbn)
        {
            return await _resourceService.AboutResource(isbn);
        }


        [HttpPost("SearchResources")]
        public async Task<List<ResourceListDto>> SearchResources(SearchbookDto searchbookDto)
        {
            return await _resourceService.SearchResources(searchbookDto);
        }

        [HttpGet("GetAuthorList")]
        public async Task<IActionResult> GetAuthorList()
        {
           return await _resourceService.GetAuthors();
        }

        [HttpGet("GetTypes")]
        public async Task<IActionResult> GetBookTypes()
        {
            return await _resourceService.GetBookTypes();
        }


    }
   
}
