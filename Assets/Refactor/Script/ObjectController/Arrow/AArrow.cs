using EnemySystem;
using EntitySystem;
using ObjectController;
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

        protected DDamageData damage;
        protected EEntityType target;

        public void Setup(FCArrowFactory _factory, DArrowData _data)
        {
            factory = _factory;

            damage = _data.damage.Clone();
            InvokeAction(Project, _data.velocity);
            InvokeAction(SwitchTargetTo, EEntityType.Player);
            target = _data.targetType;
            anim.ShowTrail(true);

            FadeFinish += SelfRecycle;
            PlayerReflect += BeReflect;
            HitPlayer += DamageToPlayer;
            HitEnemy += DamageToEnemy;
            HitGround += HitEffect;
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
            HitEffect(_player.CheckTransform());
        }

        protected void DamageToEnemy(IObjectEnemy _enemy)
        {
            _enemy.TakeObjectDamage(new WReadOnlyDamageData(damage));
            HitEffect(_enemy.CheckTransform());
        }
        protected void HitEffect(Transform _target)
        {
            anim.FadeAway();
            anim.ShowTrail(false);
            InvokeAction(StuckInto, transform, _target);
        }

        public override void Clear()
        {
            damage = null;
            FadeFinish -= SelfRecycle;
            PlayerReflect -= BeReflect;
            HitPlayer -= DamageToPlayer;
            HitEnemy -= DamageToEnemy;
        }

        public float CheckGravityScale()
        {
            return GetComponent<Rigidbody2D>().gravityScale;
        }
    }
}

