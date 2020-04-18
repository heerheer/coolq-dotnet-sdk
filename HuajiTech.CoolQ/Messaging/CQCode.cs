using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;

namespace HuajiTech.CoolQ.Messaging
{
    /// <summary>
    /// 表示CQ码。
    /// </summary>
    public class CQCode : MessageElement
    {
        /// <summary>
        /// 以指定的类型初始化一个 <see cref="CQCode"/> 类的新实例。
        /// </summary>
        /// <param name="type">CQ码的类型。</param>
        /// <exception cref="ArgumentException"><paramref name="type"/> 为 <c>null</c>、<see cref="string.Empty"/> 或仅由空白字符组成。</exception>
        public CQCode(string type)
        {
            if (string.IsNullOrWhiteSpace(type))
            {
                throw new ArgumentException(Resources.FieldCannotBeEmptyOrWhiteSpace, nameof(type));
            }

            Type = type;
            Arguments = new Dictionary<string, string>();
        }

        /// <summary>
        /// 以指定的类型和参数初始化一个 <see cref="CQCode"/> 类的新实例。
        /// </summary>
        /// <param name="type">CQ码的类型。</param>
        /// <param name="arguments">一个字典，包含 <see cref="CQCode"/> 的所有参数。</param>
        /// <exception cref="ArgumentException"><paramref name="type"/> 为 <c>null</c>、<see cref="string.Empty"/> 或仅由空白字符组成。</exception>
        /// <exception cref="ArgumentNullException"><paramref name="arguments"/> 为 <c>null</c>。</exception>
        public CQCode(string type, IDictionary<string, string> arguments)
        {
            if (string.IsNullOrWhiteSpace(type))
            {
                throw new ArgumentException(Resources.FieldCannotBeEmptyOrWhiteSpace, nameof(type));
            }

            Type = type;
            Arguments = arguments ?? throw new ArgumentNullException(nameof(arguments));
        }

        /// <summary>
        /// 获取或设置指定键处的参数的值。
        /// </summary>
        /// <param name="key">要获取或设置的参数的键。</param>
        /// <value>
        /// 指定 <paramref name="key"/> 处的参数的值。
        /// 如果指定键不存在，则为 <c>null</c>。
        /// </value>
        public string this[string key]
        {
            get
            {
                if (!Arguments.ContainsKey(key))
                {
                    return null;
                }

                return Arguments[key];
            }

            set => Arguments[key] = value;
        }

        /// <summary>
        /// 获取当前 <see cref="CQCode"/> 对象的参数。
        /// </summary>
        public IDictionary<string, string> Arguments { get; }

        /// <summary>
        /// 获取当前 <see cref="CQCode"/> 对象的类型。
        /// </summary>
        public string Type { get; }

        /// <summary>
        /// 将指定的字符串转换为可以让酷Q按原义解释字符的语法。
        /// </summary>
        /// <param name="str">要转换的字符串。</param>
        /// <returns>指定字符串的已转换值。</returns>
        public static string Escape(string str)
        {
            return PlainText.Escape(str).Replace(",", "&#44");
        }

        /// <summary>
        /// 将字符串中的转义字符转换为具有特殊意义的酷Q字符。
        /// </summary>
        /// <param name="str">要转换的字符串。</param>
        /// <returns>指定字符串的已转换值。</returns>
        public static string Unescape(string str)
        {
            return PlainText.Unescape(str).Replace("&#44", ",");
        }

        /// <summary>
        /// 将当前 <see cref="CQCode"/> 对象的值转换为它的等效字符串表示形式。
        /// </summary>
        /// <returns>当前 <see cref="CQCode"/> 对象的值的字符串表示形式。</returns>
        public override string ToString()
        {
            return Arguments.Any() ?
                $"[CQ:{Type},{string.Join(",", Arguments.Select(para => $"{para.Key}={Escape(para.Value)}"))}]" :
                $"[CQ:{Type}]";
        }

        protected bool GetParameterAsBoolean(string key)
        {
            return this[key] == "true";
        }

        protected int GetParameterAsInt32(string key)
        {
            if (int.TryParse(this[key], NumberStyles.Integer, CultureInfo.InvariantCulture, out var value))
            {
                return value;
            }

            return default;
        }

        protected long GetParameterAsInt64(string key)
        {
            if (long.TryParse(this[key], NumberStyles.Integer, CultureInfo.InvariantCulture, out var value))
            {
                return value;
            }

            return default;
        }

        protected float GetParameterAsSingle(string key)
        {
            if (float.TryParse(this[key], NumberStyles.Float, CultureInfo.InvariantCulture, out var value))
            {
                return value;
            }

            return default;
        }

        protected Uri GetParameterAsUri(string key)
        {
            try
            {
                return new Uri(this[key]);
            }
            catch (UriFormatException)
            {
                return default;
            }
        }

        protected void SetParameter(string key, bool value)
        {
            this[key] = value ? "true" : "false";
        }

        protected void SetParameter(string key, int value)
        {
            this[key] = value.ToString(CultureInfo.InvariantCulture);
        }

        protected void SetParameter(string key, long value)
        {
            this[key] = value.ToString(CultureInfo.InvariantCulture);
        }

        protected void SetParameter(string key, float value)
        {
            this[key] = value.ToString(CultureInfo.InvariantCulture);
        }

        protected void SetParameter(string key, Uri value)
        {
            this[key] = value?.ToString();
        }
    }
}