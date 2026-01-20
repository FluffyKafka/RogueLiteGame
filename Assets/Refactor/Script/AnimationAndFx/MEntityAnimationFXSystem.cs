using EntitySystem.EntityActor;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Assertions;

namespace AnimationAndFx
{
    internal class MEntityAnimationFXSystem : MonoBehaviour, IEntityAnimation
    {
        #region Action
        public Action<float> SlowBy;
        public Action RecoverSpeed;
        public Action<WReadOnlyDamageData> Hit;
        public Action<bool> Stun;
        #endregion

        #region Func
        #endregion

        #region ActionAndFuncInvokeHelper
        public void InvokeAction(Action _action)
        {
            _action?.Invoke();
        }
        public void InvokeAction<T>(Action<T> _action, T _arg)
        {
            _action?.Invoke(_arg);
        }
        public T InvokeFunc<T>(Func<T> _func)
        {
            Assert.IsNotNull(_func, GetType().Name + "的服务缺少提供者");
            Assert.IsTrue(_func.GetInvocationList().Length == 1, "服务" + _func.ToString() + "有复数提供者");
            return _func.Invoke();
        }
        public T2 InvokeFunc<T1, T2>(Func<T1, T2> _func, T1 _arg)
        {
            Assert.IsNotNull(_func, GetType().Name + "的服务缺少提供者");
            Assert.IsTrue(_func.GetInvocationList().Length == 1, "服务" + _func.ToString() + "有复数提供者");
            return _func.Invoke(_arg);
        }
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

