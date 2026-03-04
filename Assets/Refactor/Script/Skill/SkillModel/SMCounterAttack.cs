using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace SkillSystem
{
    internal class SMCounterAttack : SMSkillModel
    {
        [SerializeField] float counterDuration;
        protected Coroutine counterFailAfter;

        protected override void Awake()
        {
            base.Awake();
            manager.CounterAttackSuccessNotice += CancelFailNotice;
        }
        protected void CancelFailNotice()
        {
            StopCoroutine(counterFailAfter);
        }

        public void BeginCounter()
        {
            counterFailAfter = StartCoroutine(CounterAttackHelper());
        }
        protected IEnumerator CounterAttackHelper()
        {
            manager.CounterAttackBegin();
            yield return new WaitForSeconds(counterDuration);
            manager.CounterAttackEnd();
        }
    }
}

