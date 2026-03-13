using Item;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace UISystem
{
    internal class SLEquipmentCooldownSlot : CUIComponentBase
    {
        [SerializeField] protected EEquipmentType type;
        [SerializeField] protected IEquipment equipment;
        [SerializeField] protected Image icon;
        [SerializeField] protected Image cooldownIcon;

        protected void Update()
        {
            if(equipment != null)
            {
                cooldownIcon.fillAmount = equipment.CheckCoolDownRestPer();
            }
        }

        public void SetEquipment(IEquipment _equip)
        {
            equipment = _equip;
            icon.sprite = _equip.CheckEquipmentData().CheckIcon();
            cooldownIcon.sprite = _equip.CheckEquipmentData().CheckIcon();
            gameObject.SetActive(true);
        }

        public void Clear()
        {
            equipment = null;
            gameObject.SetActive(false);
        }

        public EEquipmentType CheckType()
        {
            return type;
        }
    }
}

