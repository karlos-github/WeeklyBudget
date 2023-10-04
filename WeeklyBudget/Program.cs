using Microsoft.EntityFrameworkCore;
using System.Text.Json.Serialization;
using WeeklyBudget.Contracts;
using WeeklyBudget.Data;
using WeeklyBudget.Repositories;
using WeeklyBudget.Service;
using Microsoft.OpenApi.Models;
using Microsoft.Extensions.Options;
using System.Reflection;

namespace WeeklyBudget
{
	public class Program
    {
        public static void Main(string[] args)
        {
            var builder = WebApplication.CreateBuilder(args);

            // Add services to the container.

            builder.Services.AddControllers();
            // Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
            builder.Services.AddEndpointsApiExplorer();
			builder.Services.AddSwaggerGen(options =>
			{
				options.SwaggerDoc("v1", new OpenApiInfo
				{
					Version = "v1",
					Title = "Monthly Budget API",
					Description = "An ASP.NET Core Web API for" +
					" managing monthly expences. The user can set some typical types of expences (like food, petrol, rent, ...) and" +
					" planned amount of money which is going to spent for each expence type per month. Planned amount of money for each expense type" +
					" is devided in a 4 weeks budget, so the user can more easily follow his/her expences on week bases. User writes down his/her expences during the month." +
					" The user can follow how much has already spent for each expence type and how much is left to spent for actual month or week of" +
					" the month. User can also check expences that were count in each expence types. With the begining of a new month a new budget is created automatically.",					
					Contact = new OpenApiContact
					{
						Name = "Link to API hosting",
						Url = new Uri("http://skrabal.aspifyhost.cz/"),
						Email = "karel.skrabal@seznam.cz"
					},
				});

				var xmlFilename = $"{Assembly.GetExecutingAssembly().GetName().Name}.xml";
				options.IncludeXmlComments(Path.Combine(AppContext.BaseDirectory, xmlFilename));
			});

			builder.Services.AddControllers().AddJsonOptions(options => options.JsonSerializerOptions.ReferenceHandler = ReferenceHandler.IgnoreCycles);
            builder.Services.AddDbContext<WeeklyBudgetContext>(options => options.UseSqlServer(builder.Configuration.GetConnectionString("WeeklyBudgetConnection")));
            builder.Services.AddScoped<IRepositoryManager, RepositoryManager>();
            builder.Services.AddScoped<IBudgetService, BudgetService>();
			builder.Services.AddScoped<IBudgetDetailService, BudgetDetailService>();
			builder.Services.AddScoped<IExpenditureService, ExpenditureService>();
			builder.Services.AddScoped<IExpenditureTypeService, ExpenditureTypeService>();

			var app = builder.Build();

            // Configure the HTTP request pipeline.
            if (app.Environment.IsDevelopment())
            {
                app.UseSwagger();
                app.UseSwaggerUI();
            }

            app.UseHttpsRedirection();

            app.UseAuthorization();


            app.MapControllers();

            app.Run();
        }
    }
}