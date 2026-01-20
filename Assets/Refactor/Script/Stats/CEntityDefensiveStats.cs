using EntitySystem.EntityActor;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Assertions;

namespace StatsSystem
{
    internal class CEntityDefensiveStats : CEntityStatsComponentBase
    {
        [SerializeField] protected DStat maxHealth;
        [SerializeField] protected DStat armor;
        [SerializeField] protected DStat evasion;
        [SerializeField] protected DStat magicResistance;

        protected override void Awake()
        {
            base.Awake();
            statsManager.CalculateFinalDamage += CalculateFinalDamage;
            statsManager.CheckDefensiveStat += CheckStats;
        }

        public virtual void CalculateFinalDamage(WReadOnlyDamageData _damageData, DDamageData _takeDamageData)
        {
            if (statsManager.InvokeFunc(statsManager.CanEnyityBeDamage))
            {
                _takeDamageData.physical = 0;
                _takeDamageData.magical = 0;
                _takeDamageData.isCrit = false;
                return;
            }

            _takeDamageData.physical = CalculatePhysicalDamageTake(_damageData);
            _takeDamageData.magical = CalculateMagicalDamageTake(_damageData);
            _takeDamageData.isCrit = _damageData.data.isCrit;
        }
        protected virtual float CalculatePhysicalDamageTake(WReadOnlyDamageData _damageData)
        {
            if (CanAvoidAttack(_damageData))
            {
                return 0;
            }

            float finalDamage = GetDamageAfterDefence(_damageData.data.physical);

            return finalDamage;
        }
        protected virtual bool CanAvoidAttack(WReadOnlyDamageData _damageData)
        {
            float totalEvasion = evasion.GetValue();
            if (Random.Range(0, 100) < totalEvasion)
            {
                return true;
            }
            return false;
        }
        protected virtual float GetDamageAfterDefence(float _damage)
        {
            float armorDefence = armor.GetValue();
            _damage -= armorDefence;
            _damage = Mathf.Clamp(_damage, 0.0f, float.MaxValue);
            return _damage;
        }
        protected virtual float CalculateMagicalDamageTake(WReadOnlyDamageData _damageData)
        {
            float finalMagicDamage = _damageData.data.magical - magicResistance.GetValue();
            finalMagicDamage = Mathf.Clamp(_damageData.data.magical, 0.0f, float.MaxValue);

            return finalMagicDamage;
        }

        protected virtual void ApplyAilment(WReadOnlyDamageData _damageData)
        {
            StartCoroutine(ChillHelper(_damageData));
        }
        protected IEnumerator ChillHelper(WReadOnlyDamageData _damageData)
        {
            armor.AddMultiplyer(1 - _damageData.data.chillReduceArmorPer * 0.01f);
            yield return new WaitForSeconds(_damageData.data.chillDuration);
            armor.RemoveMultiplyer(1 - _damageData.data.chillReduceArmorPer * 0.01f);
        }

        protected float CheckStats(EStatType _type)
        {
            switch(_type)
            {
                case EStatType.MaxHealth: return maxHealth.GetValue();
                case EStatType.Armor: return armor.GetValue();
                case EStatType.Evasion: return evasion.GetValue();
                case EStatType.MagicResistance: return magicResistance.GetValue();
                default: Assert.IsTrue(false, _type.ToString() + "不是防御型数值"); return 0;
            }
        }
    }
}