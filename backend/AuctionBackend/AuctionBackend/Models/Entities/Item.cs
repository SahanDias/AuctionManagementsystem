using System.ComponentModel.DataAnnotations;

namespace AuctionBackend.Models.Entities
{
    public class Item
    {
        [Key]
        public int Id { get; set; }
        [Required]
        [MaxLength(100)]
        public string Title { get; set; }
        [Required]
        [MaxLength(500)]
        public string Description { get; set; }
        [Required]
        public DateTime Start_Time { get; set; }
        [Required]
        public DateTime End_Time { get; set; }
        [Required]
        [Range(0, double.MaxValue)]
        public decimal Start_Price { get; set; }
        [Required]
        [Range(0, double.MaxValue)]
        public decimal Min_Price_Increase { get; set; }
        [Required]
        public string Category { get; set; }

        public string ImageFilePath { get; set; }

        //
    }
}
