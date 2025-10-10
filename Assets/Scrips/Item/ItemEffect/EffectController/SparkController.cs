using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SparkController : DamageCircleInstant
{
    public override void Project(EffectExcuteData _targetData)
    {
        GetComponent<ParticleSystem>().Play();
        EffectDamageInZoom();
        StartCoroutine(
            SelfDestoryAfterDelayHelper(
                GetComponent<ParticleSystem>().main.startLifetime.constant + GetComponent<ParticleSystem>().main.duration
            )
        );
    }

    protected IEnumerator SelfDestoryAfterDelayHelper(float _delay)
    {
        yield return new WaitForSeconds(_delay);
        SelfDestory();
    }
}
