using PlayerSystem;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace SkillSystem
{
    public interface IInitSkillManager
    {
        public void Init(ISkillManagerPlayer _player);
    }

    internal class MSkillManager : ComponentManagerBase, IInitSkillManager, IPlayerSkillManager
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

        public void Init(ISkillManagerPlayer _player)
        {
            player = _player;
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