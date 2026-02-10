using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace AnimationAndFx
{
    internal class CEnemyCounterImage : CEntityAnimFxComponentBase
    {
        [SerializeField] protected SpriteRenderer counterImage;
        protected MEnemyAnimationFxSystem enemyAnimFxSystem;

        protected override void Awake()
        {
            base.Awake();

            counterImage.gameObject.SetActive(false);

            enemyAnimFxSystem = animFxSystem as MEnemyAnimationFxSystem;
            enemyAnimFxSystem.StunOpen += ShowCounterImage;
        }

        protected void ShowCounterImage(bool _isShow)
        {
            counterImage.gameObject.SetActive(_isShow);
        }
    }
}

