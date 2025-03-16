using DevConnect.Models;

namespace DeveloperSocialPlatform.Data
{
    public static class DevConnectDbContext
    {
        public static void SeedData(DevConnect.Models.DevConnectDbContext context)
        {
            // Only seed if database is empty
            if (context.Profiles.Any())
            {
                return; // Database already has data
            }

            // Seed Profiles
            var profiles = new List<Profile>
            {
                new Profile
                {
                    Username = "sarah_coder",
                    FullName = "Sarah Johnson",
                    Description =
                        "Full-stack developer specialized in React and .NET Core. Passionate about clean code and performance optimization.",
                    AvatarUrl = "https://randomuser.me/api/portraits/women/44.jpg",
                    Email = "sarah.j@example.com",
                    Skills = "C#, React, TypeScript, SQL, Docker",
                    CreatedAt = DateTime.UtcNow.AddMonths(-8),
                    UpdatedAt = DateTime.UtcNow.AddMonths(-8)
                },
                new Profile
                {
                    Username = "dev_mike",
                    FullName = "Michael Chen",
                    Description =
                        "Backend engineer with focus on distributed systems and microservices architecture. Open source contributor.",
                    AvatarUrl = "https://randomuser.me/api/portraits/men/32.jpg",
                    Email = "mike.chen@example.com",
                    Skills = "Java, Spring Boot, Kubernetes, Kafka, AWS",
                    CreatedAt = DateTime.UtcNow.AddMonths(-7),
                    UpdatedAt = DateTime.UtcNow.AddMonths(-7)
                },
                new Profile
                {
                    Username = "js_ninja",
                    FullName = "Alex Rodriguez",
                    Description =
                        "Frontend developer and UI/UX enthusiast. Building accessible and beautiful web experiences.",
                    AvatarUrl = "https://randomuser.me/api/portraits/men/67.jpg",
                    Email = "alex.r@example.com",
                    Skills = "JavaScript, Vue.js, CSS, Figma, Accessibility",
                    CreatedAt = DateTime.UtcNow.AddMonths(-6),
                    UpdatedAt = DateTime.UtcNow.AddMonths(-6)
                },
                new Profile
                {
                    Username = "data_priya",
                    FullName = "Priya Patel",
                    Description =
                        "Data scientist working on machine learning applications in healthcare. Python enthusiast.",
                    AvatarUrl = "https://randomuser.me/api/portraits/women/26.jpg",
                    Email = "priya.p@example.com",
                    Skills = "Python, TensorFlow, PyTorch, SQL, Data Visualization",
                    CreatedAt = DateTime.UtcNow.AddMonths(-5),
                    UpdatedAt = DateTime.UtcNow.AddMonths(-5)
                },
                new Profile
                {
                    Username = "cloud_dave",
                    FullName = "David Wilson",
                    Description = "DevOps engineer with expertise in cloud infrastructure and CI/CD pipelines.",
                    AvatarUrl = "https://randomuser.me/api/portraits/men/2.jpg",
                    Email = "david.w@example.com",
                    Skills = "Azure, Terraform, GitHub Actions, PowerShell, Linux",
                    CreatedAt = DateTime.UtcNow.AddMonths(-4),
                    UpdatedAt = DateTime.UtcNow.AddMonths(-4)
                }
            };

            context.Profiles.AddRange(profiles);
            context.SaveChanges();

            // Seed Posts
            var posts = new List<Post>
            {
                new Post
                {
                    ProfileId = profiles[0].Id,
                    Title = "Optimizing React Performance",
                    Content =
                        "Today I want to share some techniques I've been using to optimize React application performance. First, always use React.memo for components that render often but with the same props. Second, move expensive calculations into useMemo hooks. Third, use the React DevTools profiler to identify bottlenecks...",
                    CreatedAt = DateTime.UtcNow.AddDays(-20),
                    UpdatedAt = DateTime.UtcNow.AddDays(-20)
                },
                new Post
                {
                    ProfileId = profiles[1].Id,
                    Title = "Building Resilient Microservices",
                    Content =
                        "Microservices are great, but they come with challenges. Here's how I approach building resilient microservices that can handle failures gracefully. The circuit breaker pattern is essential - I recommend using libraries like Polly in .NET or Resilience4j in Java...",
                    CreatedAt = DateTime.UtcNow.AddDays(-18),
                    UpdatedAt = DateTime.UtcNow.AddDays(-18)
                },
                new Post
                {
                    ProfileId = profiles[2].Id,
                    Title = "Creating Accessible Web Forms",
                    Content =
                        "Accessibility is not optional! Here are some practical tips for making your web forms more accessible: 1. Always use labels with form controls. 2. Add proper ARIA attributes. 3. Ensure keyboard navigation works. 4. Provide clear error messages...",
                    CreatedAt = DateTime.UtcNow.AddDays(-15),
                    UpdatedAt = DateTime.UtcNow.AddDays(-15)
                },
                new Post
                {
                    ProfileId = profiles[3].Id,
                    Title = "Introduction to Feature Engineering",
                    Content =
                        "Feature engineering is often more important than the choice of algorithm in machine learning projects. Here's a beginner-friendly guide to effective feature engineering techniques including normalization, one-hot encoding, and dealing with missing values...",
                    CreatedAt = DateTime.UtcNow.AddDays(-12),
                    UpdatedAt = DateTime.UtcNow.AddDays(-12)
                },
                new Post
                {
                    ProfileId = profiles[4].Id,
                    Title = "Infrastructure as Code Best Practices",
                    Content =
                        "After managing cloud infrastructure for several years, here are my top 5 best practices for Infrastructure as Code: 1. Use version control. 2. Implement automated testing. 3. Keep modules small and focused. 4. Use consistent naming conventions. 5. Document everything...",
                    CreatedAt = DateTime.UtcNow.AddDays(-10),
                    UpdatedAt = DateTime.UtcNow.AddDays(-10)
                },
                new Post
                {
                    ProfileId = profiles[0].Id,
                    Title = "Entity Framework Core Tips and Tricks",
                    Content =
                        "After working with EF Core for several projects, I've compiled some non-obvious tips that have helped me a lot: 1. Use AsNoTracking() for read-only queries. 2. Take advantage of global query filters. 3. Understand the difference between Include() and explicit loading...",
                    CreatedAt = DateTime.UtcNow.AddDays(-5),
                    UpdatedAt = DateTime.UtcNow.AddDays(-5)
                },
                new Post
                {
                    ProfileId = profiles[2].Id,
                    Title = "The Power of CSS Grid",
                    Content =
                        "CSS Grid has transformed how I approach layouts. Here's a tutorial on how to create complex, responsive layouts with minimal code using CSS Grid. The best part is that it works well with Flexbox - they're complementary tools!",
                    CreatedAt = DateTime.UtcNow.AddDays(-3),
                    UpdatedAt = DateTime.UtcNow.AddDays(-3)
                }
            };

            context.Posts.AddRange(posts);
            context.SaveChanges();

            // Seed Comments
            var comments = new List<Comment>
            {
                new Comment
                {
                    PostId = posts[0].Id,
                    ProfileId = profiles[2].Id,
                    Content =
                        "Great tips! I've been using React.memo a lot lately and it's made a huge difference in my app's performance.",
                    CreatedAt = DateTime.UtcNow.AddDays(-19),
                    UpdatedAt = DateTime.UtcNow.AddDays(-19)
                },
                new Comment
                {
                    PostId = posts[0].Id,
                    ProfileId = profiles[4].Id,
                    Content =
                        "Have you tried the new React Server Components? Would be interesting to hear your thoughts on those.",
                    CreatedAt = DateTime.UtcNow.AddDays(-19),
                    UpdatedAt = DateTime.UtcNow.AddDays(-19)
                },
                new Comment
                {
                    PostId = posts[1].Id,
                    ProfileId = profiles[0].Id,
                    Content =
                        "I've been using Polly in my .NET projects and it's a game changer for handling transient failures.",
                    CreatedAt = DateTime.UtcNow.AddDays(-17),
                    UpdatedAt = DateTime.UtcNow.AddDays(-17)
                },
                new Comment
                {
                    PostId = posts[2].Id,
                    ProfileId = profiles[3].Id,
                    Content = "Accessibility is so important and often overlooked. Thanks for raising awareness!",
                    CreatedAt = DateTime.UtcNow.AddDays(-14),
                    UpdatedAt = DateTime.UtcNow.AddDays(-14)
                },
                new Comment
                {
                    PostId = posts[3].Id,
                    ProfileId = profiles[1].Id,
                    Content =
                        "Great intro! Do you have any recommendations for handling categorical features with high cardinality?",
                    CreatedAt = DateTime.UtcNow.AddDays(-11),
                    UpdatedAt = DateTime.UtcNow.AddDays(-11)
                },
                new Comment
                {
                    PostId = posts[4].Id,
                    ProfileId = profiles[0].Id,
                    Content =
                        "I've been following these practices and they've made our deployments so much more reliable. Especially automated testing!",
                    CreatedAt = DateTime.UtcNow.AddDays(-9),
                    UpdatedAt = DateTime.UtcNow.AddDays(-9)
                },
                new Comment
                {
                    PostId = posts[5].Id,
                    ProfileId = profiles[1].Id,
                    Content =
                        "The AsNoTracking tip alone gave us a 30% performance boost in our read-heavy application. Highly recommended!",
                    CreatedAt = DateTime.UtcNow.AddDays(-4),
                    UpdatedAt = DateTime.UtcNow.AddDays(-4)
                },
                new Comment
                {
                    PostId = posts[6].Id,
                    ProfileId = profiles[4].Id,
                    Content = "CSS Grid changed my life too! Have you tried the subgrid feature yet?",
                    CreatedAt = DateTime.UtcNow.AddDays(-2),
                    UpdatedAt = DateTime.UtcNow.AddDays(-2)
                }
            };

            context.Comments.AddRange(comments);
            context.SaveChanges();

            // Seed Subscriptions
            var subscriptions = new List<Subscription>
            {
                new Subscription
                {
                    FollowerId = profiles[1].Id, FollowingId = profiles[0].Id, CreatedAt = DateTime.UtcNow.AddDays(-15)
                },
                new Subscription
                {
                    FollowerId = profiles[2].Id, FollowingId = profiles[0].Id, CreatedAt = DateTime.UtcNow.AddDays(-14)
                },
                new Subscription
                {
                    FollowerId = profiles[3].Id, FollowingId = profiles[0].Id, CreatedAt = DateTime.UtcNow.AddDays(-13)
                },
                new Subscription
                {
                    FollowerId = profiles[0].Id, FollowingId = profiles[1].Id, CreatedAt = DateTime.UtcNow.AddDays(-12)
                },
                new Subscription
                {
                    FollowerId = profiles[2].Id, FollowingId = profiles[1].Id, CreatedAt = DateTime.UtcNow.AddDays(-11)
                },
                new Subscription
                {
                    FollowerId = profiles[0].Id, FollowingId = profiles[2].Id, CreatedAt = DateTime.UtcNow.AddDays(-10)
                },
                new Subscription
                {
                    FollowerId = profiles[1].Id, FollowingId = profiles[2].Id, CreatedAt = DateTime.UtcNow.AddDays(-9)
                },
                new Subscription
                {
                    FollowerId = profiles[3].Id, FollowingId = profiles[2].Id, CreatedAt = DateTime.UtcNow.AddDays(-8)
                },
                new Subscription
                {
                    FollowerId = profiles[4].Id, FollowingId = profiles[2].Id, CreatedAt = DateTime.UtcNow.AddDays(-7)
                },
                new Subscription
                {
                    FollowerId = profiles[0].Id, FollowingId = profiles[3].Id, CreatedAt = DateTime.UtcNow.AddDays(-6)
                },
                new Subscription
                {
                    FollowerId = profiles[2].Id, FollowingId = profiles[3].Id, CreatedAt = DateTime.UtcNow.AddDays(-5)
                },
                new Subscription
                {
                    FollowerId = profiles[1].Id, FollowingId = profiles[4].Id, CreatedAt = DateTime.UtcNow.AddDays(-4)
                },
                new Subscription
                {
                    FollowerId = profiles[3].Id, FollowingId = profiles[4].Id, CreatedAt = DateTime.UtcNow.AddDays(-3)
                }
            };

            context.Subscriptions.AddRange(subscriptions);
            context.SaveChanges();

            // Seed Likes
            var likes = new List<Like>
            {
                new Like { PostId = posts[0].Id, ProfileId = profiles[1].Id, CreatedAt = DateTime.UtcNow.AddDays(-19) },
                new Like { PostId = posts[0].Id, ProfileId = profiles[2].Id, CreatedAt = DateTime.UtcNow.AddDays(-19) },
                new Like { PostId = posts[0].Id, ProfileId = profiles[3].Id, CreatedAt = DateTime.UtcNow.AddDays(-18) },
                new Like { PostId = posts[1].Id, ProfileId = profiles[0].Id, CreatedAt = DateTime.UtcNow.AddDays(-17) },
                new Like { PostId = posts[1].Id, ProfileId = profiles[3].Id, CreatedAt = DateTime.UtcNow.AddDays(-17) },
                new Like { PostId = posts[1].Id, ProfileId = profiles[4].Id, CreatedAt = DateTime.UtcNow.AddDays(-16) },
                new Like { PostId = posts[2].Id, ProfileId = profiles[0].Id, CreatedAt = DateTime.UtcNow.AddDays(-14) },
                new Like { PostId = posts[2].Id, ProfileId = profiles[3].Id, CreatedAt = DateTime.UtcNow.AddDays(-14) },
                new Like { PostId = posts[3].Id, ProfileId = profiles[1].Id, CreatedAt = DateTime.UtcNow.AddDays(-11) },
                new Like { PostId = posts[3].Id, ProfileId = profiles[4].Id, CreatedAt = DateTime.UtcNow.AddDays(-11) },
                new Like { PostId = posts[4].Id, ProfileId = profiles[0].Id, CreatedAt = DateTime.UtcNow.AddDays(-9) },
            };
            
            context.Likes.AddRange(likes);
            context.SaveChanges();
        }
    }
}