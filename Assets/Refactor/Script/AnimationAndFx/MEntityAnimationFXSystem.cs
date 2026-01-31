using EntitySystem;
using StatsData;
using System;

namespace AnimationAndFx
{
    internal class MEntityAnimationFXSystem : ComponentManagerBase, IEntityAnimation
    {
        #region Action
        public Action<float> SlowBy;
        public Action RecoverSpeed;
        public Action<WReadOnlyDamageData> Hit;
        public Action<bool> Stun;
        #endregion

        #region Func
        #endregion

        protected virtual void Awake()
        {
            
        }

        void IEntityAnimation.SlowBy(float _rate)
        {
            InvokeAction(SlowBy, _rate);
        }

        void IEntityAnimation.RecoverSpeed()
        {
            InvokeAction(RecoverSpeed);
        }

        void IEntityAnimation.Hit(WReadOnlyDamageData _data)
        {
            InvokeAction(Hit, _data);
        }

        void IEntityAnimation.BeStunned()
        {
            InvokeAction(Stun, true);
        }

        void IEntityAnimation.StunFinish()
        {
            InvokeAction(Stun, false);
        }
    }
}

