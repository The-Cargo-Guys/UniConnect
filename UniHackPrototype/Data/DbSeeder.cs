using Bogus;
using MyAspNetVueApp.Data;
using MyAspNetVueApp.Models;
using UniHack.Enums;
using UniHack.Models;
using System.IO;

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

			// Ensure image directories exist
			EnsureImageDirectories();

			if (!_context.Users.Any())
			{
				// Create Tags
				var tags = GenerateTags();
				_context.Tags.AddRange(tags);
				_context.SaveChanges();

				// Create Universities
				var universities = new List<string>
				{
					"University of Sydney", "University of Melbourne", "UNSW Sydney",
					"Monash University", "Australian National University", "University of Queensland",
					"University of Western Australia", "University of Adelaide", "Macquarie University",
					"RMIT University", "Queensland University of Technology", "University of Technology Sydney",
					"Deakin University", "Curtin University", "University of Newcastle", "La Trobe University",
					"University of Tasmania", "Griffith University", "Flinders University", "Western Sydney University"
				};

				// Create Degrees
				var degrees = new List<string>
				{
					"Bachelor of Science", "Bachelor of Arts", "Bachelor of Engineering (Software)",
					"Bachelor of Commerce", "Bachelor of Business", "Bachelor of Computer Science",
					"Bachelor of Design", "Bachelor of Laws", "Bachelor of Medicine",
					"Bachelor of Education", "Bachelor of Psychology", "Bachelor of Fine Arts",
					"Master of Data Science", "Master of Business Administration", "Master of Engineering",
					"Master of Public Health", "Master of Architecture", "Master of Cybersecurity",
					"Bachelor of Information Technology", "Bachelor of Nursing", "Master of Accounting",
					"Bachelor of Communications", "Bachelor of Social Work", "Master of Teaching"
				};

				// Create Users
				var users = GenerateUsers(50, universities, degrees, tags);
				_context.Users.AddRange(users);
				_context.SaveChanges();

				// Create Courses
				var courses = GenerateCourses(20, tags, users);
				_context.Courses.AddRange(courses);
				_context.SaveChanges();

				// Create Societies
				var societies = GenerateSocieties(15, tags, users);
				_context.Societies.AddRange(societies);
				_context.SaveChanges();

				// Create Comments
				var comments = GenerateComments(500, users);
				_context.Comments.AddRange(comments);
				_context.SaveChanges();

				// Create Posts
				List<Community> communities = new List<Community>();
				communities.AddRange(courses);
				communities.AddRange(societies);

				var posts = GeneratePosts(200, users, tags, communities, comments);
				_context.Posts.AddRange(posts);
				_context.SaveChanges();

				// Create Events for Societies
				var events = GenerateEvents(30, societies);
				_context.Events.AddRange(events);
				_context.SaveChanges();

				Console.WriteLine("Database seeded successfully!");
			}
		}

		private void EnsureImageDirectories()
		{
			var baseDir = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot", "images");

			var directories = new[]
			{
				Path.Combine(baseDir, "profiles"),
				Path.Combine(baseDir, "courses"),
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
			var tagCategories = new Dictionary<string, List<string>>
			{
				{ "Academic", new List<string> { "Research", "Homework", "Study Group", "Tutoring", "Assignment Help", "Exam Prep", "Academic Integrity", "Literature Review", "Citation", "Lab Work" } },
				{ "Campus Life", new List<string> { "Housing", "Dining", "Commuting", "Parking", "Library", "Campus Events", "Orientation", "Facilities", "Recreation", "Safety" } },
				{ "Courses", new List<string> { "Computer Science", "Engineering", "Mathematics", "Physics", "Chemistry", "Biology", "Business", "Economics", "Psychology", "Law", "Medicine", "Arts", "History", "Literature", "Philosophy", "Languages" } },
				{ "Career", new List<string> { "Internship", "Job Hunting", "Resume", "Interview", "Career Fair", "Networking", "Professional Development", "Industry", "Entrepreneurship", "Startup" } },
				{ "Student Life", new List<string> { "Social", "Parties", "Clubs", "Sports", "Fitness", "Gaming", "Movies", "Music", "Travel", "Food", "Shopping", "Mental Health", "Relationships", "Self-care", "Motivation" } },
				{ "Tech", new List<string> { "Programming", "Web Development", "Mobile Apps", "Data Science", "AI", "Machine Learning", "Cybersecurity", "UI/UX", "Cloud Computing", "DevOps" } }
			};

			var tags = new List<Tag>();
			foreach (var category in tagCategories)
			{
				foreach (var tagName in category.Value)
				{
					tags.Add(new Tag { Id = Guid.NewGuid(), Value = tagName });
				}

				// Also add the category as a tag
				tags.Add(new Tag { Id = Guid.NewGuid(), Value = category.Key });
			}

			return tags;
		}

		private List<User> GenerateUsers(int count, List<string> universities, List<string> degrees, List<Tag> tags)
		{
			// Define a Faker for Australian names
			var userFaker = new Faker<User>("en_AU")
				.RuleFor(u => u.Id, f => Guid.NewGuid())
				.RuleFor(u => u.Name, f => f.Name.FullName())
				.RuleFor(u => u.Email, (f, u) => {
					// Create realistic student emails
					var nameParts = u.Name.ToLower().Split(' ');
					var firstName = nameParts[0];
					var lastName = nameParts.Length > 1 ? nameParts[^1] : "";
					var studentNumber = f.Random.Number(10000000, 99999999).ToString();
					return $"{firstName}.{lastName}{f.Random.Number(1, 99)}@student.{f.Random.ListItem(universities).ToLower().Replace("university of ", "").Replace(" university", "").Replace(" ", "")}.edu.au";
				})
				.RuleFor(u => u.Password, f => BCrypt.Net.BCrypt.HashPassword("password123"))
				.RuleFor(u => u.PhoneNumber, f => f.Phone.PhoneNumber("04## ### ###"))
				.RuleFor(u => u.University, f => f.PickRandom(universities))
				.RuleFor(u => u.Degree, f => f.PickRandom(degrees))
				.RuleFor(u => u.Bio, f => {
					return f.Random.ArrayElement(new[] {
						$"Hi, I'm a {f.Random.Number(1, 4)}th year {f.Commerce.ProductName()} student at {f.PickRandom(universities)}. Interested in {f.Random.Words(3)}!",
						$"{f.Random.Number(1, 4)}th year {f.PickRandom(degrees)} student. Passionate about {f.Hacker.Adjective()} {f.Hacker.Noun()}. Looking to connect with like-minded people!",
						$"Studying {f.PickRandom(degrees)}. {f.Hacker.Phrase()}. Feel free to reach out if you need help with {f.Random.Word()}!",
						$"Aspiring {f.Name.JobTitle()}. I enjoy {f.Random.Word()} and {f.Random.Word()} in my free time.",
						$"Currently completing my {f.PickRandom(degrees)}. Interested in {f.Commerce.ProductName()}, {f.Commerce.ProductName()}, and {f.Commerce.ProductName()}.",
						$"Exchange student from {f.Address.Country()}. Studying {f.Random.Word()} at {f.PickRandom(universities)}."
					});
				})
				.RuleFor(u => u.ImagePath, f => $"/images/profiles/user_{f.Random.Number(1, 50)}.jpg")
				.RuleFor(u => u.IsAdmin, f => false);

			var users = userFaker.Generate(count);

			// Make first user admin
			users[0].IsAdmin = true;
			users[0].Name = "Admin User";
			users[0].Email = "admin@uniconnect.dev";

			// Assign 3-7 random tags to each user
			foreach (var user in users)
			{
				user.Tags = tags.OrderBy(x => Guid.NewGuid()).Take(_random.Next(3, 8)).ToList();
			}

			return users;
		}

		private List<Course> GenerateCourses(int count, List<Tag> tags, List<User> users)
		{
			// Real course structure with real course names
			var actualCourses = new List<string>
			{
				"COMP2123: Data Structures and Algorithms",
				"COMP3027: Algorithm Design",
				"COMP3308: Introduction to Artificial Intelligence",
				"COMP3419: Graphics and Multimedia",
				"COMP3520: Operating Systems Internals",
				"MATH1002: Linear Algebra",
				"MATH1003: Integral Calculus and Modelling",
				"MATH2069: Discrete Mathematics and Graph Theory",
				"PHYS1001: Physics 1 (Regular)",
				"PHYS1003: Physics 1 (Technological)",
				"CHEM1101: Chemistry 1A",
				"CHEM1102: Chemistry 1B",
				"BIOL1001: Concepts in Biology",
				"BIOL1003: Human Biology",
				"ECON1001: Introductory Microeconomics",
				"ECON1002: Introductory Macroeconomics",
				"PSYC1001: Psychology 1001",
				"ARTS1450: Speaking to Power: Introduction to Language",
				"LAWS1006: Foundations of Law",
				"MEDI3001: Clinical and Professional Practice",
				"ENGG1811: Computing for Engineers",
				"BUSS1000: Future of Business",
				"HIST1001: History of the Present",
				"INFO1110: Introduction to Programming",
				"INFO2222: Computing 2 Usability and Security",
				"ELEC1601: Introduction to Computer Systems",
				"SOFT2412: Agile Software Development Practices",
				"COMP5046: Natural Language Processing",
				"COMP5424: Information Technology in Biomedicine",
				"COMP5703: Capstone Project - IT Research Methodology"
			};

			var courseDescriptions = new Dictionary<string, string>
			{
				{ "COMP2123", "This course introduces fundamental data structures and algorithms. Topics include: abstract data types, lists, stacks, queues, trees, hash tables, graphs; recursion; searching and sorting algorithms; algorithm analysis; and memory management." },
				{ "COMP3027", "This course focuses on the design and analysis of efficient algorithms. Topics include: asymptotic analysis, divide-and-conquer, greedy algorithms, dynamic programming, network flow, NP-completeness, and approximation algorithms." },
				{ "COMP3308", "This course provides an introduction to artificial intelligence. Topics include: problem solving by search, knowledge representation and reasoning, machine learning, neural networks, natural language processing, and expert systems." },
				{ "COMP3419", "This course covers principles of computer graphics and multimedia. Topics include: 2D and 3D geometry and transformations, rendering, animation, color models, image processing, compression techniques, and multimedia systems." },
				{ "COMP3520", "This course explores the internal operations of modern operating systems. Topics include: process management, memory management, file systems, I/O systems, protection and security, and distributed systems." },
				{ "MATH1002", "This course provides a thorough introduction to linear algebra. Topics include: vectors, matrices, linear equations, determinants, eigenvalues and eigenvectors, and applications." },
				{ "MATH1003", "This course covers integral calculus and mathematical modeling. Topics include: techniques of integration, differential equations, sequences and series, and applications to physical problems." },
				{ "MATH2069", "This course introduces discrete mathematics and graph theory. Topics include: sets, relations, functions, number theory, combinatorics, graph theory, trees, and algorithms on graphs." },
				{ "PHYS1001", "This is a calculus-based introductory physics course covering classical mechanics, waves, and thermodynamics. Topics include: motion, forces, energy, momentum, oscillations, and heat." },
				{ "PHYS1003", "This course provides an introduction to physics for technology and engineering students. Topics include: mechanics, electricity and magnetism, optics, and modern physics." },
				{ "CHEM1101", "This course introduces fundamental principles of chemistry. Topics include: atomic structure, chemical bonding, states of matter, chemical reactions, thermodynamics, and kinetics." },
				{ "CHEM1102", "This course builds on Chemistry 1A and explores advanced topics. Topics include: equilibrium, acids and bases, electrochemistry, organic chemistry, and spectroscopy." },
				{ "BIOL1001", "This course introduces the fundamental concepts of biology. Topics include: cell structure and function, genetics, evolution, biodiversity, ecology, and human biology." },
				{ "BIOL1003", "This course focuses on human biology. Topics include: human anatomy and physiology, genetics, reproduction, development, and health and disease." },
				{ "ECON1001", "This course introduces microeconomic principles. Topics include: supply and demand, consumer choice, production, market structures, market failures, and government intervention." },
				{ "ECON1002", "This course introduces macroeconomic principles. Topics include: national income, unemployment, inflation, economic growth, monetary and fiscal policy, and international trade." },
				{ "PSYC1001", "This course provides an introduction to psychology. Topics include: perception, cognition, learning, memory, social psychology, developmental psychology, and abnormal psychology." },
				{ "ARTS1450", "This course introduces the study of language and its social context. Topics include: language structure, language variation, language change, and language policy." },
				{ "LAWS1006", "This course introduces the foundations of law. Topics include: legal systems, legal reasoning, legal sources, constitutional law, criminal law, contract law, and tort law." },
				{ "MEDI3001", "This course focuses on clinical and professional practice in medicine. Topics include: patient care, medical ethics, communication skills, and evidence-based medicine." },
				{ "ENGG1811", "This course introduces computing for engineers. Topics include: programming concepts, algorithms, data structures, numerical methods, and engineering applications." },
				{ "BUSS1000", "This course explores the future of business. Topics include: business models, innovation, entrepreneurship, digital transformation, sustainability, and global business." },
				{ "HIST1001", "This course examines the historical roots of contemporary issues. Topics include: democracy, nationalism, colonialism, globalization, war, and social movements." },
				{ "INFO1110", "This course introduces programming concepts and techniques. Topics include: variables, control structures, functions, arrays, file I/O, and basic algorithms." },
				{ "INFO2222", "This course focuses on usability and security in computing. Topics include: human-computer interaction, user interface design, information security, cryptography, and privacy." },
				{ "ELEC1601", "This course introduces computer systems. Topics include: digital logic, computer architecture, assembly language, memory systems, and input/output devices." },
				{ "SOFT2412", "This course covers agile software development practices. Topics include: requirements, design, implementation, testing, deployment, and project management using agile methodologies." },
				{ "COMP5046", "This course focuses on natural language processing. Topics include: language modeling, part-of-speech tagging, parsing, semantics, information extraction, and machine translation." },
				{ "COMP5424", "This course explores information technology applications in biomedicine. Topics include: medical imaging, bioinformatics, health informatics, medical decision support systems, and telemedicine." },
				{ "COMP5703", "This capstone project course focuses on IT research methodology. Students work in teams to design, implement, and evaluate an IT solution to a real-world problem." }
			};

			var courseFaker = new Faker<Course>()
				.RuleFor(c => c.Id, f => Guid.NewGuid())
				.RuleFor(c => c.Name, f => f.Random.ListItem(actualCourses))
				.RuleFor(c => c.Description, (f, c) => {
					string courseCode = c.Name.Split(':')[0].Trim();
					return courseDescriptions.ContainsKey(courseCode)
						? courseDescriptions[courseCode]
						: $"This course provides a comprehensive introduction to {c.Name.Split(':')[1].Trim()}. Students will learn key concepts, methodologies, and practical applications.";
				})
				.RuleFor(c => c.ImagePathBanner, f => $"/images/courses/course_{f.Random.Number(1, 20)}.jpg")
				.RuleFor(c => c.CreatedAt, f => f.Date.Past(2));

			var courses = courseFaker.Generate(Math.Min(count, actualCourses.Count));

			// Ensure unique course names
			courses = courses.GroupBy(c => c.Name).Select(g => g.First()).ToList();

			// Assign relevant tags to each course
			foreach (var course in courses)
			{
				string coursePrefix = course.Name.Split(':')[0].Substring(0, 4);
				string courseName = course.Name.Split(':')[1].Trim();

				// Assign department tag
				var departmentTag = coursePrefix switch
				{
					"COMP" or "INFO" => tags.FirstOrDefault(t => t.Value == "Computer Science"),
					"MATH" => tags.FirstOrDefault(t => t.Value == "Mathematics"),
					"PHYS" => tags.FirstOrDefault(t => t.Value == "Physics"),
					"CHEM" => tags.FirstOrDefault(t => t.Value == "Chemistry"),
					"BIOL" => tags.FirstOrDefault(t => t.Value == "Biology"),
					"ECON" => tags.FirstOrDefault(t => t.Value == "Economics"),
					"PSYC" => tags.FirstOrDefault(t => t.Value == "Psychology"),
					"ARTS" => tags.FirstOrDefault(t => t.Value == "Arts"),
					"LAWS" => tags.FirstOrDefault(t => t.Value == "Law"),
					"MEDI" => tags.FirstOrDefault(t => t.Value == "Medicine"),
					"ENGG" => tags.FirstOrDefault(t => t.Value == "Engineering"),
					"BUSS" => tags.FirstOrDefault(t => t.Value == "Business"),
					"HIST" => tags.FirstOrDefault(t => t.Value == "History"),
					"SOFT" => tags.FirstOrDefault(t => t.Value == "Programming"),
					_ => tags.FirstOrDefault(t => t.Value == "Academic")
				};

				var courseTagList = new List<Tag>();
				if (departmentTag != null) courseTagList.Add(departmentTag);

				// Add academic tag
				var academicTag = tags.FirstOrDefault(t => t.Value == "Academic");
				if (academicTag != null) courseTagList.Add(academicTag);

				// Add relevant topic tags
				foreach (var tag in tags)
				{
					if (course.Description.Contains(tag.Value) || courseName.Contains(tag.Value))
					{
						courseTagList.Add(tag);
					}
				}

				// Ensure we have at least 3 tags, adding random academic-related tags if needed
				var academicTags = tags.Where(t => t.Value == "Homework" || t.Value == "Study Group" ||
												  t.Value == "Tutoring" || t.Value == "Assignment Help" ||
												  t.Value == "Exam Prep" || t.Value == "Research").ToList();

				while (courseTagList.Count < 3)
				{
					var randomTag = _random.Next(0, academicTags.Count);
					if (!courseTagList.Contains(academicTags[randomTag]))
					{
						courseTagList.Add(academicTags[randomTag]);
					}
				}

				course.Tags = courseTagList.Distinct().ToList();

				// Add 10-30 random members to each course
				course.Members = users.OrderBy(x => Guid.NewGuid()).Take(_random.Next(10, 31)).ToList();

				// Set community type
				course.communityType = CommunityType.Course;
			}

			return courses;
		}

		private List<Society> GenerateSocieties(int count, List<Tag> tags, List<User> users)
		{
			// Real university societies
			var actualSocieties = new List<(string Name, string Description, List<string> TagNames)>
			{
				("Computer Science Society", "A community for students passionate about computer science, programming, and technology. We organize coding competitions, tech talks, industry networking events, and social gatherings.", new List<string> { "Computer Science", "Programming", "Technology" }),
				("Engineering Students Association", "Representing all engineering students, we provide academic support, professional development opportunities, and social events to bring engineers together across all disciplines.", new List<string> { "Engineering", "Academic", "Professional Development" }),
				("Business Society", "Connecting business students with industry professionals through networking events, case competitions, workshops, and social activities to develop professional skills and expand career opportunities.", new List<string> { "Business", "Career", "Networking" }),
				("Law Students' Society", "Supporting law students through their academic journey with moot competitions, networking events, career panels, social activities, and advocacy for student welfare.", new List<string> { "Law", "Career", "Academic" }),
				("Medical Students' Society", "Representing medical students' interests, providing academic support, wellbeing initiatives, community outreach, and social events to balance the demands of medical education.", new List<string> { "Medicine", "Academic", "Mental Health" }),
				("Arts Society", "Celebrating creative expression through visual arts, music, theatre, literature, and cultural events. We provide workshops, exhibitions, performances, and collaborative projects.", new List<string> { "Arts", "Music", "Literature" }),
				("International Students Association", "Supporting international students with cultural integration, social events, academic assistance, and advocacy to enhance the overseas student experience.", new List<string> { "Campus Life", "Social", "Cultural" }),
				("Debating Society", "Fostering critical thinking and public speaking skills through competitive debates, workshops, and social events for both beginners and experienced debaters.", new List<string> { "Career", "Academic", "Professional Development" }),
				("Environmental Society", "Promoting sustainability and environmental awareness through campus initiatives, community projects, educational events, and advocacy for climate action.", new List<string> { "Campus Life", "Social", "Academic" }),
				("Sports Association", "Coordinating recreational and competitive sports programs, fitness classes, and sporting events to promote physical wellbeing and team spirit across campus.", new List<string> { "Sports", "Fitness", "Campus Life" }),
				("Gaming Society", "Bringing together gaming enthusiasts for LAN parties, eSports tournaments, board game nights, game development workshops, and social events.", new List<string> { "Gaming", "Technology", "Social" }),
				("Photography Club", "Exploring the art of photography through workshops, photo walks, exhibitions, competitions, and collaborative projects for all skill levels.", new List<string> { "Arts", "Campus Life", "Technology" }),
				("Women in STEM", "Supporting and promoting women in science, technology, engineering, and mathematics through mentoring, networking, workshops, and advocacy.", new List<string> { "Academic", "Career", "Professional Development" }),
				("Mental Health Awareness Club", "Promoting mental wellbeing, reducing stigma, and providing peer support through workshops, awareness campaigns, and social activities.", new List<string> { "Mental Health", "Self-care", "Student Life" }),
				("Entrepreneurship Society", "Fostering innovation and entrepreneurial thinking through startup competitions, workshops, mentoring, networking, and access to resources.", new List<string> { "Entrepreneurship", "Career", "Business" }),
				("Cultural Society", "Celebrating cultural diversity through festivals, food events, performances, language exchanges, and educational activities to promote cross-cultural understanding.", new List<string> { "Cultural", "Campus Life", "Social" }),
				("Volunteering Society", "Connecting students with volunteering opportunities, community service projects, and social impact initiatives to make a positive difference.", new List<string> { "Student Life", "Social", "Career" }),
				("Film Society", "Appreciating cinema through screenings, discussions, filmmaking workshops, and student film festivals for both casual viewers and aspiring filmmakers.", new List<string> { "Arts", "Student Life", "Technology" }),
			};

			var societyFaker = new Faker<Society>()
				.RuleFor(s => s.Id, f => Guid.NewGuid())
				.RuleFor(s => s.Name, f => "")  // Will be set later
				.RuleFor(s => s.Description, f => "")  // Will be set later
				.RuleFor(s => s.ImagePathBanner, f => $"/images/societies/society_{f.Random.Number(1, 15)}.jpg")
				.RuleFor(s => s.CreatedAt, f => f.Date.Past(3))
				.RuleFor(s => s.Events, f => new List<Event>());

			var societies = societyFaker.Generate(Math.Min(count, actualSocieties.Count));

			// Use actual society data
			for (int i = 0; i < societies.Count; i++)
			{
				societies[i].Name = actualSocieties[i].Name;
				societies[i].Description = actualSocieties[i].Description;

				// Find and add the relevant tags
				var societyTags = new List<Tag>();
				foreach (var tagName in actualSocieties[i].TagNames)
				{
					var tag = tags.FirstOrDefault(t => t.Value == tagName);
					if (tag != null)
					{
						societyTags.Add(tag);
					}
				}

				// Add a few random tags if we don't have enough
				while (societyTags.Count < 3)
				{
					var randomTag = tags[_random.Next(tags.Count)];
					if (!societyTags.Contains(randomTag))
					{
						societyTags.Add(randomTag);
					}
				}

				societies[i].Tags = societyTags;

				// Add 20-80 random members to each society
				societies[i].Members = users.OrderBy(x => Guid.NewGuid()).Take(_random.Next(20, 81)).ToList();

				// Set community type
				societies[i].communityType = CommunityType.Society;
			}

			return societies;
		}

		private List<Comment> GenerateComments(int count, List<User> users)
		{
			// Realistic student comments for different contexts
			var courseComments = new List<string>
			{
				"Thanks for sharing your notes! This really helped me understand the lecture.",
				"Can anyone explain how to solve problem 7 from the tutorial?",
				"When is the assignment due? I can't find the deadline on the LMS.",
				"Is anyone else having trouble with the online quiz? It keeps crashing for me.",
				"Great explanation! I finally understand recursive functions.",
				"Does anyone have the textbook PDF? The library copies are all checked out.",
				"Who's planning to attend the review session on Thursday?",
				"Our tutor mentioned something different about this topic. I'm confused now.",
				"I'm looking for a study group for the final exam. Anyone interested?",
				"Has anyone done this course last semester? How was the final exam?",
				"The lecture recording isn't working. Did anyone take good notes today?",
				"Just finished the assignment. That was harder than I expected!",
				"Prof said the midterm will focus on chapters 3-7, just FYI.",
				"Is attendance mandatory for the tutorials? I have a work conflict.",
				"Thanks for organizing the study session yesterday. It was really helpful!",
				"Can we form a group for the group assignment? I need 2 more people.",
				"Has the sample exam been released yet? Can't find it on the LMS.",
				"Does anyone understand the marking criteria for the report?",
				"Just found out there's a practice quiz available that doesn't count toward your grade.",
				"Is the formula sheet provided in the exam or do we need to memorize everything?",
				"This subject is way harder than I thought it would be. Any tips to stay on top of the material?",
				"Does anyone have recommendations for additional resources to understand this topic better?",
				"The tutor explained this really well today. Everything makes so much more sense now!",
				"Has anyone received their marks for the last assignment yet?",
				"What topics is everyone focusing on for the research project?",
				"The recommended reading for this week is really interesting!",
				"Anyone else struggling with the online lab exercises? They're so buggy!",
				"Just realized I've been doing the wrong readings all week. Feeling discouraged...",
				"The guest lecture yesterday was fantastic. So many insights from industry!",
				"Does anyone know if the lectures are recorded? I have a clash with another subject."
			};

			var societyComments = new List<string>
			{
				"Really enjoyed the event last week! When's the next one?",
				"Can't make it to the meeting tomorrow. Will there be minutes shared afterward?",
				"Just joined the society. When's the next social event?",
				"The workshop yesterday was really informative. Thanks for organizing!",
				"Who's volunteering for the fundraiser next month?",
				"Love the new society hoodie design! How can I order one?",
				"Is anyone carpooling to the conference next weekend?",
				"Great job on the charity drive! How much did we end up raising?",
				"The guest speaker today was so inspiring. Thanks for arranging it!",
				"Does anyone have photos from the last social? Would love to see them.",
				"Looking forward to the industry night. Do we need to prepare anything?",
				"Just a reminder that membership fees are due by the end of the week.",
				"The committee is looking for volunteers for the orientation day stall. Anyone interested?",
				"Thanks for representing our society at the clubs fair! We got so many new sign-ups.",
				"Can we get more board game options for the next game night?",
				"Would anyone be interested in a skill-sharing workshop series?",
				"The society room will be closed for renovations next week. Please plan accordingly.",
				"Congratulations to our team for winning the intervarsity competition!",
				"Does anyone have contacts at companies for potential sponsorships?",
				"Just a heads up that the venue for Friday's event has changed. Check your emails for details.",
				"Can the committee consider scheduling events outside typical class hours? Many of us have late labs.",
				"The newsletter this month looks amazing! Great job to the media team.",
				"Is there a discount for society members at the university bookstore?",
				"Who's interested in helping plan the end-of-semester celebration?",
				"The mentoring program has been so valuable. Thanks to all the mentors!",
				"Just posted the photos from our retreat on the Instagram page. Tag yourselves!",
				"Don't forget to RSVP for the annual dinner by this Friday.",
				"Any suggestions for activities for the upcoming welcome event?",
				"Is anyone interested in running for the executive committee next year?",
				"Shout out to everyone who helped at the open day - we couldn't have done it without you!"
			};

			var generalComments = new List<string>
			{
				"Thanks for sharing this information!",
				"This is really helpful, appreciate it.",
				"I had no idea about this. Thanks for posting!",
				"Has anyone else experienced this issue?",
				"Great point! I hadn't thought about it that way.",
				"I completely agree with what you're saying.",
				"This changed my perspective on the topic.",
				"Can someone provide more details about this?",
				"I'm having the same problem. Did you find a solution?",
				"Looking forward to hearing more about this.",
				"This is exactly what I needed to know!",
				"Does anyone have any additional resources on this topic?",
				"I'm still a bit confused. Could you clarify?",
				"This deserves more attention from everyone.",
				"I've had a similar experience and completely relate.",
				"Just wanted to say thank you for organizing this discussion.",
				"I'm going to try this approach and see if it works for me.",
				"Has the university made any official announcements about this?",
				"This is such a common issue among students. Glad someone brought it up.",
				"Perfectly explained! I couldn't have said it better myself.",
				"I've been struggling with this for weeks. Any advice would be appreciated.",
				"Is there a deadline we need to be aware of?",
				"I've found a different solution that might help others...",
				"Did anyone attend the workshop last week? Worth going to the next one?",
				"Can we create a separate thread to discuss this specific issue?",
				"As a first-year student, this information is invaluable. Thank you!",
				"I've shared this with my friends from other unis - they're having the same problems.",
				"This seems to contradict what was said in the email last week...",
				"I'm relieved to know I'm not the only one feeling this way.",
				"This should be pinned to the top of the forum!"
			};

			var commentFaker = new Faker<Comment>()
				.RuleFor(c => c.Id, f => Guid.NewGuid())
				.RuleFor(c => c.Content, f => {
					// Choose a comment type based on weighted probability
					int type = f.Random.Number(1, 10);
					if (type <= 5) // 50% course comments
						return f.PickRandom(courseComments);
					else if (type <= 8) // 30% society comments
						return f.PickRandom(societyComments);
					else // 20% general comments
						return f.PickRandom(generalComments);
				})
				.RuleFor(c => c.Author, f => f.PickRandom(users))
				.RuleFor(c => c.CreatedAt, f => f.Date.Recent(30));

			var comments = commentFaker.Generate(count);

			// Ensure Author Id is properly set
			foreach (var comment in comments)
			{
				comment.UserId = comment.Author.Id;
			}

			return comments;
		}

		private List<Post> GeneratePosts(int count, List<User> users, List<Tag> tags, List<Community> communities, List<Comment> allComments)
		{
			// Realistic post titles and content by category
			var coursePosts = new List<PostContent>
			{
				new PostContent
				{
					Title = "Lecture Notes for Week 3",
					Content = "Hi everyone! I've compiled my notes from this week's lectures into a single document. They cover everything the professor discussed about [topic], including the examples and case studies. I've also added some extra explanations for the more complex concepts that weren't completely clear in class. Feel free to use them for your revision. Let me know if you spot any errors or have questions!"
				},

				new PostContent
				{
					Title = "Assignment 2 Discussion Thread",
					Content = "Now that the assignment details have been released, I thought it would be helpful to create a thread where we can discuss our approaches (without sharing actual solutions of course). I'm particularly confused about question 3 - it seems like we need to apply the concepts from week 4, but I'm not sure how to start. Has anyone made progress on this?"
				},

				new PostContent
				{
					Title = "Study Group for Midterm Exam",
					Content = "I'm organizing a study group for the upcoming midterm. We'll be meeting in the library, study room 4B, this Saturday from 2-5pm. We'll go over practice questions, review difficult concepts, and quiz each other. Everyone is welcome! Please comment below if you're interested so I can get an idea of numbers. Bring your notes and questions!"
				},

				new PostContent
				{
					Title = "Tutorial 5 Solutions",
					Content = "I worked through all the tutorial problems for week 5 and thought I'd share my solutions for everyone to check their work. I'm pretty confident about most of them, but I'm not 100% sure about problems 7 and 9, so if anyone has different answers, please let me know. Hope this helps with your preparation for the quiz!"
				},

				new PostContent
				{
					Title = "Resources for Final Project",
					Content = "I've collected some useful resources for the final project that might help everyone. There are some great papers on Google Scholar about this topic, and I found some relevant datasets we can use. I've also linked to some similar projects from previous years that got high marks. Hope this gives you some inspiration for your own projects!"
				},

				new PostContent
				{
					Title = "Clarification on Assessment Criteria",
					Content = "After speaking with the professor, I got some clarification on the assessment criteria for our research papers. Apparently, they're looking for critical analysis rather than just description, and referencing is worth 20% of the mark. He also mentioned that we should focus on recent developments (last 5 years) in the field. Thought this might help everyone with their planning!"
				},

				new PostContent
				{
					Title = "Lecture Recording Issues",
					Content = "Is anyone else having trouble accessing the lecture recordings from last week? I've tried different browsers and devices, but they still won't load for me. I've already emailed IT support, but they haven't responded yet. Did anyone take comprehensive notes they'd be willing to share until this gets fixed?"
				},

				new PostContent
				{
					Title = "Software Installation Guide",
					Content = "Since many people were having trouble with the software setup for the practical sessions, I've created a step-by-step installation guide with screenshots. It covers all the common errors and how to resolve them. This should work for both Windows and Mac users. Let me know if you encounter any issues not covered in the guide!"
				},

				new PostContent
				{
					Title = "Feedback on Draft Submissions",
					Content = "Has anyone received feedback on their draft submissions yet? It's been two weeks, and I'm getting anxious as the final deadline approaches. I wanted to incorporate the tutor's suggestions before finalizing my paper. If you've received feedback, how detailed was it and how long did it take to arrive?"
				},

				new PostContent
				{
					Title = "Group Project Partner Search",
					Content = "I'm looking for 2-3 partners for the group project. My interests are in machine learning and natural language processing, and I'm hoping to work on something related to sentiment analysis. I'm organized, reliable, and good with deadlines. If you're interested in teaming up, let me know a bit about your interests and availability for meetings!"
				},

				new PostContent
				{
					Title = "Past Exam Paper Summary",
					Content = "I went through all the past exam papers from the last 5 years and noticed some patterns. There's always at least one question on [specific topic], and the essay section usually gives options related to weeks 4, 7, and 10. The multiple choice seems to focus heavily on terminology and definitions. Hope this helps with your revision strategy!"
				},

				new PostContent
				{
					Title = "Extension Request Process",
					Content = "For those who might need it: I found out that the process for requesting extensions has changed this semester. You need to fill out the new form on the faculty website (not the old one linked in the course outline) and provide supporting documentation. Requests need to be submitted at least 3 business days before the deadline unless it's an emergency."
				},

				new PostContent
				{
					Title = "Interesting Related Article",
					Content = "I came across this fascinating article in [Journal Name] that relates directly to what we're studying in week 8. It presents a different perspective on [theory/concept] than what's covered in our textbook. I thought it might interest those who want to dive deeper into this topic. There's a particularly good section on practical applications that might be useful for our final projects."
				},

				new PostContent
				{
					Title = "Tutor Office Hours Change",
					Content = "Just a heads up that our tutor has changed their office hours for the rest of the semester. The new times are Tuesdays 2-4pm and Thursdays 10-11am in room E6.22. They mentioned they're particularly willing to help with the upcoming assignment, so it might be worth dropping by if you're struggling with it."
				},

				new PostContent
				{
					Title = "Calculator Policy for Final Exam",
					Content = "I checked with the course coordinator about the calculator policy for our final exam. Only non-programmable scientific calculators are allowed (the same policy as for the midterm). They'll be checking calculator models at the door, so make sure yours complies with the policy. The specific approved models are listed on the faculty website."
				},

				new PostContent
				{
					Title = "Recommended Textbook Alternatives",
					Content = "For those finding the prescribed textbook difficult to understand, I've discovered some great alternatives that explain the same concepts in a more accessible way. [Book Title] by [Author] is particularly good for [specific topic], and there's a free PDF of [Another Book] available online that covers the foundations really well. These really helped me understand the material better!"
				}
			};

			var societyPosts = new List<PostContent>
			{
				new PostContent
				{
					Title = "Welcome to New Members!",
					Content = "A big welcome to all the new members who joined our society during O-Week! We're excited to have you on board. This post contains everything you need to know about our upcoming events, how to get involved, and the benefits of your membership. Don't forget to join our Facebook group and follow our Instagram for regular updates. Looking forward to meeting you all at our welcome social next Friday!"
				},

				new PostContent
				{
					Title = "Annual General Meeting Announcement",
					Content = "Notice is hereby given for our Annual General Meeting to be held on [date] at [time] in [location]. We'll be reviewing our activities from the past year, discussing plans for the upcoming year, and electing the new executive committee. All members are encouraged to attend and vote. If you're interested in running for a position, please submit your nomination by [deadline]. Refreshments will be provided!"
				},

				new PostContent
				{
					Title = "Upcoming Industry Networking Event",
					Content = "We're excited to announce our biggest networking event of the year, taking place on [date] at [venue]. We'll have representatives from over 15 companies including [Company Names], offering insights into graduate programs, internships, and career pathways. Bring your resume and questions! Business attire is recommended. Registration is essential as places are limited - link in bio to secure your spot."
				},

				new PostContent
				{
					Title = "Workshop Series: Professional Skills Development",
					Content = "Based on member feedback, we're launching a new workshop series focused on developing professional skills! The first three sessions will cover resume building, interview techniques, and LinkedIn optimization, led by career advisors from the university's careers service. Sessions will run fortnightly on Wednesday evenings starting next week. Sign up for individual workshops or the entire series using the form below."
				},

				new PostContent
				{
					Title = "Community Service Opportunity",
					Content = "We're partnering with [Charity Name] for a community service day on [date]. We need volunteers to help with their [specific project/event], which supports [cause/community]. This is a great opportunity to give back to the community and earn volunteer hours for your co-curricular recognition program. No experience necessary - training will be provided on the day. Please register your interest by commenting below!"
				},

				new PostContent
				{
					Title = "Competition Winners Announcement",
					Content = "Congratulations to the winners of our annual [competition name]! After careful deliberation by our panel of judges, the results are: 1st Place: [Name] for [project/entry description] 2nd Place: [Name] for [project/entry description] 3rd Place: [Name] for [project/entry description] Honorable Mentions: [Names] Thank you to all participants for your outstanding submissions and to our sponsors for providing the amazing prizes."
				},

				new PostContent
				{
					Title = "End of Semester Social",
					Content = "Join us for our end of semester celebration at [venue] on [date] from [time]! We'll be reflecting on our achievements this semester, recognizing our outstanding members, and simply enjoying each other's company before the exam period begins. Tickets are $15 for members and include food, your first drink, and entertainment. Book through the link below by [deadline] - spots are filling fast!"
				},

				new PostContent
				{
					Title = "Call for Committee Applications",
					Content = "We're looking for enthusiastic members to join our organizing committee for next semester! Available roles include Events Coordinator, Marketing Officer, Treasurer, and First Year Representative. This is a fantastic opportunity to develop leadership skills, expand your network, and make a real impact on the society. No previous experience required - just passion and commitment! Apply using the form linked below by [deadline]."
				},

				new PostContent
				{
					Title = "Member Spotlight: Sarah's Internship Success",
					Content = "In this month's member spotlight, we're featuring Sarah Zhang, a third-year student who recently secured an internship at [Company Name]! In this post, Sarah shares her journey, application process tips, and how being part of our society helped her develop the skills that impressed her interviewers. Check out the full interview for some great insights and inspiration for your own career journey!"
				},

				new PostContent
				{
					Title = "Discount Partnership Announcement",
					Content = "We're thrilled to announce our new partnership with [Company/Service Name]! All society members can now enjoy a 15% discount on their products/services by showing your membership card or using the code [CODE] online. This is perfect for [specific use case relevant to members]. Plus, a percentage of each purchase goes back to supporting our society events. Win-win!"
				},

				new PostContent
				{
					Title = "Survey: Planning Next Semester's Events",
					Content = "Help us plan next semester's calendar by completing our quick 5-minute survey! We want to ensure our events and initiatives align with what you want to get out of your membership. Everyone who completes the survey by [deadline] will go into the draw to win [prize]. Your feedback is invaluable in helping us continue to improve and grow as a society."
				},

				new PostContent
				{
					Title = "Resources from Last Week's Workshop",
					Content = "For those who attended our [workshop topic] workshop last week, the slides, handouts, and additional resources mentioned by our speaker are now available on our website in the members' area. For those who couldn't make it, we've also uploaded a recording of the session. The password to access these materials has been emailed to all members."
				},

				new PostContent
				{
					Title = "Merchandise Pre-orders Now Open",
					Content = "Our new merchandise range is here and looking amazing! We've got hoodies ($45), t-shirts ($25), tote bags ($15), and the ever-popular sticker packs ($5). All items feature our new design voted for by members last month. Pre-orders are open until [deadline], with items available for collection at our next general meeting. Order through the link in our bio!"
				},

				new PostContent
				{
					Title = "Congratulations to Our Graduating Members",
					Content = "As the academic year comes to a close, we want to take a moment to celebrate our members who are graduating! You've all contributed so much to our society and we can't wait to see what you achieve next. We're organizing a special graduation dinner on [date] at [venue] - details and RSVP in the link below. Once a member, always a member!"
				},

				new PostContent
				{
					Title = "Sponsorship Opportunity for Student Projects",
					Content = "Great news! Our industry partner [Company Name] has offered to sponsor student projects related to [field/topic]. Selected projects will receive funding of up to $500, mentorship from industry professionals, and the opportunity to present at their offices to senior staff. Applications are open to all members until [deadline]. See the attached brief for eligibility criteria and application details."
				},

				new PostContent
				{
					Title = "Inter-University Collaboration Event",
					Content = "We're excited to announce a joint event with the [Similar Society] from [Other University] on [date]! This will be a fantastic opportunity to network with peers from another institution, share ideas, and participate in [activity/competition/discussion]. The event will be held at [location] and is free for members of both societies. Transport options will be available - register your interest below."
				}
			};

			var generalPosts = new List<PostContent>
			{
				new PostContent
				{
					Title = "Tips for Managing Study-Life Balance",
					Content = "After three years at university, I've finally figured out some strategies for maintaining a healthy study-life balance. The key for me has been establishing a consistent schedule that includes dedicated time for classes, study, exercise, socializing, and rest. I use time blocking in my calendar and the Pomodoro technique to stay focused during study sessions. Also, learning to say no to commitments that don't align with my priorities has been game-changing. Would love to hear what works for others!"
				},

				new PostContent
				{
					Title = "Affordable Housing Options Near Campus",
					Content = "With rent prices continuing to rise, I thought it would be helpful to share some affordable housing options I've found near campus. [Area Name] has some reasonably priced share houses, typically $200-250/week for a single room. The [Building Name] apartments offer student discounts for 12-month leases. Also, don't forget to check the university housing service - they often have last-minute vacancies at reduced rates. Has anyone had good experiences with other areas or accommodation types?"
				},

				new PostContent
				{
					Title = "Best Study Spots on Campus",
					Content = "After exploring every corner of campus, I've compiled my list of the best study spots for different needs: For absolute quiet: The 4th floor of the main library has single desks with dividers For group work: The new collaborative learning space in the [Building] has great tech setup For long sessions: The café in [Building] has comfortable seating, power outlets, and decent coffee For late night: The 24-hour computer lab in [Building] (swipe card access required) Any hidden gems I've missed?"
				},

				new PostContent
				{
					Title = "Mental Health Resources Review",
					Content = "As someone who's utilized various mental health resources on campus, I wanted to share my experiences to help others. The university counseling service offers free sessions, but there can be a 2-3 week wait. The peer support program is great for immediate conversations. For ongoing support, the psychology clinic offers reduced-rate sessions with supervised trainee psychologists. Don't forget that most courses also allow mental health extensions for assignments - the process is straightforward and respectful."
				},

				new PostContent
				{
					Title = "Part-time Job Opportunities for Students",
					Content = "For those looking to earn while studying, I've researched student-friendly employment options: University positions: Check the internal jobs board for casual admin, library, and ambassador roles Retail/Hospitality: [Shopping Center] and [Area] restaurants often hire students with flexible hours Tutoring: The university tutoring service pays $30-35/hour for undergrad tutors Online gigs: Transcription, user testing, and virtual assistance work can fit around study commitments Remember that international students have work restrictions to consider!"
				},

				new PostContent
				{
					Title = "Technology Tools That Saved My Degree",
					Content = "Throughout my degree, I've discovered several tech tools that have significantly improved my productivity and learning: Notion for organizing all my notes and assignments Zotero for painlessly managing references and citations Forest app for focusing during study sessions Anki for spaced repetition flashcards when memorizing is needed Google Calendar for keeping track of all commitments I'd love to hear what tools others find indispensable!"
				},

				new PostContent
				{
					Title = "Navigating Scholarship Applications",
					Content = "After successfully applying for three scholarships, I've learned some valuable lessons I want to share: Start researching opportunities months in advance - many have similar requirements so you can adapt applications Use the university scholarships office - they provide feedback on drafts Address selection criteria directly with specific examples Focus on unique aspects of your background and achievements Tailor your application to each scholarship's values Happy to answer questions about the process!"
				},

				new PostContent
				{
					Title = "Fitness Options on a Student Budget",
					Content = "Staying active is crucial for mental and physical health, but gym memberships can be expensive. Here are some budget-friendly fitness options I've found: The university gym offers student rates and free trial classes Various clubs run free or low-cost yoga, running groups, and dance classes [Local Park] has outdoor exercise equipment The university pool is free for students The [Sport] courts can be booked at no cost during off-peak times What other affordable fitness options have you discovered?"
				},

				new PostContent
				{
					Title = "Sustainable Living as a Student",
					Content = "I've been working on reducing my environmental footprint while at uni, and wanted to share some accessible sustainability practices: Our campus has water refill stations everywhere - invest in a good bottle Bulk food stores near campus allow you to reduce packaging waste The student co-op sells affordable second-hand textbooks The sustainability office loans out reusable coffee cups The bike share program is free for the first 30 minutes What other eco-friendly campus resources have you found?"
				},

				new PostContent
				{
					Title = "International Exchange Experience: What I Wish I'd Known",
					Content = "Having just returned from a semester abroad at [University] in [Country], I wanted to share insights for those considering an exchange: The application process takes much longer than you expect - start a year ahead Scholarships specifically for exchange students exist but aren't well advertised Credit transfer approval is crucial - get it in writing before you go Budget for local travel during mid-semester breaks - it's the best part! Joining student clubs immediately helps with making friends I'm happy to chat with anyone considering an exchange program!"
				},

				new PostContent
				{
					Title = "Recommended Electives Review",
					Content = "For those looking for interesting electives, here's my honest review of the ones I've taken: [Course Code]: Light workload, fascinating content, engaging lecturer [Course Code]: Challenging but rewarding, practical skills gained, responsive teaching team [Course Code]: Avoid - disorganized, unclear expectations, heavy theory with no application [Course Code]: Perfect GPA booster, interesting topics, no exam Which electives have others enjoyed or regretted?"
				},

				new PostContent
				{
					Title = "Creating a Standout LinkedIn Profile",
					Content = "After working with a career advisor to optimize my LinkedIn profile, I've seen a significant increase in recruiter connections. Here are the key improvements I made: Professional headshot (the university careers service offers free sessions) Comprehensive skills section with endorsements from professors and peers Detailed course projects with outcomes and metrics Consistent activity through sharing relevant articles and commenting Has anyone else found effective strategies for building their professional online presence?"
				},

				new PostContent
				{
					Title = "Apps That Make Uni Life Easier",
					Content = "I've compiled a list of apps that have been game-changers for my university experience: [Transit App] for accurate public transport times [Meal Planning App] for budget-friendly grocery shopping and reducing food waste [Study Planner App] for breaking down assignments into manageable tasks [Expense Tracker] for staying on top of student finances [Campus App] for room bookings and quick access to timetables Which apps do you find essential for student life?"
				},

				new PostContent
				{
					Title = "Creative Date Ideas Near Campus",
					Content = "For those balancing romance and study, here are some affordable and fun date ideas around campus: The rooftop garden in [Building] has amazing sunset views The [Museum/Gallery] on campus has free student entry on Thursdays The monthly night markets in [Area] have great food and atmosphere The film society screens classics for $5 including popcorn [Café Name] does student happy hour with 2-for-1 drinks from 3-5pm What are your go-to date spots around university?"
				},

				new PostContent
				{
					Title = "Textbook Alternatives That Saved Me $$",
					Content = "After spending way too much on textbooks in my first year, I've found several alternatives that have saved me hundreds: The library's course reserve section has copies of all required texts The [Website] has older editions that are usually 90% identical to current ones Forming a textbook share group with classmates Digital access through the university library (often overlooked!) Open educational resources for common first-year subjects What textbook hacks have worked for others?"
				}
			};

			var postFaker = new Faker<Post>()
				.RuleFor(p => p.Id, f => Guid.NewGuid())
				.RuleFor(p => p.Title, f => "")  // Will be set based on content type
				.RuleFor(p => p.Content, f => "")  // Will be set based on content type 
				.RuleFor(p => p.Author, f => f.PickRandom(users))
				.RuleFor(p => p.CreatedAt, f => f.Date.Recent(90))
				.RuleFor(p => p.Upvotes, f => f.Random.Number(0, 150))
				.RuleFor(p => p.Comments, f => new List<Comment>())
				.RuleFor(p => p.Community, f => f.PickRandom(communities))
				.RuleFor(p => p.CommunityType, (f, p) => p.Community is Course ? CommunityType.Course : CommunityType.Society);

			var posts = postFaker.Generate(count);

			// Set content based on community type and add relevant tags
			foreach (var post in posts)
			{
				// Choose post type based on community
				PostContent postContent;

				if (post.Community is Course)
				{
					// Course-related post
					if (coursePosts.Any())
					{
						int index = _random.Next(coursePosts.Count);
						postContent = coursePosts[index];
						coursePosts.RemoveAt(index); // Ensure uniqueness
					}
					else
					{
						// Fallback if we run out of predefined content
						postContent = new PostContent
						{
							Title = $"Question about {post.Community.Name}",
							Content = $"I'm currently taking {post.Community.Name} and I'm having trouble understanding some of the concepts covered in the recent lectures. Could someone please explain how {post.Community.Name.Split(':')[1].Trim()} applies to real-world scenarios? I'm particularly interested in examples that might help clarify the theoretical framework we've been discussing."
						};
					}

					// Add relevant tags for course posts
					var relevantTags = new List<Tag>();

					// Add course-specific tag if exists
					string courseName = post.Community.Name.Split(':')[1].Trim();
					var courseTag = tags.FirstOrDefault(t => courseName.Contains(t.Value));
					if (courseTag != null) relevantTags.Add(courseTag);

					// Add academic tags
					var academicTag = tags.FirstOrDefault(t => t.Value == "Academic");
					if (academicTag != null) relevantTags.Add(academicTag);

					// Add specific academic activity tags based on post title
					if (postContent.Title.Contains("Assignment") || postContent.Title.Contains("Project"))
					{
						var assignmentTag = tags.FirstOrDefault(t => t.Value == "Assignment Help");
						if (assignmentTag != null) relevantTags.Add(assignmentTag);
					}
					else if (postContent.Title.Contains("Exam") || postContent.Title.Contains("Quiz"))
					{
						var examTag = tags.FirstOrDefault(t => t.Value == "Exam Prep");
						if (examTag != null) relevantTags.Add(examTag);
					}
					else if (postContent.Title.Contains("Study Group"))
					{
						var studyTag = tags.FirstOrDefault(t => t.Value == "Study Group");
						if (studyTag != null) relevantTags.Add(studyTag);
					}

					post.Tags = relevantTags.Distinct().Take(Math.Min(relevantTags.Count, 4)).ToList();
				}
				else if (post.Community is Society)
				{
					// Society-related post
					if (societyPosts.Any())
					{
						int index = _random.Next(societyPosts.Count);
						postContent = societyPosts[index];
						societyPosts.RemoveAt(index); // Ensure uniqueness
					}
					else
					{
						// Fallback if we run out of predefined content
						postContent = new PostContent
						{
							Title = $"Updates from {post.Community.Name}",
							Content = $"Just wanted to share some updates from our recent {post.Community.Name} meeting. We've planned several exciting events for the upcoming months and are looking for volunteers to help organize them. If you're interested in getting more involved with the society, please let us know in the comments or send us a direct message."
						};
					}

					// Add tags from the society plus relevant activity tags
					var relevantTags = new List<Tag>(post.Community.Tags);

					// Add additional tags based on post title
					if (postContent.Title.Contains("Event") || postContent.Title.Contains("Workshop") || postContent.Title.Contains("Social"))
					{
						var eventTag = tags.FirstOrDefault(t => t.Value == "Campus Events");
						if (eventTag != null) relevantTags.Add(eventTag);
					}
					else if (postContent.Title.Contains("Career") || postContent.Title.Contains("Industry") || postContent.Title.Contains("Professional"))
					{
						var careerTag = tags.FirstOrDefault(t => t.Value == "Career");
						if (careerTag != null) relevantTags.Add(careerTag);
					}

					post.Tags = relevantTags.Distinct().Take(Math.Min(relevantTags.Count, 4)).ToList();
				}
				else
				{
					// General post (fallback)
					if (generalPosts.Any())
					{
						int index = _random.Next(generalPosts.Count);
						postContent = generalPosts[index];
						generalPosts.RemoveAt(index); // Ensure uniqueness
					}
					else
					{
						// Final fallback
						postContent = new PostContent
						{
							Title = "Question for everyone",
							Content = "I'm curious to hear everyone's thoughts on the best ways to balance academic commitments with extracurricular activities. What strategies have worked for you to stay organized and manage your time effectively while still enjoying your university experience?"
						};
					}

					// Add general tags based on content
					var relevantTags = new List<Tag>();
					foreach (var tag in tags)
					{
						if (postContent.Title.Contains(tag.Value) || postContent.Content.Contains(tag.Value))
						{
							relevantTags.Add(tag);
						}
					}

					// If no specific tags found, add some general ones
					if (relevantTags.Count < 2)
					{
						var campusTag = tags.FirstOrDefault(t => t.Value == "Campus Life");
						if (campusTag != null) relevantTags.Add(campusTag);

						var studentTag = tags.FirstOrDefault(t => t.Value == "Student Life");
						if (studentTag != null) relevantTags.Add(studentTag);
					}

					post.Tags = relevantTags.Distinct().Take(Math.Min(relevantTags.Count, 4)).ToList();
				}

				// Set the title and content
				post.Title = postContent.Title;
				post.Content = postContent.Content;

				// Ensure we have at least one tag
				if (post.Tags.Count == 0)
				{
					var defaultTag = tags.FirstOrDefault(t => t.Value == "Academic") ?? tags.First();
					post.Tags = new List<Tag> { defaultTag };
				}

				// Make author ID match the author object
				post.AuthorId = post.Author.Id;

				// Make community ID match the community object
				post.CommunityId = post.Community.Id;

				// Add 0-8 random comments to each post
				var commentsForPost = allComments.Where(c => !c.Author.Id.Equals(post.Author.Id)) // Avoid author commenting on own post
											   .OrderBy(x => Guid.NewGuid())
											   .Take(_random.Next(0, 9))
											   .ToList();
				post.Comments = commentsForPost;
			}

			return posts;
		}

		private List<Event> GenerateEvents(int count, List<Society> societies)
		{
			// Event data for university societies
			var eventData = new List<EventContent>
			{
				new EventContent
				{
					Name = "Freshers Welcome Social",
					Description = "Join us for our first social event of the semester! This is a perfect opportunity for new members to meet the committee and other members in a relaxed environment. We'll have games, food, drinks, and plenty of chances to chat and make new friends. Whether you're a first-year or final-year student, everyone is welcome!",
					Date = DateTime.Now.AddDays(5)
				},

				new EventContent
				{
					Name = "Industry Panel: Careers in Tech",
					Description = "We're bringing together professionals from various tech companies to discuss career paths, industry trends, and advice for students entering the job market. Panelists will share their experiences and insights, followed by a Q&A session where you can ask your burning questions. This is an invaluable opportunity to gain industry knowledge and make professional connections.",
					Date = DateTime.Now.AddDays(14)
				},

				new EventContent
				{
					Name = "Workshop: Resume Building",
					Description = "Boost your job application success with our resume building workshop! Led by career advisors from the university's careers service, this session will cover effective resume formatting, content strategies, and common mistakes to avoid. Bring your current resume for personalized feedback. Limited spots available to ensure individual attention for all participants.",
					Date = DateTime.Now.AddDays(21)
				},

				new EventContent
				{
					Name = "Guest Lecture: Emerging Technologies",
					Description = "We're excited to host Dr. Sarah Chen, a leading researcher in artificial intelligence, who will be discussing the latest developments in emerging technologies and their potential impact on society. Dr. Chen's work has been featured in numerous publications, and this is a rare opportunity to hear from her in person. Open to all students with an interest in technology and innovation.",
					Date = DateTime.Now.AddDays(30)
				},

				new EventContent
				{
					Name = "Annual General Meeting",
					Description = "As per our constitution, we're holding our Annual General Meeting to review the past year's activities, discuss our financial report, and elect the new executive committee for the coming year. All members are entitled to vote and run for positions. This is your chance to have a say in the future direction of our society. Refreshments will be provided.",
					Date = DateTime.Now.AddDays(45)
				},

				new EventContent
				{
					Name = "Networking Mixer with Alumni",
					Description = "We're bringing together current students and successful alumni for an evening of networking and knowledge sharing. Our alumni are now working at companies like Google, Microsoft, Goldman Sachs, and leading startups, and they're eager to share their experiences and advice. This event consistently leads to internship and job opportunities for our members. Business casual attire recommended.",
					Date = DateTime.Now.AddDays(60)
				},

				new EventContent
				{
					Name = "Field Trip: Industry Site Visit",
					Description = "We've arranged an exclusive tour of the local offices of a leading company in our field. See real-world applications of what you're learning in your courses, meet professionals in various roles, and get a feel for the working environment. Transport will be provided from campus, departing at 9am and returning by 3pm. Registration is essential as visitor passes need to be arranged in advance.",
					Date = DateTime.Now.AddDays(75)
				},

				new EventContent
				{
					Name = "End of Semester Celebration",
					Description = "Join us to celebrate the end of another successful semester! We'll be recognizing our outstanding members, thanking our outgoing committee, and simply enjoying each other's company before the break. The ticket price includes dinner, entertainment, and your first drink. This is always our most popular event of the year, so book early to avoid disappointment.",
					Date = DateTime.Now.AddDays(90)
				},

				new EventContent
				{
					Name = "Skills Workshop: Public Speaking",
					Description = "Develop one of the most valuable professional skills in this interactive workshop focused on public speaking and presentation techniques. Led by a professional communications coach, you'll learn strategies to overcome nervousness, structure effective presentations, and engage your audience. You'll have the opportunity to practice in a supportive environment and receive constructive feedback.",
					Date = DateTime.Now.AddDays(7)
				},

				new EventContent
				{
					Name = "Hackathon: Solutions for Sustainability",
					Description = "Put your skills to work for a good cause in our 48-hour hackathon focused on creating technological solutions for environmental sustainability. Work in teams to develop prototypes that address real-world problems. Mentors from industry will be available throughout the event to provide guidance. Prizes include cash awards, tech gadgets, and opportunities to further develop your project with industry support.",
					Date = DateTime.Now.AddDays(28)
				},

				new EventContent
				{
					Name = "Career Fair Preparation Session",
					Description = "With the university career fair approaching, we're hosting a preparation session to help you make the most of this opportunity. Learn how to research companies, prepare your elevator pitch, navigate the fair effectively, and follow up with contacts afterward. We'll also have a resume review station and LinkedIn profile check to ensure you're presenting yourself optimally to potential employers.",
					Date = DateTime.Now.AddDays(14)
				},

				new EventContent
				{
					Name = "Academic Support: Exam Preparation",
					Description = "As exams approach, we're organizing study sessions led by high-achieving senior students. These sessions will focus on reviewing key concepts, discussing past exam questions, and sharing effective study strategies specific to your courses. Small groups will ensure you get personalized assistance with challenging topics. Bring your questions and study materials!",
					Date = DateTime.Now.AddDays(30)
				},

				new EventContent
				{
					Name = "Community Volunteering Day",
					Description = "Give back to the community while building friendships within our society! We're partnering with a local organization for a day of meaningful volunteer work. Activities will include environmental cleanup, assistance at a community center, and support for a local fundraising event. No specific skills required - just your enthusiasm and willingness to help. Transport and lunch will be provided.",
					Date = DateTime.Now.AddDays(21)
				},

				new EventContent
				{
					Name = "Industry Mentorship Program Launch",
					Description = "We're excited to launch our industry mentorship program, connecting students with professionals in their field of interest. Join us for the kickoff event where you'll learn about the program structure, expectations, and benefits. You'll also have the opportunity to meet potential mentors and express your preferences. This program has helped countless students clarify their career goals and build valuable industry connections.",
					Date = DateTime.Now.AddDays(10)
				},

				new EventContent
				{
					Name = "Research Showcase",
					Description = "Celebrating the research achievements of our members! Selected students will present their research projects, ranging from course-related assignments to independent studies and honors theses. This is a fantastic opportunity to learn about diverse research topics, practice your presentation skills, and get inspired for your own academic pursuits. Presenters will receive valuable feedback from faculty members in attendance.",
					Date = DateTime.Now.AddDays(40)
				},

				new EventContent
				{
					Name = "Cultural Exchange Night",
					Description = "Celebrate the diversity within our society at this multicultural event featuring food, music, performances, and traditions from around the world. Members are encouraged to share aspects of their cultural heritage through displays, performances, or bringing a traditional dish to share. This is always one of our most popular and enriching events, fostering cross-cultural understanding and appreciation.",
					Date = DateTime.Now.AddDays(25)
				},

				new EventContent
				{
					Name = "Professional Photoshoot Session",
					Description = "Need a professional headshot for your LinkedIn profile or job applications? We've arranged for a professional photographer to take high-quality headshots for our members at a fraction of the usual cost. Multiple backdrop options will be available, and you'll receive both color and black-and-white edited images. Appointments are available in 15-minute slots throughout the day.",
					Date = DateTime.Now.AddDays(35)
				}
			};

			var eventFaker = new Faker<Event>()
				.RuleFor(e => e.Id, f => Guid.NewGuid())
				.RuleFor(e => e.Name, f => "")  // Will set later
				.RuleFor(e => e.Description, f => "")  // Will set later
				.RuleFor(e => e.ImagePathBanner, f => $"/images/events/event_{f.Random.Number(1, 30)}.jpg")
				.RuleFor(e => e.Date, f => f.Date.Future(1))
				.RuleFor(e => e.SocietyId, f => f.PickRandom(societies).Id);

			var events = eventFaker.Generate(Math.Min(count, eventData.Count));

			// Use predefined event data
			for (int i = 0; i < events.Count; i++)
			{
				events[i].Name = eventData[i].Name;
				events[i].Description = eventData[i].Description;

				// Use the predefined date if available, otherwise keep the random future date
				var predefinedDate = eventData[i].Date;
				if (predefinedDate != null)
				{
					events[i].Date = predefinedDate.Value;
				}

				// Ensure SocietyId references an existing society
				// Distribute events among societies based on event type
				// Match events to relevant societies where possible
				if (events[i].Name.Contains("Tech") || events[i].Name.Contains("Hackathon") || events[i].Name.Contains("Programming"))
				{
					var techSociety = societies.FirstOrDefault(s => s.Name.Contains("Computer") || s.Name.Contains("Tech") || s.Name.Contains("Engineering"));
					if (techSociety != null)
					{
						events[i].SocietyId = techSociety.Id;
					}
				}
				else if (events[i].Name.Contains("Business") || events[i].Name.Contains("Career") || events[i].Name.Contains("Networking"))
				{
					var businessSociety = societies.FirstOrDefault(s => s.Name.Contains("Business") || s.Name.Contains("Entrepreneurship"));
					if (businessSociety != null)
					{
						events[i].SocietyId = businessSociety.Id;
					}
				}
				else if (events[i].Name.Contains("Law") || events[i].Name.Contains("Legal") || events[i].Name.Contains("Debate"))
				{
					var lawSociety = societies.FirstOrDefault(s => s.Name.Contains("Law") || s.Name.Contains("Debating"));
					if (lawSociety != null)
					{
						events[i].SocietyId = lawSociety.Id;
					}
				}
				else if (events[i].Name.Contains("Cultural") || events[i].Name.Contains("International"))
				{
					var culturalSociety = societies.FirstOrDefault(s => s.Name.Contains("Cultural") || s.Name.Contains("International"));
					if (culturalSociety != null)
					{
						events[i].SocietyId = culturalSociety.Id;
					}
				}
				else if (events[i].Name.Contains("Arts") || events[i].Name.Contains("Film") || events[i].Name.Contains("Photography"))
				{
					var artsSociety = societies.FirstOrDefault(s => s.Name.Contains("Arts") || s.Name.Contains("Film") || s.Name.Contains("Photography"));
					if (artsSociety != null)
					{
						events[i].SocietyId = artsSociety.Id;
					}
				}
				// General events can stay with random society assignment
			}

			return events;
		}
	}

	// Helper classes to fix tuple access issues
	public class PostContent
	{
		public string Title { get; set; } = "";
		public string Content { get; set; } = "";
	}

	public class EventContent
	{
		public string Name { get; set; } = "";
		public string Description { get; set; } = "";
		public DateTime? Date { get; set; }
	}
}