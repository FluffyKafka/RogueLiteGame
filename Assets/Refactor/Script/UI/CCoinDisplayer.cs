using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

namespace UISystem
{
    internal class CCoinDisplayer : CUIComponentBase
    {
        [SerializeField] protected TextMeshProUGUI coinText;

        protected override void OnEnable()
        {
            base.OnEnable();
            ui.CoinChangeNotice += CoinUpdate;
        }

        protected override void OnDisable()
        {
            base.OnDisable();
            ui.CoinChangeNotice -= CoinUpdate;
        }

        protected void CoinUpdate(float _cur)
        {
            coinText.text = ((int)_cur).ToString();
        }
    }
}

