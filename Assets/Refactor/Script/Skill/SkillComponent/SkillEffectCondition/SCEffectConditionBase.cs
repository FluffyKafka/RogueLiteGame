using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace SkillSystem
{
    internal class SCEffectConditionBase : SCBase
    {

        public override void Init(SMSkillModel[] _modelManager)
        {
            
        }

        public virtual bool CanEffect(string _id)
        {
            return true;
        }
    }
}

