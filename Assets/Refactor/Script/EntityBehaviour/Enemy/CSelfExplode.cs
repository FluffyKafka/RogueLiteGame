using EntityBehaviour;
using EntitySystem;
using StatsData;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace EnemyBehaviour
{
    internal class CSelfExplode : CEntityComponentBase
    {
        [Header("自爆伤害为角色基础伤害的数倍")]
        [SerializeField] protected float damageMultiplier;
        [SerializeField] protected float reflectDamageMultiplier;
        [SerializeField] protected float explodeHoldingDuration;
        [Header("自爆执行时的伤害组件")]
        [SerializeField] protected TDamageEffect explodeDamageEffect;

        protected MEnemyBehaviour behaviour;
        protected DDamageData damage;
        protected EEntityType target;

        protected override void Awake()
        {
            base.Awake();

            behaviour = GetComponent<MEnemyBehaviour>();
            behaviour.CheckSelfExplodeHoldingDurationNotice += CheckExplodeHoldingDuration;
            behaviour.SelfExplodeNotice_isReflect += Explode;
        }

        protected float CheckExplodeHoldingDuration()
        {
            return explodeHoldingDuration;
        }

        protected void Explode(bool _isReflect)
        {
            damage = behaviour.InvokeFunc(behaviour.GetPrimaryAttackDamage).Clone();
            if(_isReflect)
            {
                target = EEntityType.Enemy;
                damage.physical *= reflectDamageMultiplier;
                damage.magical *= reflectDamageMultiplier;
            }
            else
            {
                target = EEntityType.Player;
                damage.physical *= damageMultiplier;
                damage.magical *= damageMultiplier;
            }
            behaviour.OnSelfExplodeDamageTrigger += Damage;
        }
        protected void SelfExplodeFinish()
        {
            behaviour.OnSelfExplodeDamageTrigger -= Damage;
        }

        protected void Damage()
        {            
            explodeDamageEffect.EffectDamage(new WReadOnlyDamageData(damage), target);
        }
    }
}

