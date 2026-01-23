using Item;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

namespace UISystem
{
    internal class SLEquipmentStashSlot : MonoBehaviour, IPointerDownHandler, IPointerEnterHandler, IPointerExitHandler
    {
        protected Image itemImage;
        protected IEquipment equipment = null;
        protected MUIManager ui;

        protected void OnEnable()
        {
            itemImage = GetComponent<Image>();
        }

        public void Init(MUIManager _ui)
        {
            ui = _ui;
        }

        public void UpdateSlot(IEquipment _newEquipment)
        {
            equipment = _newEquipment;

            if (equipment != null)
            {
                itemImage.color = Color.white;
                itemImage.sprite = equipment.CheckData().CheckIcon();
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

        public virtual void OnPointerDown(PointerEventData _eventData)
        {
            if (equipment == null)
            {
                return;
            }
            if (_eventData.button == PointerEventData.InputButton.Left)
            {
                //使用物品逻辑（穿装备）
            }
            else if (_eventData.button == PointerEventData.InputButton.Right)
            {
                //丢弃物品逻辑
            }
        }

        public void OnPointerEnter(PointerEventData _eventData)
        {
            if (equipment == null)
            {
                return;
            }
            //显示物品详情逻辑
        }

        public void OnPointerExit(PointerEventData _eventData)
        {
            //取消显示物品详情逻辑
        }
    }
}