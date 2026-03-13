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

            input.SkillInputEndNotice += SkillInputEndHandle;
            input.SkillInputBeginNotice += SkillInoutBeginHandle;
        }

        protected void SkillInputEndHandle(int _input)
        {
            if(_input == skillEffectInput)
            {
                EffectEnd?.Invoke();
            }
        }
        protected void SkillInoutBeginHandle(int _input)
        {
            if (_input == skillEffectInput)
            {
                EffectBegin?.Invoke();
            }
        }

        public int CheckInputIndex()
        {
            return skillEffectInput;
        }
    }
}