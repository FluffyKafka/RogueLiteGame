using Item;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace UISystem
{
    internal class CCraftMaterialBlockUI : CUIComponentBase
    {
        [SerializeField] protected SLCraftMaterialSlot displaySlotPrefab;
        [SerializeField] protected Transform slotContainer;

        protected List<SLCraftMaterialSlot> slots = new();

        public void DisplayEquipmentCraftMaterials(IReadOnlyList<IItemData> _data)
        {
            GenerateSlotTo(_data.Count);
            HideAllSlot();
            for(int i = 0; i < _data.Count; ++i)
            {
                slots[i].DisplayItem(_data[i]);
            }
        }

        protected void HideAllSlot()
        {
            foreach(var slot in slots)
            {
                slot.gameObject.SetActive(false);
            }
        }

        protected void GenerateSlotTo(int _count)
        {
            while(slots.Count < _count)
            {
                slots.Add(Instantiate(displaySlotPrefab.gameObject, slotContainer).GetComponent<SLCraftMaterialSlot>());
            }
        }
    }
}
