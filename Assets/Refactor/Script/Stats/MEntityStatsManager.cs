using EntitySystem;
using StatsData;
using System;
using System.Collections;
using UnityEngine;
using UnityEngine.Assertions;

namespace StatsSystem
{
    internal class MEntityStatsManager : ComponentManagerBase, IEntityStats
    {
        protected IStatEntity entity;

        protected DDamageData damageData;
        protected DDamageData takeDamageData;

        #region Action
        public Action<DDamageData> CalculatePrimaryAttackData;
        public Action<WReadOnlyDamageData> TakeDamageStatsEffect;
        public Action<WReadOnlyDamageData, DDamageData> CalculateFinalDamage;
        public Action<WReadOnlyStatsData> AddModifier;
        public Action<WReadOnlyStatsData> RemoveModifier;
        #endregion

        #region Func
        public Func<bool> CanEnyityBeDamage;
        public Func<EStatType, float> CheckOffensiveStat;
        public Func<EStatType, float> CheckDefensiveStat;
        public Func<EStatType, float> CheckMagicStat;
        #endregion       

        #region RunTimeStats
        [SerializeField] protected float currentHealth;//必须使用UpdateCurrentHealth进行修改
        protected bool isIgnite;
        protected bool isChill;
        protected bool isShock;
        protected float accuracy = 100;
        #endregion

        protected virtual void Awake()
        {
            damageData = new DDamageData();
            takeDamageData = new DDamageData();

            entity = GetComponentInParent<IStatEntity>();
            Assert.IsNotNull(entity, "数值系统必须附加在一个Entity上");
            CanEnyityBeDamage += entity.CanBeDamage;  
        }

        protected virtual void Start()
        {
            UpdateCurrentHealth(InvokeFunc(CheckDefensiveStat, EStatType.MaxHealth));
        }

        protected virtual void ComponentValidCheck()
        {
            Assert.IsNotNull(GetComponent<CEntityDefensiveStats>(), "缺少防御数值组件");
            Assert.IsNotNull(GetComponent<CEntityPhysicsStats>(), "缺少物理攻击数值组件");
            Assert.IsNotNull(GetComponent<CEntityMagicStats>(), "缺少魔法攻击数值组件");
        }
        WReadOnlyDamageData IEntityStats.GetPrimaryAttackData()
        {
            damageData.damageSourceTransform = transform;
            if(UnityEngine.Random.Range(0, 100) < accuracy)
            {
                InvokeAction(CalculatePrimaryAttackData, damageData);
            }
            else
            {
                damageData.physical = 0;
                damageData.magical = 0;
            }
            return new WReadOnlyDamageData(damageData);
        }

        WReadOnlyDamageData IEntityStats.CalculateDamageTaken(WReadOnlyDamageData _damageData)
        {
            takeDamageData.damageSourceTransform = _damageData.data.damageSourceTransform;
            InvokeAction(CalculateFinalDamage, _damageData, takeDamageData);
            CalculateTakeDamageAilmentArg(_damageData);
            return new WReadOnlyDamageData(takeDamageData);
        }
        private void CalculateTakeDamageAilmentArg(WReadOnlyDamageData _damageData)
        {
            if (isIgnite || isChill || isShock)
            {
                takeDamageData.ignite = false;
                takeDamageData.chill = false;
                takeDamageData.shock = false;
                return;
            }

            takeDamageData.ignite = _damageData.data.ignite;
            takeDamageData.chill = _damageData.data.chill;
            takeDamageData.shock = _damageData.data.shock;
            takeDamageData.igniteDuration = _damageData.data.igniteDuration;
            takeDamageData.igniteDamage = _damageData.data.igniteDamage;
            takeDamageData.igniteDamageCooldown = _damageData.data.igniteDamageCooldown;
            takeDamageData.chillDuration = _damageData.data.chillDuration;
            takeDamageData.chillReduceArmorPer = _damageData.data.chillReduceArmorPer;
            takeDamageData.chillSlowPercentage = _damageData.data.chillSlowPercentage;
            takeDamageData.shockDuration = _damageData.data.shockDuration;
            takeDamageData.shockReduceAccuracy = _damageData.data.shockReduceAccuracy;
            takeDamageData.thunderStrikeCounter = _damageData.data.thunderStrikeCounter;
            takeDamageData.thunderStrikeRadius = _damageData.data.thunderStrikeRadius;
            takeDamageData.thunderStrikeRate = _damageData.data.thunderStrikeRate;
        }

