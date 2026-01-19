using EntitySystem.EntityActor;
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
        }

        protected void CulculatePhysicsDamage(DDamageData _damage)
        {
            _damage.physical = damage.GetValue();
            if (Random.Range(0, 100) < critChance.GetValue())
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
    }
}
