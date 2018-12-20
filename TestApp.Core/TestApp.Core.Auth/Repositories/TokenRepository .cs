using System;
using System.Linq;
using TestApp.Core.Auth.Interfaces;
using TestApp.Core.Auth.Models;

namespace TestApp.Core.Auth.Repositories
{
    public class TokenRepository : ITokenRepository, IDisposable
    {
        private readonly TestAppAuthContext _dbContext;

        public TokenRepository(TestAppAuthContext context)
        {
            this._dbContext = context;
        }

        #region ITokenRepository

        public string Generate(string username)
        {
            var token = new Token { Username = username };

            this._dbContext.Token.Add(token);
            this._dbContext.SaveChanges();

            return token.Id.ToString();
        }

        public Token GetToken(string token)
        {
            return this._dbContext.Token.FirstOrDefault(t => t.Id.ToString().Equals(token));
        }

        #endregion

        public void Dispose()
        {
            this._dbContext?.Dispose();
        }


    }
}
