using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;

namespace HuajiTech.CoolQ.Messaging
{
    /// <summary>
    /// 表示复合消息。
    /// </summary>
    [System.Diagnostics.CodeAnalysis.SuppressMessage(
        "Naming", "CA1710:标识符应具有正确的后缀", Justification = "<挂起>")]
    public partial class ComplexMessage : ISendable, IEquatable<ComplexMessage?>
    {
        private static readonly Regex CQCodeRegex = new Regex(
            @"\[CQ:(?<Type>[a-zA-Z\-_\.]+)(,(?<Key>[a-zA-Z\-_\.]+)=(?<Value>[^,\]]+))*\]",
            RegexOptions.Compiled);

        private readonly List<MessageElement> _elements;

        /// <summary>
        /// 初始化一个空的 <see cref="ComplexMessage"/> 类的新实例。
        /// </summary>
        public ComplexMessage() => _elements = new List<MessageElement>();

        /// <summary>
        /// 以指定的 <see cref="MessageElement"/> 实例初始化一个 <see cref="ComplexMessage"/> 类的新实例。
        /// </summary>
        /// <param name="element">消息元素。</param>
        public ComplexMessage(MessageElement element)
            : this()
            => Add(element);

        /// <summary>
        /// 以指定的 <see cref="MessageElement"/> 集合初始化一个 <see cref="ComplexMessage"/> 类的新实例。
        /// </summary>
        /// <param name="elements"><see cref="MessageElement"/> 集合。</param>
        /// <exception cref="ArgumentNullException"><paramref name="elements"/> 为 <see langword="null"/>。</exception>
        public ComplexMessage(IEnumerable<MessageElement?> elements)
            : this()
            => Add(elements);

        /// <summary>
        /// 获取或设置指定索引处的 <see cref="MessageElement"/> 实例。
        /// </summary>
        /// <param name="index">要获取或设置的<see cref="MessageElement"/> 实例从 0 开始的索引。</param>
        /// <exception cref="ArgumentOutOfRangeException"><paramref name="index"/> 小于 0。 或 <paramref name="index"/> 等于或大于 <see cref="Count"/>。</exception>
        public MessageElement this[int index]
        {
            get => _elements[index];
            set => _elements[index] = value ?? throw new ArgumentNullException(nameof(value));
        }

        public static implicit operator ComplexMessage(string str) => FromString(str);

        public static bool operator ==(ComplexMessage? left, ComplexMessage? right)
            => left?.Equals(right) ?? right is null;

        public static bool operator !=(ComplexMessage? left, ComplexMessage? right)
            => !(left == right);

        [System.Diagnostics.CodeAnalysis.SuppressMessage(
            "Usage", "CA2225:运算符重载具有命名的备用项", Justification = "<挂起>")]
        public static ComplexMessage operator -(ComplexMessage? left, MessageElement? right)
            => left?.Remove(right) ?? right ?? new ComplexMessage();

        public static ComplexMessage operator +(ComplexMessage? left, MessageElement? right)
            => right is null ? left ?? new ComplexMessage() : left?.Add(right) ?? right;

        public static ComplexMessage operator +(MessageElement? left, ComplexMessage? right)
            => left is null ? right ?? new ComplexMessage() : right?.Insert(0, left) ?? left;

        public static ComplexMessage operator +(ComplexMessage? left, ComplexMessage? right)
            => right is null ? left ?? new ComplexMessage() : left?.Add(right) ?? right;

        /// <summary>
        /// 创建一个包含指定的字符串的 <see cref="ComplexMessage"/> 实例。
        /// </summary>
        /// <param name="str">要包含的内容。</param>
        public static ComplexMessage FromString(string str) => MessageElement.FromString(str);

