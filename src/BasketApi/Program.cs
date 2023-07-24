using BasketApi.Infrastructure.ApiClients;
using BasketApi.Infrastructure;
using BasketApi.Application;

public class Program
{
    public static void Main(string[] args)
    {
        var builder = WebApplication.CreateBuilder(args);

        // Add services to the container.

        builder.Services.AddControllers();
        // Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
        builder.Services.AddEndpointsApiExplorer();
        builder.Services.AddSwaggerGen();


        builder.Services.AddMemoryCache();

        builder.Services.AddInfrastructure();

        builder.Services.AddHttpClient<IProductApiClient, ProductApiClient>(client =>
          {
              // Get base URL from app settings
              var baseUrl = builder.Configuration.GetSection("ProductApi:BaseUrl").Value;
              client.BaseAddress = new Uri(baseUrl);
          });

        builder.Services.AddApplication();

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