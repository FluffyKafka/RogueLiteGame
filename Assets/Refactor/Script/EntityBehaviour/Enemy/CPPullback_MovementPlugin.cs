
using EntityBehaviour;
using System;
using UnityEngine;

namespace EnemyBehaviour
{
    internal class CPPullback_MovementPlugin : CEntityComponentBase
    {
        protected CEnemyMovement movement;
        protected MEnemyBehaviour enemy;

        [SerializeField] protected float pullbackSpeed;
        [SerializeField] protected float pullbackRadius;
        protected override void Awake()
        {
            base.Awake();
            movement = GetComponent<CEnemyMovement>();
            enemy = entity as MEnemyBehaviour;

            enemy.CanPullBack += CanPullBack;
            enemy.PullBackUpdate += UpdatePullBack;
        }

        protected bool CanPullBack()
        {
            Vector3 playerPosition = enemy.InvokeFunc(enemy.CheckPlayerPosition);
            if (Vector3.Distance(playerPosition, transform.position) < pullbackRadius)
            {
                return true;
            }
            else
            {
                return false;
            }
        }
        protected void UpdatePullBack()
        {
            enemy.InvokeAction(enemy.FacingToPlayer);
            movement.SetVelocity(new Vector2(pullbackSpeed * -movement.facingDir, movement.rg.velocity.y), false);
        }

        protected void OnDrawGizmos()
        {
            Gizmos.color = Color.yellow;
            Gizmos.DrawWireSphere(transform.position, pullbackRadius);
        }
    }
}
