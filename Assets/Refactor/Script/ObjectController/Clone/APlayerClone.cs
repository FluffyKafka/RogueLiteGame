using ObjectGenerateData;
using StatsData;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace ObjectController
{
    internal class APlayerClone : AObjectController
    {
        protected WReadOnlyDamageData damage;
        protected int attackTypeCount;
        protected float facingDir = 1;
        protected bool canAttack;

        public void Setup(FCPlayerCloneFactory _factory, DPlayerCloneData _data)
        {
            damage = _data.damage;
            canAttack = _data.canAttack;
            if(_data.canAttack)
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

