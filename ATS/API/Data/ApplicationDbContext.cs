using Microsoft.EntityFrameworkCore;
using ATS.Api.Models;

namespace API.Data
{
    public class ApplicationDbContext : DbContext
    {
        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options) : base(options) { }

        public DbSet<ApiLoginModel> Users { get; set; }
    }
}
