using PlayerSystem;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace SkillSystem
{
    internal class MSkillManager : ComponentManagerBase, IPlayerSkillManager
    {
        protected ISkillManagerPlayer player;

        public Action<int> SkillInputNotice;

        #region Dash
        public Action DashEnd;
        public Action<float> DashBegin;
        #endregion

        protected void Awake()
        {
            DashBegin += player.DashBegin;
            DashEnd += player.DashEnd;
        }
        public void SkillInput(int _input)
        {
            InvokeAction(SkillInputNotice, _input);
        }
        public bool CanEffectBehaviourSkill()
        {
            return player.CanEffectBehaviourSkill();
        }
    }
}