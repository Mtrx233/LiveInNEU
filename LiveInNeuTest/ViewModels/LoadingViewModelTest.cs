using System.Collections.ObjectModel;
using System.Threading.Tasks;
using LiveInNEU.Services;
using LiveInNEU.Services.implementations;
using LiveInNEU.ViewModels;
using LiveInNEU.Views;
using Moq;
using NUnit.Framework;
using Xamarin.Forms;

namespace TestProject1.ViewModels
{
    /// <author>赵全</author>
    public class LoadingViewModelTest
    {
        [Test]
        public async Task TestPageAppearingCommandFunctionNotInitialized()
        {
            var routingServiceMock = new Mock<IRoutingService>();
            var isLoginToReturn = false;
            var loginServiceMock = new Mock<ILoginService>();
            loginServiceMock.Setup(p => p.IsLogin()).ReturnsAsync(isLoginToReturn);
            var mockLoginService = loginServiceMock.Object;
            var mockRoutingService = routingServiceMock.Object;
            var loadingPageViewModel = new LoadingPageViewModel(mockRoutingService,mockLoginService);
            await loadingPageViewModel.PageAppearingCommandFunction();
            routingServiceMock.Verify(p=>p.NavigateToAsync("///login"),Times.Once);
        }
        
        [Test]
        public async Task TestPageAppearingCommandFunctionInitialized()
        {
            var routingServiceMock = new Mock<IRoutingService>();
            var isLoginToReturn = true;
            var loginServiceMock = new Mock<ILoginService>();
            loginServiceMock.Setup(p => p.IsLogin()).ReturnsAsync(isLoginToReturn);
            var mockLoginService = loginServiceMock.Object;
            var mockRoutingService = routingServiceMock.Object;
            var loadingPageViewModel = new LoadingPageViewModel(mockRoutingService,mockLoginService);
            await loadingPageViewModel.PageAppearingCommandFunction();
            Assert.IsNotNull(loadingPageViewModel.PageAppearingCommand);
            routingServiceMock.Verify(p=>p.NavigateToAsync("///main"),Times.Once);
        }
        
    }
}