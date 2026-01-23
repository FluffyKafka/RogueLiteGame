using StatsData;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

namespace UISystem
{
    internal class SLStatSlot : MonoBehaviour
    {
        protected MUIManager ui;

        [SerializeField] protected EStatType type;
        [SerializeField] protected TextMeshProUGUI statName;
        [SerializeField] protected TextMeshProUGUI statValue;

        private void OnValidate()
        {
            statName.text = type.ToString();
        }

        public void Init(MUIManager _ui)
        {
            ui = _ui;
            statName.text = _ui.InvokeFunc(ui.Translate, type.ToString());
        }

        public void SetValue(WReadOnlyStatsData _data)
        {
            statValue.text = _data.data.CheckDataByType(type).ToString();
        }

        public EStatType CheckType()
        {
            return type;
        }
    }
}

