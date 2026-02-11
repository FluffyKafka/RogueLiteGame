using EntitySystem;
using Item;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Assertions;
using static UnityEditor.Progress;

namespace ObjectController
{
    internal class AAfterImage : AObjectController
    {
        public void Setup(FCAfterImageFactory _factory, DAfterImageData _data)
        {
            factory = _factory;

            if(_data.facingDir < 0)
            {
                transform.Rotate(0, 180, 0);
            }
            anim.InitAnimImage(_data.image);
            anim.SetFadeAway(_data.fadeSpeed, _data.duration * _data.fadeSpeed, 0);
            StartCoroutine(SelfRecycle(_data.duration));
        }
        protected IEnumerator SelfRecycle(float _delay)
        {
            yield return new WaitForSeconds(_delay);
            factory.RecycleObject(this);
        }

        public override void Clear()
        {
            base.Clear();
            transform.rotation = Quaternion.identity;
        }
    }
}

