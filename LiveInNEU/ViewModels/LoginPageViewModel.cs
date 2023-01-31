using System;
using System.IO;
using System.Net.Http;
using System.Threading.Tasks;
using GalaSoft.MvvmLight;
using GalaSoft.MvvmLight.Command;
using LiveInNEU.Services;
using LiveInNEU.Services.implementations;
using LiveInNEU.Views;
using Xamarin.Forms;

namespace LiveInNEU.ViewModels
{
    /// <author>赵全</author>
    public class LoginPageViewModel : ViewModelBase
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

        /// <summary>
        /// 消息弹窗服务
        /// </summary>
        private readonly IAlertService _alertService;

        public LoginPageViewModel(ILoginService loginService,
            IRoutingService routingService,
            IAlertService alertService)
        {
            _loginService = loginService;
            _routingService = routingService;
            _alertService = alertService;
        }

        /******** 绑定属性 ********/

        /// <summary>
        /// 用户名
        /// </summary>
        public string UserName
        {
            get => _userName;
            set => Set(nameof(UserName), ref _userName, value);
        }

        /// <summary>
        /// 密码
        /// </summary>
        public string Password
        {
            get => _password;
            set => Set(nameof(Password), ref _password, value);
        }



        /// <summary>
        /// 登陆指令
        /// </summary>
        public RelayCommand _loginCommand;


        public RelayCommand LoginCommand =>
            _loginCommand ?? (_loginCommand = new RelayCommand(async () =>
                await LoginCommandFunction()));

        /// <summary>
        /// 登陆指令函数
        /// </summary>
        public async Task LoginCommandFunction()
        {
            if (UserName == null)
            {
                await _alertService.ShowAlertAsync("登陆失败", "账号不能为空", "确定");
            }
            else if (Password == null)
            {
                await _alertService.ShowAlertAsync("登陆失败", "密码不能为空", "确定");
            }
            else
            {

                Success = await _loginService.Login(1, UserName, Password);

                if (Success)
                {
                    await _routingService.NavigateToAsync("///main");
                }
            }
        }


        /******** 公开方法 ********/


        /******** 私有变量 ********/

        /// <summary>
        /// 登陆是否成功
        /// </summary>
        private bool Success = false;

        /// <summary>
        /// 用户名
        /// </summary>
        private string _userName;

        /// <summary>
        /// 密码
        /// </summary>
        private string _password;
    }
}