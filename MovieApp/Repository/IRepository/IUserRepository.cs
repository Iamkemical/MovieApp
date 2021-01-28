using MovieApp.API.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace MovieApp.API.Repository.IRepository
{
    public interface IUserRepository
    {
        UserModel Authenticate(string username, string password);
        IEnumerable<UserModel> GetAll();
        UserModel GetUserById(int id);
        UserModel Create(UserModel user, string password);
        void Update(UserModel user, string password = null);
        void Delete(int id);
    }
}
