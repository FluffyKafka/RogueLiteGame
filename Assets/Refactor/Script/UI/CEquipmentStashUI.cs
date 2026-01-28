using Item;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

namespace UISystem
{
    internal class CEquipmentStashUI : CUIComponentBase
    {
        protected List<SLEquipmentStashSlot> stash;
        [SerializeField] protected SLEquipmentStashSlot stashPrefab;
        [SerializeField] protected Transform slotContainer;

        protected override void OnEnable()
        {
            ui.EquipmentStashChange += StashUpdate;
            StashUpdate(ui.InvokeFunc(ui.CheckEquipmentStash));
        }

        protected override void OnDisable()
        {
            ui.EquipmentStashChange -= StashUpdate;
        }

        protected void StashUpdate(IReadOnlyList<IEquipment> _stash)
        {
            StashInitIfNull();
            ClearSlots();
            for(int i = 0; i < _stash.Count; ++i)
            {
                stash[i].DisplayItem(_stash[i]);
            }
        }

        protected void ClearSlots()
        {
            foreach(var slot in stash)
            {
                slot.Clear();
            }
        }

        protected void StashInitIfNull()
        {
            if(stash == null)
            {
                int maxEquipmentStashSize = ui.InvokeFunc(ui.CheckEquipmentStashMaxSize);
                stash = new List<SLEquipmentStashSlot>(maxEquipmentStashSize);
                for (int i = 0; i < maxEquipmentStashSize; ++i)
                {
                    SLEquipmentStashSlot slot = Instantiate(stashPrefab, slotContainer.transform).GetComponent<SLEquipmentStashSlot>();
                    stash.Add(slot);
                }
            }
        }
    }
}