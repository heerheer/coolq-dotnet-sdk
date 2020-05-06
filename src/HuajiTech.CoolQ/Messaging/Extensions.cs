using HuajiTech.QQ;
using System;
using System.Collections.Generic;

namespace HuajiTech.CoolQ.Messaging
{
    /// <summary>
    /// 定义扩展方法。
    /// </summary>
    public static class Extensions
    {
        /// <summary>
        /// 创建目标为指定用户的 <see cref="Messaging.At"/> 类的新实例。
        /// </summary>
        /// <param name="target">At (@) 的目标。</param>
        /// <returns>目标为指定用户的 <see cref="Messaging.At"/> 类的新实例。</returns>
        public static At At(this IUser target) =>
            new At
            {
                Target = target ?? throw new ArgumentNullException(nameof(target))
            };

        /// <summary>
        /// 将 <see cref="Message"/> 对象解析为 <see cref="ComplexMessage"/> 对象。
        /// </summary>
        /// <param name="message">一个 <see cref="Message"/>对象，该 <see cref="Message"/> 对象的 <see cref="QQ.IMessage.Content"/> 属性的值为要解析的 <see cref="ComplexMessage"/> 对象的字符串表示形式。</param>
        /// <param name="useEmojiCQCode">如果要在返回的 <see cref="ComplexMessage"/> 对象中包含 <see cref="Emoji"/> 对象，则为 <c>true</c>；否则为 <c>false</c>。</param>
        /// <returns>与 <see cref="QQ.IMessage.Content"/> 等效的 <see cref="ComplexMessage"/> 对象。</returns>
        public static ComplexMessage Parse(this IMessage message, bool useEmojiCQCode = false) =>
            ComplexMessage.Parse(message?.Content, useEmojiCQCode);

        /// <summary>
        /// 从 <see cref="IEnumerable{T}"/> 创建 <see cref="ComplexMessage"/>。
        /// </summary>
        /// <param name="elements">要用于创建 <see cref="ComplexMessage"/> 的消息元素集合。</param>
        /// <returns>一个 <see cref="ComplexMessage"/>，其中包含输入序列中的元素。</returns>
        /// <exception cref="ArgumentNullException"><paramref name="elements"/> 为 <c>null</c>。</exception>
        public static ComplexMessage ToComplexMessage(this IEnumerable<MessageElement> elements) =>
            new ComplexMessage(elements);

        /// <summary>
        /// 向指定聊天发送 <see cref="ComplexMessage"/>。
        /// </summary>
        /// <param name="sendable">目标可发送对象。</param>
        /// <param name="message">要发送的 <see cref="ComplexMessage"/> 对象。</param>
        /// <returns>一个 <see cref="Message"/> 对象，表示已发送的消息。</returns>
        /// <exception cref="ArgumentNullException"><paramref name="sendable"/> 为 <c>null</c>。</exception>
        /// <exception cref="ArgumentNullException"><paramref name="message"/> 为 <c>null</c>。</exception>
        /// <exception cref="ArgumentException"><paramref name="message"/> 不包含任何元素，或其等效字符串表示形式为 <see cref="string.Empty"/>。</exception>
        /// <exception cref="CoolQException">酷Q返回了指示操作失败的值。</exception>
        public static IMessage Send(this ISendable sendable, ComplexMessage message)
        {
            if (sendable is null)
            {
                throw new ArgumentNullException(nameof(sendable));
            }

            if (message is null)
            {
                throw new ArgumentNullException(nameof(message));
            }

            return sendable.Send(message.ToString());
        }

        /// <summary>
        /// 向指定聊天发送 <see cref="MessageElement"/>。
        /// </summary>
        /// <param name="sendable">目标可发送对象。</param>
        /// <param name="element">要发送的 <see cref="MessageElement"/> 对象。</param>
        /// <returns>一个 <see cref="Message"/> 对象，表示已发送的消息。</returns>
        /// <exception cref="ArgumentNullException"><paramref name="sendable"/> 为 <c>null</c>。</exception>
        /// <exception cref="ArgumentNullException"><paramref name="element"/> 为 <c>null</c>。</exception>
        /// <exception cref="ArgumentException"><paramref name="element"/> 的等效字符串表示形式为 <see cref="string.Empty"/>。</exception>
        /// <exception cref="CoolQException">酷Q返回了指示发送失败的值。</exception>
        public static IMessage Send(this ISendable sendable, MessageElement element)
        {
            if (sendable is null)
            {
                throw new ArgumentNullException(nameof(sendable));
            }

            if (element is null)
            {
                throw new ArgumentNullException(nameof(element));
            }

            return sendable.Send(element.ToString());
        }
    }
}