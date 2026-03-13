using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace UISystem
{
    internal class CHealthBar : CUIComponentBase
    {
        [SerializeField] protected Slider slider;

        protected override void OnEnable()
        {
            base.OnEnable();
            ui.CurrentHealthChangeNotice += UpdateHealth;
            ui.UpdateStats += UpdateMaxHealth;
        }
        protected override void OnDisable()
        {
            base.OnDisable();
            ui.CurrentHealthChangeNotice -= UpdateHealth;
            ui.UpdateStats -= UpdateMaxHealth;
        }

        protected void UpdateMaxHealth()
        {
            float maxHealth = ui.InvokeFunc(ui.TryCheckStat, StatsData.EStatType.MaxHealth);
            slider.maxValue = maxHealth;
        }
        protected void UpdateHealth(float _health)
        {
            slider.value = _health;
        }
    }
}

