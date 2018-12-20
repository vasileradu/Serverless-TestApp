using TestApp.Core.Auth.Models;

namespace TestApp.Core.Auth.Interfaces
{
    public interface ITokenRepository
    {
        string Generate(string username);
        Token GetToken(string token);
    }
}
