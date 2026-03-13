using Item;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace UISystem
{
    internal class CEquipmentCooldownList : CUIComponentBase
    {
        [SerializeField] protected List<SLEquipmentCooldownSlot> equipList;

        protected override void OnEnable()
        {
            base.OnEnable();
            StartCoroutine(UpdateEquipmentDelay());
        }
        protected IEnumerator UpdateEquipmentDelay()
        {
            yield return null;
            UpdateEquipmentDisplay();
        }
        protected void UpdateEquipmentDisplay()
        {
            foreach (var slot in equipList)
            {
                IEquipment equipment = ui.InvokeFunc(ui.CheckEquipmentByType, slot.CheckType());
                if(equipment != null)
                {
                    slot.SetEquipment(equipment);
                }            
            }
        }

        protected override void OnDisable()
        {
            base.OnDisable();
            foreach(var slot in equipList)
            {
                slot.Clear();
            }
        }


        protected void Equip(IEquipment _equip)
        {
            SLEquipmentCooldownSlot slot = GetSlotByType(_equip.CheckEquipmentData().CheckEquipmentType());
            slot.SetEquipment(_equip);
        }
        protected void Unequip(IEquipment _equip)
        {
            SLEquipmentCooldownSlot slot = GetSlotByType(_equip.CheckEquipmentData().CheckEquipmentType());
            slot.Clear();
        }
        protected SLEquipmentCooldownSlot GetSlotByType(EEquipmentType _type)
        {
            foreach(var slot in equipList)
            {
                if(slot.CheckType() == _type)
                {
                    return slot;
                }
            }
            return null;
        }
    }
}

