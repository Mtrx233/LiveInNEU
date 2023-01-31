
using System;
using System.Threading.Tasks;
using LiveInNEU.Services;
using LiveInNEU.Services.Validation;
using LiveInNEU.Utils;
using LiveInNEU.ViewModels;
using Moq;
using NUnit.Framework;

namespace TestProject1.ViewModels {
    /// <author>钱子昂</author>
    public class LessonPageViewModelTest {
        [SetUp]
        public void Setup() { }


        [Test]
        public async Task ValidateInputTest() {
            var lessonServiceMock = new Mock<ILessonService>();
            var lessonValidatorMock = new Mock<LessonValidator>();
            var neuDataServiceMock = new Mock<INeuDataService>();
            var loginServiceMock = new Mock<ILoginService>();
            var routingServiceMock = new Mock<IRoutingService>();
            var lessonPageViewModel =
                await LessonPageViewModelHelp.GetLessonPageViewModelAsync(lessonServiceMock, lessonValidatorMock, neuDataServiceMock, loginServiceMock, routingServiceMock);
            var validationResult = lessonPageViewModel.ValidateInput();
        }


        [Test]
        public async Task LessonAddTest() {
            var lessonServiceMock = new Mock<ILessonService>();
            var lessonValidatorMock = new Mock<LessonValidator>();
            var neuDataServiceMock = new Mock<INeuDataService>();
            var loginServiceMock = new Mock<ILoginService>();
            var routingServiceMock = new Mock<IRoutingService>();
            var lessonPageViewModel =
                await LessonPageViewModelHelp.GetLessonPageViewModelAsync(lessonServiceMock, lessonValidatorMock, neuDataServiceMock, loginServiceMock, routingServiceMock);
            lessonServiceMock.Setup(p => p.AddLesson(It.IsAny<string>(),
                    It.IsAny<string>(), It.IsAny<string>(), It.IsAny<string>(),
                    It.IsAny<DateTime>(), It.IsAny<string>(), It.IsAny<int>()))
                .ReturnsAsync(true);
            await lessonPageViewModel.LessonAdd();
            Assert.True(lessonPageViewModel.ValidateMsg == ErrorMessages.LESSON_EDIT_INIT);
            Assert.NotNull(lessonPageViewModel.LessonAddCommand);
        }

        [Test]
        public async Task LessonDelTest() {
            var lessonServiceMock = new Mock<ILessonService>();
            var lessonValidatorMock = new Mock<LessonValidator>();
            var neuDataServiceMock = new Mock<INeuDataService>();
            var loginServiceMock = new Mock<ILoginService>();
            var routingServiceMock = new Mock<IRoutingService>();
            var lessonPageViewModel =
                await LessonPageViewModelHelp.GetLessonPageViewModelAsync(lessonServiceMock, lessonValidatorMock, neuDataServiceMock, loginServiceMock, routingServiceMock);
            lessonServiceMock.Setup(p =>
                p.DelLesson(It.IsAny<string>(), It.IsAny<DateTime>()));
            await lessonPageViewModel.LessonDel();
            Assert.True(lessonPageViewModel.ValidateMsg == ErrorMessages.LESSON_EDIT_INIT);
            Assert.NotNull(lessonPageViewModel.LessonDelCommand);
        }

        [Test]
        public async Task LessonEditTest() {
            var lessonServiceMock = new Mock<ILessonService>();
            var lessonValidatorMock = new Mock<LessonValidator>();
            var neuDataServiceMock = new Mock<INeuDataService>();
            var loginServiceMock = new Mock<ILoginService>();
            var routingServiceMock = new Mock<IRoutingService>();
            var lessonPageViewModel =
                await LessonPageViewModelHelp.GetLessonPageViewModelAsync(lessonServiceMock, lessonValidatorMock, neuDataServiceMock, loginServiceMock, routingServiceMock);
            lessonServiceMock.Setup(p => p.UpdateLesson(It.IsAny<string>(),
                    It.IsAny<string>(), It.IsAny<string>(), It.IsAny<string>(),
                    It.IsAny<DateTime>(), It.IsAny<string>(), It.IsAny<int>(), It.IsAny<DateTime>()))
                .ReturnsAsync(true);
            await lessonPageViewModel.LessonEdit();
            Assert.NotNull(lessonPageViewModel.LessonEditCommand);
        }

        [Test]
        public async Task CreateLabelsTest() {
            var lessonServiceMock = new Mock<ILessonService>();
            var lessonValidatorMock = new Mock<LessonValidator>();
            var neuDataServiceMock = new Mock<INeuDataService>();
            var loginServiceMock = new Mock<ILoginService>();
            var routingServiceMock = new Mock<IRoutingService>();
            var lessonPageViewModel =
                await LessonPageViewModelHelp.GetLessonPageViewModelAsync(lessonServiceMock, lessonValidatorMock, neuDataServiceMock, loginServiceMock, routingServiceMock);
            lessonPageViewModel.CreateLabels();
            Assert.IsNotEmpty(lessonPageViewModel.Labels);
        }

        [Test]
        public async Task LessonsCollectionReLoadTest()
        {
            var lessonServiceMock = new Mock<ILessonService>();
            var lessonValidatorMock = new Mock<LessonValidator>();
            var neuDataServiceMock = new Mock<INeuDataService>();
            var loginServiceMock = new Mock<ILoginService>();
            var routingServiceMock = new Mock<IRoutingService>();
            var lessonPageViewModel =
                await LessonPageViewModelHelp.GetLessonPageViewModelAsync(lessonServiceMock, lessonValidatorMock, neuDataServiceMock, loginServiceMock, routingServiceMock);
            await lessonPageViewModel.LessonsCollectionLoad();
            Assert.IsInstanceOf<DateTime>(LessonPageViewModel.BaseTime);
            Assert.IsNotEmpty(lessonPageViewModel.LessonsCollection);
            Assert.NotNull(lessonPageViewModel.PageAppearingCommand);
        }
    }
}