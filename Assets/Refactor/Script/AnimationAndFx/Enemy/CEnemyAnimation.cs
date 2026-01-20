using EntitySystem.EntityActor.EnemyActor;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Assertions;

namespace AnimationAndFx
{
    internal class CEnemyAnimation : CEntityAnimation
    {
        protected MEnemyAnimationFxSystem enemyAnimFxSystem;

        [SerializeField] protected string idleAnimName = "Idle";
        [SerializeField] protected string moveAnimName = "Move";
        [SerializeField] protected string attackAnimName = "Attack";
        [SerializeField] protected string stunAnimName = "Stun";

        protected override void Awake()
        {
            base.Awake();
            Assert.IsTrue(animFxSystem is MEnemyAnimationFxSystem, "敌人动画组件需要附加在敌人动画特效系统上");
            enemyAnimFxSystem = animFxSystem as MEnemyAnimationFxSystem;
            enemyAnimFxSystem.Idle += Idle;
            enemyAnimFxSystem.Move += Move;
            enemyAnimFxSystem.Attack += Attack;
            enemyAnimFxSystem.Stun += Stun;
        }

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
    }
}
