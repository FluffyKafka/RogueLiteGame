using InventorySystem;
using Item;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

namespace UISystem
{
    internal class SLMaterialStashSlot : MonoBehaviour, IPointerDownHandler, IPointerEnterHandler, IPointerExitHandler
    {
        protected Image itemImage;
        protected IItem material = null;
        protected MUIManager ui;

        protected void OnEnable()
        {
            itemImage = GetComponent<Image>();
        }

        public void Init(MUIManager _ui)
        {
            ui = _ui;
        }

        public void UpdateSlot(IItem _newEquipment)
        {
            material = _newEquipment;

            if (material != null)
            {
                itemImage.color = Color.white;
                itemImage.sprite = material.CheckData().CheckIcon();
            }
        }

        public void Clear()
        {
            material = null;
            itemImage.sprite = null;
            itemImage.color = Color.clear;
        }

        public bool IsNull()
        {
            return material == null;
        }

        public virtual void OnPointerDown(PointerEventData _eventData)
        {
            if (material == null)
            {
                return;
            }
            if (_eventData.button == PointerEventData.InputButton.Right)
            {
                ui.InvokeAction(ui.dropItem, material);
            }
        }

        public void OnPointerEnter(PointerEventData _eventData)
        {
            if (material == null)
            {
                return;
            }
            ui.InvokeAction(ui.ShowMaterialDetail, material.CheckData());
        }

        public void OnPointerExit(PointerEventData _eventData)
        {
            ui.InvokeAction(ui.HideTooltip);
        }
    }
}
