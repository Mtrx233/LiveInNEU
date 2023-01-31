using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using LiveInNEU.Models;
using LiveInNEU.Services;
using LiveInNEU.Services.implementations;
using LiveInNEU.Services.Validation;
using LiveInNEU.ViewModels;
using Moq;

namespace TestProject1 {
    public class LessonPageViewModelHelp {
        public static Task<LessonPageViewModel> GetLessonPageViewModelAsync(
            Mock<ILessonService> lessonServiceMock,
            Mock<LessonValidator> lessonValidatorMock,
            Mock<INeuDataService> neuDataServiceMock,
            Mock<ILoginService> loginServiceMock,
            Mock<IRoutingService> routingServiceMock) {
            IList<LessonShowData> lessonShowData = new List<LessonShowData>();
            lessonShowData.Add(new LessonShowData());
            lessonServiceMock.Setup(p => p.GetLesson())
                .ReturnsAsync(lessonShowData);
            var lessonPageViewModel =
                new LessonPageViewModel(lessonServiceMock.Object,
                    lessonValidatorMock.Object,neuDataServiceMock.Object,routingServiceMock.Object);
            lessonPageViewModel.ContinueTime = Int32.MinValue;
            lessonPageViewModel.LessonID = nameof(Lesson.Id);
            lessonPageViewModel.LessonName = nameof(Lesson.Name);
            lessonPageViewModel.TeacherName = nameof(Lesson.TeacherName);
            lessonPageViewModel.Location = nameof(Lesson.Classroom);
            lessonPageViewModel.LessonDate = DateTime.Today;
            lessonPageViewModel.DetailTime =DateTime.Today.ToString();
            lessonPageViewModel.StartTime = "第1节课";
            return Task.FromResult(lessonPageViewModel);
        }
    }
}