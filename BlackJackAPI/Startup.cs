using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.EntityFrameworkCore; // For DbContext usage
using Microsoft.Extensions.Hosting;
using BlackjackDataAccess.Interfaces;
using BlackjackDataAccess.Data;
using BlackjackDataAccess.Repositories;
using BlackJackAPI.Api.Services;
//using BlackJackAPI.Api.Interfaces;  //TODO Naveen

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
        // Configure the connection string from appsettings.json
        var connectionString = Configuration.GetConnectionString("DefaultConnection");

        // Register DbContext for SQL Server with connection string
        services.AddDbContext<ApplicationDbContext>(options =>
            options.UseSqlServer(connectionString));

        // Register Repositories with DI
        services.AddScoped<IUserRepository, UserRepository>();
        services.AddScoped<IGameSessionRepository, GameSessionRepository>();
        services.AddScoped<IGameService, GameService>();
        // services.AddScoped<IPerformanceMetricRepository, PerformanceMetricRepository>(); // Uncomment when needed

        // Add services to support controllers (API Controllers)
        services.AddControllers();

        // Add Swagger generation service for API documentation
        services.AddSwaggerGen();

        // If you plan on adding authentication later, it can go here.
    }

    // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
    public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
    {
        if (env.IsDevelopment())
        {
            app.UseDeveloperExceptionPage();
        }

        app.UseHttpsRedirection();
        app.UseStaticFiles();
        app.UseRouting();
        app.UseAuthorization();

        // Enable middleware to serve generated Swagger as a JSON endpoint.
        app.UseSwagger();

        // Enable middleware to serve swagger-ui (HTML, JS, CSS, etc.),
        // specifying the Swagger JSON endpoint.
        app.UseSwaggerUI(c =>
        {
            c.SwaggerEndpoint("/swagger/v1/swagger.json", "BlackJackAPI v1");
            c.RoutePrefix = "swagger"; // This makes Swagger available at the root "/"
        });


        app.UseEndpoints(endpoints =>
        {
            endpoints.MapControllers();
        });
    }

}
