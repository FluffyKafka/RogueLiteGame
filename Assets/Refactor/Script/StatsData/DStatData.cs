using System.Collections;
using System.Collections.Generic;
using UnityEditor.Playables;
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

        public float CheckDataByType(EStatType _type)
        {
            switch (_type)
            {
                case EStatType.MaxHealth: return maxHealth;
                case EStatType.Armor: return armor;
                case EStatType.Evasion: return evasion;
                case EStatType.MagicResistance: return magicResistance;

                case EStatType.MaxFlaskUsageTime: return maxFlaskUsageTime;
                case EStatType.FlaskUsageRecover: return flaskUsageRecover;

                case EStatType.Damage: return damage;
                case EStatType.CritChance: return critChance;
                case EStatType.CritPower: return critPower;
                case EStatType.AttackSpeed: return attackSpeed;

                case EStatType.FireDamage: return fireDamage;
                case EStatType.IceDamage: return iceDamage;
                case EStatType.LightningDamage: return lightningDamage;
                case EStatType.FireDuration: return fireDuration;
                case EStatType.IceDuration: return iceDuration;
                case EStatType.LightningDuration: return lightningDuration;

                case EStatType.FireDamageCooldown: return fireDamageCooldown;
                case EStatType.FireDamageTransform: return fireDamageTransform;
                case EStatType.ChillArmorReduce: return chillArmorReduce;
                case EStatType.ChillSlowRate: return chillSlowRate;
                case EStatType.ShockAccuracyReduce: return shockAccuracyReduce;
                case EStatType.ThunderStrikeCount: return thunderStrikeCount;
                case EStatType.ThunderStrikeRate: return thunderStrikeRate;
                case EStatType.ThunderStrikeRadius:return thunderStrikeRadius;

                default: return -1;
            }
        }
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
