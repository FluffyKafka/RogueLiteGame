using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

namespace UISystem
{
    internal class CSoulDisplayer : CUIComponentBase
    {
        [SerializeField] protected TextMeshProUGUI soulText;

        protected override void OnEnable()
        {
            base.OnEnable();
            ui.SoulChangeNotice += SoulUpdate;
        }

        protected override void OnDisable()
        {
            base.OnDisable();
            ui.SoulChangeNotice -= SoulUpdate;
        }

        protected void SoulUpdate(float _cur)
        {
            soulText.text = ((int)_cur).ToString();
        }
    }
}

