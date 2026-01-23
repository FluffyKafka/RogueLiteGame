using Item;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Assertions;
using UnityEngine.EventSystems;
using UnityEngine.UI;

namespace UISystem
{
    internal class SLEquipmentSlot : MonoBehaviour, IPointerDownHandler, IPointerEnterHandler, IPointerExitHandler
    {
        [SerializeField] protected EEquipmentType equipmentType;

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
            Assert.IsTrue(_newEquipment.CheckEquipmentData().CheckEquipmentType() == equipmentType, "装备被放入错误的槽位");
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
                //使用物品逻辑（脱装备）
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