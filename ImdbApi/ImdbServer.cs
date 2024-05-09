using System.Reflection;
using ImdbApi.Logging;
using ImdbData;
using ImdbData.Interfaces;
using ImdbData.Services;
using Microsoft.EntityFrameworkCore;

namespace ImdbApi;

public class ImdbServer
{
    private readonly bool _testing;
    private readonly WebApplication _app;
    
    public IServiceProvider ServiceProvider => _app.Services;
    public ICollection<string> Urls => _app.Urls;
    
    public ImdbServer(bool testing, string[] args)
    {
        _testing = testing;
        
        var builder = WebApplication.CreateBuilder(args);
        InitConfiguration(builder);
        InitServices(builder);

        _app = builder.Build();
        InitPipeline();
    }
    
    public void Run()
    {
        _app.Run();
    }

    public async Task Start()
    {
        await _app.StartAsync();
    }

    public Task Stop()
    {
        return _app.StopAsync();
    }

    private static void InitConfiguration(WebApplicationBuilder appBuilder)
    {
        appBuilder.Configuration
            .AddJsonFile("appsettings.json")
            .AddJsonFile($"appsettings.{appBuilder.Environment.EnvironmentName}.json", optional: true);
    }

    private void InitServices(WebApplicationBuilder appBuilder)
    {
        appBuilder.Services.AddLogging();
        appBuilder.Services.AddLoggingMiddleware();
        appBuilder.Services.AddEndpointsApiExplorer();
        appBuilder.Services.AddSwaggerGen();

        appBuilder.Services.AddControllers()
            .AddApplicationPart(Assembly.GetExecutingAssembly());

        if (_testing)
        {
            appBuilder.Services.AddDbContext<ImdbContext>(options =>
            {
                var tempPath = Path.Combine(Path.GetTempPath(), "ImdbServer.db");
                options.UseSqlite($"Data Source={tempPath}");
            });
        }
        else
        {
            var imdbConnectionString = appBuilder.Configuration.GetConnectionString("imdb");
            appBuilder.Services.AddDbContext<ImdbContext>(options => 
                options.UseMySql(imdbConnectionString, ServerVersion.AutoDetect(imdbConnectionString)));
        }

        appBuilder.Services.AddScoped<ITitleBasicService, TitleBasicService>();
        appBuilder.Services.AddScoped<INameBasicService, NameBasicService>();
    }

    private void InitPipeline()
    {
        _app.UseLoggingMiddleware();
        
        if (_app.Environment.IsDevelopment() || _testing)
        {
            _app.UseSwagger();
            _app.UseSwaggerUI();
        }
        else
            _app.UseHttpsRedirection();
        
        _app.MapControllers();
    }
}