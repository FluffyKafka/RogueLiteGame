using PlayerSystem;
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
        [SerializeField] protected EAudioType type;

        protected override void Awake()
        {
            base.Awake();

            ui.AudioVolumeUpdateNotice += SliderUpdate; 

            slider.onValueChanged.AddListener(SliderValue);
            SliderValue(slider.value);
        }

        protected void SliderValue(float _value)
        {
            ui.UpdateAudioVolumeByType(type, _value);
        }

        protected void SliderUpdate(EAudioType _type, float _value)
        {
            if(_type == type)
            {
                slider.value = _value;
            }
        }
    }
}

