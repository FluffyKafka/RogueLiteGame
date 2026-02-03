using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace SkillSystem
{
    [CreateAssetMenu(fileName = "New Skill Entity", menuName = "Skill System/Skill Component/EffectTime/InputTime")]
    internal class SCInputTime : SCEffectTimeBase
    {
        [SerializeField] protected int skillEffectInput;

        protected SMInput input;
        public override void Init(SMSkillModel[] _modelManager)
        {
            input = TryGetModel<SMInput>(_modelManager);

            input.SkillInput += SkillInputHandle;
        }

        protected void SkillInputHandle(int _input)
        {
            if(_input == skillEffectInput)
            {
                Effect.Invoke();
            }
        }
    }
}