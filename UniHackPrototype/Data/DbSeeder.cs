using Bogus;
using MyAspNetVueApp.Data;
using MyAspNetVueApp.Models;
using UniHack.Enums;
using UniHack.Models;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;

namespace UniHack.Data
{
	public class DbSeeder
	{
		private readonly AppDbContext _context;
		private readonly Random _random = new Random();

		public DbSeeder(AppDbContext context)
		{
			_context = context;
		}

		public void Seed()
		{
			_context.Database.EnsureCreated();
			EnsureImageDirectories();

			if (_context.Users.Any()) return;

			// Seed Tags
			var tags = GenerateTags();
			_context.Tags.AddRange(tags);
			_context.SaveChanges();

			// Seed Users
			var users = GenerateUsers(50, tags);
			_context.Users.AddRange(users);
			_context.SaveChanges();

			// Seed Degrees (previously called Courses)
			var degrees = GenerateDegrees(20, tags, users);
			_context.Courses.AddRange(degrees);
			_context.SaveChanges();

			// Seed Societies
			var societies = GenerateSocieties(15, tags, users);
			_context.Societies.AddRange(societies);
			_context.SaveChanges();

			// Seed Comments
			var comments = GenerateComments(500, users);
			_context.Comments.AddRange(comments);
			_context.SaveChanges();

			// Seed Posts for Degrees and Societies
			var posts = GeneratePosts(200, users, tags, degrees, societies, comments);
			_context.Posts.AddRange(posts);
			_context.SaveChanges();

			// Seed Events for Societies
			var events = GenerateEvents(30, societies);
			_context.Events.AddRange(events);
			_context.SaveChanges();

			Console.WriteLine("Database seeded successfully!");
		}

		private void EnsureImageDirectories()
		{
			var baseDir = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot", "images");

			var directories = new[]
			{
				Path.Combine(baseDir, "profiles"),
				Path.Combine(baseDir, "degrees"),
				Path.Combine(baseDir, "societies"),
				Path.Combine(baseDir, "events")
			};

			foreach (var dir in directories)
			{
				if (!Directory.Exists(dir))
				{
					Directory.CreateDirectory(dir);
					Console.WriteLine($"Created directory: {dir}");
				}
			}
		}

		private List<Tag> GenerateTags()
		{
			var tagValues = new[]
			{
				"Academic", "Homework", "Study Group", "Tutoring", "Exam Prep",
				"Computer Science", "Engineering", "Business", "Law", "Medicine",
				"Psychology", "Arts", "Career", "Networking", "Gaming", "Fitness"
			};

			return tagValues.Select(t => new Tag { Id = Guid.NewGuid(), Value = t }).ToList();
		}

		private List<User> GenerateUsers(int count, List<Tag> tags)
		{
			var userFaker = new Faker<User>("en_AU")
				.RuleFor(u => u.Id, f => Guid.NewGuid())
				.RuleFor(u => u.Name, f => f.Name.FullName())
				.RuleFor(u => u.Email, (f, u) => $"{u.Name.ToLower().Replace(" ", ".")}{f.Random.Number(1, 99)}@university.edu.au")
				.RuleFor(u => u.Password, f => BCrypt.Net.BCrypt.HashPassword("password123"))
				.RuleFor(u => u.PhoneNumber, f => f.Phone.PhoneNumber("04## ### ###"))
				.RuleFor(u => u.University, f => f.Company.CompanyName())
				.RuleFor(u => u.Degree, f => f.Commerce.Department())
				.RuleFor(u => u.Bio, f => f.Lorem.Sentence())
				.RuleFor(u => u.ImagePath, f => $"/images/profiles/user_{f.Random.Number(1, 50)}.jpg")
				.RuleFor(u => u.IsAdmin, f => false);

			var users = userFaker.Generate(count);
			users[0].IsAdmin = true;
			users[0].Email = "admin@uniconnect.dev";

			return users;
		}

