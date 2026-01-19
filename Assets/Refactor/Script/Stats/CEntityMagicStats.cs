using EntitySystem.EntityActor;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Assertions;

namespace StatsSystem
{
    internal class CEntityMagicStats : CEntityStatsComponentBase
    {
        [SerializeField] protected DStat fireDamage;
        [SerializeField] protected DStat iceDamage;
        [SerializeField] protected DStat lightningDamage;
        [SerializeField] protected DStat fireDuration;
        [SerializeField] protected DStat iceDuration;
        [SerializeField] protected DStat lightningDuration;
        [SerializeField] protected DStat fireDamageCooldown;
        [SerializeField] protected DStat fireDamageTransform;
        [SerializeField] protected DStat chillArmorReduce;
        [SerializeField] protected DStat chillSlowRate;
        [SerializeField] protected DStat shockAccuracyReduce;
        [SerializeField] protected DStat thunderStrikeCount;
        [SerializeField] protected DStat thunderStrikeRate;
        [SerializeField] protected DStat thunderStrikeRadius;

        protected override void Awake()
        {
            base.Awake();
            statsManager.CalculatePrimaryAttackData += CalculateMagicDamage;
        }

        protected void CalculateMagicDamage(DDamageData _damage)
        {
            float fire = fireDamage.GetValue();
            float ice = iceDamage.GetValue();
            float lightning = lightningDamage.GetValue();

            _damage.magical = Mathf.Max(fire, ice, lightning);

            bool ignite = fire == _damage.magical;
            bool chill = ice == _damage.magical;
            bool shock = lightning == _damage.magical;

            int sum = 0;
            while (true)
            {
                if (ignite) ++sum;
                if (chill) ++sum;
                if (shock) ++sum;
                if (sum > 1)
                {
                    int rd = UnityEngine.Random.Range(0, 3);
                    if (rd == 0) ignite = false;
                    else if (rd == 1) chill = false;
                    else if (rd == 2) shock = false;
                    sum = 0;
                }
                else
                {
                    break;
                }
            }

            if (ignite)
            {
                _damage.ignite = true;
                _damage.igniteDamage = fire * fireDamageTransform.GetValue();
                _damage.igniteDamageCooldown = fireDamageCooldown.GetValue();
                _damage.igniteDuration = fireDuration.GetValue();
            }
            if (chill)
            {
                _damage.chill = true;
                _damage.chillDuration = iceDuration.GetValue();
                _damage.chillReduceArmorPer = chillArmorReduce.GetValue();
                _damage.chillSlowPercentage = chillSlowRate.GetValue();
            }
            if (shock)
            {
                _damage.shock = true;
                _damage.shockDuration = lightningDuration.GetValue();
                _damage.thunderStrikeRadius = thunderStrikeRadius.GetValue();
                _damage.thunderStrikeRate = thunderStrikeRate.GetValue();
                _damage.thunderStrikeCounter = (int)thunderStrikeCount.GetValue();
                _damage.shockReduceAccuracy = shockAccuracyReduce.GetValue();
            }
        }
        protected float CheckStats(EStatType _type)
        {
            switch (_type)
            {
                case EStatType.FireDamage: return fireDamage.GetValue();
                case EStatType.IceDamage: return iceDamage.GetValue();
                case EStatType.LightningDamage: return lightningDamage.GetValue();
                case EStatType.FireDuration: return fireDamage.GetValue();
                case EStatType.IceDuration: return iceDuration.GetValue();
                case EStatType.LightningDuration: return lightningDuration.GetValue();
                case EStatType.FireDamageCooldown: return fireDamageCooldown.GetValue();
                case EStatType.FireDamageTransform: return fireDamageTransform.GetValue();
                case EStatType.ChillArmorReduce: return chillArmorReduce.GetValue();
                case EStatType.ChillSlowRate: return chillSlowRate.GetValue();
                case EStatType.ShockAccuracyReduce: return shockAccuracyReduce.GetValue();
                case EStatType.ThunderStrikeCount: return thunderStrikeCount.GetValue();
                case EStatType.ThunderStrikeRate: return thunderStrikeRate.GetValue();
                case EStatType.ThunderStrikeRadius: return thunderStrikeRadius.GetValue();
                default: Assert.IsTrue(false, _type.ToString() + "不是魔法型数值"); return 0;
            }
        }
    }
}
