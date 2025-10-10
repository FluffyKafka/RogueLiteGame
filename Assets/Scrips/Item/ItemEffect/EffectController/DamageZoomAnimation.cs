using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DamageZoomAnimation : MonoBehaviour
{
    public void OnEffectDamage()
    {
        GetComponentInParent<DamageZoomInstant>().EffectDamageInZoom();
    }

    public void OnAnimFinish()
    {
        GetComponentInParent<DamageZoomInstant>().SelfDestory();
    }
}
