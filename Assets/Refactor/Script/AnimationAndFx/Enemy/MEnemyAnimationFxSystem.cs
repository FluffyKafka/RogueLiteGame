using EnemySystem;
using System;
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
        public Action<bool> StunOpen;
        public Action PullBack;
        public Action PullBackJump;
        public Action Fall;
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
            InvokeAction(StunOpen, true);
        }

        public void OnStunClose()
        {
            enemy.OpenStun(false);
            InvokeAction(StunOpen, false);
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

        void IEnemyAnimation.PullBack()
        {
            InvokeAction(PullBack);
        }

        void IEnemyAnimation.PullBackJump()
        {
            InvokeAction(PullBackJump);
        }

        void IEnemyAnimation.Fall()
        {
            InvokeAction(Fall);
        }
    }
}