using Item;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
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
            if(stats == null)
            {
                stats = GetComponentsInChildren<SLStatSlot>();
            }          
        }

        protected override void OnDisable()
        {
            base.OnDisable();
        }

        public override void ShowDetail(IItemData _item)
        {
            base.ShowDetail(_item);
            IEquipmentData equip = _item as IEquipmentData;

            foreach (var stat in stats)
            {
                float modifyValue = equip.CheckStatsModifierData().data.CheckDataByType(stat.CheckType());
                if(modifyValue == 0)
                {
                    stat.gameObject.SetActive(false);
                }
                else
                {
                    stat.gameObject.SetActive(true);
                    stat.SetValue(equip.CheckStatsModifierData());
                }
            }

            itemTypeText.text = ui.InvokeFunc(ui.Translate, equip.CheckEquipmentType().ToString());
            itemEffectText.text = equip.CheckEffectText();
        }
    }
}
