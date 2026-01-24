using StatsData;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace UISystem
{
    internal class CUIDescriptionDatabase : CUIComponentBase
    {
        [Serializable]
        protected class DStatsDescription
        {
            public EStatType origin;
            [TextArea] public string description;
        }

        [SerializeField] protected List<DStatsDescription> statsTypeInfo;
        protected Dictionary<string, string> dictionary = new Dictionary<string, string>();
        protected bool isInit = false;

        protected override void OnEnable()
        {
            if (!isInit)
            {
                foreach (var pair in statsTypeInfo)
                {
                    dictionary.Add(pair.origin.ToString(), pair.description);
                }
                isInit = true;
            }


            ui.CheckKeyWordStatDescription += CheckDescription;
        }

        protected override void OnDisable()
        {
            ui.CheckKeyWordStatDescription -= CheckDescription;
        }

        public string CheckDescription(string _orgin)
        {
            if (dictionary.TryGetValue(_orgin, out string trans))
            {
                return trans;
            }
            else
            {
                Debug.LogWarning("查询失败，没有其详细信息：" + _orgin);
                return _orgin;
            }
        }
    }
}