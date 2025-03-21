﻿using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using MyAspNetVueApp.Data;
using UniHack.Controllers;
using UniHack.ForYouPageNamespace;
using UniHack.Repositories;
using UniHack.Repositories.Interfaces;
using UniHack.Services;
using UniHack.Services.Interfaces;
using UniHack.Services.Notifications;
using UniHack.Services.Services;

namespace UniHack.Data
{
	public static class DependencyInjection
	{
		public static IServiceCollection AddApplicationServices(this IServiceCollection services)
		{
			services.AddDbContext<AppDbContext>(options =>
				options.UseSqlite("Data Source=app.db"));

			//Repositories
			services.AddScoped<ICommentRepository, CommentRepository>();
			services.AddScoped<ICourseRepository, CourseRepository>();
			services.AddScoped<IPostRepository, PostRepository>();
			services.AddScoped<ISocietyRepository, SocietyRepository>();
			services.AddScoped<IUserRepository, UserRepository>();
			services.AddScoped<IEventRepository, EventRepository>();

			//Services
			services.AddScoped<ICommentService, CommentService>();
			services.AddScoped<ICourseService, CourseService>();
			services.AddScoped<IPostService, PostService>();
			services.AddScoped<ISocietyService, SocietyService>();
			services.AddScoped<IUserService, UserService>();
			services.AddScoped<IEventService, EventService>();

			//Fyp
			services.AddScoped<IForYouPageLogic, ForYouPageLogic>();

			//Authentication
			services.AddScoped<IAuthService, AuthService>();

			services.AddHostedService<EventNotificationScheduler>();
			 
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
}