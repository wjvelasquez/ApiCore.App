using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.IdentityModel.Tokens;
using System.Text;
using Microsoft.AspNetCore.Cors;
using Microsoft.OpenApi.Models;

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

// -> seguridad
builder.Configuration.AddJsonFile("appsettings.json");
var secretkey = builder.Configuration.GetSection("settings").GetSection("secretkey").ToString();
var keyBytes = Encoding.UTF8.GetBytes(secretkey);

builder.Services.AddAuthentication(options =>
{
    options.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
    options.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
}).AddJwtBearer(options => {
    options.RequireHttpsMetadata = false;
    options.SaveToken = true;
    options.TokenValidationParameters = new TokenValidationParameters
    {
        ValidateIssuerSigningKey = true,
        IssuerSigningKey = new SymmetricSecurityKey(keyBytes),
        ValidateIssuer = true,
        ValidateAudience = false
    };
});
// <- seguridad
builder.Services.AddControllers();
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen(c =>
{
    c.SwaggerDoc("v1", new() { Title = "ApiCore.App", Version = "v1" });
    c.AddSecurityDefinition("Bearer", new OpenApiSecurityScheme
    {
        Description = @"JWT Authorization header using the Bearer scheme. \r\n\r\n 
          Enter 'Bearer' [space] and then your token in the text input below.
          \r\n\r\nExample: 'Bearer 12345abcdef'",
        Name = "Authorization",
        In = ParameterLocation.Header,
        Type = SecuritySchemeType.ApiKey,
        Scheme = "Bearer"
    });
    //c.EnableTryItOutByDefault();
});

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI(c => c.SwaggerEndpoint("/swagger/v1/swagger.json", "ApiCore.App v1"));
}

//enable cors second
app.UseCors(corsRules);

//seguridad
app.UseAuthentication();

app.UseAuthorization();

app.MapControllers();

app.Run();
