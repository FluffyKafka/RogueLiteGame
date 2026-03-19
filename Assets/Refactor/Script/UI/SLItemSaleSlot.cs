using PlayerSystem;
using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

namespace UISystem
{
    internal class SLItemSaleSlot : CUIComponentBase, IPointerClickHandler
    {
        [SerializeField] protected Image itemIcon;
        [SerializeField] protected TextMeshProUGUI itemName;

        protected DItemForSaleToUi itemForSale;
        protected Action<DItemForSaleToUi> ShowItemDetail;

        public void SetItemForSale(DItemForSaleToUi _item, Action<DItemForSaleToUi> _showItemDetailAction)
        {
            itemForSale = _item;
            itemIcon.sprite = _item.item.CheckIcon();
            itemName.text = _item.item.CheckItemName();
            ShowItemDetail = _showItemDetailAction;
            gameObject.SetActive(true);
        }

        public void Hide()
        {
            gameObject.SetActive(false);
        }

        public void OnPointerClick(PointerEventData eventData)
        {
            ShowItemDetail.Invoke(itemForSale);
        }
    }
}

