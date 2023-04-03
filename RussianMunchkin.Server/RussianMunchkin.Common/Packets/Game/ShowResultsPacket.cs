using System.Collections.Generic;
using MessagePack;
using RussianMunchkin.Common.Models;

namespace RussianMunchkin.Common.Packets.Game
{
    [MessagePackObject]
    public class ShowResultsPacket: Packet
    {
        [Key(0)]
        public List<GamePlayerInfoModel> PlayerInfoModels { get; set;}
    }
}