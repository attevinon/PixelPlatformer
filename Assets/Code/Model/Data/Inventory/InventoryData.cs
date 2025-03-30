using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using PixelCrew.Model.Definitions;

namespace PixelCrew.Model.Data.Inventory
{
    [Serializable]
    public class InventoryData
    {
        [SerializeField] private List<InventoryItemData> _inventory = new List<InventoryItemData>();

        public event Action<string, int> OnInventoryChanged;

        public IReadOnlyItemData[] GetAll(params ItemTag[] _tags)
        {
            var filtratedInventory = new List<IReadOnlyItemData>();
            foreach (var itemData in _inventory)
            {
                var itemDef = DefsFacade.I.ItemsDef.Get(itemData.Id);
                bool isAllRequirementsMet = _tags.All(x => itemDef.HasTag(x));
                if(isAllRequirementsMet) filtratedInventory.Add(itemData);
            }
            return filtratedInventory.ToArray();
        }

        public bool TryAdd(string id, int value)
        {
            if (IsNoDef(id)) return false;
            if (value <= 0) return false;

            var itemDef = DefsFacade.I.ItemsDef.Get(id);
            if (itemDef.HasTag(ItemTag.Stackable))
            {
                TryAddStackable(id, value);
            }
            else
            {
                if(IsInventoryFull()) return false;
                AddNonStackable(id, value);
            }

            OnInventoryChanged?.Invoke(id,Count(id));
            return true;
        }

        private bool TryAddStackable(string id, int value)
        {
            var item = GetItem(id);
            if (item == null)
            {
                if (IsInventoryFull()) return false;
                item = new InventoryItemData(id);
                _inventory.Add(item);
            }
            item.Value += value;
            return true;
        }
        private void AddNonStackable (string id, int value)
        {
            int slotsLeft = DefsFacade.I.PlayerDef.InventorySize - _inventory.Count;
            int ableToAdd = Mathf.Min(slotsLeft, value);
            for (int i = 0; i < ableToAdd; i++)
            {  
                _inventory.Add(new InventoryItemData(id) {Value = 1});
            }

            if(ableToAdd < value)
                Debug.Log("Inventory is full");
        }

        public void Remove(string id, int value)
        {
            if (IsNoDef(id)) return;

            bool isSucces = DefsFacade.I.ItemsDef.Get(id).HasTag(ItemTag.Stackable) ?
                TryRemoveStackable(id, value) : TryRemoveNonStackable(id, value);
            if(isSucces)
                OnInventoryChanged?.Invoke(id, Count(id));
        }

        private bool TryRemoveStackable(string id, int value)
        {
            var item = GetItem(id);
            if (item == null) return false;
            item.Value -= value;
            if (item.Value <= 0)
                _inventory.Remove(item);

            return true;
        }

        private bool TryRemoveNonStackable(string id, int value)
        {
            var items = GetItems(id);
            if(items.Length == 0) return false;
            int itemsToRemove = Mathf.Min(items.Length, value);
            for (int i = 0; i < itemsToRemove; i++)
            {
                _inventory.Remove(items[i]);
            }
            return true;
        }
        
        public int Count(string id)
        {
            int count = 0;
            foreach (var item in _inventory)
            {
                if(item.Id == id)
                    count += item.Value;
            }
            return count;  
        }

        public bool TryUseItem(string id)
        {
            if (IsNoDef(id)) return false;
            var itemDef = DefsFacade.I.ItemsDef.Get(id);

            if(itemDef.OnUse == null)
            {
                Debug.LogWarning($"{id} use is unspecified");
                return false;
            }

            itemDef.OnUse.Invoke();
            return true;
        }

        private InventoryItemData GetItem(string id)
        {
            foreach (var item in _inventory)
            { 
                if(item.Id == id) return item;
            }

            return null;
        }

        private InventoryItemData[] GetItems(string id)
        {
            var items = new List<InventoryItemData>();
            foreach (var item in _inventory)
            {
                if (item.Id == id) items.Add(item);
            }
            return items?.ToArray();
        }

        private bool IsNoDef(string id)
        {
            var itemDef = DefsFacade.I.ItemsDef.Get(id);
            if (itemDef.IsVoid)
                Debug.LogWarning($"Definition for item with id={id} is not found");

            return itemDef.IsVoid;
        }

        private bool IsInventoryFull()
        {
            if (_inventory.Count >= DefsFacade.I.PlayerDef.InventorySize)
            {
                Debug.Log("Inventory is full");
                return true;
            }
            return false;
        }
    }
}
