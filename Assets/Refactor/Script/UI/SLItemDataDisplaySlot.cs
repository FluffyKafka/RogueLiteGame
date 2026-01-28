using Item;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

namespace UISystem
{
    internal class SLItemDataDisplaySlot : CUIComponentBase
    {
        [SerializeField] protected Image itemImage;
        protected IItemData item;

        public virtual void DisplayItem(IItemData _itemData)
        {
            item = _itemData;

            if (_itemData != null)
            {
                gameObject.SetActive(true);
                itemImage.color = Color.white;
                itemImage.sprite = _itemData.CheckIcon();
            }
        }

        public virtual void Clear()
        {
            itemImage.sprite = null;
            itemImage.color = Color.clear;
            item = null;
        } 
    }
}

