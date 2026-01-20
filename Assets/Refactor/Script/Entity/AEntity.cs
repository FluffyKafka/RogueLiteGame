using EntitySystem.EntityComponent.BattleComponent;
using EntitySystem.EntityComponent.MovementComponent;
using EntitySystem.EntityComponent.StateMachineComponent;
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

        public interface IStatEntity
        {
            public bool CanBeDamage();
            public void SlowEntityByDuring(float _rate, float _duration);
        }

        internal abstract class AEntity : MonoBehaviour, IAnimEntity, IStatEntity
        {
            #region Actions
            public Action<EntitySpeedSetData> SetEntitySpeed;
            public struct EntitySpeedSetData
            {
                public bool isSetToDefault;
                public float rate;
                public float duration;
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

            public Action<float> SlowEntityBy;
            public Action RecoverEntitySpeed;

            public Action<WReadOnlyDamageData> TakeDamage;
            #endregion

            #region Func
            public Func<bool> IsFacingLeft;
            public Func<int> CheckFacingDir;

            public Func<bool> IsGroundedOrPlatForm;
            public Func<bool> IsTouchWall;

            public Func<bool> IsKnockBack;

            public Func<bool> CanBeDamage;

            public Func<bool> IsFall;
            public Func<float> CheckYVelocity;

            public Func<WReadOnlyDamageData> GetPrimaryAttackDamage;
            public Func<WReadOnlyDamageData, WReadOnlyDamageData> CalculateDamageTaken;
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
                Assert.IsNotNull(_func, GetType().Name + "的服务"+ _func.ToString() +"缺少提供者");
                Assert.IsTrue(_func.GetInvocationList().Length == 1, "服务" + _func.ToString() + "有复数提供者");
                return _func.Invoke();
            }
            public T2 InvokeFunc<T1, T2>(Func<T1, T2> _func, T1 _arg)
            {
                Assert.IsNotNull(_func, GetType().Name + "的服务" + _func.ToString() + "缺少提供者");
                Assert.IsTrue(_func.GetInvocationList().Length == 1, "服务" + _func.ToString() + "有复数提供者");
                return _func.Invoke(_arg);
            }
            #endregion

            #region Animation
            protected IEntityAnimation anim;
            void IAnimEntity.AttackFinish()
            {
                InvokeAction(AttackFinish);
            }

            void IAnimEntity.AttackDamageTrigger()
            {
                InvokeAction(AttackDamageTrigger);
            }
            #endregion

            #region Stats
            protected IEntityStats stats;
            bool IStatEntity.CanBeDamage()
            {
                return InvokeFunc(CanBeDamage);
            }
            void IStatEntity.SlowEntityByDuring(float _rate, float _duration)
            {
                StartCoroutine(SlowEntityHelper(_rate, _duration));
            }
            protected IEnumerator SlowEntityHelper(float _rate, float _duration)
            {
                InvokeAction(SlowEntityBy, _rate);
                yield return new WaitForSeconds(_duration);
                InvokeAction(RecoverEntitySpeed);
            }
            #endregion

            #region Entity Base Info
            [Header("Entity Base Info")]
            [SerializeField] public string entityName;
            [SerializeField] public Sprite entityIcon;
            [SerializeField] float selfDestroyAfterDead = 10f;
            public bool isDead = false;

            protected virtual void EntityDie()
            {
                if (!isDead)
                {
                    isDead = true;
                    Invoke("SelfDestroyAfterDead", selfDestroyAfterDead);
                }
            }
            protected virtual void SelfDestroyAfterDead()
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

            virtual protected void Awake()
            {
                anim = GetComponentInChildren<IEntityAnimation>();
                Assert.IsNotNull(anim, "实体缺少动画系统");
                SlowEntityBy += anim.SlowBy;
                RecoverEntitySpeed += anim.RecoverSpeed;
                TakeDamage += anim.Hit;

                stats = GetComponentInChildren<IEntityStats>();
                Assert.IsNotNull(stats, "实体缺少数值系统");
                GetPrimaryAttackDamage += stats.GetPrimaryAttackData;
                CalculateDamageTaken += stats.CalculateDamageTaken;
                TakeDamage += stats.TakeDamage;

                Die += EntityDie;

                ComponentValidCheck();
            }
            protected abstract void ComponentValidCheck();
        }

        public interface IEntityAnimation
        {
            public abstract void SlowBy(float _rate);
            public abstract void RecoverSpeed();
            public abstract void Hit(WReadOnlyDamageData _data);
            public abstract void BeStunned();
            public abstract void StunFinish();
        }

        public interface IEntityStats
        {
            public abstract WReadOnlyDamageData GetPrimaryAttackData();
            public abstract WReadOnlyDamageData CalculateDamageTaken(WReadOnlyDamageData _damageData);
            public abstract void TakeDamage(WReadOnlyDamageData _damage);
        }
        public class DDamageData
        {
            public Transform damageSourceTransform = null;
            public bool shouldPlayAnim = true;
            public float physical = 0;
            public bool isCrit = false;
            public float magical = 0;
            public bool ignite = false;
            public float igniteDamageCooldown = float.PositiveInfinity;
            public float igniteDuration = 0f;
            public float igniteDamage = 0f;
            public bool chill = false;
            public float chillSlowPercentage = 0f;
            public float chillDuration = 0f;
            public float chillReduceArmorPer = 0f;
            public bool shock = false;
            public float shockDuration = 0f;
            public float thunderStrikeRadius = 0f;
            public float thunderStrikeRate = 0f;
            public int thunderStrikeCounter = 0;
            public float shockReduceAccuracy = 0f;
        }
        public struct WReadOnlyDamageData
        {
            public readonly DDamageData data;
            public WReadOnlyDamageData(DDamageData _damageData)
            {
                data = _damageData;
            }
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

