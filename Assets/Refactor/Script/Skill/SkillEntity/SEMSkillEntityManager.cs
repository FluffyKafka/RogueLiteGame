using PlayerSystem;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

namespace SkillSystem
{
    internal class SEMSkillEntityManager : MonoBehaviour
    {
        [SerializeField] protected string assetDirectorPath;

        [Serializable]
        protected struct DSkillEntityState
        {
            public SESkillEntity entity;
            public bool isUnlock;
            public DSkillEntityState(SESkillEntity _entity)
            {
                entity = _entity;
                isUnlock = false;
            }
        }
        [SerializeField] protected List<DSkillEntityState> skillEnetities;
        protected Dictionary<string, DSkillEntityState> skillDictionary;

        protected void Awake()
        {            
            foreach (var se in skillEnetities)
            {
                se.entity.Init(se.isUnlock, GetComponent<MSkillManager>());
            }

            MSkillManager manager = GetComponent<MSkillManager>();
            manager.ShowAllSkillEntityToUINotice += ShowAllSkillEntityToUi;
            manager.CheckAllSkillUnlockStateNotice += CheckAllSkillUnlockState;
            manager.InitSkillNotice += InitSkillById;
            manager.CheckSkillsHaveCooldownToUiNotice += CheckSkillsHaveCooldownToUi;

            skillDictionary = new();
            foreach (var skill in skillEnetities)
            {
                skillDictionary.Add(skill.entity.CheckId(), skill);
            }
        }

        protected List<DSkillEntityUIData> ShowAllSkillEntityToUi()
        {
            List<DSkillEntityUIData> res = new();
            foreach(var skill in skillEnetities)
            {
                SESkillEntity se = skill.entity;
                res.Add(new DSkillEntityUIData(se.CheckId(), se.CheckName(), se.CheckIcon(), se.CheckDescription(), se.CheckPrice(), se.CheckDependSkillIds(), se.CheckConflictSkillIds()));
            }
            return res;
        }

        protected List<DSkillUnlockData> CheckAllSkillUnlockState()
        {
            List<DSkillUnlockData> res = new();
            foreach(var skill in skillEnetities)
            {
                SESkillEntity se = skill.entity;
                res.Add(new DSkillUnlockData(se.CheckId(), se.IsUnlock()));
            }
            return res;
        }

        protected void InitSkillById(string _id, bool _isUnlock)
        {
            skillDictionary[_id].entity.Init(_isUnlock, GetComponent<MSkillManager>());
        }

        protected List<IUISkill> CheckSkillsHaveCooldownToUi()
        {
            List<IUISkill> res = new();
            foreach(var skill in skillEnetities)
            {
                if(skill.entity.IsSkillHaveCooldown() && skill.isUnlock)
                {
                    res.Add(skill.entity);
                }
            }
            return res;
        }

#if UNITY_EDITOR
        [ContextMenu("Update Skill Components")]
        protected void UpdateSCDatabase()
        {
            string[] assetNames = AssetDatabase.FindAssets("t:SESkillEntity", new[] { assetDirectorPath });
            skillEnetities.Clear();

            foreach (string SOName in assetNames)
            {
                var SOPath = AssetDatabase.GUIDToAssetPath(SOName);
                var se = AssetDatabase.LoadAssetAtPath<SESkillEntity>(SOPath);
                skillEnetities.Add(new DSkillEntityState(se));
            }
        }
#endif
    }
}
