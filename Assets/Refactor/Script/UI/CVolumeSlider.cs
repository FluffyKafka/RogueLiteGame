using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Audio;
using UnityEngine.UI;

namespace UISystem
{
    internal class CVolumeSlider : CUIComponentBase
    {
        [SerializeField] protected Slider slider;
        [SerializeField] protected string paramter;
        [SerializeField] protected AudioMixer audioMixer;
        [SerializeField] protected float multiplier;

        protected override void Awake()
        {
            base.Awake();
            slider.onValueChanged.AddListener(SliderValue);
        }

        public void SliderValue(float _value)
        {
            audioMixer.SetFloat(paramter, (1 + Mathf.Log(_value + 0.00000001f)) * multiplier);
        }
    }
}

