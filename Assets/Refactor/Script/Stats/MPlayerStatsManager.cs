using PlayerSystem;
using StatsData;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Assertions;

namespace StatsSystem
{
    public interface ISaveStats
    {
        public class DStatsData
        {
            public float hp = -1;
        }
        public void Save(ref DStatsData _data);
        public void Load(DStatsData _data);
    }

    internal class MPlayerStatsManager : MEntityStatsManager, ISaveStats
    {
        protected IStatsPlayer player;

        public Func<EStatType, float> CheckFlaskStat;

        protected override void Awake()
        {
            base.Awake();
            player = GetComponentInParent<IStatsPlayer>();
            Assert.IsNotNull(player, "MPlayerStatsManager组件必须附加至一个Player");

            AddModifier += (WReadOnlyStatsData _data) => { player.StatsChangeNotice(); };
            RemoveModifier += (WReadOnlyStatsData _data) => { player.StatsChangeNotice(); };
        }

        protected override void Start()
        {
            base.Start();
            player.StatsChangeNotice();
        }

        protected override void ComponentValidCheck()
        {
            base.ComponentValidCheck();
            Assert.IsNotNull(GetComponent<CPlayerFlaskStats>(), "缺少药物数值组件");
        }

        public override float TryCheckStat(EStatType _type)
        {
            float stat = base.TryCheckStat(_type);
            if (!float.IsNaN(stat)) return stat;
            stat = InvokeFunc(CheckFlaskStat, _type);
            if (!float.IsNaN(stat)) return stat;
            return float.NaN;
        }

        public void Save(ref ISaveStats.DStatsData _data)
        {
            _data.hp = currentHealth;
        }

        public void Load(ISaveStats.DStatsData _data)
        {
            currentHealth = _data.hp;
        }
    }
}