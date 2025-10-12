using Asp.Versioning;
using Asp.Versioning.ApiExplorer;
using eshop_rest_api.Data;
using eshop_rest_api.Services;
using eshop_rest_api.Swagger;
using Microsoft.Data.Sqlite;
using Microsoft.EntityFrameworkCore;
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
    // configure swagger
    builder.Services.AddTransient<IConfigureOptions<SwaggerGenOptions>, ConfigureSwaggerOptions>();

    // db
    var conn = new SqliteConnection("Data Source=:memory:");
    conn.Open();
    builder.Services.AddSingleton(conn);
    builder.Services.AddDbContext<AppDbContext>((sp, o) => o.UseSqlite(sp.GetRequiredService<SqliteConnection>()));

    // product service
    builder.Services.AddScoped<IProductService, ProductService>();

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

    using (var scope = app.Services.CreateScope())
    {
        var db = scope.ServiceProvider.GetRequiredService<AppDbContext>();
        await db.Database.EnsureCreatedAsync();
        //await db.Database.MigrateAsync();
        await DbSeed.RunAsync(db);
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


