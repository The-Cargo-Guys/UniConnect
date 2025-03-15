using MyAspNetVueApp.Data;
using MyAspNetVueApp.Models;
using UniHackPrototype.Models;
using System;
using System.Collections.Generic;
using System.Linq;

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
                var user = new User
                {
                    Name = "Admin",
                    Email = "admin@example.com",
                    Password = "password123",
                    IsAdmin = true,
                    PhoneNumber = "0457174936",
                    University = "University of Melbourne",
                    Degree = "Bachelor of Science",
                    Bio = "I am the admin",
                    ImagePath = "https://example.com/admin.jpg",
                    Tags = new List<Tag> { new Tag { Value = "Rugby" }, new Tag { Value = "Sports" } }
                };
                _context.Users.Add(user);
                _context.SaveChanges();

                var user2 = new User
                {
                    Name = "NON_Admin",
                    Email = "admin@example.com",
                    Password = "password123",
                    IsAdmin = false,
                    PhoneNumber = "04563894936",
                    University = "University of sydney",
                    Degree = "Bachelor of arts",
                    Bio = "I am not the admin",
                    ImagePath = "https://example.com/admin.jpg",
                    Tags = new List<Tag> { new Tag { Value = "phones" }, new Tag { Value = "swimming" } }
                };
                _context.Users.Add(user2);
                _context.SaveChanges();

                var tag = new Tag
                {
                    Value = "Technology"
                };
                _context.Tags.Add(tag);
                _context.SaveChanges();

                var comment = new Comment
                {
                    Content = "Great first post!",
                    UserId = user.Id,
                    Author = user
                };
                _context.Comments.Add(comment);
                _context.SaveChanges();

                var course = new Course
                {
                    Name = "Computer Science 101",
                    Description = "An introductory course to computer science.",
                    ImagePathBanner = "https://example.com/course-banner.jpg",
                    Tags = new List<Tag> { tag },
                    Members = new List<User> { user }
                };
                _context.Courses.Add(course);
                _context.SaveChanges();

                var society = new Society
                {
                    Name = "Tech Enthusiasts",
                    Description = "A society for people passionate about technology.",
                    ImagePathBanner = "https://example.com/society-banner.jpg",
                    Tags = new List<Tag> { tag },
                    Members = new List<User> { user }
                };
                _context.Societies.Add(society);
                _context.SaveChanges();

                var post = new Post
                {
                    Title = "Welcome Post",
                    Content = "This is the first post in the system.",
                    AuthorId = user.Id,
                    Tags = new List<Tag> { tag },
                    Comments = new List<Comment> { comment },
                    Author = user,
                    Community = course
                };
                _context.Posts.Add(post);
                _context.SaveChanges();
            }
        }
    }
}
