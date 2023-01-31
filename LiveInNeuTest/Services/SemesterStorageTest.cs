using System;
using System.IO;
using System.Threading.Tasks;
using DevExpress.Mvvm.Native;
using DevExpress.XamarinForms.Scheduler.Internal;
using LiveInNEU.Models;
using LiveInNEU.Services;
using LiveInNEU.Services.implementations;
using Moq;
using NUnit.Framework;

namespace TestProject1.Services
{
    /// <author>殷昭伉</author>
    public class SemesterStorageTest
    {
        [SetUp, TearDown]
        public static void RemoveDatabaseFile()
        {
            SemesterStorageHelper.RemoveDatabaseFile();
        }


        [Test]
        public async Task TestInitializeAsync()
        {
            Assert.IsFalse(File.Exists(SemesterStorage.SemesterDbPath));

            var preferenceStorageMock = new Mock<IPreferenceStorage>();
            var mockPreferenceStorage = preferenceStorageMock.Object;
            var infoStorageMock = new Mock<IInfoStorage>();
            var mockInfoStorage = infoStorageMock.Object;

            var semesterStorage = new SemesterStorage(mockInfoStorage, mockPreferenceStorage);
            await semesterStorage.InitializeAsync();

            Assert.IsTrue(File.Exists(SemesterStorage.SemesterDbPath));
            preferenceStorageMock.Verify(
                p => p.Set(LessonStorageConstants.VERSION_KEY, LessonStorageConstants.VERSION),
                Times.Once);
            await semesterStorage.CloseAsync();
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

            var semesterStorage = new SemesterStorage(mockInfoStorage, mockPreferenceStorage);

            Assert.IsTrue(semesterStorage.Initialized());
            await semesterStorage.CloseAsync();
        }



        [Test]
        public async Task TestAddSemesterAsync()
        {
            var semesterStorage =
                await SemesterStorageHelper.GetInitializeLessonStorageAsync();
            var semester = new Semester() {
                Id = 20211, StartDate = "2021/9/5", WeekNum = 17
            };
            await semesterStorage.AddSemesterAsync(semester);
            await semesterStorage.CloseAsync();
            Assert.Pass();
        }



        [Test]
        public async Task TestGetSemesterAsync()
        {
            var semesterStorage =
                await SemesterStorageHelper.GetInitializeLessonStorageAsync();
            var semester = new Semester()
            {
                Id = 20211,
                StartDate = "2021/9/5",
                WeekNum = 17
            };
            await semesterStorage.AddSemesterAsync(semester);
            var check = await semesterStorage.GetSemesterAsync(20211);
            await semesterStorage.CloseAsync();
            Assert.IsTrue(check.WeekNum == 17);
        }

        [Test]
        public async Task TestGetSemestersAsync()
        {
            var semesterStorage =
                await SemesterStorageHelper.GetInitializeLessonStorageAsync();
            var semester = new Semester()
            {
                Id = 20211,
                StartDate = "2021/9/5",
                WeekNum = 17
            };
            await semesterStorage.AddSemesterAsync(semester);
            var check = await semesterStorage.GetSemestersAsync(p=>p.WeekNum == 17);
            await semesterStorage.CloseAsync();
            Assert.IsTrue(check.Count == 1);
        }

        [Test]
        public async Task TestGetInitBeginDate()
        {
            var preferenceStorageMock = new Mock<IPreferenceStorage>();
            var infoStorageMock = new Mock<IInfoStorage>();
            infoStorageMock
                .Setup(p => p.Get(LessonStorageConstants.SEMESTER_START_KEY, "time"))
                .ReturnsAsync(LessonStorageConstants.SEMESTER_START);
            var mockPreferenceStorage = preferenceStorageMock.Object;
            var mockInfoStorage = infoStorageMock.Object;

            var semesterStorage = new SemesterStorage(mockInfoStorage, mockPreferenceStorage);
            var year = await semesterStorage.GetInitBeginDate();
            await semesterStorage.CloseAsync();
            Assert.IsTrue(year.Year == 2021);
        }

        [Test]
        public async Task TestGetInitSemester()
        {
            var preferenceStorageMock = new Mock<IPreferenceStorage>();
            var infoStorageMock = new Mock<IInfoStorage>();
            infoStorageMock
                .Setup(p => p.Get(LessonStorageConstants.SEMESTER_KEY, -1))
                .ReturnsAsync(LessonStorageConstants.SEMESTER);
            var mockPreferenceStorage = preferenceStorageMock.Object;
            var mockInfoStorage = infoStorageMock.Object;

            var semesterStorage = new SemesterStorage(mockInfoStorage, mockPreferenceStorage);
            var year = await semesterStorage.GetInitSemester();
            await semesterStorage.CloseAsync();
            Assert.IsTrue(year == 2021);
        }

        [Test]
        public async Task TestHelpSelected() {
            var preferenceStorageMock = new Mock<IPreferenceStorage>();
            var infoStorageMock = new Mock<IInfoStorage>();
            infoStorageMock.Setup(p=>
                p.Set(LessonStorageConstants.SEMESTER_START_KEY, 20211));
            infoStorageMock.Setup(p =>
                p.Set(LessonStorageConstants.SEMESTER_KEY, "2021.9.5"));
            var mockPreferenceStorage = preferenceStorageMock.Object;
            var mockInfoStorage = infoStorageMock.Object;

            var semester = new Semester()
            {
                Id = 20211,
                StartDate = "2021.9.5",
                WeekNum = 17
            };
            var semesterStorage = new SemesterStorage(mockInfoStorage, mockPreferenceStorage);
            await semesterStorage.InitializeAsync();

            await semesterStorage.AddSemesterAsync(semester);
            var check = await semesterStorage.HelpSelected();
            await semesterStorage.CloseAsync();
            Assert.IsTrue(check == 20211);
        }
    }
}
