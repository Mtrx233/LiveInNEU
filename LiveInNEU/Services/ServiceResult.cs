namespace LiveInNEU.Services {
    /// <summary>
    /// 服务结果
    /// </summary>
    public class ServiceResult
    {
        /// <summary>
        /// 服务结果状态。
        /// </summary>
        public ServiceResultStatus Status { get; set; }

        /// <summary>
        /// 信息。
        /// </summary>
        public string Message { get; set; }
    }

    /// <summary>
    /// 服务结果。
    /// </summary>
    /// <typeparam name="T">结果类型。</typeparam>
    public class ServiceResult<T> : ServiceResult
    {
        /// <summary>
        /// 服务结果。
        /// </summary>
        public T Result { get; set; }
    }

    /// <summary>
    /// 服务结果状态。
    /// </summary>
    public enum ServiceResultStatus
    {
        Ok,
        Exception
    }
}