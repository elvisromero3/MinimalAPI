using Microsoft.EntityFrameworkCore;
using MinimalAPI;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

var minimalcors = "minimalcors";
builder.Services.AddCors(options =>
{
    options.AddPolicy(minimalcors, builder =>
    {
        builder
        .AllowAnyOrigin()
        .AllowAnyMethod()
        .AllowAnyHeader();
    });

});



builder.Services.AddDbContext<InfoDb>(options =>
    options.UseSqlServer(builder.Configuration.GetConnectionString("InfoValueContext"))
);

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}
using (var scope = app.Services.CreateScope())
{
    var services = scope.ServiceProvider;

    var context = services.GetRequiredService<InfoDb>();
    context.Database.EnsureCreated(); // DbInitializer.Initialize(context);
}

app.UseHttpsRedirection();



app.MapGet("/add", async (InfoDb db) =>
    await db.infoValue.ToListAsync());

app.MapPost("/add", async (Parameter parameter, InfoDb db) =>
{
    var info = new InfoValue() { Num1 = parameter.Num1, Num2 = parameter.Num2, Sum = parameter.Num1 + parameter.Num2 };
    db.infoValue.Add(info);
    await db.SaveChangesAsync();
    return Results.Ok(info.Sum);
});

app.UseCors(minimalcors);

app.Run();

