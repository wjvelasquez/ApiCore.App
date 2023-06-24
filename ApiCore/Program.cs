using Microsoft.AspNetCore.Cors;

var builder = WebApplication.CreateBuilder(args);

//EnableCors first
var corsRules = "CorsRules";
builder.Services.AddCors(options =>
{
    options.AddPolicy(name: corsRules,
        builder =>
        {
            builder
                .AllowAnyOrigin()
                .AllowAnyHeader()
                .AllowAnyMethod();
        });
});

// Add services to the container.

builder.Services.AddControllers();
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

//enable cors second
app.UseCors(corsRules);

app.UseAuthorization();

app.MapControllers();

app.Run();
