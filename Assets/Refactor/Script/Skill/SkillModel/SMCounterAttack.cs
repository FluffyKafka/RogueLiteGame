using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace SkillSystem
{
    internal class SMCounterAttack : SMSkillModel
    {
        [SerializeField] float counterDuration;
        [SerializeField] float counterSuccessHealPercent;

        public enum ECounterAttackTime
        {
            CounterAttackBegin,
            CounterAttackFail,
            CounterAttackSuccess
        }
        public Action<ECounterAttackTime> CounterAttackTimeNotice;

        [Header("Test")]
        [SerializeField] protected bool isCounterSuccessHeal = false;
        protected Coroutine counterFailAfter;

        protected override void Awake()
        {
            base.Awake();
            manager.CounterAttackSuccessNotice += CounterSuccess;
        }
        protected void CounterSuccess()
        {
            StopCoroutine(counterFailAfter);
            CounterAttackTimeNotice?.Invoke(ECounterAttackTime.CounterAttackSuccess);
            if (isCounterSuccessHeal)
            {
                manager.SelfHealByPercent(counterSuccessHealPercent);                
            }
        }

        public void BeginCounter()
        {
            counterFailAfter = StartCoroutine(CounterAttackHelper());
        }
        protected IEnumerator CounterAttackHelper()
        {
            manager.CounterAttackBegin();
            CounterAttackTimeNotice?.Invoke(ECounterAttackTime.CounterAttackBegin);
            yield return new WaitForSeconds(counterDuration);
            manager.CounterAttackEnd();
            CounterAttackTimeNotice?.Invoke(ECounterAttackTime.CounterAttackFail);
        }

        public void SetCounterSuccessHeal()
        {
            isCounterSuccessHeal = true;
        }
    }
}

