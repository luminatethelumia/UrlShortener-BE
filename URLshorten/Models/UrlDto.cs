namespace URLshorten.Models
{
    public class UrlDto
    {
        public string? Url { get; set; }

        public string? OldUrl { get; set; }
        public string? NewUrl { get; set; }
    }
}
