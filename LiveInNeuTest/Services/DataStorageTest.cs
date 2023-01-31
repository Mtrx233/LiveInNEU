using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using LiveInNEU.Models;
using LiveInNEU.Services;
using LiveInNEU.Services.implementations;
using Moq;
using NUnit.Framework;
using TestProject1.Helper;

namespace TestProject1.Services
{
    /// <author>钱子昂</author>
    public class DataStorageTest
    {
        [SetUp, TearDown]
        public static void RemoveDatabaseFile()
        {
            DataStorageHelper.RemoveDatabaseFile();
        }


        


        [Test]
        public async Task TestInitialized()
        {
            var preferenceStorageMock = new Mock<IPreferenceStorage>();
            preferenceStorageMock
                .Setup(p => p.Get(LessonStorageConstants.VERSION_KEY, -1))
                .Returns(LessonStorageConstants.VERSION);
            var mockPreferenceStorage = preferenceStorageMock.Object;
            var infoStorageMock = new Mock<IInfoStorage>();
            var mockInfoStorage = infoStorageMock.Object;

            var dataStorage = new DataStorage(mockInfoStorage, mockPreferenceStorage);

            Assert.IsTrue(dataStorage.Initialized());
            await dataStorage.CloseAsync();
        }



        [Test]
        public async Task TestAddDataAsync()
        {
            var dataStorage =
                await DataStorageHelper.GetInitializeDataStorageAsync();
            var data = new Data() {HashCode = "a", ID = "a", UpdateTime = "a"};
            await dataStorage.AddDataAsync(data);
            await dataStorage.CloseAsync();
            Assert.Pass();
        }



        [Test]
        public async Task TestGetLessonAsync()
        {
            var dataStorage =
                await DataStorageHelper.GetInitializeDataStorageAsync();
            var data = new Data() { HashCode = "a", ID = "a", UpdateTime = "a" };
            await dataStorage.AddDataAsync(data);
            var datas = await dataStorage.GetDataAsync("a");
            await dataStorage.CloseAsync();
            Assert.IsTrue(datas.ID == "a");
        }

        [Test]
        public async Task TestGetLessonsAsync()
        {
            var dataStorage =
                await DataStorageHelper.GetInitializeDataStorageAsync();
            var data = new Data() { HashCode = "a", ID = "a", UpdateTime = "a" };
            await dataStorage.AddDataAsync(data);
            var datas = await dataStorage.GetDatasAsync(p=>p.ID=="a");
            await dataStorage.CloseAsync();
            Assert.IsTrue(datas.Count == 1);
        }

        [Test]
        public async Task TestDeleteLessonAsync()
        {
            var dataStorage =
                await DataStorageHelper.GetInitializeDataStorageAsync();
            var data = new Data() { HashCode = "a", ID = "a", UpdateTime = "a" };
            await dataStorage.AddDataAsync(data);
            await dataStorage.DeleteDatasAsync();
            var datas = await dataStorage.GetDatasAsync(p => p.ID == "a");
            await dataStorage.CloseAsync();
            Assert.IsTrue(datas.Count == 0);
        }

        [Test]
        public async Task TestGetUpdateTime()
        {
            var preferenceStorageMock = new Mock<IPreferenceStorage>();
            var mockPreferenceStorage = preferenceStorageMock.Object;
            var infoStorageMock = new Mock<IInfoStorage>();
            var mockInfoStorage = infoStorageMock.Object;
            infoStorageMock
                .Setup(p =>
                    p.Get(LessonStorageConstants.UPDATEDATE_KEY, "time"))
                .ReturnsAsync("a");
            var dataStorage = new DataStorage(mockInfoStorage, mockPreferenceStorage);
            var str = await dataStorage.GetUpdateTime();
            Assert.IsTrue(str == "a");
        }

        [Test]
        public async Task TestSetUpdateTime()
        {
            var preferenceStorageMock = new Mock<IPreferenceStorage>();
            var mockPreferenceStorage = preferenceStorageMock.Object;
            var infoStorageMock = new Mock<IInfoStorage>();
            var mockInfoStorage = infoStorageMock.Object;
            var dataStorage = new DataStorage(mockInfoStorage, mockPreferenceStorage);
            await dataStorage.SetUpdateTime("a");
            infoStorageMock.Verify(
                p => p.Set(LessonStorageConstants.UPDATEDATE_KEY, "a"),
                Times.Once);
        }

        [Test]
        public async Task TestInitializeAsync()
        {
            Assert.IsFalse(File.Exists(LessonStorage.LessonDbPath));

            var preferenceStorageMock = new Mock<IPreferenceStorage>();
            var mockPreferenceStorage = preferenceStorageMock.Object;
            var infoStorageMock = new Mock<IInfoStorage>();
            var mockInfoStorage = infoStorageMock.Object;

            var dataStorage = new DataStorage(mockInfoStorage, mockPreferenceStorage);
            await dataStorage.InitializeAsync();

            Assert.IsTrue(File.Exists(LessonStorage.LessonDbPath));
            preferenceStorageMock.Verify(
                p => p.Set(LessonStorageConstants.VERSION_KEY, LessonStorageConstants.VERSION),
                Times.Once);
            await dataStorage.CloseAsync();
        }
    }
}
