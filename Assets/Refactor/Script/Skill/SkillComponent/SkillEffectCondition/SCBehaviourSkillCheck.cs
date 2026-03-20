using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace SkillSystem
{
    [CreateAssetMenu(fileName = "New Skill Entity", menuName = "Skill System/Skill Component/EffectCondition/BehaviourSkillCheck")]
    internal class SCBehaviourSkillCheck : SCEffectConditionBase
    {
        [SerializeField] protected string behaviourSkillCheckFailText = "正在施放其他技能...";

        protected SMPlayer player;

        public override void Init(SMSkillModel[] _modelManager)
        {
            base.Init(_modelManager);
            player = TryGetModel<SMPlayer>(_modelManager);
        }

        public override bool CanEffect(string _id, bool _isShowPopUpText = false)
        {
            if(player.CanEffectBehaviourSkill())
            {
                return true;
            }
            else
            {
                if(_isShowPopUpText)
                {
                    player.GeneratePopUpText(behaviourSkillCheckFailText);
                }
                return false;
            }
        }
    }
}

