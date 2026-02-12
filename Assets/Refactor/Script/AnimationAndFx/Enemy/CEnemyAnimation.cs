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
        [SerializeField] protected string pullbackAnimName = "Move";
        [SerializeField] protected string fallAnimName = "Fall";
        [SerializeField] protected string pullbackJumpAnimName = "PullBackJump";
        [SerializeField] protected string controllAmmoAnimName = "Controll";
        [SerializeField] protected string selfExplodeAnimName = "Explode";

        protected override void Awake()
        {
            base.Awake();
            Assert.IsTrue(animFxSystem is MEnemyAnimationFxSystem, "敌人动画组件需要附加在敌人动画特效系统上");
            enemyAnimFxSystem = animFxSystem as MEnemyAnimationFxSystem;
            enemyAnimFxSystem.Idle += Idle;
            enemyAnimFxSystem.Move += Move;
            enemyAnimFxSystem.Attack += Attack;
            enemyAnimFxSystem.Stun += Stun;
            enemyAnimFxSystem.PullBack += PullBack;
            enemyAnimFxSystem.PullBackJump += PullBackJump;
            enemyAnimFxSystem.Fall += Fall;
            enemyAnimFxSystem.Controll += Controll;
            enemyAnimFxSystem.SelfExplode += SelfExplode;
            enemyAnimFxSystem.SelfExplodeHolding += SelfExplodeHolding;
        }

        protected void Idle()
        {
            ChangeAnimationTo(idleAnimName);
        }
        protected void Move()
        {
            ChangeAnimationTo(moveAnimName);
        }
        protected void Attack()
        {
            ChangeAnimationTo(attackAnimName);
        }
        protected void Stun(bool isStun)
        {
            if(isStun)
            {
                ChangeAnimationTo(stunAnimName);
            }         
        }

        protected void PullBack()
        {
            ChangeAnimationTo(pullbackAnimName);
        }

        protected void PullBackJump()
        {
            ChangeAnimationTo(pullbackJumpAnimName);
        }

        protected void Fall()
        {
            ChangeAnimationTo(fallAnimName);
        }

        protected void Controll()
        {
            ChangeAnimationTo(controllAmmoAnimName);
        }

        protected void Dead()
        {
            anim.SetBool(currentAnimName, true);
            anim.speed = 0;
        }

        protected void SelfExplodeHolding()
        {
            ChangeAnimationTo(selfExplodeAnimName);
        }
        protected void SelfExplode()
        {
            anim.SetBool(selfExplodeAnimName, false);
        }
    }
}
