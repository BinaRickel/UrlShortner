using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ShortenUrl.Data
{
    public class UserRepository
    {
        private string _connectionString;

        public UserRepository(string connectionString)
        {
            _connectionString = connectionString;
        }
        public void RegisterUser(User user, string password)
        {
            string passwordSalt = PasswordHelper.GenerateSalt();
            string passwordHash = PasswordHelper.HashPassword(password, passwordSalt);
            user.PasswordSalt = passwordSalt;
            user.PasswordHash = passwordHash;
            using (var context = new UserUrlDataContext(_connectionString))
            {
                context.Users.InsertOnSubmit(user);
                context.SubmitChanges();
            }

        }
        public User Login(string email, string password)
        {
            var user = GetByEmail(email);
            if (user == null)
            {
                return null;
            }
            bool isCorrectPassword = PasswordHelper.PasswordMatch(password, user.PasswordSalt, user.PasswordHash);
            if (isCorrectPassword)
            {
                return user;
            }
            return null;
        }
        public User GetByEmail(string email)
        {
            using (var context = new UserUrlDataContext(_connectionString))
            {
                return context.Users.FirstOrDefault(u => u.Email == email);
            }
        }
    }
}
