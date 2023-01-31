using System.Threading.Tasks;

namespace LiveInNEU.Services {
    /// <summary>
    /// 警告服务。
    /// </summary>
    public interface IAlertService
    {
        /// <summary>
        /// 显示警告。
        /// </summary>
        /// <param name="title">标题。</param>
        /// <param name="message">信息。</param>
        /// <param name="button">按钮文字。</param>
        Task ShowAlertAsync(string title, string message, string button);
        Task ShowsAlertAsync(string title, string message, string button);
    }
}