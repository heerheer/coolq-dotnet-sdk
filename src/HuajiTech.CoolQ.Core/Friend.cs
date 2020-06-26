using System;
using System.Linq;
using HuajiTech.CoolQ.Interop;

namespace HuajiTech.CoolQ
{
    internal class Friend : User, IFriend
    {
        private FriendInfo? _info;

        public Friend(long number)
            : base(number)
        {
        }

        internal Friend(FriendInfo info)
            : base(info.Number)
        {
            _info = info;
        }

        public override bool IsRequestedSuccessfully => !(_info is null);

        public string? Alias => GetInfo().Alias;

        public override string? Nickname => _info?.Nickname ?? base.Nickname;

        public override string DisplayName => Alias ?? base.DisplayName;

        public override void Request() => GetInfo(true);

        public override void Refresh()
        {
            Request();
            base.Refresh();
        }

        private FriendInfo GetInfo(bool requesting = false)
        {
            if (IsRequested && !IsRequestedSuccessfully && !requesting)
            {
                return FriendInfo.Empty;
            }

            IsRequested = true;

            try
            {
                _info = CurrentUser.GetFriendInfos().First(info => info.Number == Number);
                return _info;
            }
            catch (ApiException) when (!requesting)
            {
            }
            catch (InvalidOperationException)
            {
                if (requesting)
                {
                    throw new InvalidOperationException(CoreResources.FriendNotExist);
                }
            }

            return FriendInfo.Empty;
        }
    }
}