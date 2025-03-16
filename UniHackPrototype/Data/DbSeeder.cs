using MyAspNetVueApp.Data;
using MyAspNetVueApp.Models;
using UniHack.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using BCrypt.Net;

namespace UniHack.Data
{
    public class DbSeeder
    {
        private readonly AppDbContext _context;
        public DbSeeder(AppDbContext context)
        {
            _context = context;
        }

        public void Seed()
        {
            _context.Database.EnsureCreated();

            if (!_context.Users.Any())
            {
                var tags = new List<Tag>();
                for (int i = 1; i <= 50; i++)
                {
                    tags.Add(new Tag { Value = $"Tag-{i}" });
                }
                _context.Tags.AddRange(tags);
                _context.SaveChanges();

                var users = new List<User>();
                for (int i = 1; i <= 10; i++)
                {
                    users.Add(new User
                    {
                        Name = $"User-{i}",
                        Email = $"user{i}@{i}.com",
                        Password = BCrypt.Net.BCrypt.HashPassword("password123"),
                        IsAdmin = i == 1, // First user is admin
                        PhoneNumber = $"04571749{i:D2}",
                        University = i % 2 == 0 ? "University of Melbourne" : "University of Sydney",
                        Degree = i % 2 == 0 ? "Bachelor of Science" : "Bachelor of Arts",
                        Bio = $"This is user-{i}",
                        ImagePath = $"https://example.com/user{i}.jpg",
                        Tags = tags.OrderBy(x => Guid.NewGuid()).Take(5).ToList()
                    });
                }
                _context.Users.AddRange(users);
                _context.SaveChanges();

                var courses = new List<Course>();
                for (int i = 1; i <= 10; i++)
                {
                    courses.Add(new Course
                    {
                        Name = $"Course-{i}",
                        Description = $"Description for course-{i}",
                        ImagePathBanner = $"https://example.com/course-banner{i}.jpg",
                        Tags = tags.OrderBy(x => Guid.NewGuid()).Take(3).ToList(),
                        Members = users.OrderBy(x => Guid.NewGuid()).Take(5).ToList()
                    });
                }
                _context.Courses.AddRange(courses);
                _context.SaveChanges();

                var societies = new List<Society>();
                for (int i = 1; i <= 10; i++)
                {
                    societies.Add(new Society
                    {
                        Name = $"Society-{i}",
                        Description = $"A society for {i}",
                        ImagePathBanner = $"https://example.com/society-banner{i}.jpg",
                        Tags = tags.OrderBy(x => Guid.NewGuid()).Take(3).ToList(),
                        Members = users.OrderBy(x => Guid.NewGuid()).Take(5).ToList()
                    });
                }
                _context.Societies.AddRange(societies);
                _context.SaveChanges();

                var comments = new List<Comment>();
                foreach (var user in users)
                {
                    for (int i = 1; i <= 10; i++)
                    {
                        comments.Add(new Comment
                        {
                            Content = $"Comment {i} by {user.Name}",
                            UserId = user.Id,
                            Author = user
                        });
                    }
                }
                _context.Comments.AddRange(comments);
                _context.SaveChanges();

                var posts = new List<Post>();
                foreach (var user in users)
                {
                    for (int i = 1; i <= 10; i++)
                    {
                        posts.Add(new Post
                        {
                            Title = $"Post {i} by {user.Name}",
                            Content = $"This is post {i}.",
                            AuthorId = user.Id,
                            Tags = tags.OrderBy(x => Guid.NewGuid()).Take(4).ToList(),
                            Comments = comments.OrderBy(x => Guid.NewGuid()).Take(3).ToList(),
                            Author = user,
                            Community = courses.OrderBy(x => Guid.NewGuid()).First()
                        });
                    }
                }
                _context.Posts.AddRange(posts);
                _context.SaveChanges();
            }
        }
    }
}
