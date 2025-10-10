using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EffectFXControllerBase : MonoBehaviour
{
    public virtual void SelfDestory()
    {
        Destroy(gameObject);
    }

    protected IEnumerator SelfDestoryAfterDelayHelper(float _delay)
    {
        yield return new WaitForSeconds(_delay);
        SelfDestory();
    }

    public virtual void PlayFX(EffectExcuteData _targetData)
    {

    }
}
