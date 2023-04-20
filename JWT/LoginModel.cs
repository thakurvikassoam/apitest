using Newtonsoft.Json;

namespace CFAWebApi.JWT
{
    public class LoginModel
    {
        [JsonProperty("userName")]
        public string UserName { get; set; }
        [JsonProperty("password")]
        public string Password { get; set; }
    }
}
