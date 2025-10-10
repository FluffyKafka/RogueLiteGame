using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FireFloorEffectController : DamageRrojectableZoomController
{
    [SerializeField] protected Vector2 beginVelocity;

    public override void Project(EffectExcuteData _targetData)
    {
        base.Project(_targetData);
        Rigidbody2D rg = GetComponentInParent<Rigidbody2D>();
        rg.velocity = beginVelocity;
    }

    protected override void OnTriggerEnter2D(Collider2D _collision)
    {
        base.OnTriggerEnter2D(_collision);
    }

    protected override void Update()
    {
        lifeTime -= Time.deltaTime;
        if (lifeTime < 0)
        {
            Destroy(transform.parent.gameObject);
        }
    }
}
