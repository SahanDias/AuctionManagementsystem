using AuctionBackend.Models.Entities;
using Microsoft.EntityFrameworkCore;

namespace AuctionBackend.Data
{
    public class ApplicationDbContext : DbContext
    {
        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options) : base(options)
        {  
        }

        public DbSet<Admin> Admins { get; set; }
        public DbSet<User> Users { get; set; }
        public DbSet<Item> Items { get; set; }
    }
}
