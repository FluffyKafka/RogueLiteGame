using PlayerSystem;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

namespace UISystem
{
    internal class CSkillTreeUI : CUIComponentBase
    {
        [Header("UI组件")]
        public RectTransform skillTreeContainer;
        public GameObject skillSlotPrefab;

        [Header("布局设置")]
        public float horizontalSpacing = 150f;
        public float verticalSpacing = 120f;
        public Vector2 startPosition = new Vector2(100, -100);

        // 从技能系统获取的数据
        protected List<DSkillEntityUIData> systemSkillData = new List<DSkillEntityUIData>();

        // UI层技能数据
        protected Dictionary<string, CDUISkillData> uiSkillData = new Dictionary<string, CDUISkillData>();
        protected Dictionary<string, SLSkillSlotUI> skillSlots = new Dictionary<string, SLSkillSlotUI>();

        protected SLSkillSlotUI currentHoveredSkill;
        protected SLSkillSlotUI currentSelectedSkill; // 当前选中的技能

        protected bool isInit = false;

        protected override void OnEnable()
        {
            base.OnEnable();
            Debug.Log("Init");
            if (!isInit)
            {
                systemSkillData = ui.CheckAllSkillEntity();
                RefreshSkillTree();
                isInit = true;
            }
            UpdateSkillsUnlockStatus();
        }

        // 刷新整个技能树
        public void RefreshSkillTree()
        {
            // 创建UI层数据
            UpdateUISkillData();

            // 计算层级
            CalculateHierarchyLevels();

            // 重建UI
            RebuildSkillTree();
        }

        private void UpdateUISkillData()
        {
            uiSkillData.Clear();
            // 处理所有技能
            foreach (var systemData in systemSkillData)
            {
                uiSkillData[systemData.id] = new CDUISkillData(systemData);
            }
        }

        private void CalculateHierarchyLevels()
        {
            // 清除所有层级标记
            foreach (var skill in uiSkillData.Values)
            {
                skill.hierarchyLevel = -1;
            }

            // 计算每个技能的层级
            foreach (var skill in uiSkillData.Values)
            {
                CalculateSkillLevel(skill);
            }
        }

        private int CalculateSkillLevel(CDUISkillData skill)
        {
            if (skill.hierarchyLevel != -1)
                return skill.hierarchyLevel;

            if (skill.dependIds.Count == 0)
            {
                skill.hierarchyLevel = 0;
                return 0;
            }

            int maxPrereqLevel = -1;
            foreach (var dependId in skill.dependIds)
            {
                if (uiSkillData.ContainsKey(dependId))
                {
                    int prereqLevel = CalculateSkillLevel(uiSkillData[dependId]);
                    maxPrereqLevel = Mathf.Max(maxPrereqLevel, prereqLevel);
                }
            }

            skill.hierarchyLevel = maxPrereqLevel + 1;
            return skill.hierarchyLevel;
        }

        private void RebuildSkillTree()
        {
            // 清除现有内容
            ClearSkillTree();

            // 按层级组织技能
            var skillsByLevel = new Dictionary<int, List<CDUISkillData>>();
            foreach (var skill in uiSkillData.Values)
            {
                if (!skillsByLevel.ContainsKey(skill.hierarchyLevel))
                    skillsByLevel[skill.hierarchyLevel] = new List<CDUISkillData>();

                skillsByLevel[skill.hierarchyLevel].Add(skill);
            }

            // 布局技能
            foreach (var level in skillsByLevel.Keys.OrderBy(l => l)/*从0到n，一层层绘制*/)
            {
                var skillsAtLevel = skillsByLevel[level];

                // 可以在同一层内根据依赖关系进行排序，让相关技能靠近
                skillsAtLevel = SortSkillsByDependencies(skillsAtLevel);

                //对于层内的每个具体的技能，计算其位置并生成技能槽
                for (int i = 0; i < skillsAtLevel.Count; i++)
                {
                    var skill = skillsAtLevel[i];

                    // 计算位置
                    float x = startPosition.x + i * horizontalSpacing;
                    float y = startPosition.y - level * verticalSpacing;
                    skill.uiPosition = new Vector2(x, y);

                    // 创建技能槽
                    CreateSkillSlot(skill);
                }
            }
        }

        private List<CDUISkillData> SortSkillsByDependencies(List<CDUISkillData> skills)
        {
            // 简单的排序：将有共同依赖的技能放在一起
            //return skills.OrderBy(s => s.dependIds.Count).ToList();
            return skills;
        }

