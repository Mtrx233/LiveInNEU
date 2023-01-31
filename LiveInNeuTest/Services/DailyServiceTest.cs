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

namespace LiveInNeuTest.Services
{
    public class DailyServiceTest
    {
        [Test]
        public async Task GetAllNumberAsyncTest() {
            var list = new List<Daily> {
                new Daily() {
                    Number = 12
                },
                new Daily() {
                    Number = 13
                }
            };
            var dailyStorageMock = new Mock<IDailyStorage>();
            dailyStorageMock.Setup(p =>
                    p.GetDailysAsync(
                        It.IsAny<Expression<Func<Daily, bool>>>()))
                .ReturnsAsync(list);
            var dailyService =
                new DailyService(dailyStorageMock.Object);
            var check = await dailyService.GetAllNumberAsync();
            Assert.IsTrue(check == 25);
        }

        [Test]
        public async Task GetRightNumberTest()
        {
            var list = new List<Daily> {
                new Daily() {
                    RightNumber  = 12
                },
                new Daily() {
                    RightNumber = 13
                }
            };
            var dailyStorageMock = new Mock<IDailyStorage>();
            dailyStorageMock.Setup(p =>
                    p.GetDailysAsync(
                        It.IsAny<Expression<Func<Daily, bool>>>()))
                .ReturnsAsync(list);
            var dailyService =
                new DailyService(dailyStorageMock.Object);
            var check = await dailyService.GetRightNumberAsync();
            Assert.IsTrue(check == 25);
        }

        [Test]
        public async Task GetRightAsyncTest()
        {
            var list = new List<Daily> {
                new Daily() {
                    Number = 12,
                    RightNumber  = 10
                },
                new Daily() {
                    Number = 13,
                    RightNumber = 10
                }
            };
            var dailyStorageMock = new Mock<IDailyStorage>();
            dailyStorageMock.Setup(p =>
                    p.GetDailysAsync(
                        It.IsAny<Expression<Func<Daily, bool>>>()))
                .ReturnsAsync(list);
            var dailyService =
                new DailyService(dailyStorageMock.Object);
            var check = await dailyService.GetRightAsync();
            Assert.IsTrue(check == 0.80);
        }

        [Test]
        public async Task GetFirstDailysAsyncTest()
        {
            var list = new List<Daily> {
                new Daily() {
                    Number = 12,
                    RightNumber  = 10
                },
                new Daily() {
                    Number = 13,
                    RightNumber = 10
                }
            };
            var dailyStorageMock = new Mock<IDailyStorage>();
            dailyStorageMock.Setup(p =>
                    p.GetDailysAsync(
                        It.IsAny<Expression<Func<Daily, bool>>>()))
                .ReturnsAsync(list);
            var dailyService =
                new DailyService(dailyStorageMock.Object);
            var check = await dailyService.GetFirstDailysAsync();
            Assert.IsTrue(check.Count == 2);
        }

        [Test]
        public async Task GetSecondDailysAsyncTest()
        {
            var list = new List<Daily> {
                new Daily() {
                    Number = 12,
                    RightNumber  = 10
                },
                new Daily() {
                    Number = 13,
                    RightNumber = 10
                }
            };
            var dailyStorageMock = new Mock<IDailyStorage>();
            dailyStorageMock.Setup(p =>
                    p.GetDailysAsync(
                        It.IsAny<Expression<Func<Daily, bool>>>()))
                .ReturnsAsync(list);
            var dailyService =
                new DailyService(dailyStorageMock.Object);
            var check = await dailyService.GetSecondDailysAsync("a");
            Assert.IsTrue(check.Count == 2);
        }
    }
}
