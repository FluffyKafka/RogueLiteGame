using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace SkillSystem
{
    internal class SMDash : SMSkillModel
    {
        public enum EDashTime
        {
            DashBegin,
            DashEnd
        }

        [SerializeField] protected float dashSpeed;
        [SerializeField] protected float dashDuration;

        public Action<EDashTime> DashTimeNotice;

        public void Dash()
        {
            StartCoroutine(DashHelper());
        }
        protected IEnumerator DashHelper()
        {
            manager.DashBegin?.Invoke(dashSpeed);
            DashTimeNotice?.Invoke(EDashTime.DashBegin);
            yield return new WaitForSeconds(dashDuration);
            manager.DashEnd?.Invoke();
            DashTimeNotice?.Invoke(EDashTime.DashEnd);
        }
    }
}

