using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DamageCircleInstant : DamageZoomInstant
{
    [SerializeField] protected Vector2 point;
    [SerializeField] protected float radius;

    protected override void CollisionCheckAndDamage()
    {
        Collider2D[] hits = Physics2D.OverlapCircleAll(new Vector2(transform.position.x + point.x, transform.position.y + point.y), radius);
        foreach (Collider2D hit in hits)
        {
            if (hit.GetComponent<Enemy>() != null)
            {
                Enemy enemy = hit.GetComponent<Enemy>();
                enemy.GetStats().TakeDamage(damage, transform);
            }
        }
    }

    private void OnDrawGizmos()
    {
        Gizmos.color = Color.red;
        Gizmos.DrawWireSphere(new Vector2(transform.position.x + point.x, transform.position.y + point.y), radius);
    }
}
