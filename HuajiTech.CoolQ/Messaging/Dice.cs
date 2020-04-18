using System.Collections.Generic;

namespace HuajiTech.CoolQ.Messaging
{
    /// <summary>
    /// 表示骰子的 <see cref="CQCode"/>。
    /// </summary>
    public class Dice : CQCode
    {
        public Dice()
            : base("emoji")
        {
        }

        public Dice(IDictionary<string, string> arguments)
            : base("emoji", arguments)
        {
        }

        /// <summary>
        /// 获取或设置当前 <see cref="Dice"/> 对象的点数。
        /// </summary>
        public int Point
        {
            get => GetParameterAsInt32("type");
            set => SetParameter("type", value);
        }
    }
}