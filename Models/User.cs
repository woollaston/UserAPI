namespace UserAPI.Models
{
    public class User
    {
        public required Guid Id { get; set; }
        public required string FirstName { get; set; }
        public required string LastName { get; set; }
        public required string DateOfBirth { get; set; }
        public required string Address { get; set; }

    }
}
