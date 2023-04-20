using System;

namespace CFAWebApi.JWT.Services
{
    public class RefreshToken
    {
        public string Token { get; set; }
        public string JwtId { get; set; }
        public string Name { get; set; }
        public DateTime CreationDate { get; set; }
        public DateTime ExpiryDate { get; set; }
    }
}