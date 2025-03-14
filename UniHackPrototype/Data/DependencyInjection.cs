using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using MyAspNetVueApp.Data;
using UniHack.Repositories;

public static class DependencyInjection
{
    public static IServiceCollection AddApplicationServices(this IServiceCollection services)
    {
        services.AddDbContext<AppDbContext>(options =>
            options.UseSqlite("Data Source=app.db"));

        services.AddScoped<ICommentRepository, CommentRepository>();
        services.AddScoped<ICourseRepository, CourseRepository>();
        services.AddScoped<IPostRepository, PostRepository>();
        services.AddScoped<ISocietyRepository, SocietyRepository>();
        services.AddScoped<IUserRepository, UserRepository>();

        services.AddCors(options =>
        {
            options.AddPolicy("AllowVueApp", policy =>
                policy.SetIsOriginAllowed(origin => 
                    origin.StartsWith("http://localhost") || 
                    origin.StartsWith("https://localhost"))
                      .AllowAnyMethod()
                      .AllowAnyHeader()
                      .AllowCredentials());
        });

        services.AddControllers();
        services.AddEndpointsApiExplorer();
        services.AddSwaggerGen();

        return services;
    }
}
