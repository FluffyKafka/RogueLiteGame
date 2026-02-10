using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace AnimationAndFx
{
    internal class CEntityHealthBar : CEntityAnimFxComponentBase
    {
        [SerializeField] protected Slider healthBar;

        protected override void Awake()
        {
            base.Awake();
            animFxSystem.UpdateHealthBar += UpdateHealthBar;
        }

        protected void UpdateHealthBar(float _hpPercent)
        {
            healthBar.value = _hpPercent;
        }
    }
}

