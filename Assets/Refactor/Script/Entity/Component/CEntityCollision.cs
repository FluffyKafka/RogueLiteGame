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

                    entity.IsGrounded += IsGrounded;
                    entity.IsTouchWall += IsTouchWall;
                }

                protected virtual bool IsGrounded()
                {
                    return Physics2D.Raycast(groundCheck.position, Vector2.down, groundCheckDistance, whatIsGround|whatIsPlatform);
                }

                protected virtual bool IsTouchWall()
                {
                    Assert.IsTrue(entity.CheckFacingDir != null, "无法获取Entity的朝向，缺少CheckFacingDir服务的提供者");
                    Assert.IsTrue(entity.CheckFacingDir.GetInvocationList().Length == 1, "CheckFacingDir服务的提供者大于1");
                    return Physics2D.Raycast(wallCheck.position, Vector2.right * entity.CheckFacingDir.Invoke(), wallCheckDistance, whatIsGround);
                }
            }
        }
    }
}

