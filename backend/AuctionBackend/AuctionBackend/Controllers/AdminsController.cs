using AuctionBackend.Data;
using AuctionBackend.Models;
using AuctionBackend.Models.Entities;
using Azure.Core.Pipeline;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System.ComponentModel.DataAnnotations;

namespace AuctionBackend.Controllers
{
    //localhost:xxxx/api/admins
    [Route("api/[controller]")]
    [ApiController]
    public class AdminsController : ControllerBase
    {
        private readonly ApplicationDbContext dbContext;

        public AdminsController(ApplicationDbContext dbContext)
        {
            this.dbContext = dbContext;
        }


        [HttpGet]
        public IActionResult GetAllAdmins()
        {
            return Ok(dbContext.Admins.ToList());
        }

        [HttpGet]
        [Route("{id:guid}")]
        public IActionResult GetAdminsById(Guid id)
        {
            var admin = dbContext.Admins.Find(id);
            if(admin is null)
            {
                return NotFound();
            }

            return Ok(admin);
        }

        [HttpPost]
        public IActionResult AddAdmin(AddAdminDto addAdminDto)
        {
            var adminEntity = new Admin()
            {
                Name = addAdminDto.Name,
                Password = addAdminDto.Password
            };

            dbContext.Admins.Add(adminEntity);
            dbContext.SaveChanges();

            return Ok(adminEntity);
        }

        [HttpPut]
        [Route("{id:guid}")]
        public IActionResult UpdateAdmin(Guid id, UpdartrAdminDto updartrAdminDto) 
        {
            var admin = dbContext.Admins.Find(id);

            if(admin is null)
            {
                return NotFound();
            }

            admin.Name = updartrAdminDto.Name;
            admin.Password = updartrAdminDto.Phone;

            dbContext.SaveChanges();
            return Ok(admin);
        }

        [HttpDelete]
        [Route("{id:guid}")]
        public IActionResult DeleteAdmin(Guid id)
        {
            var admin = dbContext.Admins.Find(id);
            if(admin is null)
            {
                return NotFound();
            }

            dbContext.Admins.Remove(admin);
            dbContext.SaveChanges();

            return Ok();
        }
    }
}
