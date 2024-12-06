namespace ShortenURL.Abstractions
{
    public interface IUrlShorteningService
    {
        public Task<string> GenerateUniqueCode();
    }
}
