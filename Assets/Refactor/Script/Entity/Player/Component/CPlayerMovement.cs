using EntitySystem.EntityActor.PlayerActor;
using System.Collections;
using System.Collections.Generic;
using Unity.Mathematics;
using UnityEngine;
using UnityEngine.Assertions;

namespace EntitySystem
{
    namespace EntityComponent
    {
        namespace MovementComponent
        {
            internal class CPlayerMovement : CEntityMovement
            {
                [Header("Move Info")]
                [SerializeField] protected float moveSpeed;
                protected float defaultMoveSpeed;

                [Header("Wall Slide Info")]
                [SerializeField] protected float wallSlideSpeed;
                [SerializeField] protected float wallSlideUpAdjustSpeed;
                [SerializeField] protected float wallSlideDownAdjustSpeed;
                [SerializeField] protected float wallJumpHorizontalSpeed;

                [Header("Jump Info")]
                [SerializeField] protected float jumpSpeed;
                protected float defaultJumpSpeed;
                [SerializeField] protected float moveInAirSpeed;
                [SerializeField] protected int jumpCountMax = 2;
                protected int jumpCount = 0;

                APlayer player;
                protected override void Awake()
                {
                    base.Awake();

                    Assert.IsTrue(entity is APlayer, "此组件属于Player组件，必须附加至一个APlayer");
                    player = entity as APlayer;
                    player.Move += Move;
                    player.Jump += Jump;
                }

                protected void Move(float _dir)
                {
                    if(_dir < 0)
                    {
                        _dir = -1;
                    }
                    if(_dir > 0)
                    {
                        _dir = 1;
                    }
                    Vector2 newVelocity = new Vector2(moveSpeed * _dir, rg.velocity.y);
                    SetVelocity(newVelocity, false, true);
                }

                protected void Jump()
                {
                    Vector2 newVelocity = new Vector2(rg.velocity.x, jumpSpeed);
                    SetVelocity(newVelocity, false, true);
                }
            }
        }
    }
}

