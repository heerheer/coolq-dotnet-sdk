using System;

namespace HuajiTech.CoolQ
{
    /// <summary>
    /// ��ʾ��Ϣ��
    /// ����Ϊ�����ࡣ
    /// </summary>
    public abstract class Message : IEquatable<Message?>
    {
        /// <summary>
        /// ������������дʱ����ȡ��ǰ <see cref="Message"/> ʵ���� ID��
        /// </summary>
        public abstract int Id { get; }

        /// <summary>
        /// ������������дʱ����ȡ��ǰ <see cref="Message"/> ʵ�������ݡ�
        /// </summary>
        public abstract string Content { get; }

        public static implicit operator string?(Message? message) => message?.ToString();

        public sealed override string ToString() => Content ?? string.Empty;

        public override bool Equals(object? obj) => Equals(obj as Message);

        public bool Equals(Message? other) => other?.Id == Id;

        public override int GetHashCode() => Id.GetHashCode();

        /// <summary>
        /// ������������дʱ�����ص�ǰ <see cref="Message"/> ʵ����
        /// </summary>
        public abstract void Recall();
    }
}