namespace Buisness_Logic_Layer.DTOs
{
    public class TokenRequest
    {
        public string accessToken { get; set; }
        public string refreshToken { get; set; }
    }

    public class TokenResponse
    {
        public string accessToken { get; set; }
        public string refreshToken { get; set; }
    }

}
