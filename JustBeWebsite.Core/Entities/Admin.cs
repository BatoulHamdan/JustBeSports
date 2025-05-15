namespace JustBeSports.Core.Entities
{
    public partial class Admin
    {
        public int Id { get; set; } 
        public string Username { get; set; } 
        public string PasswordHash { get; set; }
        public string Role { get; set; } 
    }
}
