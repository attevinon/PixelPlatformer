using System;
using PixelCrew.Model.Data.Properties;
using PixelCrew.Utils.Disposables;
using UnityEngine;

namespace PixelCrew.Model.Data.Inventory
{
    public class QuickInventoryData
    {
        public event Action OnChanged;
        
        private PlayerData _playerData;
        private IReadOnlyItemData[] _quickInventory;
        private IntObservableProperty _selectedIndex = new IntObservableProperty();

        public IntObservableProperty SelectedIndex => _selectedIndex;

        public IReadOnlyItemData[] Inventory => _quickInventory;
        
        public QuickInventoryData(PlayerData playerData)
        {
            _playerData = playerData;
            _quickInventory = _playerData.Inventory.GetAll();
            
            _playerData.Inventory.OnInventoryChanged += OnInventoryChanged;
        }

        public IDisposable Subscribe(Action onChanged)
        {
            OnChanged += onChanged;
            return new ActionDisposable(() => OnChanged -= onChanged);
        }
        private void OnInventoryChanged(string id, int value)
        {
            _quickInventory = _playerData.Inventory.GetAll();
            _selectedIndex.Value = Mathf.Clamp(_selectedIndex.Value, 0, _quickInventory.Length - 1);
            OnChanged?.Invoke();
        }
    }
}