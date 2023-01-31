namespace LiveInNEU.Utils {
    /// <author>殷昭伉</author>
    public static class ErrorMessages
    {
        public const string HTTP_CLIENT_ERROR_TITLE = "连接错误";

        public static string HttpClientErrorMessage(string server,
            string message) => string.Format($"与{server}连接时发生了错误：\n{message}");

        public const string HTTP_CLIENT_ERROR_BUTTON = "确定";

        public const string LESSON_EDIT_SUCESS = "success!";

        public const string LESSON_EDIT_INIT = "";

        public const string LESSON_EDIT_CONFLICT = "课程冲突";

        public const string ERRO = "erro!";

        public const string UPDATE_MESSAGE = "更新信息";
        
    }
}