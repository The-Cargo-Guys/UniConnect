using MyAspNetVueApp.Data;
using Microsoft.AspNetCore.Builder;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.EntityFrameworkCore;
using NSwag;
using NSwag.Generation.Processors.Security;
using UniHack.Data;
using System.IO;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddApplicationServices();
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

// Enable OpenAPI Documentation
builder.Services.AddOpenApiDocument(config =>
{
    config.Title = "MyAspNetVueApp API";
    config.Description = "OpenAPI Specification for TypeScript DTO Generation";
    config.AddSecurity("Bearer", Enumerable.Empty<string>(), new OpenApiSecurityScheme
    {
        Type = OpenApiSecuritySchemeType.Http,
        Scheme = "bearer",
        BearerFormat = "JWT",
        Description = "Enter 'Bearer {your JWT token}'"
    });
    config.OperationProcessors.Add(new AspNetCoreOperationSecurityScopeProcessor("Bearer"));
});

// Enable Dependency Injection for Database Seeding
builder.Services.AddScoped<DbSeeder>();

var app = builder.Build();

if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI(c =>
    {
        c.SwaggerEndpoint("/swagger/v1/swagger.json", "MyAspNetVueApp API v1");
    });

    app.UseOpenApi();
}

// Enable CORS (Ensures Vue.js frontend can access images)
app.UseCors("AllowVueApp");

// Forcefully Create wwwroot if Missing
var wwwRootPath = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot");
if (!Directory.Exists(wwwRootPath))
{
    Directory.CreateDirectory(wwwRootPath);
}

// Enable Serving Static Files (Required for Images)
app.UseStaticFiles();

// Enforce HTTPS
app.UseHttpsRedirection();

// Enable Routing & Controllers
app.UseRouting();
app.UseAuthorization();
app.MapControllers();

// Database Initialization
using (var scope = app.Services.CreateScope())
{
    var services = scope.ServiceProvider;

    var dbContext = services.GetRequiredService<AppDbContext>();

    // Ensure database is properly set up
    dbContext.Database.EnsureDeleted();
    dbContext.Database.EnsureCreated();

    var seeder = services.GetRequiredService<DbSeeder>();
    seeder.Seed();
}

app.Run();
