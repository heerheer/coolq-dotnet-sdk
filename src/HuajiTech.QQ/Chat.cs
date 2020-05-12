namespace HuajiTech.QQ
{
    /// <summary>
    /// ��ʾ���졣
    /// ����Ϊ�����ࡣ
    /// </summary>
    public abstract class Chat : IChattable
    {
        protected Chat(long number) => Number = number;

        public abstract string DisplayName { get; }

        public long Number { get; }

        public override bool Equals(object obj) => Equals(obj as IChattable);

        public virtual bool Equals(IChattable other) => base.Equals(other) || (other is Chat && other?.Number == Number);

        public override int GetHashCode() => (int)Number;

        public abstract IMessage Send(string message);

        public override string ToString() => $"{GetType().Name}({Number})";
    }
}