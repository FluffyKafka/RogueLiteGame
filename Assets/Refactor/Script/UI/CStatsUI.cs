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
            base.OnEnable();
            slots = GetComponentsInChildren<SLStatSlot>();
        }

        protected override void OnDisable()
        {
            base.OnDisable();
        }
    }
}