        /// <summary>
        /// 将字符串解析为 <see cref="ComplexMessage"/> 实例。
        /// </summary>
        /// <param name="str">要解析的 <see cref="ComplexMessage"/> 实例的字符串表示形式。</param>
        /// <param name="useEmoji">如果要在返回的 <see cref="ComplexMessage"/> 实例中包含 <see cref="Emoji"/> 实例，则为 <see langword="true"/>；否则为 <see langword="false"/>。</param>
        /// <returns>与字符串等效的 <see cref="ComplexMessage"/> 实例。</returns>
        public static ComplexMessage Parse(string str, bool useEmoji = false)
        {
            if (str is null)
            {
                throw new ArgumentNullException(nameof(str));
            }

            if (useEmoji)
            {
                return new ComplexMessage(GetMessageElements());
            }

            var convertedElements = GetMessageElements()
                .Select(element => element is Emoji emoji ? emoji.ConvertToString() : element);
            return new ComplexMessage(GetPlainTextCombinedMessageElements(convertedElements));

            IEnumerable<MessageElement> GetMessageElements()
            {
                var matches = CQCodeRegex.Matches(str);
                var lastMatchEndIndex = 0;

                foreach (Match match in matches)
                {
                    var length = match.Index - lastMatchEndIndex;
                    if (length != 0)
                    {
                        var text = str.Substring(lastMatchEndIndex, length);
                        yield return new PlainText(PlainText.Unescape(text));
                    }

                    lastMatchEndIndex = match.Index + match.Length;

                    var groups = match.Groups;
                    var type = groups["Type"].Value;

                    IEnumerable<KeyValuePair<string, string>> GetParameters()
                    {
                        var keyCaptures = groups["Key"].Captures;
                        var valueCaptures = groups["Value"].Captures;

                        for (var i = 0; i < keyCaptures.Count; i++)
                        {
                            yield return new KeyValuePair<string, string>(
                                keyCaptures[i].Value,
                                valueCaptures[i].Value);
                        }
                    }

                    yield return CQCode.Create(
                        type, GetParameters().Distinct().ToDictionary(
                            item => item.Key,
                            item => CQCode.Unescape(item.Value)));
                }

                if (lastMatchEndIndex != str.Length)
                {
                    yield return new PlainText(PlainText.Unescape(str.Substring(lastMatchEndIndex)));
                }
            }
        }

        /// <summary>
        /// 使用指定的分隔符串联 <see cref="ComplexMessage"/> 集合中的所有成员。
        /// </summary>
        /// <param name="separator">要用作分隔符的 <see cref="MessageElement"/> 实例。</param>
        /// <param name="messages">一个包含要串联的 <see cref="ComplexMessage"/> 实例的集合。</param>
        /// <returns>
        /// 一个包含 <paramref name="messages"/> 中所有成员的 <see cref="ComplexMessage"/> 实例，这些成员以 <paramref name="separator"/> 分隔。
        /// 如果 <paramref name="messages"/> 没有成员，则该方法返回一个空的 <see cref="ComplexMessage"/> 实例。
        /// </returns>
        /// <exception cref="ArgumentNullException"><paramref name="messages"/> 为 <see langword="null"/>。</exception>
        public static ComplexMessage Join(MessageElement separator, IEnumerable<ComplexMessage> messages)
        {
            if (messages is null)
            {
                throw new ArgumentNullException(nameof(messages));
            }

            if (!messages.Any())
            {
                return new ComplexMessage();
            }

            if (messages.Count() is 1)
            {
                return messages.First();
            }

            IEnumerable<MessageElement> GetMessageElements()
            {
                foreach (var element in messages.First())
                {
                    yield return element;
                }

                foreach (var message in messages.Skip(1))
                {
                    yield return separator;

                    foreach (var element in message)
                    {
                        yield return element;
                    }
                }
            }

            return new ComplexMessage(GetMessageElements());
        }

        private static IEnumerable<MessageElement> GetPlainTextCombinedMessageElements(
            IEnumerable<MessageElement> elements)
        {
            var buffer = new StringBuilder();

            foreach (var element in elements)
            {
                if (element is PlainText text)
                {
                    buffer.Append(text);
                    continue;
                }

                if (buffer.Length > 0)
                {
                    yield return buffer.ToString();
                    buffer.Clear();
                }

                yield return element;
            }

            if (buffer.Length > 0)
            {
                yield return buffer.ToString();
            }
        }

        /// <summary>
        /// 将当前 <see cref="ComplexMessage"/> 实例中的所有连续的 <see cref="PlainText"/> 实例拼接为单个 <see cref="PlainText"/> 实例。
        /// </summary>
        /// <returns>一个新的 <see cref="ComplexMessage"/> 实例，包含已被拼接的 <see cref="PlainText"/> 实例。</returns>
        public ComplexMessage CombinePlainText()
            => new ComplexMessage(GetPlainTextCombinedMessageElements(this));

        /// <summary>
        /// 使用指定的分隔符将当前 <see cref="ComplexMessage"/> 实例中的所有 <see cref="PlainText"/> 实例拼接为字符串。
        /// </summary>
        /// <param name="separator">要用作分隔符的字符串。</param>
        /// <returns>
        /// 一个由当前 <see cref="ComplexMessage"/> 实例中所有 <see cref="PlainText"/> 实例组成的字符串，这些字符串以 <paramref name="separator"/> 分隔。
        /// 如果当前  <see cref="ComplexMessage"/> 实例不包含任何 <see cref="PlainText"/> 实例，则该方法返回 <see cref="string.Empty"/>。
        /// </returns>
        public string GetPlainText(string separator = "")
            => string.Join(separator, this.OfType<PlainText>().Select(text => text.Content));

