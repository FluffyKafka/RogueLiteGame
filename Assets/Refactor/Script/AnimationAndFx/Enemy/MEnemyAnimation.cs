using EntitySystem.EntityActor.EnemyActor;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Assertions;

namespace AnimationAndFx
{
    public class MEnemyAnimation : MEntityAnimation, IEnemyAnimation
    {
        protected IAnimEnemy enemy;
        [SerializeField] protected string idleAnimName = "Idle";
        [SerializeField] protected string moveAnimName = "Move";
        [SerializeField] protected string attackAnimName = "Attack";
        [SerializeField] protected string stunAnimName = "Stun";

        protected override void Awake()
        {
            base.Awake();
            enemy = GetComponentInParent<IAnimEnemy>();
            Assert.IsNotNull(enemy, "MEnemyAnimation需要挂载在一个Enemy上");
        }
        public void OnAttackFinish()
        {
            enemy.AttackFinish();
        }

        public void OnAttackDamageTrigger()
        {
            enemy.AttackDamageTrigger();
        }

        public void OnStunOpen()
        {
            enemy.OpenStun(true);
        }

        public void OnStunClose()
        {
            enemy.OpenStun(false);
        }

        #region Enemy Animation
        public void Idle()
        {
            ChangeAnimationTo(idleAnimName);
        }
        public void Move()
        {
            ChangeAnimationTo(moveAnimName);
        }
        public void Attack()
        {
            ChangeAnimationTo(attackAnimName);
        }
        public void Stun()
        {
            ChangeAnimationTo(stunAnimName);
        }
        #endregion
    }
}
