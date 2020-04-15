using System.Collections.Generic;

namespace HuajiTech.CoolQ.AdvancedMessaging
{
    /// <summary>
    /// 表示 At (@)。
    /// </summary>
    public class At : CQCode
    {
        public At()
        {
        }

        public At(IDictionary<string, string> parameters)
            : base(parameters)
        {
        }

        /// <summary>
        /// 获取或设置 At (@) 的目标。
        /// </summary>
        public User Target
        {
            get => new User(GetParameterAsInt64("qq"));
            set => SetParameter("qq", value?.Number ?? default);
        }

        public override string Type => "at";
    }
}