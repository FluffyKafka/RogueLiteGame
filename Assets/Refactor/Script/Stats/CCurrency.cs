using PlayerBebaviour;
using StatsSystem;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace StatsSystem
{
    internal class CCurrency : CEntityStatsComponentBase
    {
        [Header("Test")]
        [SerializeField] protected float coin;
        [SerializeField] protected float soul;

        MPlayerStatsManager playerStats;

        protected override void Awake()
        {
            base.Awake();
     
            playerStats = statsManager as MPlayerStatsManager;

            playerStats.SetCoinNotice += SetCoin;
            playerStats.CheckCoinNotice += CheckCoin;
            playerStats.SetSoulNotice += SetSoul;
            playerStats.CheckSoulNotice += CheckSoul;
        }

        protected void SetCoin(float _coin)
        {
            coin = _coin;
            playerStats.ToCoinChange(coin);
        }
        protected float CheckCoin()
        {
            return coin;
        }

        protected void SetSoul(float _soul)
        {
            soul = _soul;
            playerStats.ToSoulChange(soul);
        }
        protected float CheckSoul()
        {
            return soul;
        }
    }
}

