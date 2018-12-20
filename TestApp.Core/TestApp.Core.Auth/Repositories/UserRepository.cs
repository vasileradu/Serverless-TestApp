using System;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using TestApp.Core.Auth.Interfaces;
using TestApp.Core.Auth.Models;

namespace TestApp.Core.Auth.Repositories
{
    public class UserRepository : IUserRepository, IDisposable
    {
        private readonly TestAppAuthContext _dbContext;

        public UserRepository(TestAppAuthContext context)
        {
            this._dbContext = context;
        }

        #region IUserRepository

        public Task<bool> ValidateCredentials(string username, string password)
        {
            // Does not account for upper/lower case; anything works.
            return this._dbContext.User
                .AnyAsync(user => user.Username.Equals(username) && user.Password.Equals(password));
        }

        #endregion

        public void Dispose()
        {
            this._dbContext?.Dispose();
        }
    }
}
