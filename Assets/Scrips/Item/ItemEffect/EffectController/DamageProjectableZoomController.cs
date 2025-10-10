using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DamageRrojectableZoomController : ProjectileControllerBase
{
    [SerializeField] protected DamageDataSerializable damageData;
    [SerializeField] protected bool isMagicEffectUseStatsValue = true;
    [SerializeField] protected float lifeTime;

    protected DamageData damage;

    public override void Project(EffectExcuteData _targetData)
    {
        damage = damageData.GetDamageData();
        PlayerManager.instance.player.GetStats().CalculateDamageDataWithStats(ref damage, isMagicEffectUseStatsValue);
    }

    protected virtual void OnTriggerEnter2D(Collider2D _collision)
    {
        if(_collision.GetComponent<Enemy>() != null)
        {
            Enemy _target = _collision.GetComponent<Enemy>();
            _target.GetStats().TakeDamage(damage, transform);
        }
    }

    protected virtual void Update()
    {
        lifeTime -= Time.deltaTime;
        if(lifeTime < 0)
        {
            Destroy(gameObject);
        }
    }
}
