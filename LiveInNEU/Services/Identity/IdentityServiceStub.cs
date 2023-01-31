using System;
using System.Threading.Tasks;

namespace LiveInNEU.Services.Identity
{
     /// <summary>
    /// 异常处理相关
    /// </summary>
    /// <author>赵全</author>
    public class  IdentityServiceStub : IIdentityService
    {
        public Task Authenticate()
        {
            throw new NotImplementedException();
        }

        public async Task<bool> VerifyRegistration()
        {
            await Task.Delay(1337);
            return false;
        }
    }
}
