using EntitySystem.EntityActor.PlayerActor;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Assertions;

namespace StatsSystem
{
    internal class MPlayerStatsManager : MEntityStatsManager
    {
        protected IStatsPlayer player;

        protected override void Awake()
        {
            base.Awake();
            player = GetComponentInParent<IStatsPlayer>();
            Assert.IsNotNull(player, "MPlayerStatsManager组件必须附加至一个Player");
            AddModifier += player.StatsChangeNotice;
            RemoveModifier += player.StatsChangeNotice;
        }

        protected override void ComponentValidCheck()
        {
            base.ComponentValidCheck();
            Assert.IsNotNull(GetComponent<CPlayerFlaskStats>(), "缺少药物数值组件");
        }
    }
}