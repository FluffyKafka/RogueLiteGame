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
            public float coin;
            public float soul;
        }
        public void Save(ref DStatsData _data);
        public void Load(DStatsData _data);
    }

    internal class MPlayerStatsManager : MEntityStatsManager, ISaveStats, IPlayerStats
    {
        protected IStatsPlayer player;

        public Func<EStatType, float> CheckFlaskStat;

        #region Currency
        public Action<float> SetCoinNotice;
        public Func<float> CheckCoinNotice;
        public Action<float> SetSoulNotice;
        public Func<float> CheckSoulNotice;
        public Action<float> ConsumeSoulNotice;
        #endregion

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
            _data.coin = InvokeFunc(CheckCoinNotice);
            _data.soul = InvokeFunc(CheckSoulNotice);
        }

        public void Load(ISaveStats.DStatsData _data)
        {
            UpdateCurrentHealth(_data.hp);
            InvokeAction(SetCoinNotice, _data.coin);
            InvokeAction(SetSoulNotice, _data.soul);
        }

        public void ToCoinChange(float _coin)
        {
            player.CoinChange(_coin);
        }
        public void ToSoulChange(float _soul)
        {
            player.SoulChange(_soul);
        }
        protected override void UpdateCurrentHealth(float _current)
        {
            player.CurrentHealthChange(_current);
            base.UpdateCurrentHealth(_current);
        }

        public float CheckSoulAmount()
        {
            return InvokeFunc(CheckSoulNotice);
        }
        public void ConsumeSoul(float _soul)
        {
            InvokeAction(ConsumeSoulNotice, _soul);
        }

        public bool CanPurchase_coin(float _coin)
        {
            return InvokeFunc(CheckCoinNotice) >= _coin;
        }
        public void ConsumeCoin(float _coin)
        {
            InvokeAction(SetCoinNotice, InvokeFunc(CheckCoinNotice) - _coin);
        }
        public void AddSoul(float _soul)
        {
            InvokeAction(SetSoulNotice, InvokeFunc(CheckSoulNotice) + _soul);
        }
        public void AddCoin(float _coin)
        {
            InvokeAction(SetCoinNotice, InvokeFunc(CheckCoinNotice) + _coin);
        }
    }
}