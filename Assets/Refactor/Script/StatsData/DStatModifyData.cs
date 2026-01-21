using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace StatsData
{
    //此数据结构容纳所有属性值，不同的数值插件管理其中不同部分
    [System.Serializable]
    public class DStatsData
    {
        [Header("Defensive Stats")]
        public float maxHealth;
        public float armor;
        public float evasion;
        public float magicResistance;

        [Header("Offensive Stats")]
        public float damage;
        public float critChance;
        public float critPower;
        public float attackSpeed;


        [Header("Magic Stats")]
        public float fireDamage;
        public float iceDamage;
        public float lightningDamage;
        public float fireDuration;
        public float iceDuration;
        public float lightningDuration;
        public float fireDamageCooldown;
        public float fireDamageTransform;
        public float chillArmorReduce;
        public float chillSlowRate;
        public float shockAccuracyReduce;
        public float thunderStrikeCount;
        public float thunderStrikeRate;
        public float thunderStrikeRadius;

        [Header("Player Flask Stats")]
        public float maxFlaskUsageTime;
        public float flaskUsageRecover;
    }
    public struct WReadOnlyStatsData
    {
        public DStatsData data { get; private set; }
        public WReadOnlyStatsData(DStatsData _data)
        {
            data = _data;
        }
    }
}
