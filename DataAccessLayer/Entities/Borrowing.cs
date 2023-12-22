namespace DataAccessLayer.Entities
{
    public class Borrowing
    {
        public Guid Id { get; set; } = Guid.NewGuid();
        public Guid UserId { get; set; }
        public Guid BookId { get; set; }
        public DateOnly BorrowDate { get; set; } = DateOnly.FromDateTime(DateTime.UtcNow);
        public DateOnly? ReturnDate { get; set; }
    }
}