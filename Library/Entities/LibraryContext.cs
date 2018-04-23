using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;

namespace Library.Entities
{
    public class LibraryContext : DbContext
    {
		public DbSet<Author> Authors { get; set; }
		public DbSet<Book> Books { get; set; }

		//protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
		//{
		//	var config = new ConfigurationBuilder()
		//		.AddJsonFile("appsettings.json")
		//		.Build();

		//	optionsBuilder.UseSqlServer(config.GetConnectionString("DefaultConnection"));
		//}

		public LibraryContext(DbContextOptions<LibraryContext> options)
	: base(options)
		{ }
	}
}
