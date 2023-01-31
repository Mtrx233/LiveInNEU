using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using LiveInNEU.Services;
using LiveInNEU.Services.implementations;
using LiveInNEU.Services.Validation;
using LiveInNEU.ViewModels;
using Moq;
using NUnit.Framework;

namespace LiveInNeuTest.ViewModels
{
    public class ViewModelTest
    {
        [Test]
        public async Task TestLessonViewModel() {
            var lessonView = new LessonPageViewModel(
                new LessonService(
                    new LessonStorage(new InfoStorage(new PreferenceStorage()),
                        new PreferenceStorage()),
                    new SemesterStorage(
                        new InfoStorage(new PreferenceStorage()),
                        new PreferenceStorage())), new LessonValidator(),
                new NeuDataService(
                    new LessonStorage(new InfoStorage(new PreferenceStorage()),
                        new PreferenceStorage()),
                    new LoginStorage(new InfoStorage(new PreferenceStorage())),
                    new SemesterStorage(
                        new InfoStorage(new PreferenceStorage()),
                        new PreferenceStorage()), new AlertService(),
                    new DataStorage(new InfoStorage(new PreferenceStorage()),
                        new PreferenceStorage())),new RoutingService());
            // lessonView.LessonAdd();
            //await lessonView.LessonDel();
            //await lessonView.LessonEdit();
            await lessonView.LessonsCollectionLoad();
        }
    }
}
