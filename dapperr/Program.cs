using Dapper;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;

namespace Deppper
{
    class Program
    {
        static void Main(string[] args)
        {
            private string _connectionString = (@"Server=MUHAMMAD\\SQLEXPRESS;Database=Test;Trusted_Connection=True;");

            public List<User> getUser()
            {
                var result = new List<User>();
                using (SqlConnection connection = new SqlConnection(_connectionString))
                {
                    result = connection.Query<User>("SELECT UserID, UserName, FirstName, LastName, , CreatedDate,  FROM Users").ToList();
                }
                return result;
            }

            public int createUser(User user)
            {
                int res = 0;
            
                using (SqlConnection connection = new SqlConnection(_connectionString))
                {
                    string sqlQuery = "INSERT INTO Users (UserName, FirstName, LastName,  CreatedDate, )" +
                                    "VALUES (@UserName, @FirstName, @LastName,  @CreatedDate, @IsActive)" +
                                    "SELECT CAST(SCOPE_IDENTITY() AS INT);";

                    var insertFields = new
                    {
                        UserName = user.UserName,
                        FirstName = user.FirstName,
                        LastName = user.LastName,
                        CreatedDate = DateTime.Now,
                    };

                    var id = connection.Query<int>(sqlQuery, insertFields).Single();
                    if (id > 0)
                        res = 1;

                    return res;
                }
            }

            public int updateUser(User user)
            {
                using (SqlConnection connection = new SqlConnection(_connectionString))
                {
                    var affectedRows = connection.Execute("UPDATE Users SET UserName = @UserName, FirstName = @FirstName, LastName = @LastName,   WHERE UserID = @UserID",
                        new
                        {
                            UserID = user.UserID,
                            UserName = user.UserName,
                            FirstName = user.FirstName,
                            LastName = user.LastName,
                        
                        });

                    return affectedRows;
                }
            }

            public int deleteUser(int id)
            {
                using (SqlConnection connection = new SqlConnection(_connectionString))
                {
                    return connection.Execute("DELETE FROM Users WHERE UserID = @Id", new { Id = id });
                }
            

            }
        }
    }

    

    public class User
    {
        
        public int UserID { get; set; }
        public string UserName { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public DateTime CreatedDate { get; set; }
    }

}
