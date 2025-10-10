using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DamageZoomInstant : ProjectileControllerBase
{
    [SerializeField] protected DamageDataSerializable damageData;
    [SerializeField] protected bool isMagicEffectUseStatsValue = true;

    protected DamageData damage;

    public override void Project(EffectExcuteData _targetData)
    {
        if(PlayerManager.instance.player.transform.position.x > _targetData.target.transform.position.x)
        {
            transform.Rotate(0, 180, 0);
        }
    }

    public void EffectDamageInZoom()
    {
        damage = damageData.GetDamageData();
        PlayerManager.instance.player.GetStats().CalculateDamageDataWithStats(ref damage, isMagicEffectUseStatsValue);
        CollisionCheckAndDamage();
    }

    protected virtual void CollisionCheckAndDamage()
    {

    }

    public void SelfDestory()
    {
        Destroy(gameObject);
    }
}
