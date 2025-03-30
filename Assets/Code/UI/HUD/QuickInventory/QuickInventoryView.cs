using System.Collections.Generic;
using PixelCrew.Model;
using PixelCrew.UI.Widgets;
using PixelCrew.Utils.Disposables;
using UnityEngine;

namespace PixelCrew.UI.HUD.QuickInventory
{
    public class QuickInventoryView : MonoBehaviour
    {
        [SerializeField] private Transform _itemsContainer;
        [SerializeField] private InventoryItemWidget _itemPrefab;

        private GameSession _gameSession;
        private List<InventoryItemWidget> _createdItems;
        private CompositeDisposable _trash = new CompositeDisposable();
        private void Awake()
        {
            _gameSession = FindObjectOfType<GameSession>();
            _trash.Retain(_gameSession.QuickInventory.Subscribe(UpdateInventory));
            _createdItems = new List<InventoryItemWidget>(8);
            UpdateInventory();
            _trash.Retain(_gameSession.QuickInventory.SelectedIndex
                .SubscribeAndInvoke(OnSelectedIndexChanged));
        }

        private void UpdateInventory()
        {
            var inventoryData = _gameSession.QuickInventory.Inventory;

            for (int i = _createdItems.Count; i < inventoryData.Length; i++)
            {
                var item = Instantiate(_itemPrefab, _itemsContainer);
                _createdItems.Add(item);
            }

            for (int i = 0; i < inventoryData.Length; i++)
            {
                _createdItems[i].SetData(inventoryData[i], i);
                _createdItems[i].gameObject.SetActive(true);
            }

            for (int i = inventoryData.Length; i < _createdItems.Count; i++)
            {
                _createdItems[i].gameObject.SetActive(false);
            }
        }

        private void OnSelectedIndexChanged(int newValue)
        {
            foreach (var item in _createdItems)
            {
                if (!item.isActiveAndEnabled) return;
                item.OnSelected(item.Index == newValue);
            }
        }
 
        private void OnDestroy()
        {
            _trash.Dispose();
        }
    }
}