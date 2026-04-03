using EnemySystem;
using EntitySystem;
using ObjectGenerateData;
using SkillSystem;
using StatsData;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace ObjectController
{
    internal class ASword : AObjectController
    {
        protected WReadOnlyDamageData damage;
        protected ISwordObjectModel swordModel;

        public void Setup(FCSwordFactory _factory, DProjectileData _data)
        {
            factory = _factory;

            damage = _data.damage;
            swordModel = _data.manager.GetComponent<ISwordObjectModel>();
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
            HitEffect(_enemy.GetTransform());
        }

        protected void HitGround(Transform _ground)
        {
            swordModel.HitGround();
            HitEffect(_ground);
        }
        protected void HitEffect(Transform _target)
        {
            InvokeAction(StuckInto, _target);
            anim.ToEffect();
            anim.ShowHitFx();
            InvokeAction(SetLookAtMovement, false);
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