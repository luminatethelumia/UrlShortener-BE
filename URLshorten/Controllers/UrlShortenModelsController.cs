using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using ShortenURL.Abstractions;
using URLshorten.Data;
using URLshorten.Models;

namespace URLshorten.Controllers
{
    [ApiController]
    public class UrlShortenModelsController : ControllerBase
    {
        private readonly URLshortenContext _context;
        private readonly IUrlShorteningService _shortenService;

        public UrlShortenModelsController(URLshortenContext context, IUrlShorteningService urlShorteningService)
        {
            _context = context;
            _shortenService = urlShorteningService;
        }

        
        [HttpGet("/")]
        public async Task<ActionResult<IEnumerable<UrlShortenModel>>> GetUrlShortenModel()
        {
            return await _context.UrlShortenModel.ToListAsync();
        }

        
        [HttpGet("{code}")]
        public async Task<IActionResult> GetRedirectUrl(string code)
        {
            var shortenedUrl = await _context.UrlShortenModel.FirstOrDefaultAsync(predicate: u => u.Code == code);

            if (shortenedUrl == null)
            {
                return NotFound();
            }

            return Redirect(shortenedUrl.Url);
            
        }
        
        [HttpPut("/api/edit-shorten")]
        public async Task<IActionResult> PutUrlShortenModel(string code, UrlDto url)
        {
            // Find the existing record by the old code
            var existingUrl = await _context.UrlShortenModel.FirstOrDefaultAsync(u => u.ShortUrl == url.Url);

            if (existingUrl == null)
            {
                return NotFound($"No URL found.");
            }

            // check the existance of uri in db

            if (await _context.UrlShortenModel.AnyAsync(u => u.Code == code))
            {
                return BadRequest("The code is already in use!");
            }

            var result = $"{this.Request.Scheme}://{this.Request.Host}/{code}";

            existingUrl.ShortUrl = result;

            _context.UrlShortenModel.Update(existingUrl);
            await _context.SaveChangesAsync();

            return Ok(new UrlShortenResDto()
            {
                Url = result,
            });
        }

        [HttpPost("api/Shorten")]
        public async Task<ActionResult<UrlShortenModel>> PostUrlShortenModel(UrlDto url)
        {
            // validating the uri
            if (!Uri.TryCreate(url.Url, UriKind.Absolute, out var uri))
            {
                return BadRequest("Provided an invalid uri");
            }

            // check the existance of uri in db

            if (await _context.UrlShortenModel.AnyAsync(u => u.Url == url.Url))
            {
                return BadRequest("The uri is existed!");
            }

            var randomCode = await _shortenService.GenerateUniqueCode();
            var result = $"{this.Request.Scheme}://{this.Request.Host}/{randomCode}";

            var urlShorten = new UrlShortenModel()
            {
                Url = url.Url,
                ShortUrl = result,
                Code = randomCode,
                CreatedAt = DateTime.Now,
            };

            _context.UrlShortenModel.Add(urlShorten);
            await _context.SaveChangesAsync();            

            return Ok(new UrlShortenResDto()
            {
                Url = result,
            });
            
        }

        // DELETE: api/UrlShortenModels/5
        [HttpDelete("api/delete-shorten")]
        public async Task<IActionResult> DeleteUrlShortenModel(UrlDto url)
        {
            var shortenedUrl = await _context.UrlShortenModel.FirstOrDefaultAsync(u => u.ShortUrl == url.Url);// check the existance of uri in db

            if (shortenedUrl == null)
            {
                return NotFound();
            }

            _context.UrlShortenModel.Remove(shortenedUrl);
            await _context.SaveChangesAsync();

            return Ok("Removed Url Succesfully");
        }
    }
}
