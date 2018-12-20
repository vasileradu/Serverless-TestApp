using TestApp.Core.Auth.Models;

namespace TestApp.Core.Auth.Interfaces
{
    public interface ITokenRepository
    {
        Token Generate(string username);
        Token GetToken(string tokenId);
    }
}
