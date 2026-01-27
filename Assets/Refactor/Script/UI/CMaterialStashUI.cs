using Item;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace UISystem
{
    internal class CMaterialStashUI : CUIComponentBase
    {
        protected List<SLMaterialStashSlot> stash;
        [SerializeField] protected GameObject stashPrefab;
        [SerializeField] protected GameObject slotContainer;

        protected override void OnEnable()
        {
            ui.MaterialStashChange += StashUpdate;
        }

        protected override void OnDisable()
        {
            ui.MaterialStashChange -= StashUpdate;
        }

        protected virtual void Start()
        {
            int maxMaterialStashSize = ui.InvokeFunc(ui.CheckMaterialStashMaxSize);
            stash = new List<SLMaterialStashSlot>(maxMaterialStashSize);
            for (int i = 0; i < maxMaterialStashSize; ++i)
            {
                SLMaterialStashSlot slot = Instantiate(stashPrefab, slotContainer.transform).GetComponent<SLMaterialStashSlot>();
                stash.Add(slot);
            }
        }

        protected void StashUpdate(IReadOnlyList<IItem> _stash)
        {
            ClearSlots();
            for (int i = 0; i < _stash.Count; ++i)
            {
                stash[i].DisplayItem(_stash[i]);
            }
        }

        protected void ClearSlots()
        {
            foreach (var slot in stash)
            {
                slot.Clear();
            }
        }
    }
}
