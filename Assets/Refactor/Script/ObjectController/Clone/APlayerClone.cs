using ObjectGenerateData;
using StatsData;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace ObjectController
{
    internal class APlayerClone : AObjectController
    {
        [SerializeField] protected bool isInitFacingLeft = false;

        protected WReadOnlyDamageData damage;
        protected int attackTypeCount;
        protected bool canAttack;

        public void Setup(FCPlayerCloneFactory _factory, DPlayerCloneData _data)
        {
            factory = _factory;
            damage = _data.damage;
            canAttack = _data.canAttack;

            FacingToNearestEnemy();

            if (_data.canAttack)
            {
                attackTypeCount = _data.attackTypeCount;
                anim.ToEffect(Random.Range(0, attackTypeCount));
                DamageTriggerNotice += Damage;
                DamageFinishNotice += Finish;
            }
            else
            {
                anim.FadeAway();
            }
            FadeFinish += SelfRecycle;
        }

        private void FacingToNearestEnemy()
        {
            Transform nearestEnemy = InvokeFunc(TryGetNearestEnemyInRadiusNotice, -1);

            if(nearestEnemy == null)
            {
                return;
            }

            if (isInitFacingLeft && nearestEnemy.transform.position.x > transform.position.x)
            {
                transform.Rotate(0, 180, 0);
            }
            else if (!isInitFacingLeft && nearestEnemy.transform.position.x < transform.position.x)
            {
                transform.Rotate(0, 180, 0);
            }
        }

        protected void Damage()
        {
            InvokeAction(EffectAreaDamageTo, damage, EntitySystem.EEntityType.Enemy);
        }

        protected void Finish()
        {
            anim.FadeAway();
        }

        public override void Clear()
        {
            base.Clear();
            if(canAttack)
            {
                DamageTriggerNotice -= Damage;
                DamageFinishNotice -= Finish;
            }
            FadeFinish -= SelfRecycle;
            StopAllCoroutines();
        }
    }
}

