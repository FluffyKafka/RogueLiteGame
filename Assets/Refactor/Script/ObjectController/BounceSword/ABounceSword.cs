using EnemySystem;
using EntitySystem;
using ObjectGenerateData;
using StatsData;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

namespace ObjectController
{
    internal class ABounceSword : AObjectController
    {
        protected WReadOnlyDamageData damage;
        protected float bounceRadius;
        protected float bounceSpeed;
        protected int bounceCount;
        public void Setup(FCBounceSwordFactory _factory, DBounceSwordData _data)
        {
            factory = _factory;

            damage = _data.damage;
            bounceCount = _data.bounceCount;
            bounceRadius = _data.bounceRadius;
            bounceSpeed = _data.bounceSpeed;
            GetComponent<Rigidbody2D>().gravityScale = _data.gravity;
            InvokeAction(SwitchTargetTo, EEntityType.Enemy);
            InvokeAction(Project, _data.velocity);
            InvokeAction(SetLookAtMovement, true);

            HitEnemyNotice += DamageToEnemy;
            HitGroundNotice += HitGround;
        }

        protected IEnumerator SelfRecycleAfter(float _time)
        {
            yield return new WaitForSeconds(_time);
            SelfRecycle();
        }

        protected void DamageToEnemy(IObjectEnemy _enemy)
        {
            _enemy.TakeObjectDamage(damage);
            HitEffect(_enemy.CheckTransform());

            if(bounceCount <= 0)
            {
                InvokeAction(StuckInto, _enemy.CheckTransform());
                anim.ToEffect();
                InvokeAction(SetLookAtMovement, false);
                return;
            }

            Transform nextEnemy = InvokeFunc(TryGetRandomEnemyInRadiusNotice, bounceRadius);
            if(nextEnemy == null)
            {
                InvokeAction(StuckInto, _enemy.CheckTransform());
                anim.ToEffect();
                InvokeAction(SetLookAtMovement, false);
            }            
            InvokeAction(SetMoveToTargetNotice, nextEnemy, bounceSpeed);
            --bounceCount;

        }
        protected void HitGround(Transform _ground)
        {
            InvokeAction(StuckInto, _ground);
            anim.ToEffect();
            HitEffect(_ground);
            InvokeAction(SetLookAtMovement, false);
        }
        protected void HitEffect(Transform _target)
        {
            anim.ShowHitFx();
        }

        public override void TakeBack()
        {
            base.TakeBack();
            InvokeAction(StuckInto, null);
        }

        public override void Clear()
        {
            base.Clear();
            HitEnemyNotice -= DamageToEnemy;
            HitGroundNotice -= HitEffect;
            StopAllCoroutines();
        }
    }
}

