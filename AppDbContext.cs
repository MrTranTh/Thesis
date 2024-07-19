using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using System.Security.Claims;
using Thesis.Models;
using Thesis.Models.Entity;
using Thesis.Models.Identity;

namespace Thesis
{
    public class AppDbContext : IdentityDbContext<AppUser>
    {
        //public AppDbContext() { }

        public AppDbContext(DbContextOptions<AppDbContext> options) : base(options)
        {
        }

        public DbSet<AccessLog> AccessLogs { get; set; }

        public DbSet<Advertisement> Advertisement { set; get; }

        public DbSet<Categories> Categories { set; get; }

        public DbSet<Contact> Contacts { set; get; }

        public DbSet<News> News { set; get; }

        public DbSet<Order> Orders { set; get; }

        public DbSet<OrderDetail> OrderDetails { set; get; }

        public DbSet<Posts> Posts { set; get; }
                    
        public DbSet<Product> Product { set; get; }

        public DbSet<ProductCategories> ProductCategories { set; get; }

        public DbSet<ProductImages> ProductImages { set; get; }

        public DbSet<SystemSettings> SystemSettings { set; get; }

		public DbSet<ThongKeTruyCap> ThongKeTruyCap { set; get; }

		IConfiguration _configuration = new ConfigurationBuilder()
            .SetBasePath(Directory.GetCurrentDirectory())
            .AddJsonFile("appsettings.json")
            .Build();


        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            base.OnConfiguring(optionsBuilder);
            optionsBuilder.UseSqlServer(_configuration.GetConnectionString("DefaultConnection"));
            optionsBuilder.UseLoggerFactory(loggerFactory);
        }

        public static readonly ILoggerFactory loggerFactory = LoggerFactory.Create(builder =>
            builder.AddFilter(DbLoggerCategory.Database.Command.Name, LogLevel.Warning)
            .AddFilter(DbLoggerCategory.Query.Name, LogLevel.Debug)
            .AddConsole());

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            foreach(var entityType in modelBuilder.Model.GetEntityTypes())
            {
                var tableName = entityType.GetTableName();
                if (tableName.StartsWith("AspNet"))
                {
                    entityType.SetTableName(tableName.Substring(6));
                }
            }
        }
    }
}
