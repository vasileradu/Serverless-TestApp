using System;
using System.Threading.Tasks;
using TestApp.Core.Auth.Interfaces;
using TestApp.Core.Auth.Models;

namespace TestApp.Core.Auth.Repositories
{
    public class MockRepository : IUserRepository, ITokenRepository
    {
        public Token Generate(string username)
        {
            return new Token
            {
                Id = Guid.NewGuid(),
                Username = username,
                CreatedAtUtc = DateTime.UtcNow
            };
        }

        public Token GetToken(string tokenId)
        {
            return this.Generate(tokenId);
        }

        public void Remove(string tokenId)
        {
            // do nothing.
        }

        public Task<bool> ValidateCredentials(string username, string password)
        {
            return Task.FromResult(true);
        }
    }
}
