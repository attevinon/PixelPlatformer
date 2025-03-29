using System;
using PixelCrew.Model.Definitions;

namespace PixelCrew.Model.Data.Inventory
{
    [Serializable]
    public class InventoryItemData : IReadOnlyItemData
    {
        [InventoryId] public string Id;
        public int Value;

        public string GetId() => Id;
        public int GetValue() => Value;
        
        public InventoryItemData(string id)
        {
            Id = id;
        }
    }
}