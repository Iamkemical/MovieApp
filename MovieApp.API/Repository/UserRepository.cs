using Microsoft.Extensions.Options;
using Microsoft.IdentityModel.Tokens;
using MovieApp.API.ApplicationOptions;
using MovieApp.API.Data;
using MovieApp.API.Models;
using MovieApp.API.Repository.IRepository;
using PasswordHashingLibrary.Helper.PasswordHasher;
using System;
using System.Collections.Generic;
using System.IdentityModel.Tokens.Jwt;
using System.Linq;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;

namespace MovieApp.API.Repository
{
    public class UserRepository : IUserRepository
    {
        private readonly ApplicationDbContext _dbContext;
        private readonly AppSettings _appSettings;

        public UserRepository(ApplicationDbContext dbContext, IOptions<AppSettings> appSettings)
        {
            _dbContext = dbContext;
            _appSettings = appSettings.Value;
        }
        public UserModel Authenticate(string username, string password)
        {
            if (string.IsNullOrEmpty(username) || string.IsNullOrEmpty(password))
                return null;

            var user = _dbContext.Users.SingleOrDefault(x => x.UserName == username);

            //check if username exists
            if (user == null)
                return null;

            //checks if password is correct
            if (!PasswordHash.VerifyPasswordHash(password, user.PasswordHash, user.PasswordSalt))
                return null;

            //if user was found generate JWT Token
            var tokenHandler = new JwtSecurityTokenHandler();
            var key = Encoding.ASCII.GetBytes(_appSettings.Secret);
            var tokenDescriptor = new SecurityTokenDescriptor
            {
                Subject = new ClaimsIdentity(new Claim[] {
                    new Claim(ClaimTypes.Name, user.UserId.ToString()),
                    new Claim(ClaimTypes.Role, user.Role)
                }),
                Expires = DateTime.UtcNow.AddDays(7),
                SigningCredentials = new SigningCredentials
                (new SymmetricSecurityKey(key), SecurityAlgorithms.HmacSha256Signature)
            };
            var token = tokenHandler.CreateToken(tokenDescriptor);
            user.Token = tokenHandler.WriteToken(token);

            //authentication successful
            return user;
        }

        public UserModel Create(UserModel user, string password)
        {
            UserModel userObj = new UserModel
            {
                UserName = user.UserName,
                Password = user.Password,
                Role = "Customer",
                Email = user.Email,
                FirstName = user.FirstName,
                LastName = user.LastName,
            };

            //validation
            if (string.IsNullOrWhiteSpace(password))
                throw new AppException("Password is required");

            if (_dbContext.Users.Any(x => x.UserName == user.UserName))
                throw new AppException("Username \"" + user.UserName + "\" is already taken");

            byte[] passwordHash, passwordSalt;
            PasswordHash.CreatePasswordHash(password, out passwordHash, out passwordSalt);

            userObj.PasswordHash = passwordHash;
            userObj.PasswordSalt = passwordSalt;

            _dbContext.Users.Add(userObj);
            _dbContext.SaveChanges();

            return userObj;
        }

        public void Delete(int id)
        {
            var user = _dbContext.Users.Find(id);
            if (user != null)
            {
                _dbContext.Users.Remove(user);
                _dbContext.SaveChanges();
            }
        }

        public IEnumerable<UserModel> GetAll()
        {
            return _dbContext.Users;
        }

        public UserModel GetUserById(int id)
        {
            return _dbContext.Users.Find(id);
        }

        public void Update(UserModel user, string password = null)
        {
            var userParam = _dbContext.Users.Find(user.UserId);

            if (userParam == null)
                throw new AppException("User not found");

            if (userParam.UserName != user.UserName)
            {
                //username has changed so check if the new username is already taken
                if (_dbContext.Users.Any(x => x.UserName == userParam.UserName))
                    throw new AppException("Username " + userParam.UserName + " is already taken");
            }

            //update user properties
            user.FirstName = userParam.FirstName;
            user.LastName = userParam.LastName;
            user.UserName = userParam.UserName;
            user.Email = userParam.Email;

            //update password if it was entered
            if (!string.IsNullOrWhiteSpace(password))
            {
                byte[] passwordHash, passwordSalt;
                PasswordHash.CreatePasswordHash(password, out passwordHash, out passwordSalt);

                user.PasswordHash = passwordHash;
                user.PasswordSalt = passwordSalt;
            }

            _dbContext.Users.Update(user);
            _dbContext.SaveChanges();
        }
    }
}
