using System;

namespace HuajiTech.CoolQ.DataExchange
{
    internal class AnonymousMemberInfo
    {
        public long Id { get; set; }

        public string? Name { get; set; }

        [System.Diagnostics.CodeAnalysis.SuppressMessage(
            "StyleCop.CSharp.SpacingRules", "SA1011:Closing square brackets should be spaced correctly", Justification = "<����>")]
        public byte[]? Token { get; set; }
    }
}