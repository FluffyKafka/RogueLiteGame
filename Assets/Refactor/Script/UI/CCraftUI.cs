using Item;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace UISystem
{
    internal class CCraftUI : CUIComponentBase
    {
        [SerializeField] protected Button weaponListButton;
        [SerializeField] protected Button armorListButton;
        [SerializeField] protected Button amuletListButton;
        [SerializeField] protected Button flaskListButton;
        [Space]
        [SerializeField] protected SLCraftChoiceSlot slotPrefab;
        [SerializeField] protected Transform slotParent;
        protected List<SLCraftChoiceSlot> slots = new();

        protected override void OnEnable()
        {
            base.OnEnable();
            weaponListButton.onClick.AddListener(() => { ShowList(EEquipmentType.Weapon); });
            armorListButton.onClick.AddListener(() => { ShowList(EEquipmentType.Armor); });
            amuletListButton.onClick.AddListener(() => { ShowList(EEquipmentType.Amulet); });
            flaskListButton.onClick.AddListener(() => { ShowList(EEquipmentType.Flask); });
        }

        protected override void OnDisable()
        {
            base.OnDisable();
            weaponListButton.onClick.RemoveAllListeners();
            armorListButton.onClick.RemoveAllListeners();
            amuletListButton.onClick.RemoveAllListeners();
            flaskListButton.onClick.RemoveAllListeners();
        }

        protected void ShowList(EEquipmentType _type)
        {
            IReadOnlyList<IEquipmentData> choiceList = ui.InvokeFunc(ui.CheckCraftableEquipmentByType, _type);
            GenerateSlotTo(choiceList.Count);
            HideAllSlot();
            for(int i = 0; i < choiceList.Count; ++i)
            {
                slots[i].DisplayItem(choiceList[i]);
            }
        }
        protected void HideAllSlot()
        {
            foreach (var slot in slots)
            {
                slot.gameObject.SetActive(false);
            }
        }

        protected void GenerateSlotTo(int _count)
        {
            while (slots.Count < _count)
            {
                slots.Add(Instantiate(slotPrefab, slotParent).GetComponent<SLCraftChoiceSlot>());
            }
        }
    }
}
