using StatsData;
using System.Collections;
using UnityEngine;

namespace EntityBehaviour
{
    internal class CEntityMovement : CEntityComponentBase
    {
        protected Rigidbody2D rg;
        protected bool isFacingLeft = false;
        protected int facingDir = 1;
        protected float defaultGravity = 0;

        [Header("Entity Movement Collision Info")]
        [SerializeField] protected float groundCheckDistance;
        [SerializeField] protected Transform groundCheck;
        [SerializeField] protected float wallCheckDistance;
        [SerializeField] protected Transform wallCheck;
        [SerializeField] protected LayerMask whatIsGround;
        [SerializeField] protected LayerMask whatIsPlatform;

        [Header("Entity Movement KnockBack Info")]
        [SerializeField] protected Vector2 knockBackDir;
        [Range(0, 1)][SerializeField] protected float knockBackDirMapK = 0.5f;
        [SerializeField] protected float knockBackDuration = 0.07f;
        protected bool isKnocked;

        protected bool isVelocityLockUp = false;

        override protected void Awake()
        {
            base.Awake();
            rg = GetComponent<Rigidbody2D>();

            entity.NoGravity += SetNoGravity;
            entity.IsFall += IsFall;
            entity.CheckFacingDir += CheckFacingDir;
            entity.CheckYVelocity += CheckYVelocity;
            entity.IsGroundedOrPlatForm += IsGroundedOrPlatForm;
            entity.IsTouchWall += IsTouchWall;
            entity.TakeDamage += KnockBack;
        }

        protected virtual void KnockBack(WReadOnlyDamageData _damage)
        {
            float knockBackFacing = 1;
            if (_damage.data.damageSourceTransform.position.x > transform.position.x)
            {
                knockBackFacing = -1;
            }
            else if (_damage.data.damageSourceTransform.position.x == transform.position.x)
            {
                knockBackFacing = -facingDir;
            }

            float damageAmount = _damage.data.physical + _damage.data.magical;
            float alpha = 1 - (1 / Mathf.Pow(1 + damageAmount, knockBackDirMapK));

            SetVelocity(new Vector2(alpha * knockBackDir.x * knockBackFacing, alpha * knockBackDir.y), false, knockBackDuration);
        }
        protected virtual IEnumerator HitKnockBack(Transform _damageDirection, float _damageAmount)
        {
            isKnocked = true;

            yield return new WaitForSeconds(knockBackDuration);
            isKnocked = false;
        }

        protected virtual void SetNoGravity(bool _isNoGravity)
        {
            if (_isNoGravity)
            {
                rg.gravityScale = 0;
            }
            else
            {
                rg.gravityScale = defaultGravity;
            }
        }

        protected virtual void SetVelocity(Vector2 _velocity, bool _canFlip, float _lockDuration = -1)
        {
            if (isVelocityLockUp)
            {
                return;
            }

            if (_lockDuration > 0)
            {
                StartCoroutine(VelocityLockUpHelper(_lockDuration));
            }

            rg.velocity = _velocity;
            if (_canFlip)
            {
                FlipCheck(_velocity.x);
            }
        }
        protected IEnumerator VelocityLockUpHelper(float _duration)
        {
            isVelocityLockUp = true;
            yield return new WaitForSeconds(_duration);
            isVelocityLockUp = false;
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

            entity.InvokeAction(entity.Flip);
        }

        protected virtual bool IsFall()
        {
            return rg.velocity.y < 0;
        }

        protected virtual int CheckFacingDir()
        {
            return facingDir;
        }

        protected virtual float CheckYVelocity()
        {
            return rg.velocity.y;
        }

        protected virtual bool IsGroundedOrPlatForm()
        {
            return Physics2D.Raycast(groundCheck.position, Vector2.down, groundCheckDistance, whatIsGround | whatIsPlatform);
        }

        protected virtual bool IsTouchWall()
        {
            return Physics2D.Raycast(wallCheck.position, Vector2.right * entity.InvokeFunc(entity.CheckFacingDir), wallCheckDistance, whatIsGround);
        }

        protected virtual void OnDrawGizmos()
        {
            Gizmos.color = Color.white;
            Gizmos.DrawLine(groundCheck.position, new Vector2(groundCheck.position.x, groundCheck.position.y - groundCheckDistance));
            Gizmos.DrawLine(wallCheck.position, new Vector2(wallCheck.position.x + facingDir * wallCheckDistance, wallCheck.position.y));
        }
    }
}