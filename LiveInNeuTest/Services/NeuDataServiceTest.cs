using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq.Expressions;
using System.Threading.Tasks;
using LiveInNEU.Models;
using LiveInNEU.Services;
using LiveInNEU.Services.implementations;
using Moq;
using NUnit.Framework;

namespace TestProject1.Services {
    
     /// <summary>
    /// 课程数据获取service测试
    /// </summary>
    /// <author>赵全</author>
    public class NeuDataServiceTest {
        [SetUp]
        public void Setup() { }

        /// <summary>
        /// 测试登录
        /// </summary>
        [Test]
        public async Task IsRightTest() {
            var loginStorage = new Mock<ILoginStorage>();
            var neuDataService =
                await NeuDataServiceHelper.GetLessonPageViewModelAsync(loginStorage);
            Assert.True(
                await neuDataService.IsRight(1, "1111", "2222"));
            loginStorage.Verify(p => p.StorageOfInfo(1, "1111", "2222"), Times.Once);
            Assert.False(
                await neuDataService.IsRight(1, "1111", "2222"));
            loginStorage.Verify(p => p.StorageOfInfo(1, "1111", "2222"), Times.Once);
        }

        /// <summary>
        /// 测试获取课程数据
        /// </summary>
        [Test]
        public async Task GetLessonDataAsyncTest() {
            var neuDataService =
                await NeuDataServiceHelper.GetLessonPageViewModelAsync();
            Assert.True(
                await neuDataService.IsRight(1, "1111", "2222"));
            Assert.IsNotEmpty(await neuDataService.GetLessonDataAsync());
        }

        /// <summary>
        /// 测试获取开始上课时间
        /// </summary>
        [Test]
        public async Task GetBeginningAsyncTest() {
            var neuDataService =
                await NeuDataServiceHelper.GetLessonPageViewModelAsync();
            Assert.True(
                await neuDataService.IsRight(1, "1111", "2222"));
            Assert.True(await neuDataService.GetBeginning(20211)=="2021.09.05");
        }


        /// <summary>
        /// 测试数据存储
        /// </summary>
        [Test]
        public async Task DataStorageAsyncTest() {
            var alertService = new Mock<IAlertService>();
            var lessonStorage = new Mock<ILessonStorage>();
            var loginStorage = new Mock<ILoginStorage>();
            var semesterStorage = new Mock<ISemesterStorage>();
            var dataStorage = new Mock<IDataStorage>();
            var neuDataService =
                new NeuDataService(lessonStorage.Object, loginStorage.Object,
                    semesterStorage.Object, alertService.Object, dataStorage.Object);
            alertService.Setup(p => p.ShowAlertAsync(It.IsAny<string>(),
                It.IsAny<string>(), It.IsAny<string>()));
            Assert.True(
                await neuDataService.IsRight(1, "1111", "2222"));
            await neuDataService.DataStorageAsync();
            lessonStorage.Verify(p => p.AddLessonAsync(It.IsAny<Lesson>()),
                Times.AtLeastOnce);
            semesterStorage.Verify(
                p => p.AddSemesterAsync(It.IsAny<Semester>()),Times.AtLeastOnce);
            semesterStorage.Verify(
                p => p.HelpSelected(), Times.Once);
        }

        /// <summary>
        /// 测试数据存储
        /// </summary>
        [Test]
        public async Task UpdateTest() {
            IList<Lesson> list = new List<Lesson>() {
                new Lesson() {
                    Classroom = "一号楼B309",
                    ContinueTime = 2,
                    Id = "A0801051101",
                    StartTime = 1,
                    Name = "全栈开发技术实例与实现",
                    TeacherName = "1",
                    Week = 12,
                    Weekday = 2,
                    Year = 20211,
                    Kind = "0"
                },
                new Lesson(){
                    Classroom = "一号楼B309",
                    ContinueTime = 2,
                    Id = "A0801051101",
                    StartTime = 1,
                    Name = "全栈开发技术实例与实现",
                    TeacherName = "1",
                    Week = 12,
                    Weekday = 2,
                    Year = 20211,
                    Kind = "8a80f28ade75f7878a0182542a881cef7ed96c2cf7b1c2f0cd55772b7a5c5a02"
                },
                new Lesson() {
                    Classroom = "一号楼B309",
                    ContinueTime = 2,
                    Id = "A0801051101",
                    StartTime = 1,
                    Name = "全栈开发技术实例与实现",
                    TeacherName = "1",
                    Week = 11,
                    Weekday = 2,
                    Year = 20211,
                    Kind = "6704adf3600a225ea894bc94fc4e872b51875deaff454157b6e31906d6083073"
                 }
            };
            IList<Data> lists = new List<Data>() {
                new Data() {
                    HashCode = "a",
                    ID = "c",
                    UpdateTime = "1"
                }
            };
            var alertService = new Mock<IAlertService>();
            var lessonStorage = new Mock<ILessonStorage>();
            var loginStorage = new Mock<ILoginStorage>();
            var semesterStorage = new Mock<ISemesterStorage>();
            var dataStorage = new Mock<IDataStorage>();
            var neuDataService =
                new NeuDataService(lessonStorage.Object, loginStorage.Object,
                    semesterStorage.Object, alertService.Object, dataStorage.Object);
            alertService.Setup(p => p.ShowAlertAsync(It.IsAny<string>(),
                It.IsAny<string>(), It.IsAny<string>()));
            lessonStorage
                .Setup(p =>
                    p.GetLessonsAsync(
                        It.IsAny<Expression<Func<Lesson, bool>>>()))
                .ReturnsAsync(list);
            dataStorage.Setup(p => p.GetUpdateTime()).ReturnsAsync("2021/11/1 22:02:29");
            lessonStorage.Setup(p => p.GetAccount()).ReturnsAsync("1111");
            lessonStorage.Setup(p => p.GetPassword()).ReturnsAsync("2222");
            await neuDataService.Update();
        }

    }

}