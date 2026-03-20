using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace SkillSystem
{
    [CreateAssetMenu(fileName = "New Skill Entity", menuName = "Skill System/Skill Component/EffectCondition/Can Throw Sword")]
    internal class SCCheckIsSwordThrown : SCEffectConditionBase
    {
        [SerializeField] protected bool CanEffectIfNotThrow = true;
        [SerializeField] protected string swordThrownText = "剑已经被投出...";
        [SerializeField] protected string swordNoThrowText = "剑尚未被投出...";

        protected SMSword sword;

        public override void Init(SMSkillModel[] _modelManager)
        {
            base.Init(_modelManager);
            sword = TryGetModel<SMSword>(_modelManager);
        }

        public override bool CanEffect(string _id, bool _isShowPopUpText = false)
        {
            if(CanEffectIfNotThrow)
            {
                if(sword.IsSwordThrown())
                {
                    return true;
                }
                else
                {
                    if(_isShowPopUpText)
                    {
                        sword.GeneratePopUpText(swordThrownText);
                    }
                    return false;
                }
            }
            else
            {
                if(!sword.IsSwordThrown())
                {
                    return true;
                }
                else
                {
                    if(_isShowPopUpText)
                    {
                        sword.GeneratePopUpText(swordNoThrowText);
                    }
                    return false;
                }
            }
        }
    }

}
