using Npgsql;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Cw3.DAL
{
    public class PsqlAuthService : IDbAuthService
    {
        private readonly string connectionString;
        public PsqlAuthService()
        {
            connectionString = System.IO.File.ReadLines("auth\\pg.cstr").First();
        }
        public string AuthenticateAndGetRole(string login, string password)
        {
            using (var connection = new NpgsqlConnection(connectionString))
            using (var command = new NpgsqlCommand())
            {
                command.Connection = connection;
                command.CommandText = "select s.Role from Student as s " +
                        "where s.IndexNumber = @login and s.Password = @pass;";
                command.Parameters.AddWithValue("login", login);
                command.Parameters.AddWithValue("pass", password);
                connection.Open();
                var reader = command.ExecuteReader();
                if(!reader.Read()) return null;
                return reader["Role"].ToString();
            }
        }

        public string GetSalt(string login)
        {
            using (var connection = new NpgsqlConnection(connectionString))
            using (var command = new NpgsqlCommand())
            {
                command.Connection = connection;
                command.CommandText = "select Salt from Student where IndexNumber = @login";
                command.Parameters.AddWithValue("login", login);
                NpgsqlDataReader reader = command.ExecuteReader();
                if (!reader.Read())
                    return null;
                return (reader["salt"].ToString());
            }
        }

    }
}
