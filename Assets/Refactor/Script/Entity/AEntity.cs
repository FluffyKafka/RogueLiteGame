using StatsData;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Assertions;

namespace EntitySystem
{
    public interface IBehaviourEntity
    {
        public void Flip();
    }

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

    internal abstract class AEntity : ComponentManagerBase, IAnimEntity, IStatEntity, IBehaviourEntity
    {
        #region Actions
        public Action Flip;

        public Action Die;

        public Action AttackFinish;
        public Action AttackDamageTrigger;

        public Action<float> SlowEntityBy;
        public Action RecoverEntitySpeed;

        public Action<WReadOnlyDamageData> TakeDamage;

        public Action<WReadOnlyStatsData> AddModifier;
        public Action<WReadOnlyStatsData> RemoveModifier;
        #endregion

        #region Func
        public Func<bool> CanBeDamage;

        public Func<float> CheckYVelocity;

        public Func<WReadOnlyDamageData> GetPrimaryAttackDamage;
        public Func<WReadOnlyDamageData, WReadOnlyDamageData> CalculateDamageTaken;
        #endregion

        #region Behaviour
        void IBehaviourEntity.Flip()
        {
            InvokeAction(Flip);
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
                StartCoroutine(SelfDestroyAfterDead(selfDestroyAfterDead));
            }
        }
        protected virtual IEnumerator SelfDestroyAfterDead(float _delay)
        {
            Assert.IsTrue(isDead, "不应对未死亡角色调用SelfDestroyAfterDead");

            yield return new WaitForSeconds(_delay);

            Vector3 viewportPosition = UnityEngine.Camera.main.WorldToViewportPoint(transform.position);
            if (viewportPosition.x < 0 || viewportPosition.x > 1 || viewportPosition.y < 0 || viewportPosition.y > 1)
            {
                Destroy(gameObject);
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
        }
    }

    public interface IEntityBehaviour
    {
        public void AttackFinish();
        public void AttackDamageTrigger();

        public void SlowBy(float _rate);
        public void RecoverSpeed();
        public void TakeDamage(WReadOnlyDamageData _damage);
        public bool CanBeDamage();
        public void Die();
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

    public interface IEntityObject
    {

    }
}

