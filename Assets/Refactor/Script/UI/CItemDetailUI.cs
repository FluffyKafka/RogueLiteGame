using Item;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;
using static UnityEditor.Progress;

namespace UISystem
{
    internal class CItemDetailUI : CUIComponentBase
    {
        [SerializeField] protected TextMeshProUGUI itemNameText;
        [SerializeField] protected TextMeshProUGUI itemDescriptionText;
        [SerializeField] protected Image iconImage;
        [SerializeField] protected TextMeshProUGUI itemPriceText;

        protected override void OnEnable()
        {
            base.OnEnable();
        }

        protected override void OnDisable()
        {
            base.OnDisable();
        }
        public virtual void ShowDetail(IItemData _item)
        {
            if (_item != null)
            {
                gameObject.SetActive(true);

                itemNameText.text = _item.CheckItemName();
                itemDescriptionText.text = _item.CheckDescription();
                iconImage.sprite = _item.CheckIcon();
                itemPriceText.text = _item.CheckPrice().ToString();               
            }
        }

        public virtual void SetPriceRaw(float _price)
        {
            itemPriceText.text = _price.ToString();
        }

        public void HideToolTip()
        {
            gameObject.SetActive(false);
        }
    }
}