        void IEntityStats.TakeDamage(WReadOnlyDamageData _damage)
        {
            float finalDamage = takeDamageData.physical + takeDamageData.magical;
            if (finalDamage <= 0)
            {
                return;
            }

            UpdateCurrentHealth(currentHealth - finalDamage);
            ApplyAilment(_damage);
        }
        protected void ApplyAilment(WReadOnlyDamageData _damageData)
        {
            if (isIgnite || isChill || isShock)
            {
                return;
            }

            if (_damageData.data.ignite)
            {
                StartCoroutine(IgniteHelper(_damageData));
            }
            else if (_damageData.data.chill)
            {
                StartCoroutine(ChillHelper(_damageData));
            }
            else if (_damageData.data.shock)
            {
                StartCoroutine(ShockHelper(_damageData));
            }

            InvokeAction(TakeDamageStatsEffect, _damageData);
        }
        protected IEnumerator IgniteHelper(WReadOnlyDamageData _damageData)
        {
            isIgnite = true;
            StartCoroutine(IgniteHealthDamageHelper(_damageData));
            yield return new WaitForSeconds(_damageData.data.igniteDuration);
            isIgnite = false;
        }
        protected IEnumerator IgniteHealthDamageHelper(WReadOnlyDamageData _damageData)
        {
            while(isIgnite)
            {
                UpdateCurrentHealth(currentHealth - _damageData.data.igniteDamage);
                yield return new WaitForSeconds(_damageData.data.igniteDamageCooldown);
            }
        }
        protected IEnumerator ChillHelper(WReadOnlyDamageData _damageData)
        {
            isChill = true;
            entity.SlowEntityByDuring(_damageData.data.chillSlowPercentage, _damageData.data.chillDuration);
            yield return new WaitForSeconds(_damageData.data.chillDuration);
            isChill = false;
        }
        protected IEnumerator ShockHelper(WReadOnlyDamageData _damageData)
        {
            isShock = true;
            accuracy -= _damageData.data.shockReduceAccuracy;
            yield return new WaitForSeconds(_damageData.data.shockDuration);
            accuracy += _damageData.data.shockReduceAccuracy;
            isShock = false;
        }

        void IEntityStats.AddStatModifier(WReadOnlyStatsData _data)
        {
            InvokeAction(AddModifier, _data);
        }

        void IEntityStats.RemoveStatModifier(WReadOnlyStatsData _data)
        {
            InvokeAction(RemoveModifier, _data);
        }

        public virtual float TryCheckStat(EStatType _type)
        {
            float stat = InvokeFunc(CheckOffensiveStat, _type);
            if (!float.IsNaN(stat)) return stat;
            stat = InvokeFunc(CheckDefensiveStat, _type);
            if (!float.IsNaN(stat)) return stat;
            stat = InvokeFunc(CheckMagicStat, _type);
            if (!float.IsNaN(stat)) return stat;
            return float.NaN;
        }
        
        protected void UpdateCurrentHealth(float _current)
        {
            float maxHealth = InvokeFunc(CheckDefensiveStat, EStatType.MaxHealth);
            if (_current > maxHealth)
            {
                _current = maxHealth;
            }
            if(currentHealth == _current)
            {
                return;
            }
            currentHealth = _current;
            entity.CurrentHealthUpdate(_current / maxHealth);
            if(currentHealth <= 0)
            {
                entity.Die();
            }
        }

        public void SelfHealByPercent(float _rate)
        {
            UpdateCurrentHealth(currentHealth + InvokeFunc(CheckDefensiveStat, EStatType.MaxHealth) * _rate);
        }
    }
}