        private void CreateSkillSlot(CDUISkillData skill)
        {
            GameObject slotObj = Instantiate(skillSlotPrefab, skillTreeContainer);
            RectTransform rect = slotObj.GetComponent<RectTransform>();
            rect.anchoredPosition = skill.uiPosition;

            SLSkillSlotUI slotUI = slotObj.GetComponent<SLSkillSlotUI>();
            slotUI.Initialize(skill, this);

            skillSlots[skill.id] = slotUI;
        }

        private void ClearSkillTree()
        {
            foreach (Transform child in skillTreeContainer)
            {
                Destroy(child.gameObject);
            }

            skillSlots.Clear();
        }

        // 当鼠标悬停在技能上时显示关系（现在只用于调试）
        public void OnSkillHover(SLSkillSlotUI hoveredSkill)
        {
            if (hoveredSkill == null) return;
            currentHoveredSkill = hoveredSkill;
            // 可以在这里添加悬停时的其他效果，比如显示提示框等
        }

        // 处理技能点击事件
        public void OnSkillClicked(SLSkillSlotUI clickedSkill)
        {
            if (clickedSkill == null) return;

            // 如果点击的是同一个技能，取消高亮和选中状态
            if (currentSelectedSkill == clickedSkill)
            {
                // 取消当前选中技能的选中状态
                if (currentSelectedSkill != null)
                {
                    currentSelectedSkill.SetSelected(false);
                }

                ClearAllHighlights();
                currentSelectedSkill = null;
                return;
            }

            // 取消之前选中技能的选中状态
            if (currentSelectedSkill != null)
            {
                currentSelectedSkill.SetSelected(false);
            }

            // 清除之前的高亮
            ClearAllHighlights();

            // 设置新的选中技能并放大
            currentSelectedSkill = clickedSkill;
            currentSelectedSkill.SetSelected(true);

            // 高亮相关技能
            HighlightRelatedSkills(clickedSkill.uiSkillData);
        }

        // 清除所有技能的高亮
        private void ClearAllHighlights()
        {
            foreach (var skill in uiSkillData.Values)
            {
                skill.highlightType = HighlightType.None;
            }

            // 更新所有技能槽的边框显示
            foreach (var slot in skillSlots.Values)
            {
                slot.UpdateHighlightState();
            }
        }

        // 高亮相关技能
        private void HighlightRelatedSkills(CDUISkillData skill)
        {
            // 高亮前置技能（蓝色）
            foreach (string dependId in skill.dependIds)
            {
                if (uiSkillData.ContainsKey(dependId))
                {
                    uiSkillData[dependId].highlightType = HighlightType.Prerequisite;
                }
            }

            // 高亮互斥技能（红色）
            foreach (string conflictId in skill.conflictIds)
            {
                if (uiSkillData.ContainsKey(conflictId))
                {
                    uiSkillData[conflictId].highlightType = HighlightType.Conflict;
                }
            }

            // 高亮后置技能（依赖当前技能的技能）（绿色）
            foreach (var otherSkill in uiSkillData.Values)
            {
                if (otherSkill.dependIds.Contains(skill.id))
                {
                    otherSkill.highlightType = HighlightType.Dependent;
                }
            }

            // 更新所有技能槽的边框显示
            foreach (var slot in skillSlots.Values)
            {
                slot.UpdateHighlightState();
            }
        }

        // 更新单个技能的解锁状态
        public void SetSkillUnlocked(string skillId, bool unlocked)
        {
            if (uiSkillData.ContainsKey(skillId))
            {
                uiSkillData[skillId].isUnlocked = unlocked;

                if (skillSlots.ContainsKey(skillId))
                {
                    skillSlots[skillId].UpdateVisualState();
                }
            }
        }

        // 批量更新技能解锁状态
        public void UpdateSkillsUnlockStatus()
        {
            List<DSkillUnlockData> skillUnlockData = ui.CheckAllSkillUnlockState();
            foreach (var kvp in skillUnlockData)
            {
                if (uiSkillData.ContainsKey(kvp.skillId))
                {
                    uiSkillData[kvp.skillId].isUnlocked = kvp.isUnlock;

                    if (skillSlots.ContainsKey(kvp.skillId))
                    {
                        skillSlots[kvp.skillId].UpdateVisualState();
                    }
                }
            }
        }

        // 获取当前选中的技能
        public SLSkillSlotUI GetCurrentSelectedSkill()
        {
            return currentSelectedSkill;
        }
    }
}