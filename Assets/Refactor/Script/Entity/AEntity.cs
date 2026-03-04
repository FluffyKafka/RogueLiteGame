using StatsData;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Assertions;
using ObjectGenerateData;
using UnityEngine.EventSystems;

namespace EntitySystem
{
    public enum EEntityType
    {
        Player,
        Enemy
    }

    public interface IObjectEntity
    {
        public WReadOnlyDamageData TakeObjectDamage(WReadOnlyDamageData _damage);
        public Transform CheckTransform();
        public void ObjectFinish();
    }

    public interface IBehaviourEntity
    {
        public void Flip();
        public WReadOnlyDamageData GetPrimaryAttackDamage();
        public void ToDead();
    }

    public interface IAnimEntity
    {
        public void AttackFinish();
        public void AttackDamageTrigger();
        public void GenerateAfterImage(DAfterImageData _data);
        public int CheckFacingDir();
    }

    public interface IStatEntity
    {
        public bool CanBeDamage();
        public void SlowEntityByDuring(float _rate, float _duration);
        public void Die();
        public void CurrentHealthUpdate(float _hpPercent);
    }

    internal abstract class AEntity : ComponentManagerBase, IAnimEntity, IStatEntity, IBehaviourEntity
    {
        #region Actions
        public Action Flip;

        public Action Die;//提供行为组件判断死亡前行为
        public Action ToDead;//确认死亡

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
        private IEntityBehaviour behaviour;
        void IBehaviourEntity.Flip()
        {
            InvokeAction(Flip);
        }
        WReadOnlyDamageData IBehaviourEntity.GetPrimaryAttackDamage()
        {
            return InvokeFunc(GetPrimaryAttackDamage);
        }
        void IBehaviourEntity.ToDead()
        {
            InvokeAction(ToDead);
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
        void IAnimEntity.GenerateAfterImage(DAfterImageData _data)
        {
            objectFactory.GenerateAfterImage(_data);
        }
        public int CheckFacingDir()
        {
            return behaviour.CheckFacingDir();
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
        void IStatEntity.Die()
        {
            InvokeAction(Die);
        }
        void IStatEntity.CurrentHealthUpdate(float _hpPercent)
        {
            anim.UpdateHealthBar(_hpPercent);
        }
        #endregion

        #region Object
        protected IEntityObjectFactory objectFactory;
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

            behaviour = GetComponent<IEntityBehaviour>();
            Assert.IsNotNull(behaviour, "实体缺少行为系统");
            Die += behaviour.Die;

            ToDead += EntityDie;
        }
    }

    public interface IEntityObjectFactory
    {
        public void GenerateAfterImage(DAfterImageData _data);
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

        public int CheckFacingDir();
    }

    public interface IEntityAnimation
    {
        public abstract void SlowBy(float _rate);
        public abstract void RecoverSpeed();
        public abstract void Hit(WReadOnlyDamageData _data);
        public abstract void BeStunned();
        public abstract void StunFinish();
        public abstract void UpdateHealthBar(float _hpPercent);
        public abstract void ToDead();
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
        public abstract void SelfHealByPercent(float _per);
    }

    public interface IEntityObject
    {

    }
}

