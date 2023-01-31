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
    public class QuestionStorageTest
    {
        [SetUp, TearDown]
        public static void RemoveDatabaseFile()
        {
            QuestionStorageHelper.RemoveDatabaseFile();
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

            var dataStorage = new QuestionStorage(mockInfoStorage, mockPreferenceStorage);

            Assert.IsTrue(dataStorage.Initialized());
            await dataStorage.CloseAsync();
        }



        [Test]
        public async Task TestGetQuestionAsync()
        {
            var dataStorage =
                await QuestionStorageHelper.GetInitializeDataStorageAsync();
            var data = new Question()
            {
                Id = 1000,
                LessonId = "a",
                LessonName = "b"
            };
            await dataStorage.AddQuestionAsync(data);
            var check = await dataStorage.GetQuestionAsync(p => p.LessonId == "a");
            await dataStorage.CloseAsync();
            Assert.IsTrue(check.LessonName == "b");
        }



        [Test]
        public async Task TestGetSchedulesAsync()
        {
            var dataStorage =
                await QuestionStorageHelper.GetInitializeDataStorageAsync();
            var data1 = new Question()
            {
                Id = 1000,
                LessonId = "a",
                LessonName = "b"
            };
            var data2 = new Question()
            {
                Id = 1001,
                LessonId = "a",
                LessonName = "c"
            };
            await dataStorage.AddQuestionAsync(data1);
            await dataStorage.AddQuestionAsync(data2);
            var check = await dataStorage.GetQuestionsAsync(p => p.LessonId == "a");
            await dataStorage.CloseAsync();
            Assert.IsTrue(check.Count == 2);
        }

        [Test]
        public async Task TestUpdateQuestionAsync()
        {
            var dataStorage =
                await QuestionStorageHelper.GetInitializeDataStorageAsync();
            var data1 = new Question()
            {
                Id = 1000,
                LessonId = "a",
                LessonName = "b",
                Character = "c"
            };
            var data2 = new Question()
            {
                Id = 1001,
                LessonId = "a",
                LessonName = "d",
                Character = "c"
            };
            await dataStorage.AddQuestionAsync(data1);
            await dataStorage.UpdateQuestionAsync(data2);
            var check = await dataStorage.GetQuestionAsync(p => p.LessonId == "a");
            await dataStorage.CloseAsync();
            Assert.IsTrue(check.LessonName == "d");
        }

    }
}
