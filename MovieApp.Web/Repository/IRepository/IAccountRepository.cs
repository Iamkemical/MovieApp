using MovieApp.Web.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace MovieApp.Web.Repository.IRepository
{
    public interface IAccountRepository : IRepository<UserModel>
    {
        Task<UserModel> LoginAsync(string url, UserModel objToCreate);
        Task<bool> RegisterAsync(string url, UserModel ObjToCreate);
    }
}
