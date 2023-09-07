namespace Infrastructure.Common
{
    public class TokenConfiguration
    {
        public string Secret { get; set; } 
        public string Issuer { get; set; }
        public string Audience { get; set; }
        public double DurationInMinutes { get; set; }
        public string Algorithm { get; set; }
    }
}
