using System;

namespace LiveInNEU.Services {
    /// <summary>
    /// 状态改变通知接口。
    /// </summary>
    public interface INotifyStatusChanged
    {
        /// <summary>
        /// 状态。
        /// </summary>
        string Status { get; set; }

        /// <summary>
        /// 状态改变事件。
        /// </summary>
        event EventHandler StatusChanged;
    }
}