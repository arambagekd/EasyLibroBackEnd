using Buisness_Logic_Layer.AuthHelpers;
using Buisness_Logic_Layer.DTOs;
using Buisness_Logic_Layer.Interfaces;
using Data_Access_Layer;
using Data_Access_Layer.Entities;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Buisness_Logic_Layer.Services
{
    public class ReviewService:IReviewService
    {
        private readonly DataContext _context;
        private readonly JWTService _jWTService;
        public ReviewService(DataContext context,JWTService jWTService)
        {
            _context = context;
            _jWTService= jWTService;
        }

        public async Task<IActionResult> AddReviews(HttpContext httpContext,AddReview review)
        {
            var username = _jWTService.GetUsername(httpContext);
            if(username == null)
            {
                return new BadRequestObjectResult("User name not found");
            }
            else
            {
                var newreview = new Review
                {
                    Description = review.Description,
                    ISBN = review.ISBN,
                    reviewer = username,
                    Stars = review.Stars,
                    Date = DateOnly.FromDateTime(DateTime.Now)
                };
                await _context.Reviews.AddAsync(newreview);
                await _context.SaveChangesAsync();
            }
            return new OkObjectResult("Review Added");

        }

        public async Task<IActionResult> DeleteReview(int reviewid)
        {
            var review = await _context.Reviews.FirstOrDefaultAsync(e=>e.Id==reviewid);
            if(review == null)
            {
                return new BadRequestObjectResult("Cant Delete Review");
            }
             _context.Reviews.Remove(review);
            await _context.SaveChangesAsync();
            return new OkObjectResult(true);
    }

        public async Task<IActionResult> GetBookReviews(string isbn)
        {
            var reviews = await (from review in _context.Reviews
                                 join user in _context.Users on review.reviewer equals user.UserName
                                 where review.ISBN == isbn
                                 select new
                                 {
                                     review.Id,
                                     review.ISBN,
                                     review.Description,
                                     review.Stars,
                                     Username = user.FName+" "+user.LName,
                                     UserId=user.UserName,
                                     ImageUrl = user.Image
                                 }).ToListAsync();

            return new OkObjectResult(reviews);
        }
    }

}
