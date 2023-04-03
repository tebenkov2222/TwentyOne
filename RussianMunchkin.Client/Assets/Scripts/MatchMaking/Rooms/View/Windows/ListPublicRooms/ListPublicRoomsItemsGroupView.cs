using System;
using RussianMunchkin.Common.Models;
using View.Shared;
using View.Shared.ItemsGroup;

namespace MatchMaking.Rooms.View.Windows.ListPublicRooms
{
    public class ListPublicRoomsItemsGroupView: LinearListItemsGroupView<ListPublicRoomsItem>
    {
        public event Action<RoomInfoModel> ConnectToRoom;
        public override TPrefab Add<TPrefab>(TPrefab prefab)
        {
            var item = base.Add(prefab);
            item.ConnectToRoom+=ItemOnConnectToRoom;
            return item;
        }

        public override void RemoveItem(ListPublicRoomsItem item)
        {
            item.ConnectToRoom-=ItemOnConnectToRoom;
            base.RemoveItem(item);
        }

        private void ItemOnConnectToRoom(RoomInfoModel model)
        {
            ConnectToRoom?.Invoke(model);
        }
    }
}