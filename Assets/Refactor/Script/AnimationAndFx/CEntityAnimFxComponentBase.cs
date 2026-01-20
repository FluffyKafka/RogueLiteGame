using AnimationAndFx;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Assertions;

namespace AnimationAndFx
{
    internal class CEntityAnimFxComponentBase : MonoBehaviour
    {
        protected MEntityAnimationFXSystem animFxSystem;

        protected virtual void Awake()
        {
            animFxSystem = GetComponent<MEntityAnimationFXSystem>();
            Assert.IsNotNull(animFxSystem, "动画组件需要关联到一个管理器");
        }
    }
}

