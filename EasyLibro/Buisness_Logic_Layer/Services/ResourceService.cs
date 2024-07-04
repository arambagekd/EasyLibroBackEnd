using Buisness_Logic_Layer.AuthHelpers;
using Buisness_Logic_Layer.DTOs;
using Buisness_Logic_Layer.Interfaces;
using Data_Access_Layer;
using Data_Access_Layer.Entities;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace Buisness_Logic_Layer.Services
{
    public class ResourceService : IResourceService
    {

        private readonly DataContext _Context;
        private readonly JWTService _jWTService;
        private readonly INotificationService _notificationService;

        //Contructor of the ResourceService
        public ResourceService(DataContext Context,JWTService jWTService,INotificationService notificationService)
        {
            _Context = Context;
            _jWTService = jWTService;
            _notificationService= notificationService;
        }

        public async Task<IActionResult> AddResource(AddBookRequestDto book,HttpContext httpContext)
        {
            var addedby = _jWTService.GetUsername(httpContext);
            var resource = await _Context.Resources.FirstOrDefaultAsync(u => u.ISBN == book.ISBN); 
            
            if (resource == null)// Check resource already in DB
            {
                if (await _Context.Author.FirstOrDefaultAsync(u => u.AuthorName == book.Author) == null) //Check is he new author
                {
                    var auth = new Author
                    {
                        AuthorName = book.Author
                    };
                    _Context.Author.Add(auth);   //Add the Author
                    await _Context.SaveChangesAsync();
                }

                var a = new Resource();


                var reso = new Resource //Make Resource
                {
                    ISBN = book.ISBN,
                    Title = book.Title,
                    AuthorName = book.Author,
                    Type = book.Type,
                    Quantity = book.Quantity,
                    Borrowed = 0,
                    Year=book.Year,
                    Description = book.Description,
                    Price = book.Price,
                    PageCount = book.Pages,
                    AddedOn = DateTime.UtcNow,
                    ImageURL = book.ImagePath,
                    AddedByID = addedby,
                    BookLocation =  book.CupboardId+"-"+book.ShelfNo,
                };
                _Context.Resources.Add(reso); //Add the Resource
                await _Context.SaveChangesAsync();

                var response = new AddBookResponseDto //Make Response
                {
                    ISBN = reso.ISBN,
                    Title = reso.Title
                };

                return new OkObjectResult( response); //Return Response

            }
            else
            {
                return new BadRequestObjectResult("Resource Already Exists");
            }
        }

        public async Task<IActionResult> DeleteResource(string isbn)
        {
            var reso = await _Context.Resources.FirstOrDefaultAsync(u => u.ISBN == isbn);
            if (reso != null)
            {
                _Context.Resources.Remove(reso);
                await _Context.SaveChangesAsync();
                return new OkObjectResult( true);
            }
            else
            {
                return new BadRequestObjectResult("No Resource Found");
            }
        }

        public async Task<List<ResourceListDto>> SearchResources(SearchbookDto searchbookDto)
        {
            var records = new List<Resource>();
            List<ResourceListDto> reso = new List<ResourceListDto>();


            if (searchbookDto.keyword == "")
            {
                records = _Context.Resources.ToList();
            }
            if (searchbookDto.tag == "all")
            {
                records = _Context.Resources.Where(e =>
                    e.Title.ToLower().Contains(searchbookDto.keyword.ToLower())||
                   e.ISBN.ToLower().Contains(searchbookDto.keyword.ToLower()) ||
                   e.AuthorName.ToLower().Contains(searchbookDto.keyword.ToLower()) 
                   ).ToList();
            }
            else if (searchbookDto.tag == "title")
            {
                records = _Context.Resources.Where(e => e.Title.ToLower().Contains(searchbookDto.keyword.ToLower())).ToList();
            }
            else if (searchbookDto.tag == "isbn")
            {
                records = _Context.Resources.Where(e => e.ISBN.ToLower().Contains(searchbookDto.keyword.ToLower())).ToList();
            }
            else if (searchbookDto.tag == "author")
            {
                records = _Context.Resources.Where(e => e.AuthorName.ToLower().Contains(searchbookDto.keyword.ToLower())).ToList();
            }

            if (searchbookDto.type != "all")
            {
                records = records.Where(e => e.Type.ToLower() == searchbookDto.type.ToLower()).ToList();
            }


            foreach (var x in records)
            {
                int count = _Context.Reservations.Where(e => e.ResourceId == x.ISBN).Count();
                int ratings=_Context.Reviews.Where(e=>e.ISBN==x.ISBN).Sum(e=>e.Stars);
                int no_of_ratings = _Context.Reviews.Where(e => e.ISBN == x.ISBN).Count();
                double raating = 0;
                if(no_of_ratings==0
                    ) {
                    raating = 0;
                }
                else 
                {
                    raating = (double)ratings / no_of_ratings;
                }
                var y = new ResourceListDto
                {
                    isbn = x.ISBN,
                    title = x.Title,
                    noOfBooks = x.Quantity+x.Borrowed,
                    url = x.ImageURL,
                    type = x.Type,
                    remain=x.Quantity,
                    dateadded=x.AddedOn,
                    noOfRes=count,
                    author = x.AuthorName,
                    location = x.BookLocation,
                    year=x.Year,
                    ratings=raating
                };

                reso.Add(y);
            }
            return reso;
        }


        public async Task<IActionResult> EditResource(AddBookRequestDto book)
        {
            var resource = await _Context.Resources.FirstOrDefaultAsync(u => u.ISBN == book.ISBN);

            if (resource != null)
            {
                
                    resource.Title = book.Title;
                
                    resource.Price = book.Price;
                
                    resource.Quantity = book.Quantity;
                
                    resource.BookLocation =book.CupboardId.ToString()+"-"+book.ShelfNo.ToString();
                
                    resource.PageCount = book.Pages;
               
                    resource.ImageURL = book.ImagePath;
               
                    resource.Type = book.Type;

                    resource.ImageURL = book.ImagePath;

                    resource.Description = book.Description;

                    resource.Year=book.Year;

               
                    var author = await _Context.Author.FirstOrDefaultAsync(u => u.AuthorName == book.Author);
                    if (author == null)
                    {
                        var newauthor = new Author();
                        newauthor.AuthorName = book.Author;
                        _Context.Author.Add(newauthor);
                    }
                    resource.AuthorName = book.Author;

              
                await _Context.SaveChangesAsync();
                return new OkObjectResult( true);
            }
            else
            {
                return new BadRequestObjectResult("Resource not found");
            }
        }

        public async Task<IActionResult> AboutResource(string isbn)
        {
            var resource = await _Context.Resources.FirstOrDefaultAsync(u => u.ISBN == isbn);
            if(resource == null)
            {
                return new BadRequestObjectResult("Book Not Found");
            }
            else
            {
                var location= await _Context.Locations.FirstOrDefaultAsync(u => u.LocationNo == resource.BookLocation);
                var cupboard = await _Context.Cupboard.FirstOrDefaultAsync(u => u.cupboardID == location.CupboardId);
                var res = new AboutResourceDto
                {
                        ISBN=resource.ISBN,
                        Type=resource.Type,
                        Title=resource.Title,
                        Author=resource.AuthorName,
                        Remain=resource.Quantity,
                        borrowed=resource.Borrowed,
                        total=resource.Quantity+resource.Borrowed,
                        CupboardId=cupboard.cupboardID.ToString(), 
                        CupboardName=cupboard.name,
                        year=resource.Year,
                        ShelfId=location.ShelfNo.ToString(),
                        Description=resource.Description,
                        pages=resource.PageCount,
                        price=resource.Price,
                        addedon=resource.AddedOn,
                        Imagepath=resource.ImageURL
                };
                return new OkObjectResult(res);
            }
        }

        public async Task WeeklyBookUpdates()
        {
            var oneWeekAgo = DateTime.Now.AddDays(-7);
            var count=await _Context.Resources.Where(e=>e.AddedOn>= oneWeekAgo).CountAsync();
            Console.WriteLine("Hiiiiiiiiiiiiiiiiiiiiiiiiiiiiiiiiiiiiiiii");
            if (count > 0)
            {
                await _notificationService.BookAddedNotifications();
            }
        }


        public async Task<IActionResult> GetAuthors()
        {
            var authorlist = await _Context.Author.ToListAsync();
            return new OkObjectResult(authorlist);
        }

        public async Task<IActionResult> GetBookTypes()
        {
            var types = await _Context.Resources
                            .Select(r => r.Type)
                            .Distinct()
                            .ToListAsync();
            return new OkObjectResult( types);
        }
    }
}
