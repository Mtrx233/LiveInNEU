using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;
using LiveInNEU.Models;
using LiveInNEU.Services;
using LiveInNEU.Services.implementations;
using Moq;
using NUnit.Framework;

namespace TestProject1.Services
{
    /// <author>殷昭伉</author>
    public class LessonServiceTest
    {
        [Test]
        public async Task TestGetLesson() {
            IList<Lesson> list = new List<Lesson>();
            list.Add(new Lesson() {
                Classroom = "一号楼B309",
                ContinueTime = 2,
                Id = "A038549",
                StartTime = 1,
                Name = "全栈开发技术实例与实现",
                TeacherName = "张引",
                Week = 12,
                Weekday = 2,
                Year = 20211
            });
            list.Add(new Lesson() {
                Classroom = "一号楼B319",
                ContinueTime = 2,
                Id = "A038549",
                StartTime = 3,
                Name = "全栈开发技术实例与实现",
                TeacherName = "张引",
                Week = 12,
                Weekday = 2,
                Year = 20211
            });
            var lessonStorageMock = new Mock<ILessonStorage>();
            var semesterStorageMock = new Mock<ISemesterStorage>();
            semesterStorageMock.Setup(p => p.GetInitSemester())
                .ReturnsAsync(20211);
            semesterStorageMock.Setup(p => p.GetInitBeginDate())
                .ReturnsAsync(new DateTime(2021,9,5));
            lessonStorageMock
                .Setup(p => p.GetLessonsAsync(
                    It.IsAny<Expression<Func<Lesson, bool>>>()))
                .ReturnsAsync(list);
            var mockLessonStorage = lessonStorageMock.Object;
            var mockSemesterStorage = semesterStorageMock.Object;
            var lessonService = new LessonService(mockLessonStorage, mockSemesterStorage);
            var nowlist = await lessonService.GetLesson();
            Assert.IsTrue(nowlist.Count == 2);
        }

        [Test]
        public async Task TestAddLesson() {
            IList<Lesson> list = new List<Lesson>();
            list.Add(new Lesson() {
                Classroom = "一号楼B309",
                ContinueTime = 2,
                Id = "A038549",
                StartTime = 1,
                Name = "全栈开发技术实例与实现",
                TeacherName = "张引",
                Week = 12,
                Weekday = 2,
                Year = 20211
            });
            list.Add(new Lesson() {
                Classroom = "一号楼B319",
                ContinueTime = 2,
                Id = "A038549",
                StartTime = 3,
                Name = "全栈开发技术实例与实现",
                TeacherName = "张引",
                Week = 12,
                Weekday = 2,
                Year = 20211
            });
            var lessonStorageMock = new Mock<ILessonStorage>();
            var semesterStorageMock = new Mock<ISemesterStorage>();
            semesterStorageMock.Setup(p => p.GetInitSemester())
                .ReturnsAsync(20211);
            semesterStorageMock.Setup(p => p.GetInitBeginDate())
                .ReturnsAsync(new DateTime(2021, 9, 5));
            lessonStorageMock
                .Setup(p => p.GetLessonsAsync(It.IsAny<Expression<Func<Lesson, bool>>>()))
                .ReturnsAsync(list);
            var mockLessonStorage = lessonStorageMock.Object;
            var mockSemesterStorage = semesterStorageMock.Object;
            var lessonService = new LessonService(mockLessonStorage, mockSemesterStorage);
            var check1 = await lessonService.AddLesson("1", "1", "1", "1", new DateTime(2021,11,23), "第5节课", 2);
            Assert.IsTrue(check1);
            var check2 = await lessonService.AddLesson("1", "1", "1", "1", new DateTime(2021, 11, 23), "第1节课", 2);
            Assert.IsTrue(!check2);
            var check3 = await lessonService.AddLesson("A038549", "1", "1", "1", new DateTime(2021, 11, 23), "第5节课", 2);
            Assert.IsTrue(!check3);
        }

        [Test]
        public async Task TestDelLesson() {
            var lessonStorageMock = new Mock<ILessonStorage>();
            var semesterStorageMock = new Mock<ISemesterStorage>();
            semesterStorageMock.Setup(p => p.GetInitSemester())
                .ReturnsAsync(20211);
            semesterStorageMock.Setup(p => p.GetInitBeginDate())
                .ReturnsAsync(new DateTime(2021, 9, 5));
            var mockLessonStorage = lessonStorageMock.Object;
            var mockSemesterStorage = semesterStorageMock.Object;
            var lessonService = new LessonService(mockLessonStorage, mockSemesterStorage);
            await lessonService.DelLesson("aa",
                new DateTime(2021, 11, 23, 8, 30, 00));
            lessonStorageMock.Verify(p=>p.DeleteLessonAsync(p => p.Id == "aa" && p.StartTime == 1 && p.Weekday == 2 && p.Week == 12),Times.Once);
        }

