namespace WebLib.Models
{
    public class JwtAuthModel
    {
        public string ClientId { get; set; }
        public string Secret { get; set; }
        public string CultureCode { get; set; }
    }
}
