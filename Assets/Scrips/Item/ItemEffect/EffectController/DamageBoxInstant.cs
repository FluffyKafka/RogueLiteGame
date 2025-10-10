using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DamageBoxInstant : DamageZoomInstant
{
    [SerializeField] protected Vector2 point;
    [SerializeField] protected Vector2 size;

    protected override void CollisionCheckAndDamage()
    {
        Collider2D[] hits = Physics2D.OverlapBoxAll(new Vector2(transform.position.x + point.x, transform.position.y + point.y), size, 0);
        foreach(Collider2D hit in hits)
        {
            if(hit.GetComponent<Enemy>() != null)
            {
                Enemy enemy = hit.GetComponent<Enemy>();
                enemy.GetStats().TakeDamage(damage, transform);
            }
        }
    }

    private void OnDrawGizmos()
    {
        Gizmos.color = Color.red;
        Gizmos.DrawWireCube(new Vector2(transform.position.x + point.x, transform.position.y + point.y), size);
    }
}
