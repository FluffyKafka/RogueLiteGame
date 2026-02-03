using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace SkillSystem
{
    internal class SMSkillModel : ComponentManagerBase
    {
        protected MSkillManager manager;
    
        protected virtual void Awake()
        {
            manager = GetComponent<MSkillManager>();
        }
    }
}

