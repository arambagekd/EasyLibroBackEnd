using Buisness_Logic_Layer.DTOs;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Buisness_Logic_Layer.Interfaces
{
    public interface IReviewService
    {
        Task<IActionResult> AddReviews(HttpContext httpContext, AddReview review);
        Task<IActionResult> DeleteReview(int reviewid);
        Task<IActionResult> GetBookReviews(string isbn);
    }
}
