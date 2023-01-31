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
    public class QuestionServiceTest
    {
        [Test]
        public async Task GetQuestinsAsyncTest()
        {
            var question = new Question() { LessonId = "a" };
            var scheduleStorageMock = new Mock<IScheduleStorage>();
            var questionStorageMock = new Mock<IQuestionStorage>();
            var storeStorage = new Mock<IStoreStorage>();
            var remoteFavoriteStorage = new Mock<RemoteFavoriteStorage>();
            questionStorageMock.Setup(p =>
                    p.GetQuestionAsync(
                        It.IsAny<Expression<Func<Question, bool>>>()))
                .ReturnsAsync(question);
            var questionService =
                new QuestionService(scheduleStorageMock.Object, questionStorageMock.Object,storeStorage.Object, remoteFavoriteStorage.Object);
            var check1 = await questionService.GetQuestinsAsync("a", "b", 1);
            var check2 = await questionService.GetQuestinsAsync("a", "b", 2);
            Assert.IsTrue(check1[0].Subject == "null");
            Assert.IsTrue(check2.Count == 3);
        }

        [Test]
        public async Task FinishQuestionAsyncTest1()
        {
            var question = new Question() { IsTested = 1 };
            var schedule = new Schedule()
            {
                Finished = 100,
                Now = 100
            };
            var scheduleStorageMock = new Mock<IScheduleStorage>();
            var questionStorageMock = new Mock<IQuestionStorage>();
            var storeStorage = new Mock<IStoreStorage>();
            questionStorageMock.Setup(p =>
                    p.GetQuestionAsync(
                        It.IsAny<Expression<Func<Question, bool>>>()))
                .ReturnsAsync(question);
            scheduleStorageMock.Setup(p =>
                    p.GetScheduleAsync(
                        It.IsAny<Expression<Func<Schedule, bool>>>()))
                .ReturnsAsync(schedule);
            var remoteFavoriteStorage = new Mock<RemoteFavoriteStorage>();
            var questionService =
                new QuestionService(scheduleStorageMock.Object, questionStorageMock.Object, storeStorage.Object, remoteFavoriteStorage.Object);

            await questionService.FinishQuestionAsync("a", "b", 101);
            Assert.Pass();
        }

        [Test]
        public async Task FinishQuestionAsyncTest2()
        {
            var question = new Question() { IsTested = 0 };
            var schedule = new Schedule()
            {
                Finished = 100,
                Now = 100
            };
            var scheduleStorageMock = new Mock<IScheduleStorage>();
            var questionStorageMock = new Mock<IQuestionStorage>();
            var storeStorage = new Mock<IStoreStorage>();
            questionStorageMock.Setup(p =>
                    p.GetQuestionAsync(
                        It.IsAny<Expression<Func<Question, bool>>>()))
                .ReturnsAsync(question);
            scheduleStorageMock.Setup(p =>
                    p.GetScheduleAsync(
                        It.IsAny<Expression<Func<Schedule, bool>>>()))
                .ReturnsAsync(schedule);
            var remoteFavoriteStorage = new Mock<RemoteFavoriteStorage>();
            var questionService =
                new QuestionService(scheduleStorageMock.Object, questionStorageMock.Object, storeStorage.Object, remoteFavoriteStorage.Object);

            await questionService.FinishQuestionAsync("a", "b", 99);
            Assert.Pass();
        }

        [Test]
        public async Task SetStoreUpsAsyncTest1()
        {
            var question = new Question() { StoreUp = 0 };
            var scheduleStorageMock = new Mock<IScheduleStorage>();
            var questionStorageMock = new Mock<IQuestionStorage>();
            var storeStorage = new Mock<IStoreStorage>();
            questionStorageMock.Setup(p =>
                    p.GetQuestionAsync(
                        It.IsAny<Expression<Func<Question, bool>>>()))
                .ReturnsAsync(question);
            var schedule = new Schedule()
            {
                Finished = 100,
                Now = 100
            };
            scheduleStorageMock.Setup(p =>
                    p.GetScheduleAsync(
                        It.IsAny<Expression<Func<Schedule, bool>>>()))
                .ReturnsAsync(schedule);
            var remoteFavoriteStorage = new Mock<RemoteFavoriteStorage>();
            var questionService =
                new QuestionService(scheduleStorageMock.Object, questionStorageMock.Object, storeStorage.Object, remoteFavoriteStorage.Object);

            await questionService.SetStoreUpAsync("a", "b", 100);
            Assert.Pass();
        }

        [Test]
        public async Task SetStoreUpsAsyncTest2()
        {
            var question = new Question() { StoreUp = 1 };
            var scheduleStorageMock = new Mock<IScheduleStorage>();
            var questionStorageMock = new Mock<IQuestionStorage>();
            var storeStorage = new Mock<IStoreStorage>();
            questionStorageMock.Setup(p =>
                    p.GetQuestionAsync(
                        It.IsAny<Expression<Func<Question, bool>>>()))
                .ReturnsAsync(question);
            var schedule = new Schedule()
            {
                Finished = 100,
                Now = 100
            };
            scheduleStorageMock.Setup(p =>
                    p.GetScheduleAsync(
                        It.IsAny<Expression<Func<Schedule, bool>>>()))
                .ReturnsAsync(schedule);
            var remoteFavoriteStorage = new Mock<RemoteFavoriteStorage>();
            var questionService =
                new QuestionService(scheduleStorageMock.Object, questionStorageMock.Object, storeStorage.Object, remoteFavoriteStorage.Object);

            await questionService.SetStoreUpAsync("a", "b", 100);
            Assert.Pass();
        }

        [Test]
        public async Task GetStoreUpAsyncTest1()
        {
            var question = new List<Question>() {
                new Question() {
                    LessonId = "a"
                },
                new Question() {
                    LessonId = "b"
                }
            };
            var scheduleStorageMock = new Mock<IScheduleStorage>();
            var questionStorageMock = new Mock<IQuestionStorage>();
            var storeStorage = new Mock<IStoreStorage>();
            questionStorageMock.Setup(p =>
                    p.GetQuestionsAsync(
                        It.IsAny<Expression<Func<Question, bool>>>()))
                .ReturnsAsync(question);
            var remoteFavoriteStorage = new Mock<RemoteFavoriteStorage>();
            var questionService =
                new QuestionService(scheduleStorageMock.Object, questionStorageMock.Object, storeStorage.Object, remoteFavoriteStorage.Object);
            var check1 = await questionService.GetStoreUpsAsync("a", "b", 1);
            var check2 = await questionService.GetStoreUpsAsync("a", "b", 2);
            //Assert.IsTrue(check1[0].Subject == "null");
            Assert.IsTrue(check2.Count == 2);
        }

        [Test]
        public async Task GetStoreUpAsyncTest2()
        {
            var question = new List<Question>() {
                new Question() {
                    LessonId = "a"
                },
                new Question() {
                    LessonId = "b"
                },
                new Question() {
                    LessonId = "c"
                }
            };
            var scheduleStorageMock = new Mock<IScheduleStorage>();
            var questionStorageMock = new Mock<IQuestionStorage>();
            var storeStorage = new Mock<IStoreStorage>();
            questionStorageMock.Setup(p =>
                    p.GetQuestionsAsync(
                        It.IsAny<Expression<Func<Question, bool>>>()))
                .ReturnsAsync(question);
            var remoteFavoriteStorage = new Mock<RemoteFavoriteStorage>();
            var questionService =
                new QuestionService(scheduleStorageMock.Object, questionStorageMock.Object, storeStorage.Object, remoteFavoriteStorage.Object);
            var check1 = await questionService.GetStoreUpsAsync("a", "b",1);
            var check2 = await questionService.GetStoreUpsAsync("a", "b", 2);
            //Assert.IsTrue(check1[0].Subject == "null");
            Assert.IsTrue(check2.Count == 3);
        }
    }
}
