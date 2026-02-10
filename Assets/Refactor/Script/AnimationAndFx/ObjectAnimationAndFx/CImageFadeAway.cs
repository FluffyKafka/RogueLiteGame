using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Assertions;

namespace AnimationAndFx
{
    internal class CImageFadeAway : CObjectAnimationComponentBase
    {
        [Range(0, 1)][SerializeField] protected float fadeAmount = 0.1f;
        [SerializeField] protected float fadeSmooth = 0.02f;
        [SerializeField] protected float delay = 0.5f;
        protected SpriteRenderer sr;
        protected override void Awake()
        {
            base.Awake();

            sr = GetComponent<SpriteRenderer>();
            Assert.IsNotNull(sr, "CImageDisplay必须管理一个SpriteRenderer组件");
            anim.SetFadeAwayNotice += SetFadeAway;
            anim.FadeAwayNotice += () => { SetFadeAway(fadeAmount, fadeSmooth, delay); };
            anim.ClearNotice += Clear;
        }
        protected void SetFadeAway(float _speed, float _cooldown, float _delay)
        {
            StartCoroutine(FadeAwayAfter(_speed, _cooldown, _delay));
        }
        protected IEnumerator FadeAwayAfter(float _speed, float _cooldown, float _delay)
        {
            yield return new WaitForSeconds(_delay);
            StartCoroutine(FadeAway(_speed, _cooldown));
        }
        protected IEnumerator FadeAway(float _speed, float _cooldown)
        {
            while (sr.color.a > 0)
            {
                Color newColor = sr.color;
                newColor.a -= _speed;
                sr.color = newColor;
                yield return new WaitForSeconds(_cooldown);
            }
        }
        protected void Clear()
        {
            sr.color = Color.white;
            StopAllCoroutines();
        }
    }
}

