using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class IceBallEffectController : ProjectileControllerBase
{
    
    [Header("Damage")]
    [SerializeField] private DamageDataSerializable damageSerializable;
    [SerializeField] private bool isMagicEffectUseStatsValue;

    [Header("Burst")]
    [SerializeField] private float lifeTime;
    [SerializeField] private float maxSize = 2;
    [SerializeField] private float burstDuration = 0.5f;

    private float burstDamageRadius = 0.75f;
    private bool isBurst = false;
    private Vector2 baseScale;
    private Vector2 maxScale;
    private DamageData damage;


    public override void Project(EffectExcuteData _targetData)
    {
        Debug.Log("Project");

        damage = damageSerializable.GetDamageData();
        PlayerManager.instance.player.GetStats().CalculateDamageDataWithStats(ref damage, isMagicEffectUseStatsValue);
        baseScale = transform.localScale;
        maxScale = new Vector2(baseScale.x * maxSize, baseScale.y * maxSize);
    }

    private void Update()
    {
        lifeTime -= Time.deltaTime;
        if(lifeTime < 0 && !isBurst)
        {
            Burst();
        }

        if(isBurst && transform.localScale.x < maxScale.x)
        {
            transform.localScale = Vector3.Lerp(baseScale, maxScale, -lifeTime / burstDuration);
        }
    }

    private void OnTriggerEnter2D(Collider2D _collision)
    {
        if(_collision.GetComponent<Enemy>() != null)
        {
            if(!isBurst)
            {
                Burst();
            }
            _collision.GetComponent<Enemy>().GetStats().TakeDamage(damage, transform);
        }
    }

    private void Burst()
    {
        isBurst = true;
        GetComponent<CircleCollider2D>().radius = burstDamageRadius;
        GetComponentInChildren<Animator>().SetTrigger("Burst");
    }

    public void SelfDestory()
    {
        Destroy(gameObject);
    }
}
