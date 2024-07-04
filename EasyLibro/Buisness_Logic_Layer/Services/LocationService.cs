using Buisness_Logic_Layer.DTOs;
using Buisness_Logic_Layer.Interfaces;
using Data_Access_Layer;
using Data_Access_Layer.Entities;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Runtime.InteropServices;

namespace Buisness_Logic_Layer.Services
{
    public class LocationService : ILocationService
    {
        private readonly DataContext _Context;
        private readonly IResourceService _resourceService;
        public LocationService(DataContext context, IResourceService resourceService)
        {
            _Context = context;
            _resourceService = resourceService;
        }
        public async Task<IActionResult> GetAllLocation(string cupboardname)
        {
            var locations = await _Context.Locations.ToListAsync();

            // Group by CupboardId and process each group
            var locationListDtos = new List<LocationListDto>();

            foreach (var group in locations.GroupBy(cs => cs.CupboardId))
            {
                // Fetch the cupboard info asynchronously
                var cupboard = await _Context.Cupboard.FirstOrDefaultAsync(e => e.cupboardID == group.Key);

                // Create the LocationListDto object

                var Quantity = _Context.Cupboard
                .Where(c => c.cupboardID == cupboard.cupboardID)
                .Join(
                    _Context.Locations,
                    c => c.cupboardID,
                    l => l.CupboardId,
                    (c, l) => new { Cupboard = c, Locations = l })
                .Join(
                    _Context.Resources,
                    cl => cl.Locations.LocationNo,
                    r => r.BookLocation,
                    (cl, r) => r.Quantity)
                .Sum();

                var Borrow = _Context.Cupboard
              .Where(c => c.cupboardID == cupboard.cupboardID)
              .Join(
                  _Context.Locations,
                  c => c.cupboardID,
                  l => l.CupboardId,
                  (c, l) => new { Cupboard = c, Locations = l })
              .Join(
                  _Context.Resources,
                  cl => cl.Locations.LocationNo,
                  r => r.BookLocation,
                  (cl, r) => r.Borrowed)
              .Sum();

                var locationListDto = new LocationListDto
                {
                    CupboardId = cupboard.cupboardID.ToString(),
                    CupboardName = cupboard.name,
                    ShelfNo = group.Select(cs => cs.ShelfNo.ToString()).ToList(),
                    count = 0,
                    remain=Quantity,
                    borrowed=Borrow
                };

                locationListDtos.Add(locationListDto);
            }
            if (cupboardname != "")
                locationListDtos = locationListDtos.Where(e => e.CupboardName.ToLower().Contains(cupboardname.ToLower())).ToList();

            return new OkObjectResult(locationListDtos);
        }

        public async Task<IActionResult> SearchResources(SearchbookcupDto request)
        {
            var resources = new List<ResourceListDto>();
            

            var req = new SearchbookDto
            {
                keyword = request.keyword,
                tag = request.tag,
                type = request.type
            };
        
            resources = (List<ResourceListDto>)await _resourceService.SearchResources(req);
            Console.WriteLine("hiiiiiiiiiiiiiiiiiiiiiiiiiiiiiiii");

            resources = resources.Where(e => e.location == request.location).ToList();

            return new OkObjectResult(resources);
        }

        public async Task<IActionResult> AddLocation(AddLocationDto location)
        {
            var cupboard = await _Context.Cupboard.FirstOrDefaultAsync(e => e.name == location.CupboardName);

            if (cupboard == null)
            {
                cupboard = new Cupboard
                {
                    name = location.CupboardName
                };

                await _Context.Cupboard.AddAsync(cupboard);
                await _Context.SaveChangesAsync();

                for (int i = 1; i <= location.ShelfNo; i++)
                {
                    await _Context.Locations.AddAsync(new Location
                    {
                        LocationNo = cupboard.cupboardID.ToString() + "-" + i.ToString(),
                        CupboardId = cupboard.cupboardID,
                        ShelfNo = i
                    });
                    await _Context.SaveChangesAsync();
                }

                return new OkResult();
            }
            else
            {
                return new BadRequestObjectResult("Location already exists.");
            }
        }

        public async Task<IActionResult> DeleteCupboard(int cupboardId)
        {
            // Fetch the cupboard asynchronously
            var cupboard = await _Context.Cupboard.FirstOrDefaultAsync(e => e.cupboardID == cupboardId);

            if (cupboard == null)
            {
                return new BadRequestObjectResult("Cupboard is not defined");
            }

            // Get the count of resources in the specified cupboard
            var count = await _Context.Resources
                                      .Where(r => _Context.Locations.Any(l => l.CupboardId == cupboardId && l.LocationNo == r.BookLocation))
                                      .CountAsync();
            // If there are no resources, delete the cupboard and its locations
            if (count == 0)
            {
                Console.WriteLine(count);
                var deletedLocations = await _Context.Locations.Where(l => l.CupboardId == cupboardId).ToListAsync();
                _Context.Locations.RemoveRange(deletedLocations);

                _Context.Cupboard.Remove(cupboard);
                await _Context.SaveChangesAsync();

                return new OkObjectResult("Successfully Deleted");
            }
            else
            {
                return new BadRequestObjectResult("Can't delete cupboard with books");
            }
        }
    }
    }
