using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

namespace StatsData
{
    public class DDamageData
    {
        public Transform damageSourceTransform = null;
        public bool shouldPlayAnim = true;
        public float physical = 0;
        public bool isCrit = false;
        public float magical = 0;
        public bool ignite = false;
        public float igniteDamageCooldown = float.PositiveInfinity;
        public float igniteDuration = 0f;
        public float igniteDamage = 0f;
        public bool chill = false;
        public float chillSlowPercentage = 0f;
        public float chillDuration = 0f;
        public float chillReduceArmorPer = 0f;
        public bool shock = false;
        public float shockDuration = 0f;
        public float thunderStrikeRadius = 0f;
        public float thunderStrikeRate = 0f;
        public int thunderStrikeCounter = 0;
        public float shockReduceAccuracy = 0f;
    }
    public struct WReadOnlyDamageData
    {
        public DDamageData data { get; private set; }
        public WReadOnlyDamageData(DDamageData _damageData)
        {
            data = _damageData;
        }
        public DDamageData Clone()
        {
            DDamageData newData = new DDamageData
            {
                damageSourceTransform = data.damageSourceTransform, 
                shouldPlayAnim = data.shouldPlayAnim,
                physical = data.physical,
                isCrit = data.isCrit,
                magical = data.magical,
                ignite = data.ignite,
                igniteDamageCooldown = data.igniteDamageCooldown,
                igniteDuration = data.igniteDuration,
                igniteDamage = data.igniteDamage,
                chill = data.chill,
                chillSlowPercentage = data.chillSlowPercentage,
                chillDuration = data.chillDuration,
                chillReduceArmorPer = data.chillReduceArmorPer,
                shock = data.shock,
                shockDuration = data.shockDuration,
                thunderStrikeRadius = data.thunderStrikeRadius,
                thunderStrikeRate = data.thunderStrikeRate,
                thunderStrikeCounter = data.thunderStrikeCounter,
                shockReduceAccuracy = data.shockReduceAccuracy
            };
            return newData;
        }
    }
}
