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
        protected IStatEntity entity;

        #region Action
        public Action<DDamageData> CalculatePrimaryAttackData;
        public Action<WReadOnlyDamageData> TakeDamageStatsEffect;
        #endregion

        #region Func
        public Func<WReadOnlyDamageData, float> CalculateFinalDamage;
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
            damageData.damageSource = entity;
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

        float IEntityStats.TakeDamage(WReadOnlyDamageData _damageData)
        {
            float finalDamage = InvokeFunc(CalculateFinalDamage, _damageData);
            if(finalDamage <= 0)
            {
                return 0;
            }

            currentHealth -= finalDamage;
            ApplyAilment(_damageData);
            return finalDamage;
        }
        protected void ApplyAilment(WReadOnlyDamageData _damageData)
        {
            if(isIgnite || isChill || isShock)
            {
                return;
            }

            if(_damageData.data.ignite)
            {
                StartCoroutine(IgniteHelper(_damageData));
            }
            else if(_damageData.data.chill)
            {
                StartCoroutine(ChillHelper(_damageData));
            }
            else if(_damageData.data.shock)
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