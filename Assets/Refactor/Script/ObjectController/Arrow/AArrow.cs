using EnemySystem;
using EntitySystem;
using ObjectController;
using ObjectGenerateData;
using PlayerSystem;
using StatsData;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace ObjectController
{
    internal class AArrow : AObjectController
    {
        [SerializeField] protected float reflectPhysicalDamageMultiplier;
        [SerializeField] protected float lifeTime = 10f;

        protected DDamageData damage;
        protected EEntityType target;

        public void Setup(FCArrowFactory _factory, DProjectileData _data)
        {
            factory = _factory;

            damage = _data.damage.Clone();
            InvokeAction(SwitchTargetTo, _data.targetType);
            target = _data.targetType;
            anim.ShowTrail(true);

            InvokeAction(Project, _data.velocity);

            FadeFinish += SelfRecycle;
            PlayerReflect += BeReflect;
            HitPlayer += DamageToPlayer;
            HitEnemyNotice += DamageToEnemy;
            HitGroundNotice += HitEffect;
            InvokeAction(SetLookAtMovement, true);

            StartCoroutine(SelfRecycleAfter(lifeTime));
        }

        protected IEnumerator SelfRecycleAfter(float _time)
        {
            yield return new WaitForSeconds(_time);
            SelfRecycle();
        }

        public void BeReflect(IObjectPlayer _player)
        {
            if(target == EEntityType.Player)
            {
                damage.physical *= reflectPhysicalDamageMultiplier;
                InvokeAction(SwitchTargetTo, EEntityType.Enemy);
            }
        }

        protected void DamageToPlayer(IObjectPlayer _player)
        {
            _player.TakeObjectDamage(new WReadOnlyDamageData(damage));
            HitEffect(_player.GetTransform());
        }

        protected void DamageToEnemy(IObjectEnemy _enemy)
        {
            _enemy.TakeObjectDamage(new WReadOnlyDamageData(damage));
            HitEffect(_enemy.GetTransform());
        }
        protected void HitEffect(Transform _target)
        {
            anim.FadeAway();
            anim.ShowTrail(false);
            InvokeAction(StuckInto, _target);
            InvokeAction(SetLookAtMovement, false);
        }

        public override void Clear()
        {
            base.Clear();
            damage = null;
            FadeFinish -= SelfRecycle;
            PlayerReflect -= BeReflect;
            HitPlayer -= DamageToPlayer;
            HitEnemyNotice -= DamageToEnemy;
            HitGroundNotice -= HitEffect;
            StopAllCoroutines();
        }

        public float CheckGravityScale()
        {
            return GetComponent<Rigidbody2D>().gravityScale;
        }
    }
}