		private List<Course> GenerateDegrees(int count, List<Tag> tags, List<User> users)
		{
			var degreeNames = new[]
			{
				"Bachelor of Computer Science",
				"Bachelor of Engineering (Software)",
				"Bachelor of Business Administration",
				"Bachelor of Law",
				"Bachelor of Medicine",
				"Bachelor of Psychology",
				"Bachelor of Arts",
				"Bachelor of Science",
				"Bachelor of Economics",
				"Bachelor of Architecture",
				"Master of Data Science",
				"Master of Cybersecurity",
				"Master of Engineering",
				"Master of Business Administration",
				"Master of Public Health",
				"Master of Accounting",
				"Bachelor of Fine Arts",
				"Bachelor of Social Work",
				"Bachelor of Education",
				"Bachelor of Information Technology"
			};

			return degreeNames.Select(name => new Course
			{
				Id = Guid.NewGuid(),
				Name = name,
				Description = $"A comprehensive degree in {name.Split(" of ").Last()}",
				ImagePathBanner = "/images/degrees/degree.jpg",
				CreatedAt = DateTime.UtcNow,
				Tags = tags.OrderBy(x => Guid.NewGuid()).Take(3).ToList(),
				Members = users.OrderBy(x => Guid.NewGuid()).Take(_random.Next(10, 30)).ToList(),
				communityType = CommunityType.Course
			}).ToList();
		}

		private List<Society> GenerateSocieties(int count, List<Tag> tags, List<User> users)
		{
			var societyNames = new[]
			{
				"Computer Science Society",
				"Engineering Students Association",
				"Business Society",
				"Law Students' Society",
				"Medical Students' Society",
				"Gaming Society"
			};

			return societyNames.Select(name => new Society
			{
				Id = Guid.NewGuid(),
				Name = name,
				Description = $"A society for students interested in {name.Split(' ')[0]}",
				ImagePathBanner = "/images/societies/society.jpg",
				CreatedAt = DateTime.UtcNow,
				Tags = tags.OrderBy(x => Guid.NewGuid()).Take(3).ToList(),
				Members = users.OrderBy(x => Guid.NewGuid()).Take(_random.Next(10, 50)).ToList(),
				communityType = CommunityType.Society
			}).ToList();
		}

		private List<Comment> GenerateComments(int count, List<User> users)
		{
			var commentFaker = new Faker<Comment>()
				.RuleFor(c => c.Id, f => Guid.NewGuid())
				.RuleFor(c => c.Content, f => f.Lorem.Sentence())
				.RuleFor(c => c.Author, f => f.PickRandom(users))
				.RuleFor(c => c.CreatedAt, f => f.Date.Recent(30));

			return commentFaker.Generate(count);
		}

		private List<Post> GeneratePosts(int count, List<User> users, List<Tag> tags, List<Course> degrees, List<Society> societies, List<Comment> comments)
		{
			var communities = degrees.Cast<Community>().Concat(societies.Cast<Community>()).ToList();
			var postFaker = new Faker<Post>()
				.RuleFor(p => p.Id, f => Guid.NewGuid())
				.RuleFor(p => p.Title, f => f.Lorem.Sentence())
				.RuleFor(p => p.Content, f => f.Lorem.Paragraph())
				.RuleFor(p => p.Author, f => f.PickRandom(users))
				.RuleFor(p => p.Community, f => f.PickRandom(communities))
				.RuleFor(p => p.CreatedAt, f => f.Date.Recent(90));

			return postFaker.Generate(count);
		}

		private List<Event> GenerateEvents(int count, List<Society> societies)
		{
			var eventFaker = new Faker<Event>()
				.RuleFor(e => e.Id, f => Guid.NewGuid())
				.RuleFor(e => e.Name, f => f.Lorem.Sentence(3))
				.RuleFor(e => e.Description, f => f.Lorem.Paragraph())
				.RuleFor(e => e.Date, f => f.Date.Future())
				.RuleFor(e => e.Society, f => f.PickRandom(societies));

			return eventFaker.Generate(count);
		}
	}
}
