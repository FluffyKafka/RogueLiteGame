using PlayerSystem;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace UISystem
{
    [Serializable]
    internal class CDUISkillData
    {
        public string id;
        public string name;
        public Sprite icon;
        public string description;
        public float price;
        public List<string> dependIds; // 前置技能
        public List<string> conflictIds; // 互斥技能

        // UI相关状态
        public bool isUnlocked;
        public bool isVisible = true;
        public int hierarchyLevel = -1;
        public Vector2 uiPosition;

        // 高亮状态
        public HighlightType highlightType = HighlightType.None;

        public CDUISkillData(DSkillEntityUIData _data)
        {
            id = _data.id;
            name = _data.name;
            icon = _data.icon;
            description = _data.description;
            price = _data.price;
            dependIds = new List<string>(_data.dependIds);
            conflictIds = new List<string>(_data.conflictIds);

            isUnlocked = false; // 默认未解锁
        }
    }

    // 高亮类型枚举
    public enum HighlightType
    {
        None,
        Prerequisite, // 前置技能 - 蓝色
        Dependent,    // 后置技能 - 绿色
        Conflict      // 互斥技能 - 红色
    }
}