using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace ObjectController
{
    internal class CTargetSearcherInRadius : CObjectComponentBase
    {
        [SerializeField] protected LayerMask whatIsEnemy;
        [SerializeField] protected LayerMask whatIsPlayer;
        [SerializeField] protected LayerMask whatIsGround;
        [SerializeField] protected float searchRadius;
        [SerializeField] protected float ignoreRadius = 1;

        protected override void Awake()
        {
            base.Awake();
            controller.TryGetRandomEnemyInRadiusNotice += TrySearchRandomEnemyInRadius;
        }

        protected Transform TrySearchRandomEnemyInRadius(float _radius = -1)
        {
            if(_radius < 0)
            {
                _radius = searchRadius;
            }

            Collider2D[] hits = Physics2D.OverlapCircleAll(transform.position, _radius, whatIsEnemy);
            List<Collider2D> realHits = new();
            foreach(var hit in hits)
            {
                if(Vector2.Distance(hit.transform.position, transform.position) > ignoreRadius)
                {
                    realHits.Add(hit);
                }
            }

            if(realHits.Count <= 0)
            {
                return null;
            }
            return realHits[Random.Range(0, realHits.Count)].transform;
        }
    }
}

