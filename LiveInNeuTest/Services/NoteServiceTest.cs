using System;
using System.Collections.Generic;
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
    public class NoteServiceTest
    {
        [Test]
        public async Task GetAllNumberAsyncTest() {
            var noteStorage = new NoteStorage(
                new InfoStorage(new PreferenceStorage()),
                new PreferenceStorage());
            await noteStorage.InitializeAsync();
            await noteStorage.AddNoteAsync(new Note() {Id = 1000});
            await noteStorage.GetNoteAsync(p => p.Id == 1000);
            await noteStorage.GetNotesAsync(p => p.Title != "");
            await noteStorage.UpdateNoteAsync(new Note() {
                Id = 1000, Title = "asa"
            });
            await noteStorage.CloseAsync();
        }

        [Test]
        public async Task GetAllNumberAsyncTest1()
        {
            var noteStorage = new NoteStorage(
                new InfoStorage(new PreferenceStorage()),
                new PreferenceStorage());
            var noteService = new NoteService(noteStorage,
                new QuestionStorage(new InfoStorage(new PreferenceStorage()),
                    new PreferenceStorage()));
            await noteService.AddNoteAsync(new Note() {Id = 1000});
            await noteService.DeleteNoteAsync(1000);
            await noteService.FinishNoteAsync(1);
            await noteService.CoverNoteAsync(1000);
            await noteService.GetAllNotesAsync();
            await noteService.GetAllNotesAsync("ss");
            await noteService.GetDeleteNotesAsync();
            await noteService.GetNotesAsync();
            await noteService.GetNotesAsync("ss");
            await noteService.DeleteNoteAsync(1000);
            await noteService.GetTodatNoteAsync();
        }

        [Test]
        public async Task GetAllNumberAsyncTest2()
        {
            var storeStorage = new StoreStorage(
                new InfoStorage(new PreferenceStorage()),
                new PreferenceStorage());
            await storeStorage.InitializeAsync();
            await storeStorage.AddStoreAsync(new Store() {QuestionId = 1});
            await storeStorage.GetStoreAsync(p => p.QuestionId == 1);
            var list = await storeStorage.GetStoresAsync(p => p.QuestionId == 1);
            await storeStorage.GetUserName();
            await storeStorage.UpStoresAsync(list);
            await storeStorage.UpdateStoreAsync(new Store() {QuestionId = 1});
            await storeStorage.CloseAsync();
        }

        [Test]
        public async Task GetAllNumberAsyncTest3()
        {
            var menuStorage = new MenuStorage(
                new InfoStorage(new PreferenceStorage()),
                new PreferenceStorage());
            await menuStorage.InitializeAsync();
            await menuStorage.GetMenusAsync();
            await menuStorage.CloseAsync();
        }

        [Test]
        public async Task GetAllNumberAsyncTest4() {
            var questionService = new QuestionService(
                new ScheduleStorage(new InfoStorage(new PreferenceStorage()),
                    new PreferenceStorage()),
                new QuestionStorage(new InfoStorage(new PreferenceStorage()),
                    new PreferenceStorage()),new StoreStorage(new InfoStorage(new PreferenceStorage()),
                    new PreferenceStorage()),new RemoteFavoriteStorage(new AlertService(),new InfoStorage(new PreferenceStorage())));
            await questionService.GetQuestinsAsync("ss", "sss");
            await questionService.GetQuestinsAsync("ss", "sss", 2);
            await questionService.Synchronization();
        }
    }
}
