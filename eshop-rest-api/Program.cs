using eshop_rest_api.Data;
using eshop_rest_api.Services;
using Microsoft.Data.Sqlite;
using Microsoft.EntityFrameworkCore;

var builder = WebApplication.CreateBuilder(args);

// ========================== SERVICES ==========================
builder.Services.AddControllers();
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

// db
var conn = new SqliteConnection("Data Source=:memory:");
conn.Open();
builder.Services.AddSingleton(conn);
builder.Services.AddDbContext<AppDbContext>((sp, o) => o.UseSqlite(sp.GetRequiredService<SqliteConnection>()));

// services
builder.Services.AddScoped<IProductService, ProductService>();

// api versioning
// TODO: add api versioning

// ========================== BUILD APP ==========================

var app = builder.Build();

// map controllers to /api
app.MapGroup("/api").MapControllers();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();
app.UseAuthorization();

using (var scope = app.Services.CreateScope())
{
    var db = scope.ServiceProvider.GetRequiredService<AppDbContext>();
    await db.Database.EnsureCreatedAsync();
    //await db.Database.MigrateAsync();
}

app.Run();
