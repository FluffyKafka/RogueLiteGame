using EntitySystem.EntityActor;
using EntitySystem.EntityActor.PlayerActor;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Assertions;

namespace AnimationAndFx
{
    public class MPlayerAnimation : MEntityAnimation, IPlayerAnimation
    {
        protected IAnimPlayer player;

        [SerializeField] protected string idleAnimName = "Idle";
        [SerializeField] protected string moveAnimName = "Move";
        [SerializeField] protected string airAnimName = "Air";
        [SerializeField] protected string yVelocityName = "yVelocity";
        [SerializeField] protected string attackAnimName = "Attack";
        [SerializeField] protected string comboCounterName = "ComboCounter";
        [SerializeField] protected string wallSlideName = "WallSlide";
 
        protected override void Awake()
        {
            base.Awake();
            player = GetComponentInParent<IAnimPlayer>();
            Assert.IsNotNull(player, "MPlayerAnimation需要挂载在一个Player上");
        }

        public void OnAttackFinish()
        {
            player.AttackFinish();
        }

        public void OnAttackDamageTrigger()
        {
            player.AttackDamageTrigger();
        }

        void IPlayerAnimation.Idle()
        {
            ChangeAnimationTo(idleAnimName);
        }

        void IPlayerAnimation.Move()
        {
            ChangeAnimationTo(moveAnimName);
        }

        void IPlayerAnimation.Attack(int _count)
        {
            ChangeAnimationTo(attackAnimName);
            anim.SetInteger(comboCounterName, _count);
        }

        void IPlayerAnimation.Air()
        {
            ChangeAnimationTo(airAnimName);        
        }

        void IPlayerAnimation.UpdateYVelocity(float _yVelocity)
        {
            anim.SetFloat(yVelocityName, _yVelocity);
        }

        void IPlayerAnimation.WallSlide()
        {
            ChangeAnimationTo(wallSlideName);
        }
    }
}
