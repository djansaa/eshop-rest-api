using Asp.Versioning;
using Asp.Versioning.ApiExplorer;
using eshop_rest_api.Data;
using eshop_rest_api.Mock;
using eshop_rest_api.Services;
using eshop_rest_api.Swagger;
using Microsoft.Data.Sqlite;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Caching.Hybrid;
using Microsoft.Extensions.Options;
using Serilog;
using Swashbuckle.AspNetCore.SwaggerGen;

// ========================== LOGGING ==========================
Log.Logger = new LoggerConfiguration()
    .WriteTo.Console()
    .CreateBootstrapLogger();

try
{
    // ========================== WEB BUILDER ==========================

    var builder = WebApplication.CreateBuilder(args);


    // register serilog
    builder.Host.UseSerilog((ctx, lc) => lc
        .ReadFrom.Configuration(ctx.Configuration)
        .Enrich.FromLogContext()
    );

    // ========================== SERVICES ==========================
    builder.Services.AddControllers();

    // api versioning
    builder.Services.AddApiVersioning(options =>
    {
        options.DefaultApiVersion = new ApiVersion(1, 0);
        options.AssumeDefaultVersionWhenUnspecified = true;
        options.ReportApiVersions = true;
    }).AddApiExplorer(options =>
    {
        options.GroupNameFormat = "'v'VVV";
        options.SubstituteApiVersionInUrl = true;
    });

    builder.Services.AddEndpointsApiExplorer();
    builder.Services.AddSwaggerGen();
    builder.Services.AddTransient<IConfigureOptions<SwaggerGenOptions>, ConfigureSwaggerOptions>();

    // add hybrid cache service
    builder.Services.AddHybridCache(o =>
    {
        o.DefaultEntryOptions = new HybridCacheEntryOptions
        {
            Expiration = TimeSpan.FromMinutes(5),
            LocalCacheExpiration = TimeSpan.FromMinutes(1),
        };
    });

    // get mock variable from config
    bool useMock = builder.Configuration.GetValue<bool>("Data:UseMock");

    Log.Information("UseMock = {UseMock}", useMock);

    // using mock data
    if (useMock)
    {
        builder.Services.AddSingleton<IProductService, MockProductService>();
        Log.Information("Registered IProductService -> MockProductService");
    }
    else
    {
        // using real database
        var cs = builder.Configuration.GetConnectionString("Default") ?? builder.Configuration["ConnectionStrings:Default"];

        // create sqlite db file full path
        var csb = new SqliteConnectionStringBuilder(cs);
        var dbPath = Path.Combine(AppContext.BaseDirectory, "Data", Path.GetFileName(csb.DataSource));
        Directory.CreateDirectory(Path.GetDirectoryName(dbPath)!);
        csb.DataSource = dbPath;

        builder.Services.AddDbContext<AppDbContext>(opt => opt.UseSqlite(csb.ToString()));
        builder.Services.AddScoped<IProductService, ProductService>();
        Log.Information("Registered IProductService -> ProductService");

        Log.Information("SQLite DB path: {Path}", dbPath);
    }

    // ========================== BUILD APP ==========================

    var app = builder.Build();

    // Configure the HTTP request pipeline.
    if (app.Environment.IsDevelopment())
    {
        app.UseSwagger();

        // add api versions to swagger UI
        var provider = app.Services.GetRequiredService<IApiVersionDescriptionProvider>();
        app.UseSwaggerUI(options =>
        {
            foreach (var desc in provider.ApiVersionDescriptions) options.SwaggerEndpoint($"/swagger/{desc.GroupName}/swagger.json", desc.GroupName.ToUpperInvariant());
        });
    }

    app.UseHttpsRedirection();
    app.UseAuthorization();

    if (!useMock)
    {
        // apply db migrations at startup
        using (var scope = app.Services.CreateScope())
        {
            var db = scope.ServiceProvider.GetRequiredService<AppDbContext>();
            await db.Database.MigrateAsync();
        }
    }

    app.MapControllers();

    app.Run();

}
catch (Exception ex)
{
    Log.Fatal(ex, "Host terminated unexpectedly");
}
finally
{
    Log.CloseAndFlush();
}

public partial class Program { }

