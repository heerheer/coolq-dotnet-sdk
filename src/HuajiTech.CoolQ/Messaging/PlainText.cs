namespace HuajiTech.CoolQ.Messaging
{
    /// <summary>
    /// 表示纯文本。
    /// 此类不能被继承。
    /// </summary>
    public sealed class PlainText : MessageElement
    {
        /// <summary>
        /// 以指定内容初始化一个 <see cref="PlainText"/> 类的新实例。
        /// </summary>
        /// <param name="content">要被 <see cref="PlainText"/> 包装的字符串。</param>
        public PlainText(string content)
        {
            Content = content ?? throw new System.ArgumentNullException(nameof(content));
        }

        /// <summary>
        /// 获取未经转义的原始内容。
        /// </summary>
        public string Content { get; }

        /// <summary>
        /// 将指定的字符串转换为可以让酷Q按原义解释字符的语法。
        /// </summary>
        /// <param name="str">要转换的字符串。</param>
        /// <returns>指定字符串的已转换值。</returns>
        public static string Escape(string str) => str
            ?.Replace("&", "&amp;")
            ?.Replace("[", "&#91;")
            ?.Replace("]", "&#93;");

        /// <summary>
        /// 将字符串中的转义字符转换为具有特殊意义的酷Q字符。
        /// </summary>
        /// <param name="str">要转换的字符串。</param>
        /// <returns>指定字符串的已转换值。</returns>
        public static string Unescape(string str) => str
            ?.Replace("&#91;", "[")
            ?.Replace("&#93;", "]")
            ?.Replace("&amp;", "&");

        /// <summary>
        /// 返回经过 <see cref="Escape(string)"/> 后的 <see cref="Content"/>。
        /// </summary>
        /// <returns>经过 <see cref="Escape(string)"/> 后的 <see cref="Content"/>。</returns>
        public override string ToString() => Escape(Content);

        public static implicit operator string(PlainText text) => text?.Content;
    }
}