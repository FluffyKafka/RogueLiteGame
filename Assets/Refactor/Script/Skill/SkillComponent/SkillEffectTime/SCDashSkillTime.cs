using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace SkillSystem
{
    [CreateAssetMenu(fileName = "New Skill Component", menuName = "Skill System/Skill Component/EffectTime/DashSkillTime")]
    internal class SCDashSkillTime : SCEffectTimeBase
    {
        [SerializeField] SMDash.EDashTime effectTime;

        protected SMDash dash;
        public override void Init(SMSkillModel[] _modelManager)
        {
            base.Init(_modelManager);
            dash = TryGetModel<SMDash>(_modelManager);
            dash.DashTimeNotice += DashTimeCheck;
        }

        protected void DashTimeCheck(SMDash.EDashTime _time)
        {
            if(_time == effectTime)
            {
                EffectBegin?.Invoke();
            }
        }    
    }
}

