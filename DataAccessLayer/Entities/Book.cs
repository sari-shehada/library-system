namespace DataAccessLayer.Entities
{
    public class Book
    {
        public Guid Id { get; set; } = Guid.NewGuid();
        public string Title { get; set; } = string.Empty;
        public string Author { get; set; } = string.Empty;
        public bool IsBorrowed { get; set; } = false;

        public String ISBN { get; set; } = string.Empty;

        public override string ToString()
        {
            return Id + "," + Title + "," + Author + "," + IsBorrowed + "," + ISBN;
        }
    }
}