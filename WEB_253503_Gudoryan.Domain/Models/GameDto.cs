using WEB_253503_Gudoryan.Domain.Entities;

namespace WEB_253503_Gudoryan.Domain.Models
{
    public class GameDto
    {
        public int Id { get; set; }

        public string Name { get; set; }

        public string Description { get; set; }

        public int CategoryId { get; set; }

        public decimal Price { get; set; }

        public string? ImagePath { get; set; }

        public string? MimeType { get; set; }
    }
}
