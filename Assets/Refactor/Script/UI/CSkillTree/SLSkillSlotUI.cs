using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

namespace UISystem
{
    internal class SLSkillSlotUI : CUIComponentBase, IPointerEnterHandler, IPointerExitHandler, IPointerClickHandler
    {
        [Header("UI组件")]
        public Image iconImage;
        public Button skillButton;

        [Header("边框组件")]
        public Image borderImage; // 技能槽的边框组件
        public Color normalBorderColor = Color.clear; // 无高亮时透明
        public Color prerequisiteBorderColor = Color.blue; // 前置技能边框颜色
        public Color dependentBorderColor = Color.green; // 后置技能边框颜色
        public Color conflictBorderColor = Color.red; // 互斥技能边框颜色

        [Header("状态颜色")]
        public Color lockedColor = Color.gray;
        public Color unlockedColor = Color.white;

        [Header("缩放设置")]
        public float normalScale = 1f;
        public float selectedScale = 1.2f; // 选中时的缩放比例
        public float hoverScale = 1.1f; // 悬停时的缩放比例
        public float scaleAnimationSpeed = 8f; // 缩放动画速度

        public CDUISkillData uiSkillData { get; private set; }
        private CSkillTreeUI skillTreeUI;

        private RectTransform rectTransform;
        private Vector3 targetScale;
        private bool isSelected = false;
        private bool isHovered = false;

        protected override void Awake()
        {
            base.Awake();
            if (skillButton != null)
                skillButton.onClick.AddListener(OnSkillClicked);

            rectTransform = GetComponent<RectTransform>();
            if (rectTransform == null)
                rectTransform = gameObject.AddComponent<RectTransform>();

            targetScale = Vector3.one * normalScale;
        }

        void Update()
        {
            // 平滑过渡到目标缩放
            if (rectTransform != null)
            {
                rectTransform.localScale = Vector3.Lerp(
                    rectTransform.localScale,
                    targetScale,
                    Time.deltaTime * scaleAnimationSpeed
                );
            }
        }

        public void Initialize(CDUISkillData skill, CSkillTreeUI treeUI)
        {
            uiSkillData = skill;
            skillTreeUI = treeUI;

            UpdateVisualState();
            UpdateDisplayInfo();
            UpdateHighlightState(); // 初始化高亮状态
        }

        public void UpdateVisualState()
        {
            if (uiSkillData.isUnlocked)
            {
                iconImage.color = unlockedColor;
            }
            else
            {
                iconImage.color = lockedColor;
            }
        }

        private void UpdateDisplayInfo()
        {
            if (iconImage != null && uiSkillData.icon != null)
                iconImage.sprite = uiSkillData.icon;
        }

        // 更新高亮状态
        public void UpdateHighlightState()
        {
            if (borderImage == null) return;

            switch (uiSkillData.highlightType)
            {
                case HighlightType.Prerequisite:
                    borderImage.color = prerequisiteBorderColor;
                    borderImage.enabled = true;
                    break;
                case HighlightType.Dependent:
                    borderImage.color = dependentBorderColor;
                    borderImage.enabled = true;
                    break;
                case HighlightType.Conflict:
                    borderImage.color = conflictBorderColor;
                    borderImage.enabled = true;
                    break;
                default:
                    borderImage.color = normalBorderColor;
                    borderImage.enabled = false; // 完全隐藏边框
                    break;
            }
        }

        // 设置选中状态
        public void SetSelected(bool selected)
        {
            isSelected = selected;
            UpdateTargetScale();
        }

        // 更新目标缩放
        private void UpdateTargetScale()
        {
            if (isSelected)
            {
                targetScale = Vector3.one * selectedScale;
            }
            else if (isHovered)
            {
                targetScale = Vector3.one * hoverScale;
            }
            else
            {
                targetScale = Vector3.one * normalScale;
            }
        }

        void OnSkillClicked()
        {
            Debug.Log($"Clicked skill: {uiSkillData.name} (ID: {uiSkillData.id})");

            // 通知技能树UI处理点击事件
            if (skillTreeUI != null)
                skillTreeUI.OnSkillClicked(this);

            // 显示技能详情
            ShowSkillDetails();
        }

        private void ShowSkillDetails()
        {
            Debug.Log($"Skill Details:\nName: {uiSkillData.name}\nDescription: {uiSkillData.description}");
            ui.InvokeAction(ui.HideTooltip);
            ui.InvokeAction(ui.ShowSkillDetailNotice, new DSkillDetail(uiSkillData.name, uiSkillData.icon, uiSkillData.price, uiSkillData.description));
        }

        public void OnPointerEnter(PointerEventData eventData)
        {
            isHovered = true;
            UpdateTargetScale();

            if (skillTreeUI != null)
                skillTreeUI.OnSkillHover(this);
        }

        public void OnPointerExit(PointerEventData eventData)
        {
            isHovered = false;
            UpdateTargetScale();

            if (skillTreeUI != null)
                skillTreeUI.OnSkillHover(null);
        }

        // 实现IPointerClickHandler接口的OnPointerClick方法
        public void OnPointerClick(PointerEventData eventData)
        {
            // 可选：如果不想使用Button，可以用这个
            // OnSkillClicked();
        }
    }
}