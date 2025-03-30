using System;
using PixelCrew.Model.Data.Properties;
using PixelCrew.Model.Definitions;
using PixelCrew.Utils.Disposables;
using UnityEngine;

namespace PixelCrew.Model.Data.Inventory
{
    public class QuickInventoryData
    {
        public event Action OnChanged;
        
        private PlayerData _playerData;
        private IReadOnlyItemData[] _inventory;
        private IntObservableProperty _selectedIndex = new IntObservableProperty();
        private ItemTag _itemsInQuickInventoryTag;
        public IntObservableProperty SelectedIndex => _selectedIndex;

        public IReadOnlyItemData[] Inventory => _inventory;
        
        public QuickInventoryData(PlayerData playerData)
        {
            _playerData = playerData;
            _itemsInQuickInventoryTag = ItemTag.Usable;
            _inventory = _playerData.Inventory.GetAll(_itemsInQuickInventoryTag);
            _playerData.Inventory.OnInventoryChanged += OnInventoryChanged;
        }

        public IDisposable Subscribe(Action onChanged)
        {
            OnChanged += onChanged;
            return new ActionDisposable(() => OnChanged -= onChanged);
        }
        private void OnInventoryChanged(string id, int value)
        {
            var itemDef = DefsFacade.I.ItemsDef.Get(id);
            if(!itemDef.HasTag(_itemsInQuickInventoryTag)) return;
            
            _inventory = _playerData.Inventory.GetAll(_itemsInQuickInventoryTag);
            _selectedIndex.Value = Mathf.Clamp(_selectedIndex.Value, 0, _inventory.Length - 1);
            OnChanged?.Invoke();
        }

        public void SetNextItem()
        {
            SelectedIndex.Value = (int) Mathf.Repeat(SelectedIndex.Value + 1, _inventory.Length);
        }
    }
}