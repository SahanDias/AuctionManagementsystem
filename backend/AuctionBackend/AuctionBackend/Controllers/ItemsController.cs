using AuctionBackend.Data;
using AuctionBackend.Models;
using AuctionBackend.Models.Entities;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace AuctionBackend.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ItemsController : ControllerBase
    {
        private readonly ApplicationDbContext dbContext;
        private readonly IWebHostEnvironment environment;

        public ItemsController(ApplicationDbContext dbContext, IWebHostEnvironment environment)
        {
            this.dbContext = dbContext;
            this.environment = environment;
        }

        [HttpPost]
        [Route("create")]
        public async Task<IActionResult> CreateItem(CreateItemDto createItemDto)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }
            var item = new Item()
            {
                Title = createItemDto.Title,
                Description = createItemDto.Description,
                Start_Time = createItemDto.Start_Time,
                End_Time = createItemDto.End_Time,
                Start_Price = createItemDto.Start_Price,
                Min_Price_Increase = createItemDto.Min_Price_Increase,
                Category = createItemDto.Category,
            };

            if(createItemDto.Image != null)
            {
                string uploadPath = Path.Combine(environment.WebRootPath,"uploads");
                if (!Directory.Exists(uploadPath))
                {
                    Directory.CreateDirectory(uploadPath);
                }

                string filePath = Path.Combine(uploadPath, createItemDto.Image.FileName);
                using(var fileStream = new FileStream(filePath, FileMode.Create))
                {
                    await createItemDto.Image.CopyToAsync(fileStream);
                }

                item.ImageFilePath = filePath;
            }
            dbContext.Items.Add(item);
            await dbContext.SaveChangesAsync();

            return Ok(item);
        }

        [HttpGet]
        [Route("getAllItems")]
        public async Task<IActionResult> GetAllItems()
        {
            var items = await dbContext.Items.ToListAsync();
            return Ok(items);
        }

        [HttpGet]
        [Route("{id}")]
        public async Task<IActionResult> GetItemById(int id)
        {
            var item = await dbContext.Items.FindAsync(id);

            if(item == null)
            {
                return NotFound();
            }

            return Ok(item);
        }

        [HttpPut]
        [Route("{id}")]
        public async Task<IActionResult> UpdateItem(int id, UpdateItemDto updateItemDto)
        {
            var item = await dbContext.Items.FindAsync(id);

            if(item == null)
            {
                return NotFound();
            }

            item.Title = updateItemDto.Title;
            item.Description = updateItemDto.Description;
            item.Start_Time = updateItemDto.Start_Time;
            item.End_Time = updateItemDto.End_Time;
            item.Start_Price = updateItemDto.Start_Price;
            item.Min_Price_Increase = updateItemDto.Min_Price_Increase;
            item.Category = updateItemDto.Category;

            if(updateItemDto.Image != null)
            {
                string uploadPath = Path.Combine(environment.WebRootPath, "uploads");
                if(!Directory.Exists(uploadPath))
                {
                    Directory.CreateDirectory(uploadPath);
                }

                string filePath = Path.Combine(uploadPath, updateItemDto.Image.FileName);
                using(var fileStream = new FileStream(filePath, FileMode.Create))
                {
                    await updateItemDto.Image.CopyToAsync(fileStream);
                }

                item.ImageFilePath = filePath;
            }

            dbContext.Entry(item).State = EntityState.Modified;
            await dbContext.SaveChangesAsync();

            return Ok(item);
        }

        [HttpDelete]
        [Route("{id}")]
        public async Task<IActionResult> DeleteItem(int id)
        {
            var item = await dbContext.Items.FindAsync(id);

            if(item == null)
            {
                return NotFound();
            }

            dbContext.Items.Remove(item);
            await dbContext.SaveChangesAsync();

            return Ok(item);
        }
    }
}
