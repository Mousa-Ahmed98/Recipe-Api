namespace Infrastructure.Common
{
    public class RegisterResponse
    {
        public bool Success { get; set; }
        public string Message { get; set; }
        public UserResponse? User { get; set; }
    }
}
