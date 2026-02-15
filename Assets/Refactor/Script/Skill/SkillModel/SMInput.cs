using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace SkillSystem
{
    internal class SMInput : SMSkillModel
    {
        public Action<int> SkillInputEndNotice;
        public Action<int> SkillInputBeginNotice;
        protected override void Awake()
        {
            base.Awake();
            manager.SkillInputEndNotice += SkillInputEnd;
            manager.SkillInputBeginNotice += SkillInputBegin;
        }

        protected void SkillInputEnd(int _input)
        {
            InvokeAction(SkillInputEndNotice, _input);
        }

        protected void SkillInputBegin(int _input)
        {
            InvokeAction(SkillInputBeginNotice, _input);
        }

        public Vector3 CheckMousePosition()
        {
            return manager.CheckMousePosition();
        }
    }
}
