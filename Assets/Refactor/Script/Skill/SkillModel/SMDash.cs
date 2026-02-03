using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace SkillSystem
{
    internal class SMDash : SMSkillModel
    {
        [SerializeField] protected float dashSpeed;
        [SerializeField] protected float dashDuration;

        public void Dash()
        {
            StartCoroutine(DashHelper());
        }
        protected IEnumerator DashHelper()
        {
            manager.DashBegin?.Invoke(dashSpeed);
            yield return new WaitForSeconds(dashDuration);
            manager.DashEnd?.Invoke();
        }
    }
}

