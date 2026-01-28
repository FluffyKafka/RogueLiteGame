using StatsData;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.Assertions;
using UnityEngine.EventSystems;

namespace UISystem
{
    internal class SLStatSlot : CUIComponentBase, IPointerEnterHandler, IPointerExitHandler
    {
        [SerializeField] protected EStatType type;
        [SerializeField] protected TextMeshProUGUI statName;
        [SerializeField] protected TextMeshProUGUI statValue;

        protected override void OnEnable()
        {
            base.OnEnable();
            statName.text = ui.InvokeFunc(ui.Translate, type.ToString());
            ui.UpdateStats += UpdateStat;
            UpdateStat();
        }

        protected override void OnDisable()
        {
            base.OnDisable();
        }

        protected void OnValidate()
        {
            statName.text = type.ToString();
        }

        public void SetValue(WReadOnlyStatsData _data)
        {
            statValue.text = _data.data.CheckDataByType(type).ToString();
        }

        public EStatType CheckType()
        {
            return type;
        }

        public void OnPointerEnter(PointerEventData eventData)
        {
            ui.InvokeAction(ui.ShowStatsDetail, type);
        }

        public void OnPointerExit(PointerEventData eventData)
        {
            ui.InvokeAction(ui.HideTooltip);
        }

        public void UpdateStat()
        {
            float stat = ui.InvokeFunc(ui.TryCheckStat, type);
            Assert.IsTrue(!float.IsNaN(stat), "无法获取：" + type + "类型属性数据");
            statValue.text = stat.ToString();
        }
    }
}

