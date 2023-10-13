using Core.CustomModels;

namespace Infrastructure.Common
{
    public record RegisterResponse
    {
        public bool Success { get; set; }
        public string Message { get; set; }
        public UserResponse? User { get; set; }
    }
}
