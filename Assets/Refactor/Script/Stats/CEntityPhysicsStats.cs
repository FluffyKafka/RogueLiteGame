using EntitySystem.EntityActor;
using StatsData;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Assertions;

namespace StatsSystem
{
    internal class CEntityPhysicsStats : CEntityStatsComponentBase
    {
        [SerializeField] protected DStat damage;
        [SerializeField] protected DStat critChance;
        [SerializeField] protected DStat critPower;
        [SerializeField] protected DStat attackSpeed;

        protected override void Awake()
        {
            base.Awake();
            statsManager.CalculatePrimaryAttackData += CulculatePhysicsDamage;
            statsManager.AddModifier += AddModifier;
            statsManager.RemoveModifier += RemoveModifier;
        }

        protected void CulculatePhysicsDamage(DDamageData _damage)
        {
            _damage.physical = damage.GetValue();
            if (UnityEngine.Random.Range(0, 100) < critChance.GetValue())
            {
                _damage.physical = _damage.physical * critPower.GetValue() * 0.01f;
                _damage.isCrit = true;
            }
        }

        protected float CheckStats(EStatType _type)
        {
            switch (_type)
            {
                case EStatType.Damage: return damage.GetValue();
                case EStatType.CritChance: return critChance.GetValue();
                case EStatType.CritPower: return critPower.GetValue();
                case EStatType.AttackSpeed: return attackSpeed.GetValue();
                default: Assert.IsTrue(false, _type.ToString() + "不是攻击型数值"); return 0;
            }
        }

        protected void AddModifier(WReadOnlyStatsData _data)
        {
            if (_data.data.damage > 0)
                damage.AddAdder(_data.data.damage);
            if (_data.data.critChance > 0)
                critChance.AddAdder(_data.data.critChance);
            if (_data.data.critPower > 0)
                critPower.AddAdder(_data.data.critPower);
            if (_data.data.attackSpeed > 0)
                attackSpeed.AddAdder(_data.data.attackSpeed);
        }
        protected void RemoveModifier(WReadOnlyStatsData _data)
        {
            if (_data.data.damage > 0)
                damage.RemoveAdder(_data.data.damage);
            if (_data.data.critChance > 0)
                critChance.RemoveAdder(_data.data.critChance);
            if (_data.data.critPower > 0)
                critPower.RemoveAdder(_data.data.critPower);
            if (_data.data.attackSpeed > 0)
                attackSpeed.RemoveAdder(_data.data.attackSpeed);
        }
    }
}
