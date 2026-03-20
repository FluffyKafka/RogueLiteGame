using PlayerSystem;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace UISystem
{
    internal class CItemSaleWindow : CUIComponentBase
    {
        [SerializeField] protected List<SLItemSaleSlot> slots;
        [SerializeField] protected CItemDetailUI itemDetailTooltip;
        [SerializeField] protected CEquipmentDetailUI equipmentDetailTooltip;
        [SerializeField] protected Button purchaseButton;
        [SerializeField] protected Button leaveButton;

        protected DItemForSaleToUi currentChosenItem;

        protected Action<DItemForSaleToUi> ChooseItemNotice;

        public void SetItemsForSale(List<DItemForSaleToUi> _items)
        {
            if(_items != null)
            {
                ChooseItemNotice += ChooseItem;
                purchaseButton.onClick.AddListener(TryPurchase);
                leaveButton.onClick.AddListener(Leave);
                for(int i = 0; i < _items.Count; ++i)
                {
                    slots[i].SetItemForSale(_items[i], ChooseItemNotice);
                }
                gameObject.SetActive(true);
                purchaseButton.gameObject.SetActive(false);
                ui.PauseGame(true);
            }
            else
            {
                ChooseItemNotice -= ChooseItem;
                purchaseButton.onClick.RemoveAllListeners();
                leaveButton.onClick.RemoveAllListeners();
                itemDetailTooltip.HideToolTip();
                equipmentDetailTooltip.HideToolTip();
                foreach (var slot in slots)
                {
                    slot.Hide();
                }
                gameObject.SetActive(false);
                ui.PauseGame(false);
            }
        }

        protected void ChooseItem(DItemForSaleToUi _item)
        {
            currentChosenItem = _item;
            purchaseButton.gameObject.SetActive(true);

            itemDetailTooltip.HideToolTip();
            equipmentDetailTooltip.HideToolTip();
            if(_item.item.CheckItemType() == Item.EItemType.Material)
            {
                itemDetailTooltip.ShowDetail(_item.item);
                itemDetailTooltip.SetPriceRaw(_item.price);
            }
            else
            {
                equipmentDetailTooltip.ShowDetail(_item.item);
                equipmentDetailTooltip.SetPriceRaw(_item.price);
            }
        }
        
        protected void TryPurchase()
        {
            if(ui.CanPurchase_coin(currentChosenItem.price))
            {
                ui.PlayBuySFX(true);
                ui.ConsumeCoin(currentChosenItem.price);
                ui.AddItemRaw(currentChosenItem.item);
                SetItemsForSale(null);
                ui.NPCEffectFinish();
            }
            else
            {
                ui.NPCEffectFail();
            }
        }
        protected void Leave()
        {
            SetItemsForSale(null);
            ui.NPCEffectFinish();
        }
    }
}