using System.IO;
using System.Threading.Tasks;
using LiveInNEU.Models;
using LiveInNEU.Services;
using LiveInNEU.Services.implementations;
using Moq;
using NUnit.Framework;
using TestProject1.Helper;

namespace TestProject1.Services
{
    /// <author>赵全</author>
    public class InfoStorageTest
    {
        [SetUp, TearDown]
        public static void RemoveDatabaseFile()
        {
            InfoStorageHelper.RemoveDatabaseFile();
        }


        [Test]
        public async Task TestInitializeAsync()
        {
            Assert.IsFalse(File.Exists(LessonStorage.LessonDbPath));

            var preferenceStorageMock = new Mock<IPreferenceStorage>();
            var mockPreferenceStorage = preferenceStorageMock.Object;

            var infoStorage = new InfoStorage(mockPreferenceStorage);
            await infoStorage.InitializeAsync();

            Assert.IsTrue(File.Exists(LessonStorage.LessonDbPath));
            preferenceStorageMock.Verify(
                p => p.Set(LessonStorageConstants.VERSION_KEY, LessonStorageConstants.VERSION),
                Times.Once);
            await infoStorage.CloseAsync();
        }


        [Test]
        public async Task TestInitialized()
        {
            var preferenceStorageMock = new Mock<IPreferenceStorage>();
            preferenceStorageMock
                .Setup(p => p.Get(LessonStorageConstants.VERSION_KEY, -1))
                .Returns(LessonStorageConstants.VERSION);
            var mockPreferenceStorage = preferenceStorageMock.Object;
            

            var infoStorage = new InfoStorage(mockPreferenceStorage);

            Assert.IsTrue(infoStorage.Initialized());
            await infoStorage.CloseAsync();
        }



        [Test]
        public async Task TestGetInt()
        {
            var infoStorage =
                await InfoStorageHelper.GetInitializeInfoStorageAsync();

            await infoStorage.Set("test1", 1);
            var result = await infoStorage.Get("test1", -1);
            await infoStorage.CloseAsync();
            Assert.IsTrue(result == 1);
        }



        [Test]
        public async Task TestSetInt()
        {
            var infoStorage =
                await InfoStorageHelper.GetInitializeInfoStorageAsync();

            await infoStorage.Set("test1", 1);
            await infoStorage.Set("test1", 2);

            await infoStorage.CloseAsync();
            Assert.Pass();
        }

        [Test]
        public async Task TestGetString()
        {
            var infoStorage =
                await InfoStorageHelper.GetInitializeInfoStorageAsync();

            await infoStorage.Set("test1", "1");
            var result = await infoStorage.Get("test1", "-1");
            await infoStorage.CloseAsync();
            Assert.IsTrue(result == "1");
        }

        [Test]
        public async Task TestSetString()
        {
            var infoStorage =
                await InfoStorageHelper.GetInitializeInfoStorageAsync();

            await infoStorage.Set("test1", "1");
            await infoStorage.Set("test1", "2");

            await infoStorage.CloseAsync();
            Assert.Pass();
        }
    }
}
