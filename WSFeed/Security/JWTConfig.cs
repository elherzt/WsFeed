namespace WSFeed.Security
{
    public class JWTConfig
    {
        public string SecretKey { get; set; }
        public int ExpirationMinutes { get; set; }
        public string Issuer { get; set; }
        public string Audience { get; set; }
    }
}
