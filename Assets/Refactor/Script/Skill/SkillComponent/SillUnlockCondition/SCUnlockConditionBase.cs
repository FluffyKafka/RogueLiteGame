using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace SkillSystem
{
    internal class SCUnlockConditionBase : SCBase
    {
        public override void Init(SMSkillModel[] _modelManager)
        {
            
        }

        public virtual bool CanUnlock()
        {
            return true;
        }
    }
}

