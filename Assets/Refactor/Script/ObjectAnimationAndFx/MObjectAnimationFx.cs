using ObjectController;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace AnimationAndFx
{

    internal class MObjectAnimationFx : ComponentManagerBase, IObjectAnim
    {
        #region Action
        public Action<Sprite> InitAnimImageNotice;
        public Action<float, float> SetFadeAwayNotice;
        public Action ClearNotice;
        #endregion
        public void InitAnimImage(Sprite _image)
        {
            InvokeAction(InitAnimImageNotice, _image);
        }
        public void SetFadeAway(float _speed, float _cooldown)
        {
            InvokeAction(SetFadeAwayNotice, _speed, _cooldown);
        }
        public void Clear()
        {
            InvokeAction(ClearNotice);
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

