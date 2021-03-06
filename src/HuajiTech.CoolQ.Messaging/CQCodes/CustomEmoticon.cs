using System.Collections.Generic;

namespace HuajiTech.CoolQ.Messaging
{
    /// <summary>
    /// 表示自定义表情的 <see cref="CQCode"/>。
    /// </summary>
    public class CustomEmoticon : CQCode
    {
        public CustomEmoticon()
            : base("bface")
        {
        }

        public CustomEmoticon(IDictionary<string, string> parameters)
            : base("bface", parameters)
        {
        }

        /// <summary>
        /// 获取或设置当前 <see cref="CustomEmoticon"/> 实例的 ID。
        /// </summary>
        public int Id
        {
            get => GetParameterAsInt32("id");
            set => SetParameter("id", value);
        }
    }
}