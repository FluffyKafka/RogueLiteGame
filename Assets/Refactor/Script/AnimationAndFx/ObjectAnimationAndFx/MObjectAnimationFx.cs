using ObjectController;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace AnimationAndFx
{
    internal class MObjectAnimationFx : ComponentManagerBase, IObjectAnim
    {
        protected IAnimObject controller; 

        #region Action
        public Action<Sprite> InitAnimImageNotice;
        public Action<float, float, float> SetFadeAwayNotice;
        public Action FadeAwayNotice;
        public Action ClearNotice;
        public Action<bool> ShowTrailNotice;
        public Action FadeAwayFinishNotice;
        public Action ToEffectNotice;
        public Action ShowHitFxNotice;
        #endregion

        protected void Awake()
        {
            controller = GetComponentInParent<IAnimObject>();
            FadeAwayFinishNotice += controller.FadeFinishNotice;
        }

        public void InitAnimImage(Sprite _image)
        {
            InvokeAction(InitAnimImageNotice, _image);
        }
        public void SetFadeAway(float _speed, float _cooldown, float _delay)
        {
            InvokeAction(SetFadeAwayNotice, _speed, _cooldown, _delay);
        }
        public void FadeAway()
        {
            InvokeAction(FadeAwayNotice);
        }
        public void Clear()
        {
            InvokeAction(ClearNotice);
        }
        public void ShowTrail(bool _isShow)
        {
            InvokeAction(ShowTrailNotice, _isShow);
        }
        public void ToEffect()
        {
            InvokeAction(ToEffectNotice);
        }

        public void AnimDamageTrigger()
        {
            controller.DamageTrigger();
        }
        public void AnimDamageFinish()
        {
            controller.DamageFinish();
        }

        public void ShowHitFx()
        {
            InvokeAction(ShowHitFxNotice);
        }
    }

    internal class CObjectAnimationComponentBase : MonoBehaviour
    {
        protected MObjectAnimationFx anim;
        protected virtual void Awake()
        {
            anim = GetComponent<MObjectAnimationFx>();
        }
    }
}

