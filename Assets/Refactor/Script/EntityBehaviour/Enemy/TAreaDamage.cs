using EnemySystem;
using EntityBehaviour;
using EntitySystem;
using StatsData;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace EnemyBehaviour
{
    internal class TAreaDamage : TDamageEffect
    {       
        [SerializeField] protected float damageRadius;
        [SerializeField] protected LayerMask whatIsPlayer;
        [SerializeField] protected LayerMask whatIsEnemy;

        public override void EffectDamage(WReadOnlyDamageData _damage, EEntityType _target)
        {
            if(_target == EEntityType.Player)
            {
                Collider2D[] colliders = Physics2D.OverlapCircleAll(transform.position, damageRadius, whatIsPlayer);
                foreach(var hit in colliders)
                {
                    if(behaviour.InvokeFunc(behaviour.IsPlayer, hit.gameObject))
                    {
                        behaviour.InvokeFunc(behaviour.DamageToPlayer, hit.gameObject, _damage);
                    }
                }
                return;
            }

            if (_target == EEntityType.Enemy)
            {
                Collider2D[] colliders = Physics2D.OverlapCircleAll(transform.position, damageRadius, whatIsEnemy);
                foreach (var hit in colliders)
                {
                    if (hit.GetComponent<AEnemy>() != null)
                    {
                        behaviour.InvokeFunc(behaviour.DamageToEnemy, hit.gameObject, _damage);
                    }
                }
                return;
            }
        }

        protected void OnDrawGizmos()
        {
            Gizmos.color = Color.red;
            Gizmos.DrawWireSphere(transform.position, damageRadius);
        }
    }
}