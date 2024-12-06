using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using URLshorten.Models;

namespace URLshorten.Data
{
    public class URLshortenContext : DbContext
    {
        public URLshortenContext (DbContextOptions<URLshortenContext> options)
            : base(options)
        {
        }

        public DbSet<URLshorten.Models.UrlShortenModel> UrlShortenModel { get; set; } = default!;
    }
}
