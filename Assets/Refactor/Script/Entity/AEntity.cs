using EntitySystem.EntityComponent.BattleComponent;
using EntitySystem.EntityComponent.MovementComponent;
using EntitySystem.EntityComponent.StateMachineComponent;
using StatsData;
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

        internal abstract class AEntity : ComponentManagerBase, IAnimEntity, IStatEntity
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

            public Action<WReadOnlyStatsData> AddModifier;
            public Action<WReadOnlyStatsData> RemoveModifier;
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
                AddModifier += stats.AddStatModifier;
                RemoveModifier += stats.RemoveStatModifier;

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

            //装备修改流程：
            //UI传入装备修改信号
            //Player（entity）收到信号转发出装备修改信号（只有Player有装备）
            //Inventory收到信号修改装备并向Player发出数值修改信号
            //Entity收到信号再度广播
            //Stats收到信号并修改属性
            public abstract void AddStatModifier(WReadOnlyStatsData _data);
            public abstract void RemoveStatModifier(WReadOnlyStatsData _data);

            public abstract float TryCheckStat(EStatType _type);
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

