using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using LiveInNEU.Models;
using LiveInNEU.Services;
using LiveInNEU.Services.implementations;
using LiveInNeuTest.Helper;
using Moq;
using NUnit.Framework;

namespace LiveInNeuTest.Services
{
    public class DailyStorageTest
    {
        [SetUp, TearDown]
        public static void RemoveDatabaseFile()
        {
            DailyStorageHelper.RemoveDatabaseFile();
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

            var dataStorage = new DailyStorage(mockInfoStorage, mockPreferenceStorage);

            Assert.IsTrue(dataStorage.Initialized());
            await dataStorage.CloseAsync();
        }



        [Test]
        public async Task TestGetQuestionAsync()
        {
            var dataStorage =
                await DailyStorageHelper.GetInitializeDataStorageAsync();
            var data = new Daily()
            {
                LessonId = "a",
                LessonName = "b"
            };
            await dataStorage.AddDailyAsync(data);
            var check = await dataStorage.GetDailyAsync(p => p.LessonId == "a");
            await dataStorage.CloseAsync();
            Assert.IsTrue(check.LessonName == "b");
        }



        [Test]
        public async Task TestGetSchedulesAsync()
        {
            var dataStorage =
                await DailyStorageHelper.GetInitializeDataStorageAsync();
            var data1 = new Daily()
            {
                LessonId = "a",
                LessonName = "b"
            };
            var data2 = new Daily()
            {
                LessonId = "a",
                LessonName = "c"
            };
            await dataStorage.AddDailyAsync(data1);
            await dataStorage.AddDailyAsync(data2);
            var check = await dataStorage.GetDailysAsync(p => p.LessonId == "a");
            await dataStorage.CloseAsync();
            Assert.IsTrue(check.Count == 2);
        }

        [Test]
        public async Task TestUpdateDailyAsync()
        {
            var dataStorage =
                await DailyStorageHelper.GetInitializeDataStorageAsync();
            var data1 = new Daily()
            {
                LessonId = "a",
                LessonName = "b",
                Character = "c"
            };
            var data2 = new Daily()
            {
                LessonId = "a",
                LessonName = "d",
                Character = "c"
            };
            await dataStorage.AddDailyAsync(data1);
            await dataStorage.UpdateDailyAsync(data2);
            var check = await dataStorage.GetDailyAsync(p => p.LessonId == "a");
            await dataStorage.CloseAsync();
            Assert.IsTrue(check.LessonName == "d");
        }

    }
}
