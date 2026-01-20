using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;



namespace web.Models.Data
{
    public class ApplicationDBContext : IdentityDbContext<IdentityUser>
    {
        public ApplicationDBContext(DbContextOptions<ApplicationDBContext> options)
            : base(options)
        {
        }

        public DbSet<Stock> Stocks { get; set; } = default!;
        public DbSet<Comment> Comments { get; set; } = default!;

        public DbSet<PortfolioItem> Portfolios {get; set;}


        protected override void  OnModelCreating(ModelBuilder builder)
        {
            base.OnModelCreating(builder);

           builder.Entity<PortfolioItem>().HasIndex(x => new {x.StockID,x.UserId}).IsUnique();
        }
    }
}
