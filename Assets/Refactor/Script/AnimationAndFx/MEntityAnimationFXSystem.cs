using EntitySystem;
using StatsData;
using System;
using UnityEngine;
using UnityEngine.Assertions;

namespace AnimationAndFx
{
    internal class MEntityAnimationFXSystem : ComponentManagerBase, IEntityAnimation
    {
        protected IAnimEntity entity;

        #region Action
        public Action<float> SlowBy;
        public Action RecoverSpeed;
        public Action<WReadOnlyDamageData> Hit;
        public Action<bool> Stun;
        public Action AfterImageBegin;
        public Action AfterImageEnd;
        public Action<float> UpdateHealthBar;
        public Action ToDead;

        #endregion

        #region Func
        #endregion

        protected virtual void Awake()
        {
            entity = GetComponentInParent<IAnimEntity>();
            Assert.IsNotNull(entity, "实体动画组件需要被附加在一个实体下");
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
        void IEntityAnimation.UpdateHealthBar(float _hpPercent)
        {
            InvokeAction(UpdateHealthBar, _hpPercent);
        }
        void IEntityAnimation.ToDead()
        {
            InvokeAction(ToDead);
        }
        #region AfterImage
        public void EntityGenerateAfterImage(DAfterImageData _data)
        {
            entity.GenerateAfterImage(_data);
        }
        #endregion

        #region EntityRequest
        public int CheckFacingDir()
        {
            return entity.CheckFacingDir();
        }
        #endregion
    }
}

