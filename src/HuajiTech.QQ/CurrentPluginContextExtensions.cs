﻿using System;

namespace HuajiTech.QQ
{
    /// <summary>
    /// 定义用于操作 <see cref="PluginContext.CurrentContext"/> 的扩展方法。
    /// </summary>
    public static class CurrentPluginContextExtensions
    {
        public static IUser AsUser(this IUser user, PluginContext context) => context?.GetUser(user);

        public static IUser AsUser(this IUser user) => user?.AsUser(PluginContext.CurrentContext);

        public static IMember AsMemberOf(this IUser user, IGroup group, PluginContext context)
            => context?.GetMember(user, group);

        public static IMember AsMemberOf(this IUser user, IGroup group)
            => AsMemberOf(user, group, PluginContext.CurrentContext);

        public static IMember AsMemberOf(this IUser user, long groupNumber, PluginContext context)
            => context?.GetMember(user, groupNumber);

        public static IMember AsMemberOf(this IUser user, long groupNumber)
            => AsMemberOf(user, groupNumber, PluginContext.CurrentContext);

        public static TException LogAsWarning<TException>(this TException exception, ILogger logger)
            where TException : Exception
        {
            logger?.LogWarning(exception);
            return exception;
        }

        public static TException LogAsWarning<TException>(this TException exception, PluginContext context)
            where TException : Exception
            => LogAsWarning(exception, context?.Bot?.Logger);

        public static TException LogAsWarning<TException>(this TException exception)
            where TException : Exception
            => LogAsWarning(exception, PluginContext.CurrentContext);

        public static TException LogAsError<TException>(this TException exception, ILogger logger)
             where TException : Exception
        {
            logger?.LogError(exception);
            return exception;
        }

        public static TException LogAsError<TException>(this TException exception, PluginContext context)
            where TException : Exception
            => LogAsError(exception, context?.Bot?.Logger);

        public static TException LogAsError<TException>(this TException exception)
            where TException : Exception
            => LogAsError(exception, PluginContext.CurrentContext);
    }
}