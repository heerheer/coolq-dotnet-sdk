using System.Collections.Generic;

namespace HuajiTech.CoolQ.Messaging
{
    /// <summary>
    /// 表示表情。
    /// </summary>
    public class Emoticon : CQCode
    {
        public Emoticon()
        {
        }

        public Emoticon(IDictionary<string, string> arguments)
            : base(arguments)
        {
        }

        /// <summary>
        /// 获取或设置 Id。
        /// </summary>
        public int Id
        {
            get => GetParameterAsInt32("id");
            set => SetParameter("id", value);
        }

        public override string Type => "face";
    }
}