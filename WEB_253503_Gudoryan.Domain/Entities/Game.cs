namespace WEB_253503_Gudoryan.Domain.Entities
{
    public class Game
    {
        public int Id { get; set; }

        public string Name { get; set; }

        public string Description { get; set; }

        public Category? Category { get; set; }

        public decimal Price { get; set; }

        public string? ImagePath { get; set; }

        public string? MimeType { get; set; }


    }
}
