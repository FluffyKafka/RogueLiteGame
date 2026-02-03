using PlayerSystem;
using System;
using Unity.VisualScripting;
using UnityEngine.Assertions;

namespace AnimationAndFx
{
    internal class MPlayerAnimationFxSystem : MEntityAnimationFXSystem, IPlayerAnimation
    {
        protected IAnimPlayer player;

        #region Action
        public Action Air;
        public Action<int> Attack;
        public Action Idle;
        public Action Move;
        public Action WallSlide;
        public Action<float> UpdateYVelocity;
        public Action DashBeginNotice;
        public Action DashEndNotice; 
        #endregion

        protected override void Awake()
        {
            player = GetComponentInParent<IAnimPlayer>();
            Assert.IsNotNull(player, "MPlayerAnimation需要挂载在一个Player上");
        }

        void IPlayerAnimation.Air()
        {
            InvokeAction(Air);
        }

        void IPlayerAnimation.Attack(int _count)
        {
            InvokeAction(Attack, _count);
        }

        void IPlayerAnimation.Idle()
        {
            InvokeAction(Idle);
        }

        void IPlayerAnimation.Move()
        {
            InvokeAction(Move);
        }
        void IPlayerAnimation.WallSlide()
        {
            InvokeAction(WallSlide);
        }

        void IPlayerAnimation.UpdateYVelocity(float _yVelocity)
        {
            InvokeAction(UpdateYVelocity, _yVelocity);
        }

        public void DashBegin()
        {
            InvokeAction(DashBeginNotice);
            InvokeAction(AfterImageBegin);
        }

        public void DashEnd()
        {
            InvokeAction(DashEndNotice);
            InvokeAction(AfterImageEnd);
        }

        public void OnAttackFinish()
        {
            player.AttackFinish();
        }

        public void OnAttackDamageTrigger()
        {
            player.AttackDamageTrigger();
        }

        
    }
}