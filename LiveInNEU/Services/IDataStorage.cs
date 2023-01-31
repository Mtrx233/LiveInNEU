using System;
using System.Collections.Generic;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;
using LiveInNEU.Models;

namespace LiveInNEU.Services.implementations
{
    /// <summary>
    /// 数据存储接口
    /// </summary>
    public interface IDataStorage
    {
        /// <summary>
        /// 是否已经初始化。
        /// </summary>
        bool Initialized();

        /// <summary>
        /// 初始化。
        /// </summary>
        Task InitializeAsync();

        /// <summary>
        /// 获取一个数据
        /// </summary>
        /// <param name="id">课程id。</param>
        Task<Data> GetDataAsync(string id);

        /// <summary>
        /// 添加一个数据
        /// </summary>
        Task AddDataAsync(Data data);

        /// <summary>
        /// 获取满足给定条件的数据集合
        /// </summary>
        /// <param name="where">Where条件。</param>
        Task<IList<Data>> GetDatasAsync(
            Expression<Func<Data, bool>> where);

        /// <summary>
        /// 删除数据
        /// </summary>
        Task DeleteDatasAsync();

        /// <summary>
        /// 设置数据更新时间
        /// </summary>
        Task<string> GetUpdateTime();

        /// <summary>
        /// 获取数据更新时间
        /// </summary>
        Task SetUpdateTime(string time);
    }
}