using System;
using System.Threading.Tasks;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using BlogDataLibrary.Database;
using BlogDataLibrary.Data;
using BlogDataLibrary.Models;

namespace BlogTestUI
{
    internal class Program
    {
        static async Task Main(string[] args)
        {
            // 1) Load configuration (reads appsettings.json)
            var config = new ConfigurationBuilder()
                .SetBasePath(AppContext.BaseDirectory)
                .AddJsonFile("appsettings.json", optional: false, reloadOnChange: true)
                .Build();

            // 2) Configure dependency injection
            var services = new ServiceCollection()
                .AddSingleton<IConfiguration>(config)
                .AddSingleton<ISqlDataAccess, SqlDataAccess>()
                .AddSingleton<SqlData>()
                .BuildServiceProvider();

            var db = services.GetRequiredService<SqlData>();

            // 3) Menu loop
            while (true)
            {
                Console.Clear();
                Console.WriteLine("==== BLOG TEST MENU ====");
                Console.WriteLine("1 - List Users");
                Console.WriteLine("2 - Add User");
                Console.WriteLine("3 - List Posts");
                Console.WriteLine("4 - Add Post");
                Console.WriteLine("0 - Exit");
                Console.Write("Choose an option: ");
                var choice = Console.ReadLine();

                switch (choice)
                {
                    case "1":
                        await ListUsers(db);
                        break;
                    case "2":
                        await AddUser(db);
                        break;
                    case "3":
                        await ListPosts(db);
                        break;
                    case "4":
                        await AddPost(db);
                        break;
                    case "0":
                        Console.WriteLine("Exiting...");
                        return;
                    default:
                        Console.WriteLine("Invalid choice, press any key to continue...");
                        Console.ReadKey();
                        break;
                }
            }
        }

        // List Users
        private static async Task ListUsers(SqlData db)
        {
            Console.Clear();
            Console.WriteLine("== Users ==");
            var users = await db.GetUsers();
            foreach (var u in users)
            {
                Console.WriteLine($"{u.Id}: {u.FirstName} {u.LastName} ({u.Username})");
            }
            if (users.Count == 0) Console.WriteLine("No users found.");
            Pause();
        }

        // Add User
        private static async Task AddUser(SqlData db)
        {
            Console.Clear();
            Console.WriteLine("== Add User ==");

            Console.Write("First Name: ");
            string first = Console.ReadLine() ?? "";

            Console.Write("Last Name: ");
            string last = Console.ReadLine() ?? "";

            Console.Write("Username: ");
            string username = Console.ReadLine() ?? "";

            Console.Write("Password: ");
            string password = Console.ReadLine() ?? "";

            var newUser = new UserModel
            {
                FirstName = first,
                LastName = last,
                Username = username,
                Password = password
            };

            await db.SaveUser(newUser);
            Console.WriteLine("User added successfully!");
            Pause();
        }

        // List Posts
        private static async Task ListPosts(SqlData db)
        {
            Console.Clear();
            Console.WriteLine("== Posts ==");
            var posts = await db.GetPosts();
            foreach (var p in posts)
            {
                Console.WriteLine($"{p.Id}: {p.Title} by {p.Author}");
            }
            if (posts.Count == 0) Console.WriteLine("No posts found.");
            Pause();
        }

        // Add Post
        private static async Task AddPost(SqlData db)
        {
            Console.Clear();
            Console.WriteLine("== Add Post ==");

            var users = await db.GetUsers();
            if (users.Count == 0)
            {
                Console.WriteLine("No users exist. Please add a user first.");
                Pause();
                return;
            }

            Console.WriteLine("Choose an author (by Id):");
            foreach (var u in users)
            {
                Console.WriteLine($"{u.Id}: {u.FirstName} {u.LastName}");
            }

            Console.Write("Author Id: ");
            int.TryParse(Console.ReadLine(), out int authorId);

            Console.Write("Title: ");
            string title = Console.ReadLine() ?? "";

            Console.Write("Content: ");
            string content = Console.ReadLine() ?? "";

            var newPost = new PostModel
            {
                Title = title,
                Content = content,
                UserId = authorId
            };

            await db.SavePost(newPost);
            Console.WriteLine("Post added successfully!");
            Pause();
        }

        // Helper to pause
        private static void Pause()
        {
            Console.WriteLine("\nPress any key to continue...");
            Console.ReadKey();
        }
    }
}
