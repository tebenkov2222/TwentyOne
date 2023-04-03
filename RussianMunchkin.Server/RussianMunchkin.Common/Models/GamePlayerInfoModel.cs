using System.Collections.Generic;
using MessagePack;

namespace RussianMunchkin.Common.Models
{
    [MessagePackObject]
    public class GamePlayerInfoModel
    {
        [Key(0)]
        public int Id { get; set; }
        [Key(1)]
        public int Sum { get; set; }
        [Key(2)]
        public List<int> Numbers { get; set; }
        [Key(3)]
        public bool IsWinner { get; set; }
    }
}