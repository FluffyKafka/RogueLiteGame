using UnityEngine;
using UnityEngine.Assertions;

namespace AnimationAndFx
{
    internal class CPlayerAnimation : CEntityAnimation
    {
        protected MPlayerAnimationFxSystem playerAnimFxSystem;

        [SerializeField] protected string idleAnimName = "Idle";
        [SerializeField] protected string moveAnimName = "Move";
        [SerializeField] protected string airAnimName = "Air";
        [SerializeField] protected string yVelocityName = "yVelocity";
        [SerializeField] protected string attackAnimName = "Attack";
        [SerializeField] protected string comboCounterName = "ComboCounter";
        [SerializeField] protected string wallSlideName = "WallSlide";
        [SerializeField] protected string dashName = "Dash";
        [SerializeField] protected string aimAnimName = "AimSword";
        [SerializeField] protected string catchAnimNAme = "CatchSword";
        [SerializeField] protected string counterAttackAnimName = "CounterAttack";
        [SerializeField] protected string counterAttackSuccessAnimName = "SuccessfulCounterAttack";


        protected override void Awake()
        {
            base.Awake();
            Assert.IsTrue(animFxSystem is MPlayerAnimationFxSystem, "玩家动画组件需要附加在玩家动画特效系统上");
            playerAnimFxSystem = animFxSystem as MPlayerAnimationFxSystem;
            playerAnimFxSystem.Idle += Idle;
            playerAnimFxSystem.Move += Move;
            playerAnimFxSystem.Attack += Attack;
            playerAnimFxSystem.Air += Air;
            playerAnimFxSystem.WallSlide += WallSlide;
            playerAnimFxSystem.UpdateYVelocity += UpdateYVelocity;
            playerAnimFxSystem.DashBeginNotice += Dash;
            playerAnimFxSystem.AimmingBeginNotice += Aimming;
            playerAnimFxSystem.CatchNotice += Catch;
            playerAnimFxSystem.CounterAttackNotice += CounterAttack;
            playerAnimFxSystem.CounterAttackSuccessNotice += CounterAttackSuccess;
        }

        protected void Idle()
        {
            ChangeAnimationTo(idleAnimName);
        }

        protected void Move()
        {
            ChangeAnimationTo(moveAnimName);
        }

        protected void Attack(int _count)
        {
            ChangeAnimationTo(attackAnimName);
            anim.SetInteger(comboCounterName, _count);
        }

        protected void Air()
        {
            ChangeAnimationTo(airAnimName);        
        }

        protected void UpdateYVelocity(float _yVelocity)
        {
            anim.SetFloat(yVelocityName, _yVelocity);
        }

        protected void WallSlide()
        {
            ChangeAnimationTo(wallSlideName);
        }

        protected void Dash()
        {
            ChangeAnimationTo(dashName);
        }

        protected void Aimming()
        {
            ChangeAnimationTo(aimAnimName);
        }

        protected void Catch()
        {
            ChangeAnimationTo(catchAnimNAme);
        }

        protected void CounterAttack()
        {
            ChangeAnimationTo(counterAttackAnimName);
        }

        protected void CounterAttackSuccess()
        {
            ChangeAnimationTo(counterAttackSuccessAnimName);
        }
    }
}
