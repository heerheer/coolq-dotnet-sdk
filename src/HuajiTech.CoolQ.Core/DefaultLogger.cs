namespace HuajiTech.CoolQ
{
    [System.Diagnostics.CodeAnalysis.SuppressMessage(
        "Performance", "CA1806:不要忽略方法结果", Justification = "<挂起>")]
    internal class DefaultLogger : Logger
    {
        public override void Log(LogLevel level, string? type, string? message) =>
            NativeMethods.Bot_Log(Bot.Instance.AuthCode, level, type, message);

        public override void LogFatal(string? message) =>
            NativeMethods.Bot_LogFatal(Bot.Instance.AuthCode, message);
    }
}