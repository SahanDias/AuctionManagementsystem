using System.ComponentModel.DataAnnotations;

namespace AuctionBackend.Models
{
    public class CreateItemDto
    {
        public required string Title { get; set; }
        public required string Description { get; set; }
        public required DateTime Start_Time { get; set; }
        public required DateTime End_Time { get; set; }
        public required decimal Start_Price { get; set; }
        public required decimal Min_Price_Increase { get; set; }
        public required string Category { get; set; }
        public required IFormFile Image { get; set; }
    }
}
