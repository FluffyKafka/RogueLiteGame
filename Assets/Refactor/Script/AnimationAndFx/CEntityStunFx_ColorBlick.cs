using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Assertions;

namespace AnimationAndFx
{
    internal class CEntityStunFx_ColorBlick : CEntityAnimFxComponentBase
    {
        [SerializeField] protected float stunBlinkRate;
        [SerializeField] protected Color stunBlickColor;
        protected SpriteRenderer sr;
        protected bool isBlink;
        protected override void Awake()
        {
            base.Awake();

            sr = GetComponent<SpriteRenderer>();
            Assert.IsNotNull(sr, "CEntityStunFx需要管理一个SpriteRenderer组件");

            animFxSystem.Stun += Stun;
        }

        protected void Stun(bool _isStun)
        {
            isBlink = _isStun;
            if(_isStun)
            {
                StartCoroutine(ColorBlinkInRate());
            }
        }
        protected IEnumerator ColorBlinkInRate()
        {
            while(isBlink)
            {
                ColorBlink();
                yield return new WaitForSeconds(stunBlinkRate);
            }
        }
        protected void ColorBlink()
        {
            if(sr.color == Color.white)
            {
                sr.color = stunBlickColor;
            }
            else
            {
                sr.color = Color.white;
            }
        }
    }
}
