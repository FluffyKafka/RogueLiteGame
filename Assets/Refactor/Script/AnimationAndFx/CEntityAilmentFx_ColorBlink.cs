using EntitySystem.EntityActor;
using StatsData;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Assertions;

namespace AnimationAndFx
{
    internal class CEntityAilmentFx_ColorBlink : CEntityAnimFxComponentBase
    {
        protected SpriteRenderer sr;

        [Header("Ignite")]
        [SerializeField] protected List<Color> igniteColor = new();
        [Range(0.1f, 1f)][SerializeField] protected float igniteFlashRate = 0.1f;

        [Header("Chill")]
        [SerializeField] protected List<Color> chillColor = new();
        [Range(0.1f, 1f)][SerializeField] protected float chillFlashRate = 0.1f;

        [Header("Shock")]
        [SerializeField] protected List<Color> shockColor = new();
        [Range(0.1f, 1f)][SerializeField] protected float shockFlashRate = 0.1f;

        protected int colorIndex = 0;
        protected bool isBlink = false;

        protected override void Awake()
        {
            base.Awake();

            sr = GetComponent<SpriteRenderer>();
            Assert.IsNotNull(sr, "CEntityAilmentFx组件需要管理一个SpriteRenderer");

            animFxSystem.Hit += ApplyAilment;
        }

        protected void ApplyAilment(WReadOnlyDamageData _data)
        {
            if(_data.data.magical <= 0)
            {
                return;
            }

            Debug.Log("Hit");
            if(_data.data.ignite)
            {
                Debug.Log("ignite");
                StartCoroutine(StartBlink(igniteColor, igniteFlashRate, _data.data.igniteDuration));
            }
            else if(_data.data.chill)
            {
                StartCoroutine(StartBlink(chillColor, chillFlashRate, _data.data.chillDuration));
            }
            else if(_data.data.shock)
            {
                StartCoroutine(StartBlink(shockColor, shockFlashRate, _data.data.shockDuration));
            }
        }

        protected IEnumerator StartBlink(List<Color> _colorList, float _cooldown, float _duration)
        {
            Debug.Log("StartIgnite");
            isBlink = true;
            StartCoroutine(ColorBlink(_colorList, _cooldown));
            yield return new WaitForSeconds(_duration);
            sr.color = Color.white;
            isBlink = false;
            colorIndex = 0;
        }
        protected IEnumerator ColorBlink(List<Color> _colorList, float _cooldown)
        {
            while(isBlink)
            {
                Debug.Log("igniteUpdate");
                ++colorIndex;
                if (colorIndex >= _colorList.Count)
                {
                    colorIndex %= _colorList.Count;
                }
                sr.color = igniteColor[colorIndex];
                yield return new WaitForSeconds(_cooldown);
            }      
        }
    }
}