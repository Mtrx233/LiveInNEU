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
using NuGet.Frameworks;
using NUnit.Framework;

namespace LiveInNeuTest.Services
{
    public class ScheduleServiceTest
    {
        [Test]
        public async Task GetScheduleTest() {
            var list = new List<Schedule> {
                new Schedule() {
                    LessonId = "a"
                },
                new Schedule() {
                    LessonId = "b"
                }
            };
            var scheduleStorageMock = new Mock<IScheduleStorage>();
            scheduleStorageMock.Setup(p =>
                    p.GetSchedulesAsync(
                        It.IsAny<Expression<Func<Schedule, bool>>>()))
                .ReturnsAsync(list);
            var scheduleService =
                new ScheduleService(scheduleStorageMock.Object);
            var check = await scheduleService.GetSchedule();
            Assert.IsTrue(check.Count == 2);
        }

        [Test]
        public async Task GetFirstScheduleTest()
        {
            var list = new List<Schedule> {
                new Schedule() {
                    LessonId = "a"
                },
                new Schedule() {
                    LessonId = "b"
                }
            };
            var scheduleStorageMock = new Mock<IScheduleStorage>();
            scheduleStorageMock.Setup(p =>
                    p.GetSchedulesAsync(
                        It.IsAny<Expression<Func<Schedule, bool>>>()))
                .ReturnsAsync(list);
            var scheduleService =
                new ScheduleService(scheduleStorageMock.Object);
            var check = await scheduleService.GetFirstSchedule();
            Assert.IsTrue(check.Count == 2);
        }

        [Test]
        public async Task GetSecondScheduleTest()
        {
            var list = new List<Schedule> {
                new Schedule() {
                    LessonId = "a"
                },
                new Schedule() {
                    LessonId = "b"
                }
            };
            var scheduleStorageMock = new Mock<IScheduleStorage>();
            scheduleStorageMock.Setup(p =>
                    p.GetSchedulesAsync(
                        It.IsAny<Expression<Func<Schedule, bool>>>()))
                .ReturnsAsync(list);
            var scheduleService =
                new ScheduleService(scheduleStorageMock.Object);
            var check = await scheduleService.GetSecondSchedule("a");
            Assert.IsTrue(check.Count == 2);
        }
    }
}
