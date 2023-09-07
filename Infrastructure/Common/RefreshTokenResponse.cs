namespace Infrastructure.Common
{
    public record RefreshTokenResponse
    {
        public string Token { get; set; }
        public DateTime ExpDate { get; set; }
    }
}
