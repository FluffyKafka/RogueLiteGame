using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Assertions;
using static EntitySystem.EntityActor.AEntity;

namespace EntitySystem
{
    namespace EntityComponent
    {
        namespace MovementComponent
        {
            internal class CEntityMovement : CEntityComponentBase
            {
                protected Rigidbody2D rg;
                protected bool isFacingLeft = false;
                protected int facingDir = 1;
                protected float defaultGravity = 0;

                override protected void Awake()
                {
                    base.Awake();
                    rg = GetComponent<Rigidbody2D>();

                    entity.NoGravity += SetNoGravity;
                    entity.IsFall += IsFall;
                }

                protected virtual void SetNoGravity(bool _isNoGravity)
                {
                    if(_isNoGravity)
                    {
                        rg.gravityScale = 0;
                    }
                    else
                    {
                        rg.gravityScale = defaultGravity;
                    }
                }

                protected virtual void SetVelocity(Vector2 _velocity, bool _isKnockBack, bool _canFlip)
                {
                    if(!_isKnockBack && entity.IsKnockBack != null && entity.IsKnockBack.Invoke())
                    {
                        return;
                    }
                    rg.velocity = _velocity;
                    if(!_isKnockBack && _canFlip)
                    {
                        FlipCheck(_velocity.x);
                    }                   
                }

                protected virtual void FlipCheck(float _xVelocity)
                {
                    if (_xVelocity < 0 && !isFacingLeft)
                    {
                        Flip();
                    }
                    else if (_xVelocity > 0 && isFacingLeft)
                    {
                        Flip();
                    }
                }

                protected virtual void Flip()
                {
                    isFacingLeft = !isFacingLeft;
                    facingDir *= -1;
                    transform.Rotate(new Vector3(0, 180, 0));

                    if (entity.Flip != null)
                    {
                        entity.Flip();
                    }
                }

                protected virtual bool IsFall()
                {
                    return rg.velocity.y < 0;
                }
            }
        }
    }
}