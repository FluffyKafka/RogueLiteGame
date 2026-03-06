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
    internal class APierceSword : AObjectController
    {
        protected WReadOnlyDamageData damage;
        protected ISwordObjectModel swordModel;

        public void Setup(FCPierceSwordFactory _factory, DProjectileData _data)
        {
            factory = _factory;

            damage = _data.damage;
            swordModel = _data.manager.GetComponent<ISwordObjectModel>();
            GetComponent<Rigidbody2D>().gravityScale = _data.gravity;
            InvokeAction(SwitchTargetTo, EEntityType.Enemy);
            InvokeAction(Project, _data.velocity);
            InvokeAction(SetLookAtMovement, true);
            anim.ToEffect();

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
        }
        protected void HitGround(Transform _target)
        {
            swordModel.HitGround();
            InvokeAction(StuckInto, _target);
            InvokeAction(SetLookAtMovement, false);
            HitEffect(_target);
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
            HitGroundNotice -= HitGround;
            StopAllCoroutines();
        }
    }
}

