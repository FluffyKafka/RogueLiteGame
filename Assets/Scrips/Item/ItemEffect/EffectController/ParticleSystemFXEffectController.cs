using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ParticleSystemFXEffectController : EffectFXControllerBase
{
    //[_lifeTime == -1] =>> 使用ParticleSystem内定义的(startLifetime + duration)
    public override void PlayFX(EffectExcuteData _targetData, float _lifeTime, bool _isFaceToEnemy)
    {
        if(_isFaceToEnemy && _targetData != null)
        {
            if(PlayerManager.instance.player.transform.position.x > _targetData.target.transform.position.x)
            {
                transform.Rotate(0, 180, 0);
            }
        }

        if(_lifeTime < 0)
        {
            _lifeTime = GetComponentInChildren<ParticleSystem>().main.startLifetime.constant
                + GetComponentInChildren<ParticleSystem>().main.duration;
        }

        ParticleSystem[] particles = GetComponentsInChildren<ParticleSystem>();
        foreach (ParticleSystem particle in particles)
        {
            particle.Play();
        }
        StartCoroutine(SelfDestoryAfterDelayHelper(_lifeTime));
    }
}
