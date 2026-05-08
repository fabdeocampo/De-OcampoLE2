using BlogDataLibrary.Database;
using BlogDataLibrary.Models;

namespace BlogDataLibrary.Data
{
    public class SqlData
    {
        private readonly ISqlDataAccess _db;

        public SqlData(ISqlDataAccess db)
        {
            _db = db;
        }

        // Get all posts with author names
        public Task<List<ListPostModel>> GetPosts()
        {
            string sql = @"SELECT p.Id, p.Title, u.FirstName + ' ' + u.LastName AS Author
                           FROM dbo.Posts p
                           INNER JOIN dbo.Users u ON p.UserId = u.Id;";
            return _db.LoadData<ListPostModel, dynamic>(sql, new { });
        }

        // Get a single post by Id
        public Task<List<PostModel>> GetPostById(int id)
        {
            string sql = @"SELECT * FROM dbo.Posts WHERE Id = @Id;";
            return _db.LoadData<PostModel, dynamic>(sql, new { Id = id });
        }

        // Add a new post
        public Task SavePost(PostModel post)
        {
            string sql = @"INSERT INTO dbo.Posts (Title, Content, UserId)
                           VALUES (@Title, @Content, @UserId);";
            return _db.SaveData(sql, post);
        }

        // Add a new user
        public Task SaveUser(UserModel user)
        {
            string sql = @"INSERT INTO dbo.Users (Username, Password, FirstName, LastName)
                           VALUES (@Username, @Password, @FirstName, @LastName);";
            return _db.SaveData(sql, user);
        }

        // Get all users
        public Task<List<UserModel>> GetUsers()
        {
            string sql = "SELECT * FROM dbo.Users;";
            return _db.LoadData<UserModel, dynamic>(sql, new { });
        }
    }
}
