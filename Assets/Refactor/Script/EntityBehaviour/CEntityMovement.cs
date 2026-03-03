using StatsData;
using System.Collections;
using UnityEngine;

namespace EntityBehaviour
{
    internal class CEntityMovement : CEntityComponentBase
    {
        [HideInInspector] public Rigidbody2D rg;
        [HideInInspector] public bool isFacingLeft = false;
        [HideInInspector] public int facingDir = 1;
        [HideInInspector] public float defaultGravity = 0;

        [Header("Entity Init Facing Dir")]
        [SerializeField] protected bool isInitFacingLeft = false;

        [Header("Entity Movement Collision Info")]
        [SerializeField] public float groundCheckDistance;
        [SerializeField] public Transform groundCheck;
        [SerializeField] public float wallCheckDistance;
        [SerializeField] public Transform wallCheck;
        [SerializeField] public LayerMask whatIsGround;
        [SerializeField] public LayerMask whatIsPlatform;

        [Header("Entity Movement KnockBack Info")]
        [SerializeField] public Vector2 knockBackDir;
        [Range(0, 1)][SerializeField] public float knockBackDirMapK = 0.5f;
        [SerializeField] public float knockBackDuration = 0.07f;
        public bool isKnocked;

        public bool isVelocityLockUp = false;

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

            if(isInitFacingLeft)
            {
                isFacingLeft = true;
                facingDir = -1;
            }
        }

        public virtual void KnockBack(WReadOnlyDamageData _damage)
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
        public virtual IEnumerator HitKnockBack(Transform _damageDirection, float _damageAmount)
        {
            isKnocked = true;

            yield return new WaitForSeconds(knockBackDuration);
            isKnocked = false;
        }

        public virtual void SetNoGravity(bool _isNoGravity)
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

        public virtual void SetVelocity(Vector2 _velocity, bool _canFlip, float _lockDuration = -1)
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
        public IEnumerator VelocityLockUpHelper(float _duration)
        {
            isVelocityLockUp = true;
            yield return new WaitForSeconds(_duration);
            isVelocityLockUp = false;
        }

        public virtual void FlipCheck(float _xVelocity)
        {
            if (_xVelocity < 0 && !isFacingLeft)
            {
                entity.InvokeAction(entity.Flip);
                Flip();
            }
            else if (_xVelocity > 0 && isFacingLeft)
            {
                entity.InvokeAction(entity.Flip);
                Flip();
            }
        }

        public virtual void Flip()
        {
            isFacingLeft = !isFacingLeft;
            facingDir *= -1;
            transform.Rotate(new Vector3(0, 180, 0));
        }

        public virtual bool IsFall()
        {
            return rg.velocity.y < 0;
        }

        public virtual int CheckFacingDir()
        {
            return facingDir;
        }

        public virtual float CheckYVelocity()
        {
            return rg.velocity.y;
        }

        public virtual bool IsGroundedOrPlatForm()
        {
            return Physics2D.Raycast(groundCheck.position, Vector2.down, groundCheckDistance, whatIsGround | whatIsPlatform);
        }

        public virtual bool IsTouchWall()
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