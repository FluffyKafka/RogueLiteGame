using EntitySystem.EntityActor;
using EntitySystem.EntityActor.EnemyActor;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Assertions;

namespace AnimationAndFx
{
    internal class MEnemyAnimationFxSystem : MEntityAnimationFXSystem, IEnemyAnimation
    {
        protected IAnimEnemy enemy;

        #region Action
        public Action Idle;
        public Action Move;
        public Action Attack;
        #endregion

        #region Func
        #endregion

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

        void IEnemyAnimation.Idle()
        {
            InvokeAction(Idle);
        }

        void IEnemyAnimation.Move()
        {
            InvokeAction(Move);
        }

        void IEnemyAnimation.Attack()
        {
            InvokeAction(Attack);
        }
    }
}