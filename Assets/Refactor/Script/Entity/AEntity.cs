using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Assertions;

namespace EntitySystem
{
    namespace EntityActor
    {
        public interface IAnimEntity
        {
            public void AttackFinish();
            public void AttackDamageTrigger();
        }

        internal class AEntity : MonoBehaviour, IAnimEntity
        {
            #region Actions
            public Action<EntitySpeedSetData> SetEntitySpeed;
            public struct EntitySpeedSetData
            {
                public bool isSetToDefault;
                public float rate;
                public float duration;
            }

            public Action<DamageData> TakeDamage;
            public struct DamageData
            {
                public float damageAmount;
                public Entity damageSource;
            }

            public Action BeKnockedBack;

            public Action Flip;

            public Action<bool> NoGravity;

            public Action<bool> MakeTransprent;

            public Action Die;

            public Action SelfDestroy;

            public Action<CanBeDamageSetData> SetCanBeDamage;
            public struct CanBeDamageSetData
            {
                public bool isSetToDefault;
                public bool isTempSetting;
                public bool canBeDamage;
            }

            public Action<string> StateChange;

            public Action AttackFinish;
            public Action AttackDamageTrigger;
            #endregion

            #region Func
            public Func<bool> IsFacingLeft;
            public Func<int> CheckFacingDir;

            public Func<bool> IsGroundedOrPlatForm;
            public Func<bool> IsTouchWall;

            public Func<bool> IsKnockBack;

            public Func<bool> CanBeDamage;

            public Func<string> CheckStateAnimName;

            public Func<bool> IsFall;
            public Func<float> CheckYVelocity;

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
                Assert.IsNotNull(_func, "服务缺少提供者");
                Assert.IsTrue(_func.GetInvocationList().Length == 1, "服务" + _func.ToString() + "有复数提供者");
                return _func.Invoke();
            }
            #endregion

            #region Animation
            void IAnimEntity.AttackFinish()
            {
                InvokeAction(AttackFinish);
            }

            void IAnimEntity.AttackDamageTrigger()
            {
                InvokeAction(AttackDamageTrigger);
            }
            #endregion

            #region Entity Base Info
            [Header("Entity Base Info")]
            [SerializeField] public string entityName;
            [SerializeField] public Sprite entityIcon;
            [SerializeField] float selfDestroyAfterDead = 10f;
            protected IEntityAnimation anim;
            public bool isDead = false;

            virtual protected void Awake()
            {
                anim = GetComponentInChildren<IEntityAnimation>();
                Assert.IsNotNull(anim, "实体缺少动画系统");

                Die += EntityDie;
                StateChange += anim.ChangeAnimationNotice;
            }

            public virtual void EntityDie()
            {
                if (!isDead)
                {
                    isDead = true;
                    Invoke("SelfDestroyAfterDead", selfDestroyAfterDead);
                }
            }
            private void SelfDestroyAfterDead()
            {
                if (isDead)
                {
                    Vector3 viewportPosition = UnityEngine.Camera.main.WorldToViewportPoint(transform.position);
                    if (viewportPosition.x < 0 || viewportPosition.x > 1 || viewportPosition.y < 0 || viewportPosition.y > 1)
                    {
                        Destroy(gameObject);
                    }
                }
            }
            #endregion
        }

        public interface IEntityAnimation
        {
            public void ChangeAnimationNotice(string _stateAnimName);
        }
    }

    namespace EntityComponent
    {
        internal class CEntityComponentBase: MonoBehaviour
        {
            protected EntityActor.AEntity entity;
            protected virtual void Awake()
            {
                entity = GetComponent<EntityActor.AEntity>();
                Assert.IsTrue(entity != null, "组件" + GetType().ToString() +"必须挂载到一个AEntity上");
            }
            protected virtual void Update() 
            {

            }
        }
    }
}

