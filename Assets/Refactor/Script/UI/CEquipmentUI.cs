using Item;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace UISystem
{
    internal class CEquipmentUI : CUIComponentBase
    {
        protected SLEquipmentSlot[] slots;

        protected override void OnEnable()
        {
            base.OnEnable();
            slots = GetComponentsInChildren<SLEquipmentSlot>();

            ui.EquipmentChange += EquipmentChange;
        }

        protected override void OnDisable()
        {
            base.OnDisable();

            ui.EquipmentChange -= EquipmentChange;
        }

        protected void EquipmentChange(EEquipmentType _type, IEquipment _data)
        {
            foreach(var slot in slots)
            {
                if(slot.CheckType() == _type)
                {
                    slot.UpdateSlot(_data);
                }
            }
        }
    }
}

