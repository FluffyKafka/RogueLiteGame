using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ParticleSystemFXEffectController : EffectFXControllerBase
{
    public override void PlayFX(EffectExcuteData _targetData)
    {
        GetComponent<ParticleSystem>().Play();
        StartCoroutine(
            SelfDestoryAfterDelayHelper(
                GetComponent<ParticleSystem>().main.startLifetime.constant + GetComponent<ParticleSystem>().main.duration
            )
        );
    }
}
