using System.Linq;
using IRunes.Data;
using IRunes.Models;
using IRunes.Services.Interfaces;

namespace IRunes.Services
{
    public class UserService : IUserService
    {
        public bool CheckIfUserExists(string username)
        {
            using (var db = new IRunesDbContext())
            {
                return db.Users.Any(u => u.Username == username);
            }
        }

        public void CreateUser(string username, string passowrdHashed, string email)
        {
            var user = new User { Username = username, Password = passowrdHashed, Email = email };
            using (var db = new IRunesDbContext())
            {
                db.Users.Add(user);
                db.SaveChanges();
            }
        }

        public User GetUser(string login)
        {
            using (var db = new IRunesDbContext())
            {
                return db.Users.FirstOrDefault(u => u.Username.ToLower() == login || u.Email.ToLower() == login);
            }
        }

        public bool CheckIfEmailIsTaken(string email)
        {
            using (var db = new IRunesDbContext())
            {
                return db.Users.Any(u => u.Email == email);
            }
        }
    }
}