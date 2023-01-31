using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using LiveInNEU.Models;
using LiveInNEU.Services;
using LiveInNEU.Services.implementations;
using NUnit.Framework;
using Moq;

namespace TestProject1.Services {
    /// <author>钱子昂</author>
    public class LessonStorageTest {
        
        [SetUp, TearDown]
        public static void RemoveDatabaseFile() {
            LessonStorageHelper.RemoveDatabaseFile();
        }
        

        [Test]
        public async Task TestInitializeAsync() {
            Assert.IsFalse(File.Exists(LessonStorage.LessonDbPath));

            var preferenceStorageMock = new Mock<IPreferenceStorage>();
            var mockPreferenceStorage = preferenceStorageMock.Object;
            var infoStorageMock = new Mock<IInfoStorage>();
            var mockInfoStorage = infoStorageMock.Object;

            var lessonStorage = new LessonStorage(mockInfoStorage, mockPreferenceStorage);
            await lessonStorage.InitializeAsync();

            Assert.IsTrue(File.Exists(LessonStorage.LessonDbPath));
            preferenceStorageMock.Verify(
                p => p.Set(LessonStorageConstants.VERSION_KEY, LessonStorageConstants.VERSION),
                Times.Once);
            await lessonStorage.CloseAsync();
        }

        
        [Test]
        public async Task TestInitialized() {
            var preferenceStorageMock = new Mock<IPreferenceStorage>();
            preferenceStorageMock
                .Setup(p => p.Get(LessonStorageConstants.VERSION_KEY, -1))
                .Returns(LessonStorageConstants.VERSION);
            var mockPreferenceStorage = preferenceStorageMock.Object;
            var infoStorageMock = new Mock<IInfoStorage>();
            var mockInfoStorage = infoStorageMock.Object;

            var lessonStorage = new LessonStorage(mockInfoStorage, mockPreferenceStorage);

            Assert.IsTrue(lessonStorage.Initialized());
            await lessonStorage.CloseAsync();
        }

        
/*
        [Test]
        public async Task TestAddLessonAsync() {
            var lessonStorage =
                await LessonStorageHelper.GetInitializeLessonStorageAsync();
            var lesson = new Lesson() {
                Classroom = "一号楼B309",
                ContinueTime = 2,
                Id = "A038549",
                StartTime = 1,
                Name = "全栈开发技术实例与实现",
                TeacherName = "张引",
                Week = 12,
                Weekday = 2,
                Year = 20211
            };
            await lessonStorage.AddLessonAsync(lesson);
            await lessonStorage.CloseAsync();
            Assert.Pass();
        }
*/
        

        [Test]
        public async Task TestGetLessonAsync()
        {
            var lessonStorage =
                await LessonStorageHelper.GetInitializeLessonStorageAsync();
            var lessons = new Lesson()
            {
                Classroom = "一号楼B309",
                ContinueTime = 2,
                Id = "A038549",
                StartTime = 1,
                Name = "全栈开发技术实例与实现",
                TeacherName = "张引",
                Week = 12,
                Weekday = 2,
                Year = 20211
            };
            await lessonStorage.AddLessonAsync(lessons);
            var lesson = await lessonStorage.GetLessonAsync("A038549");
            await lessonStorage.CloseAsync();
            Assert.IsTrue(lesson.Week == 12);
        }

        /*
        [Test]
        public async Task TestGetLessonsAsync()
        {
            var lessonStorage =
                await LessonStorageHelper.GetInitializeLessonStorageAsync();
            var lesson = new Lesson()
            {
                Classroom = "一号楼B309",
                ContinueTime = 2,
                Id = "A038549",
                StartTime = 1,
                Name = "全栈开发技术实例与实现",
                TeacherName = "张引",
                Week = 12,
                Weekday = 2,
                Year = 20211
            };
            await lessonStorage.AddLessonAsync(lesson);
            var lessons = await lessonStorage.GetLessonsAsync(p => p.Year == 20211);
            await lessonStorage.CloseAsync();
            Assert.IsTrue(lessons.Count == 1);
        }
        */

        [Test]
        public async Task TestDeleteLessonAsync()
        {
            var lessonStorage =
                await LessonStorageHelper.GetInitializeLessonStorageAsync();
            var lesson = new Lesson()
            {
                Classroom = "一号楼B309",
                ContinueTime = 2,
                Id = "A038549",
                StartTime = 1,
                Name = "全栈开发技术实例与实现",
                TeacherName = "张引",
                Week = 12,
                Weekday = 2,
                Year = 20211
            };
            await lessonStorage.AddLessonAsync(lesson);
            await lessonStorage.DeleteLessonAsync(p=>p.Id == "A038549");
            var lessons = await lessonStorage.GetLessonsAsync(p => p.Year == 20211);
            await lessonStorage.CloseAsync();
            Assert.IsTrue(lessons.Count == 0);
        }
    }
        
}
