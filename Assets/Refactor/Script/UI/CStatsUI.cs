using StatsData;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace UISystem
{
    internal class CStatsUI : CUIComponentBase
    {
        protected SLStatSlot[] slots;

        protected override void OnEnable()
        {
            slots = GetComponentsInChildren<SLStatSlot>();
            ui.StatsUpdate += UpdateStats;
        }

        protected override void OnDisable()
        {
            ui.StatsUpdate -= UpdateStats;
        }

        protected void Start()
        {
            foreach(var slot in slots)
            {
                slot.Init(ui);
            }
        }

        protected void UpdateStats(WReadOnlyStatsData _data)
        {
            foreach(var slot in slots)
            {
                slot.SetValue(_data);
            }
        }
    }
}
