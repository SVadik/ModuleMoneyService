using System;
using System.Collections.Generic;
using System.Text;

namespace Auth
{
    public class User
    {
        public User(string username, string password, string firstname)
        {
            Username = username;
            _password = new Password(password);
            Firstname = firstname;
        }

        public User(string username, string password)
        {
            Username = username;
            _password = new Password(password);
        }

        private Password _password;

        public string Username { get; set; }

        public int Id { get; set; }

        public string Password => _password.PasswordHash;

        public string Salt => _password.Salt;

        public string Firstname { get; set; }
    }
}
