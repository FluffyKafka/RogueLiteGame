using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace SkillSystem
{
    internal class SMInput : SMSkillModel
    {
        public Action<int> SkillInput;
        protected override void Awake()
        {
            base.Awake();
            manager.SkillInputNotice += SkillInputNotice;
        }

        protected void SkillInputNotice(int _input)
        {
            InvokeAction(SkillInput, _input);
        }
    }
}
