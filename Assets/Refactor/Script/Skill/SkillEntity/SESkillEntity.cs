using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEditor;
using UnityEngine;
using UnityEngine.Assertions;

namespace SkillSystem
{
    [CreateAssetMenu(fileName = "New Skill Entity", menuName = "Skill System/Skill Entity")]
    internal class SESkillEntity : ScriptableObject
    {
        [Header("Skill Detail")]
        [SerializeField] protected string skillId;
        [SerializeField] protected string skillName;
        [SerializeField] protected Sprite icon;
        [SerializeField][TextArea] protected string description;
        [SerializeField] protected float price;

        [Header("Skill Effect ")]
        [SerializeField] protected List<SCUnlockConditionBase> unlockConditions;
        [SerializeField] protected List<SCEffectConditionBase> effectConditions;
        [SerializeField] protected List<SCUnlockEffectBase> unlockEffects;
        [SerializeField] protected List<SCAimmingEffectBase> aimmingEffects;
        [SerializeField] protected List<SCEffectBase> effects;
        [SerializeField] protected List<SCEffectTimeBase> effectTimes;

        [Header("Test")]
        [SerializeField] protected bool isUnlock = false;

#if UNITY_EDITOR
        [ContextMenu("Generate SkillId")]
        public void GetGUID()
        {
            string path = AssetDatabase.GetAssetPath(this);
            skillId = AssetDatabase.AssetPathToGUID(path);
        }
#endif
        public void Init(bool _isUnlock)
        {
            isUnlock = _isUnlock;
            if(isUnlock)
            {
                UnlockSkill();
            }
        }

        public bool CanUnlock()
        {
            foreach (var unlock in unlockConditions)
            {
                if (!unlock.CanUnlock())
                {
                    return false;
                }
            }
            return true;
        }

        public bool TryUnlock()
        {
            foreach (var unlock in unlockConditions)
            {
                if (!unlock.CanUnlock())
                {
                    return false;
                }
            }

            UnlockSkill();

            return true;
        }
        protected void UnlockSkill()
        {
            foreach (var effect in unlockEffects)
            {
                effect.Effect();
            }

            if (aimmingEffects.Count == 0)
            {
                foreach (var time in effectTimes)
                {
                    time.EffectBegin += TryEffect;
                }
            }
            else
            {
                foreach (var time in effectTimes)
                {
                    time.EffectBegin += TryAimmingBegin;
                    time.EffectEnd += AimmingEnd;
                    time.EffectEnd += TryEffect;
                }
            }
        }

        protected void TryAimmingBegin()
        {            
            foreach (var condition in effectConditions)
            {
                if (!condition.CanEffect(skillId))
                {
                    return;
                }
            }

            foreach (var aim in aimmingEffects)
            {
                aim.AimmingStart(skillId);
            }
        }

        protected void AimmingEnd()
        {
            foreach (var aim in aimmingEffects)
            {
                aim.AimmingFinish(skillId);
            }
        }

        protected void TryEffect()
        {
            foreach(var condition in effectConditions)
            {
                if(!condition.CanEffect(skillId))
                {
                    return;
                }
            }

            foreach(var effect in effects)
            {
                effect.Effect(skillId);
            }
        }

        public string CheckId()
        {
            return skillId;
        }
        public string CheckName()
        {
            return skillName;
        }
        public Sprite CheckIcon()
        {
            return icon;
        }
        public float CheckPrice()
        {
            return price;
        }

        public string CheckDescription()
        {
            return description;
        }

        public float TryCheckCooldownPer()
        {
            SCCooldown cooldown = TryGetSkillComponenet<SCCooldown, SCEffectConditionBase>(effectConditions);
            Assert.IsNotNull(cooldown, "此技能无冷却时间");
            return cooldown.CheckCooldownPer(skillId);
        }

        public float TryCheckCooldownRest()
        {
            SCCooldown cooldown = TryGetSkillComponenet<SCCooldown, SCEffectConditionBase>(effectConditions);
            Assert.IsNotNull(cooldown, "此技能无冷却时间");
            return cooldown.CheckCooldownRest(skillId);
        }
        protected T1 TryGetSkillComponenet<T1, T2>(List<T2> _scs) 
            where T1 : SCBase
            where T2 : SCBase
        {
            foreach(var sc in _scs)
            {
                if(sc is T1)
                {
                    return sc as T1;
                }
            }
            return null;
        }

        public bool IsUnlock()
        {
            return isUnlock;
        }

        public List<string> CheckDependSkillIds()
        {
            SCSkillDepend dp = TryGetSkillComponenet<SCSkillDepend, SCUnlockConditionBase>(unlockConditions);
            if(dp == null)
            {
                return new List<string>();
            }
            return dp.CheckDependIds();
        }
        public List<string> CheckConflictSkillIds()
        {
            SCSkillConflict cf = TryGetSkillComponenet<SCSkillConflict, SCUnlockConditionBase>(unlockConditions);
            if(cf == null)
            {
                return new List<string>();
            }
            return cf.checkConflictIds();
        }
    }
}

