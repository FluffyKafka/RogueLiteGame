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
        [SerializeField] protected GameObject stashPrefab;
        [SerializeField] protected GameObject slotContainer;

        protected override void OnEnable()
        {
            ui.EquipmentStashChange += StashUpdate;
        }

        protected override void OnDisable()
        {
            ui.EquipmentStashChange -= StashUpdate;
        }

        protected virtual void Start()
        {        
            int maxEquipmentStashSize = ui.InvokeFunc(ui.CheckEquipmentStashMaxSize);
            stash = new List<SLEquipmentStashSlot>(maxEquipmentStashSize);
            for(int i = 0; i < maxEquipmentStashSize; ++i)
            {
                SLEquipmentStashSlot slot = Instantiate(stashPrefab, slotContainer.transform).GetComponent<SLEquipmentStashSlot>();
                stash.Add(slot);
            }
        }

        protected void StashUpdate(IReadOnlyList<IEquipment> _stash)
        {
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
    }
}