using System.Runtime.InteropServices;
using System.Text;
using System.Threading.Tasks;
using Xamarin.Forms;

namespace LiveInNEU.Services.implementations
{
    ///<summary>
    ///ҳ·
    /// </summary>
    /// <author>钱子昂</author>
    public class RoutingService : IRoutingService
    {
        public RoutingService()
        {
        }

        public async Task GoBackAsync()
        {
            await Shell.Current.Navigation.PopAsync();
        }

        public async Task NavigateToAsync(string route)
        {
            await Shell.Current.GoToAsync(route);
        }

        // public async Task NavigateToAsync(string route, params string[] urlParams)
        // {
        //     StringBuilder url = new StringBuilder();
        //     url.Append(route);
        //     for (int i = 0; i < urlParams.Length; i++)
        //     {
        //         if (i == 0)
        //         {
        //             url.Append("?");
        //         }
        //         else
        //         {
        //             url.Append("&");
        //         }
        //
        //         url.Append(urlParams[i] + "=" + urlParams[i]);
        //     }
        //
        //     await Shell.Current.GoToAsync(url.ToString());
        // }
    }
}