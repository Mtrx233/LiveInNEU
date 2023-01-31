using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace LiveInNEU.Services.Identity
{
    public interface IIdentityService
    {
        Task<bool> VerifyRegistration();
        Task Authenticate();
    }
}
