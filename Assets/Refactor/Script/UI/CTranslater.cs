using System.Collections;
using System.Collections.Generic;
using System.Net.NetworkInformation;
using UnityEngine;

namespace UISystem
{
    internal class DTranslateSlot
    {
        public string origin;
        public string target;
    }

    internal class CTranslater : CUIComponentBase
    {
        [SerializeField] protected List<DTranslateSlot> translateInfo;
        Dictionary<string, string> dictionary = new Dictionary<string, string>();

        protected override void OnEnable()
        {
            foreach (DTranslateSlot pair in translateInfo)
            {
                dictionary.Add(pair.origin, pair.target);
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

