using Item;
using StatsData;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Net.NetworkInformation;
using UnityEngine;

namespace UISystem
{
    internal class CTranslater : CUIComponentBase
    {
        [Serializable]
        protected class DTranslateStatsTypeSlot
        {
            public EStatType origin;
            public string target;
        }

        [Serializable]
        protected class DTranslateEquipmentTypeSlot
        {
            public EEquipmentType origin;
            public string target;
        }

        [SerializeField] protected List<DTranslateStatsTypeSlot> statsTypeTranslateInfo;
        [SerializeField] protected List<DTranslateEquipmentTypeSlot> equipmentTypeTanslateInfo;
        protected Dictionary<string, string> dictionary = new Dictionary<string, string>();
        protected bool isInit = false;

        protected override void OnEnable()
        {
            if(!isInit)
            {
                foreach (var pair in statsTypeTranslateInfo)
                {
                    dictionary.Add(pair.origin.ToString(), pair.target);
                }
                foreach (var pair in equipmentTypeTanslateInfo)
                {
                    dictionary.Add(pair.origin.ToString(), pair.target);
                }
                isInit = true;
            }
            

            ui.Translate += Translate;
        }

        protected override void OnDisable()
        {
            ui.Translate -= Translate;
        }

        public string Translate(string _orgin)
        {
            if (dictionary.TryGetValue(_orgin, out string trans))
            {
                return trans;
            }
            else
            {
                Debug.LogWarning("∑≠“Î ß∞‹£¨¥ ø‚÷–√ª”–£∫" + _orgin);
                return _orgin;
            }
        }
    }
}

