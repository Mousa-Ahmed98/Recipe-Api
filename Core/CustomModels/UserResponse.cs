namespace Core.CustomModels
{
    public class UserResponse
    {
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public string UserName { get; set; }
        public string Email { get; set; }
        public bool IsFollowed { get; set; }
        public int Followers { get; set; }
        public int Following { get; set; }
    }
}
