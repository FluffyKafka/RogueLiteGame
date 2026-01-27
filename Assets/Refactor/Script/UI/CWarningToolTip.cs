using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace UISystem
{
    internal class CWarningToolTip : MonoBehaviour
    {
        [SerializeField] protected TextMeshProUGUI text;
        protected Button confirm;

        public void ShowWarning(string _text)
        {
            gameObject.SetActive(true);
            if(text == null)
            {
                text = GetComponentInChildren<TextMeshProUGUI>();
            }
            if(confirm == null)
            {
                confirm = GetComponentInChildren<Button>();
            }

            text.text = _text;
            confirm.onClick.AddListener(Hide);
        }

        public void Hide()
        {
            gameObject.SetActive(false);
        }
    }
}