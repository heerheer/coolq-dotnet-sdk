using System;
using System.Collections.Generic;

namespace HuajiTech.CoolQ.AdvancedMessaging
{
    /// <summary>
    /// 表示名片分享。
    /// </summary>
    public class ChatShare : CQCode
    {
        public ChatShare()
        {
        }

        public ChatShare(IDictionary<string, string> arguments)
            : base(arguments)
        {
        }

        /// <summary>
        /// 获取或设置当前 <see cref="ChatShare"/> 对象的内容。
        /// </summary>
        public Chat Content
        {
            get => this["type"] switch
            {
                "qq" => new User(GetArgumentAsInt64("id")),
                "group" => new Group(GetArgumentAsInt64("id")),
                _ => null
            };

            set
            {
                if (value is null)
                {
                    return;
                }

                this["type"] = value switch
                {
                    User _ => "qq",
                    Group _ => "group",
                    _ => throw new ArgumentOutOfRangeException(nameof(value)),
                };

                SetArgument("id", value.Number);
            }
        }

        public override string Type => "contact";
    }
}