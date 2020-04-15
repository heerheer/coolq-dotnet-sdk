using System.Collections.Generic;

namespace HuajiTech.CoolQ.AdvancedMessaging
{
    /// <summary>
    /// 表示 At (@) 全体成员。
    /// </summary>
    public class AtAll : CQCode
    {
        public AtAll()
        {
            this["qq"] = "all";
        }

        public AtAll(IDictionary<string, string> parameters)
            : base(parameters)
        {
        }

        public override string Type => "at";
    }
}