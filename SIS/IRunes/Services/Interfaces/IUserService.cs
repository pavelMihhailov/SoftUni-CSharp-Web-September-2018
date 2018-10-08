using IRunes.Models;

namespace IRunes.Services.Interfaces
{
    public interface IUserService
    {
        bool CheckIfUserExists(string username);

        void CreateUser(string username, string passowrdHashed, string email);

        User GetUser(string login);

        bool CheckIfEmailIsTaken(string email);
    }
}