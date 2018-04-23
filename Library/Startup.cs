using AutoMapper;
using Library.Entities;
using Library.Helpers;
using Library.Model;
using Library.Services;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc.Formatters;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;

namespace Library
{
	public class Startup
	{
		public Startup(IConfiguration configuration)
		{
			Configuration = configuration;
		}

		public IConfiguration Configuration { get; }

		// This method gets called by the runtime. Use this method to add services to the container.
		public void ConfigureServices(IServiceCollection services)
		{
			services.AddMvc(setupAction =>
			{
				setupAction.ReturnHttpNotAcceptable = true;
				setupAction.OutputFormatters.Add(new XmlDataContractSerializerOutputFormatter());
			});

			var config = new ConfigurationBuilder()
				.AddJsonFile("appsettings.json")
				.Build();

			services.AddScoped<ILibraryRepository, LibraryRepository>();
			services.AddDbContext<LibraryContext>(options =>
				options.UseSqlServer(config.GetConnectionString("DefaultConnection")));
		}

		// This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
		public void Configure(IApplicationBuilder app, IHostingEnvironment env, ILoggerFactory loggerFactory,
			LibraryContext libraryContext)
		{
			if (env.IsDevelopment())
				app.UseDeveloperExceptionPage();
			else
				app.UseExceptionHandler(appBuilder =>
				{
					appBuilder.Run(async context =>
					{
						context.Response.StatusCode = 500;
						await context.Response.WriteAsync("An unespected fault happend. Try again later");
					});
				});


			Mapper.Initialize(cfg =>
			{
				cfg.CreateMap<Author, AuthorDto>()
					.ForMember(dest => dest.Name, opt => opt.MapFrom(src => $"{src.FirstName} {src.LastName}"))
					.ForMember(dest => dest.Age, opt => opt.MapFrom(src => src.DateOfBirth.GetCurrentAge()));

				cfg.CreateMap<Book, BookDto>();
				cfg.CreateMap<AuthorForCreationDto, Author>();
				cfg.CreateMap<BookForCreationDto, Book>();
			});

			libraryContext.EnsureSeedDataForContext();

			app.UseMvc();
		}
	}
}