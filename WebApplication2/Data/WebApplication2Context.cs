using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using WebApplication2.Models;

namespace WebApplication2.Data
{
    public class WebApplication2Context : DbContext
    {
        public WebApplication2Context (DbContextOptions<WebApplication2Context> options)
            : base(options)
        {
        }

        public DbSet<WebApplication2.Models.GutterPart> GutterPart { get; set; } = default!;
        public DbSet<WebApplication2.Models.Parts> Parts { get; set; } = default!;
        public DbSet<WebApplication2.Models.Parts> PartsAll { get; set; } = default!;
        public DbSet<WebApplication2.Models.Orders> Orders { get; set; } = default!;
    }
}
