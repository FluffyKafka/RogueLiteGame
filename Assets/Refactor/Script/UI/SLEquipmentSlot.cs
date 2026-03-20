using Item;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Assertions;
using UnityEngine.EventSystems;
using UnityEngine.UI;

namespace UISystem
{
    internal class SLEquipmentSlot : CUIComponentBase, IPointerDownHandler, IPointerEnterHandler, IPointerExitHandler
    {
        [SerializeField] protected EEquipmentType equipmentType;

        protected Image itemImage;
        protected IEquipment equipment = null;

        protected override void OnEnable()
        {
            base.OnEnable();
            itemImage = GetComponent<Image>();
            UpdateSlot(ui.InvokeFunc(ui.CheckEquipmentByType, equipmentType));
        }

        public void UpdateSlot(IEquipment _newEquipment)
        {
            Assert.IsTrue(_newEquipment == null || _newEquipment.CheckEquipmentData().CheckEquipmentType() == equipmentType, "装备被放入错误的槽位");
            equipment = _newEquipment;

            if (equipment != null)
            {
                itemImage.color = Color.white;
                itemImage.sprite = equipment.CheckData().CheckIcon();
            }
            else
            {
                Clear();
            }
        }

        public void Clear()
        {
            equipment = null;
            itemImage.sprite = null;
            itemImage.color = Color.clear;
        }

        public bool IsNull()
        {
            return equipment == null;
        }

        public EEquipmentType CheckType()
        {
            return equipmentType;
        }

        public virtual void OnPointerDown(PointerEventData _eventData)
        {
            if (equipment == null)
            {
                return;
            }
            if (_eventData.button == PointerEventData.InputButton.Left)
            {
                ui.InvokeAction(ui.UnEquip, equipment);
            }
            else if (_eventData.button == PointerEventData.InputButton.Right)
            {
                ui.PlayDiscardInventorySFX(true);
                ui.InvokeAction(ui.DropItem, equipment);
            }
        }

        public void OnPointerEnter(PointerEventData _eventData)
        {
            if (equipment == null)
            {
                return;
            }
            ui.InvokeAction(ui.ShowEquipmentDetail, equipment.CheckEquipmentData());
        }

        public void OnPointerExit(PointerEventData _eventData)
        {
            ui.InvokeAction(ui.HideTooltip);
        }
    }
}