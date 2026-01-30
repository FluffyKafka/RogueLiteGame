using ObjectController;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace ObjectAnimationFx
{

    internal class MObjectAnimationFx : ComponentManagerBase, IObjectAnim
    {
        #region Action
        public Action<Sprite> InitAnimImageNotice;
        #endregion
        public void InitAnimImage(Sprite _image)
        {
            InvokeAction(InitAnimImageNotice, _image);
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

