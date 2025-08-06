using Microsoft.OpenApi.Models;
using Microsoft.AspNetCore.Builder;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using DotnetGeminiSDK;
using DotnetGeminiSDK.Models;

var builder = WebApplication.CreateBuilder(args);

// 🔑 API Anahtarı
var geminiApiKey = builder.Configuration["AIzaSyCovzDJ3lbyHBC0O-qbZcr-72MwdFryRiU"]; // Çevresel değişken ya da secrets.json kullanılabilir

// 🔧 Servisler
builder.Services.AddControllers();
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen(c =>
{
    c.SwaggerDoc("v1", new OpenApiInfo
    {
        Title = "VisionBuy API",
        Version = "v1"
    });
});
builder.Services.AddCors(options =>
{
    options.AddPolicy("AllowFrontend",
        policy => policy.AllowAnyOrigin()
                        .AllowAnyHeader()
                        .AllowAnyMethod());
});
builder.Services.AddHttpClient();

// 🔗 GeminiClient servisini ekle
builder.Services.AddSingleton(sp =>
    new GeminiClient(geminiApiKey));

// ✅ Uygulama
var app = builder.Build();

if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI(c =>
    {
        c.SwaggerEndpoint("/swagger/v1/swagger.json", "VisionBuy API v1");
    });
}

app.UseCors("AllowFrontend");
app.UseHttpsRedirection();
app.UseAuthorization();
app.MapControllers();
app.Run();
