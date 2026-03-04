using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace SkillSystem
{
    [CreateAssetMenu(fileName = "New Skill Component", menuName = "Skill System/Skill Component/EffectTime/CounterAttackSkillTime")]
    internal class SCCounterAttackSkillTime : SCEffectTimeBase
    {
        [SerializeField] SMCounterAttack.ECounterAttackTime time;

        protected SMCounterAttack counter;
        public override void Init(SMSkillModel[] _modelManager)
        {
            base.Init(_modelManager);
            counter = TryGetModel<SMCounterAttack>(_modelManager);
            counter.CounterAttackTimeNotice += CounterAttackTimeCheck;
        }

        protected void CounterAttackTimeCheck(SMCounterAttack.ECounterAttackTime _time)
        {
            if (_time == time)
            {
                EffectBegin?.Invoke();
            }
        }
    }
}