        public ComplexMessage SplitPlainText(Func<string, IEnumerable<string>> split)
        {
            if (split is null)
            {
                throw new ArgumentNullException(nameof(split));
            }

            return new ComplexMessage(GetMessageElements());

            IEnumerable<MessageElement> GetMessageElements()
            {
                foreach (var element in this)
                {
                    if (element is PlainText text)
                    {
                        foreach (var str in split(text))
                        {
                            yield return str;
                        }
                    }
                    else
                    {
                        yield return element;
                    }
                }
            }
        }

        /// <summary>
        /// 基于数组中的字符串将当前 <see cref="ComplexMessage"/> 实例中的所有 <see cref="PlainText"/> 实例拆分为多个 <see cref="PlainText"/> 实例。
        /// 可以指定子 <see cref="PlainText"/> 实例是否包含空数组元素。
        /// </summary>
        /// <param name="separator">分隔此 <see cref="ComplexMessage"/> 实例中 <see cref="PlainText"/> 实例的字符串数组、不包含分隔符的空数组或 null。</param>
        /// <param name="options">
        /// 要省略返回的数组中的空数组元素，则为 <see cref="StringSplitOptions.RemoveEmptyEntries"/>；
        /// 要包含返回的数组中的空数组元素，则为 <see cref="StringSplitOptions.None"/>。
        /// </param>
        /// <returns>一个 <see cref="ComplexMessage"/> 实例，其元素包含此 <see cref="ComplexMessage"/> 实例中的子 <see cref="PlainText"/> 实例，这些子 <see cref="PlainText"/> 实例由 <paramref name="separator"/> 中的一个或多个字符串分隔。</returns>
        /// <exception cref="ArgumentException"><paramref name="options"/> 不是 <see cref="StringSplitOptions"/> 值之一。</exception>
        public ComplexMessage SplitPlainText(string[] separator, StringSplitOptions options)
            => SplitPlainText(str => str.Split(separator, options));

        /// <summary>
        /// 基于数组中的字符串将当前 <see cref="ComplexMessage"/> 实例中的所有 <see cref="PlainText"/> 实例拆分为多个 <see cref="PlainText"/> 实例。
        /// </summary>
        /// <param name="separator">分隔此 <see cref="ComplexMessage"/> 实例中 <see cref="PlainText"/> 实例的字符串数组、不包含分隔符的空数组或 null。</param>
        /// <returns>一个 <see cref="ComplexMessage"/> 实例，其元素包含此 <see cref="ComplexMessage"/> 实例中的子 <see cref="PlainText"/> 实例，这些子子 <see cref="PlainText"/> 实例由 <paramref name="separator"/> 中的一个或多个字符串分隔。</returns>
        public ComplexMessage SplitPlainText(params string[] separator)
            => SplitPlainText(separator, StringSplitOptions.RemoveEmptyEntries);

        /// <summary>
        /// 将 <see cref="MessageElement"/> 实例添加到 <see cref="ComplexMessage"/> 的结尾处。
        /// </summary>
        /// <param name="item">要添加到 <see cref="ComplexMessage"/> 末尾的实例。</param>
        /// <returns>当前 <see cref="ComplexMessage"/> 实例。</returns>
        public ComplexMessage Add(MessageElement item)
        {
            if (item is null)
            {
                throw new ArgumentNullException(nameof(item));
            }

            _elements.Add(item);
            return this;
        }

        /// <summary>
        /// 将指定 <see cref="MessageElement"/> 集合添加到 <see cref="ComplexMessage"/> 的末尾。
        /// </summary>
        /// <param name="collection">
        /// 应将其元素添加到 <see cref="ComplexMessage"/> 的末尾的集合。
        /// 集合自身不能为 <see langword="null"/>。
        /// </param>
        /// <returns>当前 <see cref="ComplexMessage"/> 实例。</returns>
        /// <exception cref="ArgumentNullException"><paramref name="collection"/> 为 <see langword="null"/>。</exception>
        public ComplexMessage Add(IEnumerable<MessageElement?> collection)
        {
            _elements.AddRange(collection.OfType<MessageElement>());
            return this;
        }

        /// <summary>
        /// 将 <see cref="MessageElement"/> 插入到 <see cref="ComplexMessage"/> 的指定索引处。
        /// </summary>
        /// <param name="index">应插入 <paramref name="item"/> 的从零开始的索引。</param>
        /// <param name="item">要插入的 <see cref="MessageElement"/> 实例。</param>
        /// <returns>当前 <see cref="ComplexMessage"/> 实例。</returns>
        /// <exception cref="ArgumentOutOfRangeException">
        /// <paramref name="index"/> 小于 0。
        /// 或 <paramref name="index"/> 大于 <see cref="Count"/>。
        /// </exception>
        public ComplexMessage Insert(int index, MessageElement item)
        {
            if (item is null)
            {
                throw new ArgumentNullException(nameof(item));
            }

            _elements.Insert(index, item);
            return this;
        }

