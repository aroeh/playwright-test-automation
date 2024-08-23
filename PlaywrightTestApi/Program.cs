using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.Identity.Web;
using PlaywrightTestApi.DataAccess;
using PlaywrightTestApi.Repos;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.AddAuthentication(JwtBearerDefaults.AuthenticationScheme)
    .AddMicrosoftIdentityWebApi(builder.Configuration.GetSection("ApiAzureAd"));

builder.Services.AddControllers();
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

builder.Services.AddTransient<IRestuarantRepo, RestuarantRepo>();
builder.Services.AddTransient<IRestuarantData, RestuarantData>();
builder.Services.AddTransient<IMongoService, MongoService>();

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();

app.UseCors(builder =>
    builder
        .WithOrigins(["https://localhost:7116", "http://localhost:5116"])
        .AllowAnyHeader()
        .WithHeaders(["GET", "POST", "PUT"])
);

app.UseAuthorization();

app.MapControllers();

app.Run();
