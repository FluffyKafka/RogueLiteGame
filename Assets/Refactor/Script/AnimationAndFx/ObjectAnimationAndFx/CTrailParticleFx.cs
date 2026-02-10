using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace AnimationAndFx
{
    internal class CTrailParticleFx : CObjectAnimationComponentBase
    {
        [SerializeField] protected ParticleSystem trailParticleFx;

        protected override void Awake()
        {
            base.Awake();
            anim.ShowTrailNotice += ShowTrail;
        }

        protected void ShowTrail(bool _isShow)
        {
            if(_isShow && !trailParticleFx.isPlaying)
            {
                trailParticleFx.Play();
            }
            else
            {
                trailParticleFx.Stop();
            }
        }
    }
}

