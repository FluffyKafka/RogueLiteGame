using Item;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

namespace UISystem
{
    internal class SLEquipmentStashSlot : CUIComponentBase, IPointerDownHandler, IPointerEnterHandler, IPointerExitHandler
    {
        protected Image itemImage;
        IEquipment equipment = null;

        protected override void OnEnable()
        {
            itemImage = GetComponent<Image>();
        }

        public virtual void DisplayItem(IEquipment _equip)
        {
            equipment = _equip;
            if (_equip != null)
            {
                gameObject.SetActive(true);
                itemImage.color = Color.white;
                itemImage.sprite = _equip.CheckEquipmentData().CheckIcon();
            }
        }

        public virtual void Clear()
        {
            itemImage.sprite = null;
            itemImage.color = Color.clear;
            equipment = null;
        }
        
        public virtual void OnPointerDown(PointerEventData _eventData)
        {
            if (equipment == null)
            {
                return;
            }
            if (_eventData.button == PointerEventData.InputButton.Left)
            {
                ui.PlayEquipSFX(true);
                ui.InvokeAction(ui.Equip, equipment);
            }
            else if (_eventData.button == PointerEventData.InputButton.Right)
            {
                ui.InvokeAction(ui.DropItem, equipment);
            }
        }

        public void OnPointerEnter(PointerEventData _eventData)
        {
            if (equipment == null)
            {
                return;
            }
            ui.InvokeAction(ui.ShowEquipmentDetail, (equipment).CheckEquipmentData());
        }

        public void OnPointerExit(PointerEventData _eventData)
        {
            ui.InvokeAction(ui.HideTooltip);
        }
    }
}