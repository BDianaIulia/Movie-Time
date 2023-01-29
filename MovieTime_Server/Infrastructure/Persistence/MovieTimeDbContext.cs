using Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.AspNetCore.Identity;

namespace Infrastructure.Persistence
{
    public class MovieTimeDbContext : IdentityDbContext<IdentityUser>
    {
        public MovieTimeDbContext(): base()
        {

        }
        public MovieTimeDbContext(DbContextOptions<MovieTimeDbContext> options, IConfiguration configuration): base(options)
        {
            _configuration = configuration;
        }

        private readonly IConfiguration _configuration;
        public virtual DbSet<Movie> Movie { get; set; }

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            optionsBuilder.UseSqlServer(_configuration.GetConnectionString("DbConnection"));
        }
        protected override void OnModelCreating(ModelBuilder builder)
        {
            builder.Entity<Movie>().ToTable("Movie");
            builder.Entity<Genre>().ToTable("Genre");
            builder.Entity<Language>().ToTable("Language");
            builder.Entity<UserMovieActivity>().ToTable("UserMovieActivity");
            base.OnModelCreating(builder);
        }
    }
}
