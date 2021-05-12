using Microsoft.IdentityModel.Tokens;
using System.Text;
 
namespace tablinumAPI
{
    public class AuthOptions
    {
        public const string ISSUER = "Tablinum"; // издатель токена
        public const string AUDIENCE = "TablinumClient"; // потребитель токена
        const string KEY = "Admin_12345!admin";   // ключ для шифрации
        public const int LIFETIME = 3; // время жизни токена - 1 минута
        public static SymmetricSecurityKey GetSymmetricSecurityKey()
        {
            return new SymmetricSecurityKey(Encoding.ASCII.GetBytes(KEY));
        }
    }
}