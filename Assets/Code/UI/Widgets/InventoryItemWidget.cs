using System;
using PixelCrew.Model.Data.Inventory;
using PixelCrew.Model.Definitions;
using UnityEngine;
using UnityEngine.UI;

namespace PixelCrew.UI.Widgets
{
    public class InventoryItemWidget : MonoBehaviour
    {
        [SerializeField] private Image _selection;
        [SerializeField] private Image _icon;
        [SerializeField] private Text _value;

        private int _index;
        public int Index => _index;

        public void SetData(IReadOnlyItemData itemData, int index)
        {  
            ItemDef itemDef = DefsFacade.I.ItemsDef.Get(itemData.GetId());
            _icon.sprite = itemDef.Icon;
            _value.text = itemDef.IsStackable ? itemData.GetValue().ToString() : String.Empty;
            _index = index;
        }
        
        public void OnSelected(bool selected)
        {
            _selection.gameObject.SetActive(selected);
        }
    }
}