        [Test]
        public async Task TestUpdateLesson() {
            IList<Lesson> list = new List<Lesson>();
            list.Add(new Lesson() {
                Classroom = "一号楼B309",
                ContinueTime = 2,
                Id = "A038549",
                StartTime = 1,
                Name = "全栈开发技术实例与实现",
                TeacherName = "张引",
                Week = 12,
                Weekday = 2,
                Year = 20211
            });
            list.Add(new Lesson() {
                Classroom = "一号楼B319",
                ContinueTime = 2,
                Id = "A038548",
                StartTime = 3,
                Name = "全栈开发技术实例",
                TeacherName = "张引",
                Week = 12,
                Weekday = 2,
                Year = 20211
            });
            var lessonStorageMock = new Mock<ILessonStorage>();
            var semesterStorageMock = new Mock<ISemesterStorage>();
            semesterStorageMock.Setup(p => p.GetInitSemester())
                .ReturnsAsync(20211);
            semesterStorageMock.Setup(p => p.GetInitBeginDate())
                .ReturnsAsync(new DateTime(2021, 9, 5));
            lessonStorageMock
                .Setup(p => p.GetLessonsAsync(It.IsAny<Expression<Func<Lesson, bool>>>()))
                .ReturnsAsync(list);
            var mockLessonStorage = lessonStorageMock.Object;
            var mockSemesterStorage = semesterStorageMock.Object;
            var lessonService = new LessonService(mockLessonStorage, mockSemesterStorage);
            var check1 = await lessonService.UpdateLesson("A038549", "全栈开发技术实例与实现", "1", "1", new DateTime(2021, 11, 23), "第5节课", 2,new DateTime(2021,11,23,8,30,00));
            Assert.IsTrue(check1);
            var check2 = await lessonService.UpdateLesson("A038549", "全栈开发技术实例与实现", "1", "1", new DateTime(2021, 11, 23), "第3节课", 2, new DateTime(2021, 11, 23, 8, 30, 00));
            Assert.IsTrue(!check2);
            var check3 = await lessonService.UpdateLesson("A038549", "1", "1", "1", new DateTime(2021, 11, 23), "第5节课", 2, new DateTime(2021, 11, 23, 8, 30, 00));
            Assert.IsTrue(!check3);
        }

        //TODO 首页课程测试
        [Test]
        public async Task GetTodayLessons()
        {
            var lessonStorageMock = new Mock<ILessonStorage>();
            var semesterStorageMock = new Mock<ISemesterStorage>();
            semesterStorageMock.Setup(p => p.GetInitSemester())
                .ReturnsAsync(20211);
            semesterStorageMock.Setup(p => p.GetInitBeginDate())
                .ReturnsAsync(new DateTime(2021, 9, 5));
            var mockLessonStorage = lessonStorageMock.Object;
            var mockSemesterStorage = semesterStorageMock.Object;
            var lessonService = new LessonService(mockLessonStorage, mockSemesterStorage);
            IList<LessonShowData> todayList = await lessonService.GetTodayLessonAsync();
            Assert.IsTrue(todayList.Count == 0);
            IList<Lesson> list = new List<Lesson>();
            list.Add(new Lesson()
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
            });
            list.Add(new Lesson()
            {
                Classroom = "一号楼B319",
                ContinueTime = 2,
                Id = "A038548",
                StartTime = 3,
                Name = "全栈开发技术实例",
                TeacherName = "张引",
                Week = 12,
                Weekday = 2,
                Year = 20211
            });
            lessonStorageMock.Setup(p => p.GetLessonsAsync(It.IsAny<Expression<Func<Lesson, bool>>>()))
                .ReturnsAsync(list);
            IList<LessonShowData> todayLists = await lessonService.GetTodayLessonAsync();
            Assert.IsTrue(todayLists.Count != 0);
        }

        [Test]
        public async Task GetTomorrowLessons()
        {
            var lessonStorageMock = new Mock<ILessonStorage>();
            var semesterStorageMock = new Mock<ISemesterStorage>();
            semesterStorageMock.Setup(p => p.GetInitSemester())
                .ReturnsAsync(20211);
            semesterStorageMock.Setup(p => p.GetInitBeginDate())
                .ReturnsAsync(new DateTime(2021, 9, 5));
            var mockLessonStorage = lessonStorageMock.Object;
            var mockSemesterStorage = semesterStorageMock.Object;
            var lessonService = new LessonService(mockLessonStorage, mockSemesterStorage);
            IList<LessonShowData> todayList = await lessonService.GetTomorrowLessonAsync();
            Assert.IsTrue(todayList.Count == 0);
            IList<Lesson> list = new List<Lesson>();
            list.Add(new Lesson()
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
            });
            list.Add(new Lesson()
            {
                Classroom = "一号楼B319",
                ContinueTime = 2,
                Id = "A038548",
                StartTime = 3,
                Name = "全栈开发技术实例",
                TeacherName = "张引",
                Week = 12,
                Weekday = 2,
                Year = 20211
            });
            lessonStorageMock.Setup(p => p.GetLessonsAsync(It.IsAny<Expression<Func<Lesson, bool>>>()))
                .ReturnsAsync(list);
            IList<LessonShowData> todayLists = await lessonService.GetTomorrowLessonAsync();
            Assert.IsTrue(todayLists.Count != 0);
        }
    }
}
