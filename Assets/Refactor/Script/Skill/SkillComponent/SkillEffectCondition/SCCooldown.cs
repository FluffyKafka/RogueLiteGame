using System.Collections;
using System.Collections.Generic;
using System.Security.Cryptography;
using UnityEngine;

namespace SkillSystem
{
    [CreateAssetMenu(fileName = "New Skill Entity", menuName = "Skill System/Skill Component/EffectCondition/Cooldown")]
    internal class SCCooldown : SCEffectConditionBase
    {
        [SerializeField] protected string cooldownText = "ººƒ‹¿‰»¥÷–...";
        [SerializeField] protected float duration;

        protected SMTimer timer;
        public override void Init(SMSkillModel[] _modelManager)
        {
            base.Init(_modelManager);
            timer = TryGetModel<SMTimer>(_modelManager);
        }

        public override bool CanEffect(string _id, bool _isShowPopUpText = false)
        {
            float effectTime = timer.CheckTimer(_id);
            if(effectTime < 0)
            {
                return true;
            }
            else if(Time.time - effectTime > duration)
            {
                return true;
            }
            else
            {
                if(_isShowPopUpText)
                {
                    timer.GeneratePopUpText(cooldownText);
                }             
                return false;
            }
        }

        public float CheckCooldownRest(string _id)
        {
            float effectTime = timer.CheckTimer(_id);
            float pastTime = Time.time - effectTime;
            float restTime = duration - pastTime;
            if(restTime > 0)
            {
                return restTime;
            }
            else
            {
                return 0;
            }
        }

        public float CheckCooldownPer(string _id)
        {
            float effectTime = timer.CheckTimer(_id);
            float pastTime = Time.time - effectTime;
            float restTime = duration - pastTime;
            if(restTime > 0)
            {
                return restTime / duration;
            }
            else
            {
                return 0;
            }
        }
    }
}

