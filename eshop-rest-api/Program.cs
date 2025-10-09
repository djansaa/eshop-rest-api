var builder = WebApplication.CreateBuilder(args);

// ========================== SERVICES ==========================
// controllers
builder.Services.AddControllers();
// swagger
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();
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

app.MapControllers();

app.Run();
