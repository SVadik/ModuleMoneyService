using Dapper;
using Npgsql;
using System;
using System.Collections.Generic;
using System.Data;
using System.Text;

namespace Auth
{
    public interface IUserService
    {
        bool IsValidUser(string username, string password);
        User Create(User user);
        void Delete(int id);
        User GetByUsername(string username);
        //List<User> GetUsers();
        void Update(User user);
    }

    public class UserService : IUserService
    {
        string connectionString = null;
        public UserService(string conn)
        {

            connectionString = conn;
        }

        public bool IsValidUser(string username, string password)
        {
            var testUser = new User(username, password);
            return Password.CheckPassword(password, testUser.Salt, testUser.Password);
        }

        internal IDbConnection Connection
        {
            get
            {
                return new NpgsqlConnection(connectionString);
            }
        }

        //public List<User> GetUsers()
        //{
        //    using (IDbConnection db = Connection)
        //    {
        //        db.Open();
        //        return db.Query("SELECT * FROM Users").ToList();
        //    }
        //}

        public User GetByUsername(string username)
        {
            using (IDbConnection db = Connection)
            {
                return db.QuerySingleOrDefault<User>("SELECT id, username, firstname FROM public.users WHERE username = @username", new { username });
            }
        }

        public User Create(User user)
        {
            var dbUser = GetByUsername(user.Username);
            if(dbUser != null)
            {
                return null;
            }

            
            using (IDbConnection db = Connection)
            {
                var sqlQuery = "INSERT INTO users ( username, firstname, salt, password) VALUES(@Username, @Firstname, @Salt, @Password)";
                db.Execute(sqlQuery, user);

                //var sqlQuery = "INSERT INTO Users ( Username, Salt, PasswordHash) VALUES(@Username, @Salt, @PasswordHash); SELECT CAST(SCOPE_IDENTITY() as int)";
                //int? userId = db.Query<int>(sqlQuery, user).FirstOrDefault();
                //user.Id = userId.Value;
            }
            return user;
        }

        public void Update(User user)
        {
            using (IDbConnection db = Connection)
            {
                var sqlQuery = "UPDATE users SET firstname = @Firstame, salt = @Salt, password = @Password WHERE id = @Id";
                db.Execute(sqlQuery, user);
            }
        }

        public void Delete(int id)
        {
            using (IDbConnection db = Connection)
            {
                var sqlQuery = "DELETE FROM users WHERE id = @id";
                db.Execute(sqlQuery, new { id });
            }
        }
    }
}
