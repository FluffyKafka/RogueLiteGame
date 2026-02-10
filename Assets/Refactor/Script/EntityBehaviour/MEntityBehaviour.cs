using EntitySystem;
using StatsData;
using System;
using UnityEngine;
using UnityEngine.Assertions;

namespace EntityBehaviour
{
    internal class MEntityBehaviour : ComponentManagerBase, IEntityBehaviour
    {
        #region Action
        public Action<CanBeDamageSetData> SetCanBeDamage;
        public struct CanBeDamageSetData
        {
            public bool isSetToDefault;
            public bool isTempSetting;
            public bool canBeDamage;
        }
        public Action<bool> NoGravity;
        public Action<WReadOnlyDamageData> TakeDamage;
        public Action Flip;
        public Action Die;
        public Action ToDead;
        public Action AttackDamageTrigger;
        public Action<float> SlowEntityBy;
        public Action RecoverEntitySpeed;
        public Action AttackFinish;
        #endregion

        #region Func
        public Func<bool> CanBeDamage;
        public Func<bool> IsFall;
        public Func<int> CheckFacingDir;
        public Func<float> CheckYVelocity;
        public Func<bool> IsGroundedOrPlatForm;
        public Func<bool> IsTouchWall;
        public Func<WReadOnlyDamageData> GetPrimaryAttackDamage;
        #endregion

        #region Entity
        void IEntityBehaviour.AttackFinish()
        {
            InvokeAction(AttackFinish);
        }

        void IEntityBehaviour.AttackDamageTrigger()
        {
            InvokeAction(AttackDamageTrigger);
        }

        public void SlowBy(float _rate)
        {
            InvokeAction(SlowEntityBy, _rate);
        }

        public void RecoverSpeed()
        {
            InvokeAction(RecoverEntitySpeed);
        }

        void IEntityBehaviour.TakeDamage(WReadOnlyDamageData _damage)
        {
            InvokeAction(TakeDamage, _damage);
        }

        bool IEntityBehaviour.CanBeDamage()
        {
            return InvokeFunc(CanBeDamage);
        }
        void IEntityBehaviour.Die()
        {
            InvokeAction(Die);
        }
        int IEntityBehaviour.CheckFacingDir()
        {
            return InvokeFunc(CheckFacingDir);
        }
        #endregion

    }

    internal class CEntityComponentBase : MonoBehaviour
    {
        protected MEntityBehaviour entity;
        protected virtual void Awake()
        {
            entity = GetComponent<MEntityBehaviour>();
            Assert.IsTrue(entity != null, "组件" + GetType().ToString() + "必须挂载到一个AEntity上");
        }
        protected virtual void Update()
        {

        }
    }
}