        /// <summary>
        /// 从 <see cref="ComplexMessage"/> 中移除特定 <see cref="MessageElement"/> 的第一个匹配项。
        /// </summary>
        /// <param name="item">要从 <see cref="ComplexMessage"/> 中删除的实例。</param>
        /// <returns>当前 <see cref="ComplexMessage"/> 实例。</returns>
        public ComplexMessage Remove(MessageElement? item)
        {
            if (item is null)
            {
                return this;
            }

            _elements.Remove(item);
            return this;
        }

        /// <summary>
        /// 尝试从 <see cref="ComplexMessage"/> 中移除特定 <see cref="MessageElement"/> 的第一个匹配项。
        /// </summary>
        /// <param name="item">要从 <see cref="ComplexMessage"/> 中删除的实例。</param>
        /// <returns>
        /// 如果成功移除了 <paramref name="item"/>，则为 <see langword="true"/>；否则为 <see langword="false"/>。
        /// 如果在 <see cref="ComplexMessage"/> 中没有找到 <paramref name="item"/>，则此方法也会返回 <see langword="false"/>。
        /// </returns>
        public bool TryRemove(MessageElement? item)
        {
            if (item is null)
            {
                return false;
            }

            return _elements.Remove(item);
        }

        /// <summary>
        /// 从 <see cref="ComplexMessage"/> 中移除特定 <see cref="MessageElement"/> 的所有匹配项。
        /// </summary>
        /// <param name="item">要从 <see cref="ComplexMessage"/> 中删除的实例。</param>
        /// <returns>当前 <see cref="ComplexMessage"/> 实例。</returns>
        public ComplexMessage RemoveAll(MessageElement? item)
        {
            if (item is null)
            {
                return this;
            }

            _elements.RemoveAll(element => element == item);
            return this;
        }

        /// <summary>
        /// 移除 <see cref="ComplexMessage"/> 的指定索引处的 <see cref="MessageElement"/>。
        /// </summary>
        /// <param name="index">要移除的元素的从零开始的索引。</param>
        /// <returns>当前 <see cref="ComplexMessage"/> 实例。</returns>
        public ComplexMessage RemoveAt(int index)
        {
            _elements.RemoveAt(index);
            return this;
        }

        /// <summary>
        /// 从 <see cref="ComplexMessage"/> 中移除一定范围的 <see cref="MessageElement"/>。
        /// </summary>
        /// <param name="index">要移除的元素范围的从零开始的起始索引。</param>
        /// <param name="count">要移除的元素数。</param>
        /// <returns>当前 <see cref="ComplexMessage"/> 实例。</returns>
        public ComplexMessage RemoveRange(int index, int count)
        {
            _elements.RemoveRange(index, count);
            return this;
        }

        public ComplexMessage Slice(int start, int count) => new ComplexMessage(this.Skip(start).Take(count));

        public string ToSendableString() => string.Join(string.Empty, _elements);

        public bool Equals(ComplexMessage? other) => base.Equals(other) || other?.ToString() == ToString();

        public override bool Equals(object? obj) => Equals(obj as ComplexMessage);

        public override int GetHashCode() => ToSendableString().GetHashCode();
    }

    /// <summary>
    /// <see cref="IList{T}"/> 和 <see cref="IReadOnlyList{T}"/> 的实现。
    /// </summary>
    public partial class ComplexMessage : IList<MessageElement>, IReadOnlyList<MessageElement>
    {
        public int Count => _elements.Count;

        public bool IsReadOnly => ((IList<MessageElement>)_elements).IsReadOnly;

        void ICollection<MessageElement>.Add(MessageElement item)
        {
            if (item is null)
            {
                throw new ArgumentNullException(nameof(item));
            }

            _elements.Add(item);
        }

        public void Clear() => _elements.Clear();

        public bool Contains(MessageElement item)
        {
            if (item is null)
            {
                return false;
            }

            return _elements.Contains(item);
        }

        public void CopyTo(MessageElement[] array, int arrayIndex) => _elements.CopyTo(array, arrayIndex);

        public IEnumerator<MessageElement> GetEnumerator() => ((IList<MessageElement>)_elements).GetEnumerator();

        IEnumerator IEnumerable.GetEnumerator() => ((IList<MessageElement>)_elements).GetEnumerator();

        public int IndexOf(MessageElement item)
        {
            if (item is null)
            {
                return -1;
            }

            return _elements.IndexOf(item);
        }

        void IList<MessageElement>.Insert(int index, MessageElement item)
        {
            if (item is null)
            {
                throw new ArgumentNullException(nameof(item));
            }

            _elements.Insert(index, item);
        }

        bool ICollection<MessageElement>.Remove(MessageElement item) => _elements.Remove(item);

        void IList<MessageElement>.RemoveAt(int index) => _elements.RemoveAt(index);
    }
}