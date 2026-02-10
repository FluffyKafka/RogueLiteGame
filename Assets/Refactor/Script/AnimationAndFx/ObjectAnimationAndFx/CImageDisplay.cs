using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Assertions;
using UnityEngine.UI;

namespace AnimationAndFx
{
    internal class CImageDisplay : CObjectAnimationComponentBase
    {
        protected SpriteRenderer sr;

        protected override void Awake()
        {
            base.Awake();

            sr = GetComponent<SpriteRenderer>();
            Assert.IsNotNull(sr, "CImageDisplay必须管理一个SpriteRenderer组件");
            anim.InitAnimImageNotice += InitAnimImage;
        }
        protected void InitAnimImage(Sprite _image)
        {
            sr.sprite = _image;
        }

    }
}

