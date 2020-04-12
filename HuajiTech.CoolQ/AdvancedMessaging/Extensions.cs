using System;

namespace HuajiTech.CoolQ.AdvancedMessaging
{
    /// <summary>
    /// 定义扩展方法。
    /// </summary>
    public static class Extensions
    {
        /// <summary>
        /// 创建目标为指定用户的 <see cref="AdvancedMessaging.At"/> 类的新实例。
        /// </summary>
        /// <param name="target">At (@) 的目标。</param>
        /// <returns>目标为指定用户的 <see cref="AdvancedMessaging.At"/> 类的新实例。</returns>
        public static At At(this User target)
        {
            return new At
            {
                Target = target ?? throw new ArgumentNullException(nameof(target))
            };
        }

        /// <summary>
        /// 将消息解析为复合消息。
        /// </summary>
        /// <param name="message">要解析的消息。</param>
        /// <returns>解析后的复合消息。</returns>
        public static ComplexMessage Parse(this Message message)
        {
            return ComplexMessage.Parse(message?.Content);
        }

        /// <summary>
        /// 发送复合消息。
        /// </summary>
        /// <param name="chat">目标聊天。</param>
        /// <param name="message">要发送的消息。</param>
        /// <returns>发送的消息。</returns>
        public static Message Send(this Chat chat, ComplexMessage message)
        {
            if (chat is null)
            {
                throw new ArgumentNullException(nameof(chat));
            }

            if (message is null)
            {
                throw new ArgumentNullException(nameof(message));
            }

            return chat.Send(message.ToString());
        }

        /// <summary>
        /// 发送消息元素。
        /// </summary>
        /// <param name="chat">目标聊天。</param>
        /// <param name="element">要发送的消息。</param>
        /// <returns>发送的消息。</returns>
        public static Message Send(this Chat chat, MessageElement element)
        {
            if (chat is null)
            {
                throw new ArgumentNullException(nameof(chat));
            }

            if (element is null)
            {
                throw new ArgumentNullException(nameof(element));
            }

            return chat.Send(element.ToString());
        }
    }
}