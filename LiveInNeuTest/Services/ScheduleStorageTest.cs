using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using LiveInNEU.Models;
using LiveInNEU.Services;
using LiveInNEU.Services.implementations;
using LiveInNeuTest.Helper;
using Moq;
using NUnit.Framework;
using TestProject1.Helper;

namespace LiveInNeuTest.Services
{
    public class ScheduleStorageTest
    {
        [SetUp, TearDown]
        public static void RemoveDatabaseFile()
        {
            ScheduleStorageHelper.RemoveDatabaseFile();
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

            var dataStorage = new ScheduleStorage(mockInfoStorage, mockPreferenceStorage);

            Assert.IsTrue(dataStorage.Initialized());
            await dataStorage.CloseAsync();
        }



        [Test]
        public async Task TestGetScheduleAsync()
        {
            var dataStorage =
                await ScheduleStorageHelper.GetInitializeDataStorageAsync();
            var data = new Schedule() {
                LessonId = "a",
                LessonName = "b"
            };
            await dataStorage.AddScheduleAsync(data);
            var check = await dataStorage.GetScheduleAsync(p => p.LessonId == "a");
            await dataStorage.CloseAsync();
            Assert.IsTrue(check.LessonName == "b");
        }



        [Test]
        public async Task TestGetSchedulesAsync()
        {
            var dataStorage =
                await ScheduleStorageHelper.GetInitializeDataStorageAsync();
            var data1 = new Schedule()
            {
                LessonId = "a",
                LessonName = "b"
            };
            var data2 = new Schedule()
            {
                LessonId = "a",
                LessonName = "c"
            };
            await dataStorage.AddScheduleAsync(data1);
            await dataStorage.AddScheduleAsync(data2);
            var check = await dataStorage.GetSchedulesAsync(p => p.LessonId == "a");
            await dataStorage.CloseAsync();
            Assert.IsTrue(check.Count == 2);
        }

        [Test]
        public async Task TestUpdateScheduleAsync()
        {
            var dataStorage =
                await ScheduleStorageHelper.GetInitializeDataStorageAsync();
            var data1 = new Schedule()
            {
                LessonId = "a",
                LessonName = "b",
                Character = "c"
            };
            var data2 = new Schedule()
            {
                LessonId = "a",
                LessonName = "d",
                Character = "c"
            };
            await dataStorage.AddScheduleAsync(data1);
            await dataStorage.UpdateScheduleAsync(data2);
            var check = await dataStorage.GetScheduleAsync(p => p.LessonId == "a");
            await dataStorage.CloseAsync();
            Assert.IsTrue(check.LessonName == "d");
        }

    }
}
