using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using LiveInNEU.Models;

namespace LiveInNEU.Services.implementations
{
    public class NoteService:INoteService
    {
        public NoteService(INoteStorage iNoteStorage,IQuestionStorage iQuestionStorage) {
            this._noteStorage = iNoteStorage;
            this._questionStorage = iQuestionStorage;
        }

        private INoteStorage _noteStorage;

        private IQuestionStorage _questionStorage;

        public async Task<IList<Note>> GetNotesAsync() {
            //await _noteStorage.InitializeAsync();
            var list = await _noteStorage.GetNotesAsync(p => p.IsDelete == 0 && p.IsFinish == 0);
            var lists = new List<Note>();
            foreach (var note in list) {
                lists.Add(note);
            }
            lists.Sort((x,y)=> MyComparer(x,y));
            return lists;
        }

        public int MyComparer(Note x, Note y)
        {
            if (DateTime.Parse(x.EndTime) > DateTime.Parse(y.EndTime))
            {
                return 1;
            }
            else
            {
                return -1;
            }
        }

        public async Task<IList<Note>> GetNotesAsync(string lesson_id) {
            // await _noteStorage.InitializeAsync();
            return await _noteStorage.GetNotesAsync(p => p.IsDelete == 0 && p.IsFinish == 0 && p.SubjectId == lesson_id);
        }

        public async Task<IList<Note>> GetDeleteNotesAsync() {
            return await _noteStorage.GetNotesAsync(p => p.IsDelete == 1);
        }

        public async Task<IList<Note>> GetFinishNotesAsync(string lesson_id) {
            return await _noteStorage.GetNotesAsync(p => p.IsFinish == 1 && p.SubjectId == lesson_id);
        }

        public async Task<IList<Note>> GetFinishNotesAsync()
        {
            return await _noteStorage.GetNotesAsync(p => p.IsFinish == 1 && p.IsDelete == 0);
        }

        public async Task<IList<Note>> GetAllNotesAsync() {
            return await _noteStorage.GetNotesAsync(p=>p.Id != -1);
        }

        public async Task<IList<Note>> GetAllNotesAsync(string lesson_id) {
            return await _noteStorage.GetNotesAsync(p => p.Id != -1 && p.SubjectId == lesson_id);
        }

        public async Task AddNoteAsync(Note note) {
            note.SetupTime = DateTime.Now.ToString();
            note.IsFinish = 0;
            note.IsDelete = 0;
            await _noteStorage.AddNoteAsync(note);
        }

        public async Task AddNoteAsync(Note note,string question)
        {
            /*
            note.SetupTime = DateTime.Now.ToString();
            note.IsFinish = 0;
            note.IsDelete = 0;
            var list = question.Split(';');
            if (list.Length == 2 && list[0].Split('-').Length == 2) {
                note.Questions = list[0];
                note.Questions += ';';
            } else {
                foreach (var str in list) {
                    if (str == "") {
                        continue;
                    }
                    var lists = str.Split('-');
                    var lessonname = lists[0];
                    var character = lists[1];
                    var number = int.Parse(lists[2]);
                    var questions = await _questionStorage.GetQuestionAsync(p =>
                        p.LessonName == lessonname && p.Character == character &&
                        p.Number == number);
                    note.Questions += questions.Id.ToString();
                    note.Questions += ';';
                }
            }
            await _noteStorage.AddNoteAsync(note);
            */
            note.SetupTime = DateTime.Now.ToString();
            note.IsFinish = 0;
            note.IsDelete = 0;
            note.Questions = question;
            await _noteStorage.AddNoteAsync(note);
        }

        public async Task FinishNoteAsync(int id) {
            var note = await _noteStorage.GetNoteAsync(p => p.Id == id);
            //TODO 异常处理
            note.IsFinish = 1;
            await _noteStorage.UpdateNoteAsync(note);
        }

        public async Task DeleteNoteAsync(int id) {
            var note = await _noteStorage.GetNoteAsync(p => p.Id == id);
            //TODO 异常处理
            note.IsDelete = 1;
            await _noteStorage.UpdateNoteAsync(note);
        }

        public async Task CoverNoteAsync(int id) {
            var note = await _noteStorage.GetNoteAsync(p => p.Id == id);
            //TODO 异常处理
            note.IsDelete = 0;
            await _noteStorage.UpdateNoteAsync(note);
        }

        public async Task<IList<Note>> GetTodatNoteAsync() {
            IList<Note> result = new List<Note>();
            var list = await GetNotesAsync();
            var time = DateTime.Now;
            foreach (var note in list) {
                var times = DateTime.Parse(note.EndTime);
                if (times.Day == time.Day && times.Month == time.Month &&
                    times.Year == time.Year) {
                    result.Add(note);
                }
            }
            return result;
        }
    }
}
