using EntitySystem.EntityActor;
using StatsData;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace StatsSystem
{
    internal class CPlayerFlaskStats : CEntityStatsComponentBase
    {
        [SerializeField] protected DStat maxFlaskUsageTime;
        [SerializeField] protected DStat flaskUsageRecover;
        protected override void Awake()
        {
            base.Awake();
            statsManager.AddModifier += AddModifier;
            statsManager.RemoveModifier += RemoveModifier;
        }

        protected void AddModifier(WReadOnlyStatsData _data)
        {
            if (_data.data.maxFlaskUsageTime > 0)
                maxFlaskUsageTime.AddAdder(_data.data.maxFlaskUsageTime);
            if (_data.data.flaskUsageRecover > 0)
                flaskUsageRecover.AddAdder(_data.data.flaskUsageRecover);
        }
        protected void RemoveModifier(WReadOnlyStatsData _data)
        {
            if (_data.data.maxFlaskUsageTime > 0)
                maxFlaskUsageTime.RemoveAdder(_data.data.maxFlaskUsageTime);
            if (_data.data.flaskUsageRecover > 0)
                flaskUsageRecover.RemoveAdder(_data.data.flaskUsageRecover);
        }
    }
}