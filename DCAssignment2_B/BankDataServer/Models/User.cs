namespace BankDataServer.Models
{
    public class User
    {
        public uint UserId { get; set; }
        public string? Name { get; set; }
        public string? Email { get; set; }
        public string? Address { get; set; }
        public ulong PhoneNo { get; set; }
        public string? Password { get; set; }
        public string? UserType {  get; set; }

    }
}
