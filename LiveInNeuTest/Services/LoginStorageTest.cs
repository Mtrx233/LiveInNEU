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
    /// <author>赵全</author>
    public class LoginStorageTest
    {
        [Test]
        public async Task TestIsLogin()
        {
            var infoStorageMock = new Mock<IInfoStorage>();
            infoStorageMock
                .Setup(p => p.Get(LessonStorageConstants.ACCOUNT_KEY, "string"))
                .ReturnsAsync("1111");
            var mockInfoStorage = infoStorageMock.Object;

            var loginStorage = new LoginStorage(mockInfoStorage);
            var check = await loginStorage.IsLogin();
            
            Assert.IsTrue(check);
        }


        [Test]
        public async Task TestStorageOfInfo()
        {
            var infoStorageMock = new Mock<IInfoStorage>();
            var mockInfoStorage = infoStorageMock.Object;

            var loginStorage = new LoginStorage(mockInfoStorage);
            await loginStorage.StorageOfInfo(1,"1111","1111");

            infoStorageMock.Verify(
                p => p.Set(LessonStorageConstants.ACCOUNT_KEY, "1111"),
                Times.Once);
        }
    }
}
