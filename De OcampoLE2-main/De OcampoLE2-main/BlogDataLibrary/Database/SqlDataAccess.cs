using System.Data;
using System.Data.SqlClient;
using Dapper;
using Microsoft.Extensions.Configuration;

namespace BlogDataLibrary.Database
{
    public class SqlDataAccess : ISqlDataAccess
    {
        private readonly IConfiguration _config;

        public SqlDataAccess(IConfiguration config)
        {
            _config = config;
        }

        // For reading data
        public async Task<List<T>> LoadData<T, U>(
            string sql,
            U parameters,
            string connectionStringName = "Default")
        {
            using IDbConnection connection = new SqlConnection(
                _config.GetConnectionString(connectionStringName));

            var rows = await connection.QueryAsync<T>(sql, parameters);
            return rows.ToList();
        }

        // For saving data (INSERT, UPDATE, DELETE)
        public async Task SaveData<T>(
            string sql,
            T parameters,
            string connectionStringName = "Default")
        {
            using IDbConnection connection = new SqlConnection(
                _config.GetConnectionString(connectionStringName));

            await connection.ExecuteAsync(sql, parameters);
        }
    }

    // Interface for dependency injection
    public interface ISqlDataAccess
    {
        Task<List<T>> LoadData<T, U>(string sql, U parameters, string connectionStringName = "Default");
        Task SaveData<T>(string sql, T parameters, string connectionStringName = "Default");
    }
}
