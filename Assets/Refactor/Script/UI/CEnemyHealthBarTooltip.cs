using PlayerSystem;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace UISystem
{
    internal class CEnemyHealthBarTooltip : CUIComponentBase
    {
        [SerializeField] protected Slider healthBar;
        [SerializeField] protected TextMeshProUGUI enemyName;

        protected IUIEnemy enemy;

        protected void Update()
        {
            healthBar.value = enemy.CheckHealthPercent();    
        }

        public void SetEnemy(IUIEnemy _enemy)
        {
            if(_enemy != null)
            {
                enemy = _enemy;
                healthBar.value = enemy.CheckHealthPercent();
                enemyName.text = enemy.CheckName();
                gameObject.SetActive(true);
            }
            else
            {
                enemy = null;
                gameObject.SetActive(false);
            }
        }
    }
}

