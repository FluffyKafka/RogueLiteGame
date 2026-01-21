using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace StatsData
{
    //此枚举型列举所有属性类型，不同的数值插件管理其中不同部分
    public enum EStatType
    {
        MaxHealth,
        Armor,
        Evasion,
        MagicResistance,
        Damage,
        CritChance,
        CritPower,
        AttackSpeed,
        FireDamage,
        IceDamage,
        LightningDamage,
        FireDuration,
        IceDuration,
        LightningDuration,
        FireDamageCooldown,
        FireDamageTransform,
        ChillArmorReduce,
        ChillSlowRate,
        ShockAccuracyReduce,
        ThunderStrikeCount,
        ThunderStrikeRate,
        ThunderStrikeRadius,
        MaxFlaskUsageTime,
        FlaskUsageRecover
    }
}
