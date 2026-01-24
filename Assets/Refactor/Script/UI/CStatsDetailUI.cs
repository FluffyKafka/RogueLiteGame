using StatsData;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

namespace UISystem
{
    internal class CStatsDetailUI : CUIComponentBase
    {
        [SerializeField] protected TextMeshProUGUI statNameBlock;
        [SerializeField] protected TextMeshProUGUI detailBlock;
        public void ShowToolTip(EStatType _type)
        {
            gameObject.SetActive(true);
            statNameBlock.text = ui.InvokeFunc(ui.Translate, _type.ToString());
            detailBlock.text = ui.InvokeFunc(ui.CheckKeyWordStatDescription, _type.ToString());
        }
        public void HideToolTip()
        {
            gameObject.SetActive(false);
        }
    }
}
