using System.Net.Http;
using System.Threading.Tasks;
using GalaSoft.MvvmLight;
using GalaSoft.MvvmLight.Command;
using LiveInNEU.Services;
using Xamarin.Essentials;
using Xamarin.Forms.Internals;

namespace LiveInNEU.ViewModels
{
    /// <author>钱子昂</author>
    public class LoadingPageViewModel : ViewModelBase
    {
        /******** 构造函数 ********/
        /// <summary>
        /// 登陆验证服务
        /// </summary>
        private ILoginService _loginService;

        /// <summary>
        /// 路由服务
        /// </summary>
        private IRoutingService _routingService;

        public LoadingPageViewModel(IRoutingService routingService, ILoginService loginService)
        {
            _routingService = routingService;
            _loginService = loginService;
        }

        /******** 绑定属性 ********/

        private byte[] _imageBytes;

        public byte[] ImageBytes
        {
            get => _imageBytes;
            set => Set(nameof(ImageBytes), ref _imageBytes, value);
        }

        /// <summary>
        /// 页面加载指令
        /// </summary>
        public RelayCommand _pageAppearingCommand;

        public RelayCommand PageAppearingCommand =>
            _pageAppearingCommand ?? (_pageAppearingCommand = new RelayCommand(async () =>
                await PageAppearingCommandFunction()
            ));


        /// <summary>
        /// 页面加载指令函数
        /// </summary>
        public async Task PageAppearingCommandFunction()
        {
            Preferences.Clear();
            await _loginService.BeforeLogin();
            var isLogin = await _loginService.IsLogin();
            using (var httpClient = new HttpClient())
            {
                HttpResponseMessage response =
                    await httpClient.GetAsync(
                        "https://www.neu.edu.cn/_upload/article/images/37/57/2f3d619d435192fc2e351bce3b3e/e0f3049a-768d-4dc4-b19f-c924467bcabd.png");
                response.EnsureSuccessStatusCode();
                ImageBytes =
                    await response.Content.ReadAsByteArrayAsync();
            }

            if (!isLogin)
            {
                await _routingService.NavigateToAsync("///login");
            }
            else
            {
                await _routingService.NavigateToAsync("///main");
            }
        }
        /******** 公开方法 ********/


        /******** 私有变量 ********/
    }
}