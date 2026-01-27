using Item;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.EventSystems;

namespace UISystem
{
    internal class SLCraftChoiceSlot : SLItemDataDisplaySlot, IPointerDownHandler, IPointerUpHandler
    {
        [SerializeField] protected Color pressColor;
        [SerializeField] protected TextMeshProUGUI equipName;

        public override void DisplayItem(IItemData _itemData)
        {
            base.DisplayItem(_itemData);
            equipName.text = _itemData.CheckItemName();
        }

        public void OnPointerDown(PointerEventData _eventData)
        {
            ui.InvokeAction(ui.ShowCraftWindow, item as IEquipmentData);
            itemImage.color = pressColor;
        }
        public void OnPointerUp(PointerEventData _eventData)
        {
            itemImage.color = Color.white;
        }
    }
}

