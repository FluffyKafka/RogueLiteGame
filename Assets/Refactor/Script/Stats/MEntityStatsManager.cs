using EntitySystem.EntityActor;
using System;
using System.Collections;
using UnityEngine;
using UnityEngine.Assertions;

namespace StatsSystem
{
    public enum EStatType
    {
        MaxHealth,
        Armor,
        Evasion,
        MagicResistance,
        Damage,
        CritChance,
        CritPower,
        AttackSpeed,
        FireDamage,
        IceDamage,
        LightningDamage,
        FireDuration,
        IceDuration,
        LightningDuration,
        FireDamageCooldown,
        FireDamageTransform,
        ChillArmorReduce,
        ChillSlowRate,
        ShockAccuracyReduce,
        ThunderStrikeCount,
        ThunderStrikeRate,
        ThunderStrikeRadius,
        MaxFlaskUsageTime,
        FlaskUsageRecover
    }

    internal class MEntityStatsManager : MonoBehaviour, IEntityStats
    {
        protected DDamageData damageData;
        protected DDamageData takeDamageData;
        protected IStatEntity entity;

        #region Action
        public Action<DDamageData> CalculatePrimaryAttackData;
        public Action<WReadOnlyDamageData> TakeDamageStatsEffect;
        public Action<WReadOnlyDamageData, DDamageData> CalculateFinalDamage;
        #endregion

        #region Func
        public Func<bool> CanEnyityBeDamage;
        public Func<EStatType, float> CheckOffensiveStat;
        public Func<EStatType, float> CheckDefensiveStat;
        public Func<EStatType, float> CheckMagicStat;
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
        public void InvokeAction<T1, T2>(Action<T1, T2> _action, T1 _arg1, T2 _arg2)
        {
            _action?.Invoke(_arg1, _arg2);
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

        #region RunTimeStats
        protected float currentHealth;
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
            currentHealth = InvokeFunc(CheckDefensiveStat, EStatType.MaxHealth);
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

            currentHealth -= finalDamage;
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
                currentHealth -= _damageData.data.igniteDamage;
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
    }
}