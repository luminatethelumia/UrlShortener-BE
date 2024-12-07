namespace URLshorten.Models
{
    public class UrlShortenModel
    {
        public int Id { get; set; }
        public string? Url { get; set; }
        public string? ShortUrl { get; set; }

        public string? Code { get; set; }

        public byte[]? QRImage { get; set; }

        public DateTime? CreatedAt { get; set; }
    }
}
