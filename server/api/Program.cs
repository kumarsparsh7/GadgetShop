namespace api;

public class Program
{
    public static void Main(string[] args)
    {
        var builder = WebApplication.CreateBuilder(args);

        // Add services to the container.

        builder.Services.AddControllers();
        builder.Services.AddEndpointsApiExplorer();
        builder.Services.AddSwaggerGen();

        // Setting env vars from appsettings.json
        builder.Configuration.AddJsonFile("appsettings.json");
        var configuration = builder.Configuration;
        if (Environment.GetEnvironmentVariable("ASPNETCORE_ENVIRONMENT") == "Dockerized") {
            configuration["FrontendUrl"] = Environment.GetEnvironmentVariable("FrontendUrl");
        }
        
        // CORS required to allow client (Angular) to call server (.NET) api
        var MyAllowSpecificOrigins = "_myAllowSpecificOrigins";
        builder.Services.AddCors(options =>
        {
            options.AddPolicy(name: MyAllowSpecificOrigins, builder =>
            {
                builder.WithOrigins("http://localhost",configuration["FrontendUrl"])
                    .AllowAnyHeader()
                    .AllowAnyMethod()
                    .SetIsOriginAllowedToAllowWildcardSubdomains();
            });
        });

        var app = builder.Build();

        // Configure the HTTP request pipeline.
        if (app.Environment.IsDevelopment())
        {
            app.UseSwagger();
            app.UseSwaggerUI();
        }

        app.UseHttpsRedirection();
        app.UseCors(MyAllowSpecificOrigins);
        app.UseAuthorization();


        app.MapControllers();

        app.Run();
    }
}