using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace TestApp.Core.Auth.Interfaces
{
    public interface IUserRepository
    {
        Task<bool> ValidateCredentials(string username, string password);
    }
}
