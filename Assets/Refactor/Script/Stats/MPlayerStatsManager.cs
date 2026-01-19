using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Assertions;

namespace StatsSystem
{
    internal class MPlayerStatsManager : MEntityStatsManager
    {
        protected override void ComponentValidCheck()
        {
            base.ComponentValidCheck();
            Assert.IsNotNull(GetComponent<CPlayerFlaskStats>(), "缺少药物数值组件");
        }
    }
}