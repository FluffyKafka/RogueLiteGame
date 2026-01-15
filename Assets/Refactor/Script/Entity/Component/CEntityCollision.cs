using System.Collections;
using System.Collections.Generic;
using UnityEditor.VersionControl;
using UnityEngine;
using UnityEngine.Assertions;

namespace EntitySystem
{
    namespace EntityComponent
    {
        namespace CollisionComponent
        {
            internal class CEntityCollision : CEntityComponentBase
            {
                [Header("Entity Collision Info")]
                [SerializeField] protected float groundCheckDistance;
                [SerializeField] protected Transform groundCheck;
                [SerializeField] protected float wallCheckDistance;
                [SerializeField] protected Transform wallCheck;
                [SerializeField] protected LayerMask whatIsGround;
                [SerializeField] protected LayerMask whatIsPlatform;

                override protected void Awake()
                {
                    base.Awake();

                    entity.IsGroundedOrPlatForm += IsGroundedOrPlatForm;
                    entity.IsTouchWall += IsTouchWall;
                }

                protected virtual bool IsGroundedOrPlatForm()
                {
                    return Physics2D.Raycast(groundCheck.position, Vector2.down, groundCheckDistance, whatIsGround|whatIsPlatform);
                }

                protected virtual bool IsTouchWall()
                {
                    return Physics2D.Raycast(wallCheck.position, Vector2.right * entity.InvokeFunc(entity.CheckFacingDir), wallCheckDistance, whatIsGround);
                }

                private void OnDrawGizmos()
                {
                    
                }
            }
        }
    }
}

