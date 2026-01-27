using InventorySystem;
using Item;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

namespace UISystem
{
    internal class SLMaterialStashSlot : CUIComponentBase, IPointerDownHandler, IPointerEnterHandler, IPointerExitHandler
    {
        protected IItem item = null;
        protected Image itemImage;

        protected override void OnEnable()
        {
            itemImage = GetComponent<Image>();
        }

        public virtual void DisplayItem(IItem _item)
        {
            item = _item;
            if (item != null)
            {
                gameObject.SetActive(true);
                itemImage.color = Color.white;
                itemImage.sprite = item.CheckData().CheckIcon();
            }
        }

        public virtual void Clear()
        {
            itemImage.sprite = null;
            itemImage.color = Color.clear;
            item = null;
        }
        public virtual void OnPointerDown(PointerEventData _eventData)
        {
            if (item == null)
            {
                return;
            }
            if (_eventData.button == PointerEventData.InputButton.Right)
            {
                ui.InvokeAction(ui.DropItem, item);
            }
        }

        public void OnPointerEnter(PointerEventData _eventData)
        {
            if (item == null)
            {
                return;
            }
            ui.InvokeAction(ui.ShowMaterialDetail, item.CheckData());
        }

        public void OnPointerExit(PointerEventData _eventData)
        {
            ui.InvokeAction(ui.HideTooltip);
        }
    }
}
