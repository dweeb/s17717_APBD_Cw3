using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Cw3.DAL
{
    public interface IDbAuthService
    {
        public String AuthenticateAndGetRole(string login, string password);
        public String GetSalt(string login);
    }
}
