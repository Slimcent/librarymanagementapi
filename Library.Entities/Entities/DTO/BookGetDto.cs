namespace Library.Entities.Entities.DTO
{
    public class BookGetDto
    {
        public string Title { get; set; }
        public int ISBN { get; set; }
        public string Category { get; set; }
        public Author Author { get; set; }
    }
}
