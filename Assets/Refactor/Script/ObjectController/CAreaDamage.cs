using EnemySystem;
using EntitySystem;
using PlayerSystem;
using StatsData;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace ObjectController
{
    internal class CAreaDamage : CObjectComponentBase
    {
        [SerializeField] protected float damageRadius;
        [SerializeField] protected LayerMask whatIsPlayer;
        [SerializeField] protected LayerMask whatIsEnemy;


        protected override void Awake()
        {
            base.Awake();
            controller.EffectAreaDamageTo += EffectDamage;
        }
        protected void EffectDamage(WReadOnlyDamageData _damage, EEntityType _targetType)
        {
            LayerMask targetMask = whatIsPlayer;
            if(_targetType == EEntityType.Enemy)
            {
                targetMask = whatIsEnemy;
            }

            Collider2D[] hits = Physics2D.OverlapCircleAll(transform.position, damageRadius, targetMask);
            foreach(var hit in hits)
            {
                if(_targetType == EEntityType.Player)
                {
                    hit.GetComponent<IObjectPlayer>().TakeObjectDamage(_damage);
                }

                if(_targetType == EEntityType.Enemy)
                {
                    hit.GetComponent<IObjectEnemy>().TakeObjectDamage(_damage);
                }
            }
        }

        protected void OnDrawGizmos()
        {
            Gizmos.color = Color.red;
            Gizmos.DrawWireSphere(transform.position, damageRadius);
        }
    }
}

