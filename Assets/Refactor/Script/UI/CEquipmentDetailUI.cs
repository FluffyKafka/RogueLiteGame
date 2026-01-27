using Item;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

namespace UISystem
{
    internal class CEquipmentDetailUI : CItemDetailUI
    {
        [SerializeField] protected TextMeshProUGUI itemTypeText;
        [SerializeField] protected TextMeshProUGUI itemEffectText;
        [SerializeField] protected Transform statsSlotParent;
        protected SLStatSlot[] stats;

        protected override void OnEnable()
        {
            base.OnEnable();
            if(stats.Length == 0)
            {
                stats = GetComponentsInChildren<SLStatSlot>();
                foreach (var stat in stats)
                {
                    stat.Init(ui);
                }
            }
        }

        protected override void OnDisable()
        {
            base.OnDisable();
            foreach (var stat in stats)
            {
                stat.gameObject.SetActive(true);
            }
        }

        public override void ShowDetail(IItemData _item)
        {
            base.ShowDetail(_item);

            IEquipmentData equip = _item as IEquipmentData;

            foreach(var stat in stats)
            {
                float modifyValue = equip.CheckStatsModifierData().data.CheckDataByType(stat.CheckType());
                if(modifyValue == 0)
                {
                    stat.gameObject.SetActive(false);
                }
                else
                {
                    stat.SetValue(equip.CheckStatsModifierData());
                }
            }

            itemTypeText.text = ui.InvokeFunc(ui.Translate, equip.CheckEquipmentType().ToString());
            itemEffectText.text = equip.CheckEffectText();
        }
    }
}
