using EntitySystem.EntityActor.PlayerActor;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Assertions;

namespace EntitySystem
{
    namespace EntityComponent
    {
        namespace CollisionComponent
        {
            internal class CPlayerCollision : CEntityCollision
            {
                APlayer player;

                [Header("Player Collision Check")]
                [SerializeField] protected float groundCheckWidth;
                [SerializeField] protected float jumpGroundCheckDistance;

                protected override void Awake()
                {
                    base.Awake();

                    Assert.IsTrue(entity is APlayer, "组件" + GetType().ToString() + "必须依附于一个APlayer实体");
                    player = (entity as APlayer);
                    player.IsGrounded_Jump += IsGrounded_Jump;
                }

                protected override bool IsGrounded()
                {
                    Vector2 leftUp = new Vector2(groundCheck.position.x - groundCheckWidth / 2, groundCheck.position.y + groundCheckDistance / 2);
                    Vector2 rightDown = new Vector2(groundCheck.position.x + groundCheckWidth / 2, groundCheck.position.y - groundCheckDistance / 2);
                    return Physics2D.OverlapArea(leftUp, rightDown, whatIsGround | whatIsPlatform);
                }

                //IsGrounded_Jump专用于检查二段跳，
                //IsGrounded利用体积碰撞检查解决了走楼梯的行为问题，
                //但可能导致角色可以在墙边连续跳跃
                protected virtual bool IsGrounded_Jump()
                {
                    return Physics2D.Raycast(groundCheck.position, Vector2.down, jumpGroundCheckDistance, whatIsGround | whatIsPlatform);
                }
            }
        }
    }
}