namespace AuctionBackend.Models.Entities
{
    public class Admin
    {
        public Guid Id { get; set; }
        public required string Name { get; set; }
        public required string Password { get; set; }
    }
}
