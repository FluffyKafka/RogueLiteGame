using EntityBehaviour;
using System.Collections;
using System.Collections.Generic;
using System.Runtime.Versioning;
using Unity.VisualScripting;
using UnityEngine;

namespace EnemyBehaviour
{
    internal class CPPullBackJump_MovementPlugin : CEntityComponentBase
    {
        protected CEnemyMovement movement;
        protected MEnemyBehaviour enemy;

        [SerializeField] protected float pullbackJumpRadius;
        [SerializeField] protected float pullbackJumpCooldown;
        [SerializeField] protected Vector2 maxPullbackJumpForce;
        [SerializeField] protected Vector2 minPullbackJumpForce;
        [Tooltip("当后方有障碍物或没有地面导致角色无法跳跃时，角色将将跳跃强度减少此值再次尝试，直到强度小于等于最小值")]
        [SerializeField] protected float pullbackJumpXForceCulculateAlpha;
        protected bool isCooldown = false;
        protected Vector2 pullBackJumpForce;

        protected override void Awake()
        {
            base.Awake();
            movement = GetComponent<CEnemyMovement>();
            enemy = GetComponent<MEnemyBehaviour>();

            enemy.CanPullBackJump += CanPullBackJump;
            enemy.EffectPullBackJump += EffectPullBackJump;
        }

        protected bool CanPullBackJump()
        {            
            if(isCooldown)
            {
                return false;
            }
            
            if(Vector3.Distance(enemy.InvokeFunc(enemy.CheckPlayerPosition), transform.position) < pullbackJumpRadius)
            {
                int pullBackDir = GetPullBackDir();
                pullBackJumpForce = maxPullbackJumpForce;
                while (pullBackJumpForce.x > minPullbackJumpForce.x)
                {
                    float jumpDuration = (2 * pullBackJumpForce.y) / (-Physics2D.gravity.y * movement.rg.gravityScale);
                    float moveDistance = pullBackJumpForce.x * jumpDuration;

                    bool haveGround =
                        Physics2D.Raycast(
                            movement.groundCheck.transform.position + new Vector3(pullBackDir * moveDistance, 0),
                            Vector2.down,
                            movement.groundCheckDistance,
                            movement.whatIsGround
                        );
                    bool haveWall =
                        Physics2D.Raycast(
                            movement.wallCheck.transform.position,
                            Vector2.right * pullBackDir,
                            movement.groundCheckDistance + moveDistance,
                            movement.whatIsGround
                        );
                    if (haveGround && !haveWall)
                    {
                        pullBackJumpForce.x *= pullBackDir;                        
                        return true;
                    }
                    else
                    {
                        pullBackJumpForce.x -= pullbackJumpXForceCulculateAlpha;
                    }
                }
                return false;
            }
            else
            {
                return false;
            }

        }
        protected void EffectPullBackJump()
        {
            Cooldown();
            movement.SetVelocity(pullBackJumpForce, false);
        }
        public int GetPullBackDir()
        {
            Vector3 playerPosition = enemy.InvokeFunc(enemy.CheckPlayerPosition);

            int pullBackDir = 1;
            if (playerPosition.x > transform.position.x)
            {
                pullBackDir = -1;
            }
            return pullBackDir;
        }
        protected IEnumerator Cooldown()
        {
            isCooldown = true;
            yield return new WaitForSeconds(pullbackJumpCooldown);
            isCooldown = false;
        }

        protected void OnDrawGizmos()
        {
            Gizmos.color = Color.yellow;
            Gizmos.DrawWireSphere(transform.position, pullbackJumpRadius);
        }
    }
}

