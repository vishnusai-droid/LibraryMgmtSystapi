namespace LibraryManagementAPI.Models
{
    public class Book
    {
        public string Title { get; set; }
        public string Author { get; set; }
        public string ISBN { get; set; }
        public bool Available { get; set; } = true;
    }
}