using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace AnimationAndFx
{
    internal class CHitParticleFx : CObjectAnimationComponentBase
    {
        [SerializeField] protected ParticleSystem hitFxParticleSystem;

        protected override void Awake()
        {
            base.Awake();
            anim.ShowHitFxNotice += ShowHitFx;
        }

        protected void ShowHitFx()
        {
            hitFxParticleSystem.Play();
        }
    }
}
