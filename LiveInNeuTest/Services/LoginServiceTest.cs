using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using LiveInNEU.Services;
using LiveInNEU.Services.implementations;
using Moq;
using NUnit.Framework;

namespace TestProject1.Services
{
    /// <author>钱子昂</author>
    public class LoginServiceTest
    {
        [Test]
        public async Task TestBeforeLogin() {
            var lessonStorageMock = new Mock<ILessonStorage>();
            var loginStorageMock = new Mock<ILoginStorage>();
            var neuDataServiceMock = new Mock<INeuDataService>();
            lessonStorageMock.Setup(p => p.Initialized()).Returns(false);
            var mockLessonStorage = lessonStorageMock.Object;
            var mockLoginStorage = loginStorageMock.Object;
            var mockNeuDataService = neuDataServiceMock.Object;
            var loginService = new LoginService(mockLessonStorage,mockLoginStorage,mockNeuDataService);
            await loginService.BeforeLogin();
            lessonStorageMock.Verify(
                p => p.InitializeAsync(),
                Times.Once);
        }


        [Test]
        public async Task TestLogin() {
            var lessonStorageMock = new Mock<ILessonStorage>();
            var loginStorageMock = new Mock<ILoginStorage>();
            var neuDataServiceMock = new Mock<INeuDataService>();
            neuDataServiceMock.Setup(p => p.IsRight(1,"1111","1111")).ReturnsAsync(true);
            var mockLessonStorage = lessonStorageMock.Object;
            var mockLoginStorage = loginStorageMock.Object;
            var mockNeuDataService = neuDataServiceMock.Object;
            var loginService = new LoginService(mockLessonStorage, mockLoginStorage, mockNeuDataService);
            var check = await loginService.Login(1, "1111", "1111");
            Assert.IsTrue(check);
        }

        [Test]
        public async Task TestIsLogin()
        {
            var lessonStorageMock = new Mock<ILessonStorage>();
            var loginStorageMock = new Mock<ILoginStorage>();
            var neuDataServiceMock = new Mock<INeuDataService>();
            loginStorageMock.Setup(p => p.IsLogin()).ReturnsAsync(true);
            var mockLessonStorage = lessonStorageMock.Object;
            var mockLoginStorage = loginStorageMock.Object;
            var mockNeuDataService = neuDataServiceMock.Object;
            var loginService = new LoginService(mockLessonStorage, mockLoginStorage, mockNeuDataService);
            var check = await loginService.IsLogin();
            Assert.IsTrue(check);
        }

        [Test]
        public async Task TestGetDataAsync() {
            var lessonStorageMock = new Mock<ILessonStorage>();
            var loginStorageMock = new Mock<ILoginStorage>();
            var neuDataServiceMock = new Mock<INeuDataService>();
            var mockLessonStorage = lessonStorageMock.Object;
            var mockLoginStorage = loginStorageMock.Object;
            var mockNeuDataService = neuDataServiceMock.Object;
            var loginService = new LoginService(mockLessonStorage, mockLoginStorage, mockNeuDataService);
            await loginService.GetDataAsync();
            neuDataServiceMock.Verify(
                p => p.DataStorageAsync(),
                Times.Once);
        }

        
    }
}
