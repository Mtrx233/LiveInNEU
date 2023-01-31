using System.Threading.Tasks;
using GalaSoft.MvvmLight.Command;
using LiveInNEU.Services;
using LiveInNEU.ViewModels;
using Moq;
using NUnit.Framework;

namespace TestProject1.ViewModels
{
    /// <author>殷昭伉</author>
    public class LoginPageViewModelTest
    {
        [Test]
        public async Task TestLoginCommandFunctionSuccess()
        {
            var routingServiceMock = new Mock<IRoutingService>();
            var loginServiceMock = new Mock<ILoginService>();
            var alertServiceMock = new Mock<IAlertService>();
            var isSuccess = true;
            loginServiceMock.Setup(p => p.Login(1,"1","2")).ReturnsAsync(isSuccess);
            var mockLoginService = loginServiceMock.Object;
            var mockRoutingService = routingServiceMock.Object;
            var mockAlertService = alertServiceMock.Object;
            var loginPageViewModel = new LoginPageViewModel(mockLoginService,mockRoutingService,mockAlertService);
            loginPageViewModel.UserName = "1";
            loginPageViewModel.Password = "2";
            await loginPageViewModel.LoginCommandFunction();
            routingServiceMock.Verify(p=>p.NavigateToAsync("///main"),Times.Once);
        }
        [Test]
        public async Task TestLoginCommandFunctionDefeat()
        {
            var routingServiceMock = new Mock<IRoutingService>();
            var loginServiceMock = new Mock<ILoginService>();
            var alertServiceMock = new Mock<IAlertService>();
            var mockLoginService = loginServiceMock.Object;
            var mockRoutingService = routingServiceMock.Object;
            var mockAlertService = alertServiceMock.Object;
            var loginPageViewModel = new LoginPageViewModel(mockLoginService,mockRoutingService,mockAlertService);
            var isSuccess = false;
            loginServiceMock.Setup(p => p.Login(1,"1","2")).ReturnsAsync(isSuccess);
            await loginPageViewModel.LoginCommandFunction();
            routingServiceMock.Verify(p=>p.NavigateToAsync("///main"),Times.Never);
        }
        [Test]
        public async Task TestLoginCommandFunctionNull()
        {
            var routingServiceMock = new Mock<IRoutingService>();
            var loginServiceMock = new Mock<ILoginService>();
            var alertServiceMock = new Mock<IAlertService>();
            var mockLoginService = loginServiceMock.Object;
            var mockRoutingService = routingServiceMock.Object;
            var mockAlertService = alertServiceMock.Object;
            var loginPageViewModel = new LoginPageViewModel(mockLoginService,mockRoutingService,mockAlertService);
            await loginPageViewModel.LoginCommandFunction();
            alertServiceMock.Verify(p=>p.ShowAlertAsync("登陆失败", "账号不能为空", "确定"),Times.Once);
            loginPageViewModel.UserName = "1";
            loginPageViewModel.Password = null;
            await loginPageViewModel.LoginCommandFunction();
            alertServiceMock.Verify(p=>p.ShowAlertAsync("登陆失败", "密码不能为空", "确定"),Times.Once);
        }

        [Test]
        public void TestLoginCommand()
        {
            var routingServiceMock = new Mock<IRoutingService>();
            var loginServiceMock = new Mock<ILoginService>();
            var alertServiceMock = new Mock<IAlertService>();
            var mockLoginService = loginServiceMock.Object;
            var mockRoutingService = routingServiceMock.Object;
            var mockAlertService = alertServiceMock.Object;
            var loginPageViewModel = new LoginPageViewModel(mockLoginService,mockRoutingService,mockAlertService);
            loginPageViewModel._loginCommand = null;
            Assert.IsNotNull(loginPageViewModel.LoginCommand);
        }
    }
}