namespace HuajiTech.CoolQ.Interop
{
    internal sealed class GroupInfoReader : Reader<GroupInfo>
    {
        public GroupInfoReader(string base64String)
            : base(base64String)
        {
        }

        public override GroupInfo Read()
            => new GroupInfo
            {
                Number = ReadInt64(),
                Name = ReadString(),
                MemberCount = ReadInt32(),
                MemberCapacity = ReadInt32()
            };
    }
}