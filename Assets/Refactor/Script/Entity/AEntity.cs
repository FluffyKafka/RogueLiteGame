using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Assertions;

namespace EntitySystem
{
    namespace EntityActor
    {
        public interface IAnimationController
        {
            public string CheckStateAnimationName();
        }

        internal class AEntity : MonoBehaviour, IAnimationController
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

            public Action StateChange;
            #endregion
            #region Func
            public Func<bool> IsFacingLeft;
            public Func<int> CheckFacingDir;

            public Func<bool> IsGrounded;
            public Func<bool> IsTouchWall;

            public Func<bool> IsKnockBack;

            public Func<bool> CanBeDamage;

            public Func<string> CheckStateAnimName;

            public Func<bool> IsFall;
            #endregion
            #region Entity Base Info
            [Header("Entity Base Info")]
            [SerializeField] public string entityName;
            [SerializeField] public Sprite entityIcon;
            [SerializeField] float selfDestroyAfterDead = 10f;
            protected IEntityAnimation anim;
            public bool isDead;

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

            string IAnimationController.CheckStateAnimationName()
            {
                Assert.IsTrue(CheckStateAnimName != null, "无法获取状态动画名称，请检查状态机及其组件");
                Assert.IsTrue(CheckStateAnimName.GetInvocationList().Length == 1, "存在复数状态机，单一实体上只允许一个状态机组件");
                return CheckStateAnimName.Invoke();
            }
        }

        public interface IEntityAnimation
        {
            public void ChangeAnimationNotice();
        }
    }

    namespace EntityComponent
    {
        internal class CEntityComponentBase: MonoBehaviour
        {
            protected EntityActor.AEntity entity;
            virtual protected void Awake()
            {
                entity = GetComponent<EntityActor.AEntity>();
                Assert.IsTrue(entity != null, "组件" + GetType().ToString() +"必须挂载到一个AEntity上");
            }
        }
    }
}

