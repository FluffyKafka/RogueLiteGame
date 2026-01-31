using StatsData;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace AnimationAndFx
{
    internal class CEntityAilmentFx_ParticleFx : CEntityAnimFxComponentBase
    {
        [SerializeField] protected ParticleSystem igniteParticleFX;
        [SerializeField] protected ParticleSystem chillParticleFX;
        [SerializeField] protected ParticleSystem shockParticleFX;

        protected override void Awake()
        {
            base.Awake();

            animFxSystem.Hit += ApplyAilment;
        }

        protected void ApplyAilment(WReadOnlyDamageData _data)
        {
            if(_data.data.magical <= 0)
            {
                return;
            }

            if (_data.data.ignite)
            {
                StartCoroutine(ParticleGenerate(igniteParticleFX, _data.data.igniteDuration));
            }
            else if (_data.data.chill)
            {
                StartCoroutine(ParticleGenerate(chillParticleFX,  _data.data.chillDuration));
            }
            else if (_data.data.shock)
            {
                StartCoroutine(ParticleGenerate(shockParticleFX, _data.data.shockDuration));
            }
        }

        protected IEnumerator ParticleGenerate(ParticleSystem _particle, float _duration)
        {
            _particle.Play();
            yield return new WaitForSeconds(_duration);
            _particle.Stop();
        }
    }
}