using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using LiveInNEU.Models;

namespace LiveInNEU.Services
{
    /// <summary>
    /// 备忘录数据处理接口
    /// </summary>
    public interface INoteService
    {
        /// <summary>
        /// 获取备忘录
        /// </summary>
        Task<IList<Note>> GetNotesAsync();

        /// <summary>
        /// 获取备忘录
        /// </summary>
        Task<IList<Note>> GetNotesAsync(string lesson_id);

        /// <summary>
        /// 获取备忘录
        /// </summary>
        Task<IList<Note>> GetDeleteNotesAsync();

        /// <summary>
        /// 获取备忘录
        /// </summary>
        Task<IList<Note>> GetFinishNotesAsync(string lesson_id);

        /// <summary>
        /// 获取备忘录
        /// </summary>
        Task<IList<Note>> GetFinishNotesAsync();

        /// <summary>
        /// 获取备忘录
        /// </summary>
        Task<IList<Note>> GetAllNotesAsync();

        /// <summary>
        /// 获取备忘录
        /// </summary>
        Task<IList<Note>> GetAllNotesAsync(string lesson_id);

        /// <summary>
        /// 添加备忘录
        /// </summary>
        Task AddNoteAsync(Note note);

        /// <summary>
        /// 添加备忘录
        /// </summary>
        Task AddNoteAsync(Note note,string question);

        /// <summary>
        /// 完成备忘录
        /// </summary>
        Task FinishNoteAsync(int id);

        /// <summary>
        /// 删除备忘录
        /// </summary>
        Task DeleteNoteAsync(int id);

        /// <summary>
        /// 恢复备忘录
        /// </summary>
        Task CoverNoteAsync(int id);

        /// <summary>
        /// 得到今日备忘录
        /// </summary>
        Task<IList<Note>> GetTodatNoteAsync();
    }
}
