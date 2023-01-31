using System;
using System.Collections.Generic;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;
using LiveInNEU.Models;

namespace LiveInNEU.Services
{
    /// <summary>
    /// 备忘录存储接口
    /// </summary>
    public interface INoteStorage
    {
        /// <summary>
        /// 是否已经初始化
        /// </summary>
        bool Initialized();

        /// <summary>
        /// 初始化
        /// </summary>
        Task InitializeAsync();

        /// <summary>
        /// 获取一个备忘录
        /// </summary>
        /// <param name="where">Where条件。</param>
        Task<Note> GetNoteAsync(Expression<Func<Note, bool>> where);

        /// <summary>
        /// 获取满足给定条件的备忘录集合
        /// </summary>
        /// <param name="where">Where条件。</param>
        Task<IList<Note>> GetNotesAsync(Expression<Func<Note, bool>> where);

        /// <summary>
        /// 更新备忘录
        /// </summary>
        Task UpdateNoteAsync(Note note);

        /// <summary>
        /// 添加备忘录
        /// </summary>
        Task AddNoteAsync(Note note);
    }
}
