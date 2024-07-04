using Buisness_Logic_Layer.DTOs;
using Buisness_Logic_Layer.Interfaces;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace Web_Api.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ReviewController : ControllerBase
    {
        private readonly IReviewService _reviewService;
        public ReviewController(IReviewService reviewService)
        {
            _reviewService = reviewService;
        }
        [HttpGet("GetBookReviews")]
        public async Task<IActionResult> GetBookReviews(string isbn)
        {
            return await _reviewService.GetBookReviews(isbn);
        }

        [HttpPost("AddReview")]
        public async Task<IActionResult> AddReviews(AddReview review) 
        {
            var x = HttpContext;
            return await _reviewService.AddReviews(x, review);
        }

        [HttpDelete("DeleteReview")]
        public async Task<IActionResult> DeleteReview(int reviewid) 
        {
            return await _reviewService.DeleteReview(reviewid);
        }
    }
}
