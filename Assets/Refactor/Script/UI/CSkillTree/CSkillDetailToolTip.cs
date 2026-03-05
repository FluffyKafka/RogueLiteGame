using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace UISystem
{
    internal struct DSkillDetail
    {
        public string skillName;
        public Sprite icon;
        public float price;
        public string description;

        public DSkillDetail(string skillName, Sprite icon, float price, string description)
        {
            this.skillName = skillName;
            this.icon = icon;
            this.price = price;
            this.description = description;
        }
    }

    internal class CSkillDetailToolTip : CUIComponentBase
    {
        [SerializeField] protected TextMeshProUGUI skillName;
        [SerializeField] protected Image icon;
        [SerializeField] protected TextMeshProUGUI price;
        [SerializeField] protected TextMeshProUGUI description;

        public void ShowDetail(DSkillDetail _data)
        {
            gameObject.SetActive(true);
            skillName.text = _data.skillName;
            icon.sprite = _data.icon;
            price.text = _data.price.ToString();
            description.text = _data.description;
        }

        public void HideToolTip()
        {
            gameObject.SetActive(false);
        }
    }
}

