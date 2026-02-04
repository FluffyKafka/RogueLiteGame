using EntityBehaviour;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Assertions;

namespace PlayerBebaviour
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
        [SerializeField] protected bool canWallSlide = true;
        [SerializeField] protected int jumpCountMax = 2;
        protected int jumpCount = 0;
        protected bool isJumpFinish = true;

        [Header("Battle Movement Info")]
        [SerializeField] public float movableDurationInAttacking;
        [SerializeField] public float unmovableDurationAfterAttack;
        [SerializeField] public float[] attackMovement;

        [Header("Player Movement Collision Check")]
        [SerializeField] protected float groundCheckWidth;
        [SerializeField] protected float strictGroundCheckDistance;

        #region Skill
        protected float dashSpeed;
        #endregion

        MPlayerBeviour player;
        protected override void Awake()
        {
            base.Awake();

            defaultMoveSpeed = moveSpeed;
            defaultJumpSpeed = jumpSpeed;

            Assert.IsTrue(entity is MPlayerBeviour, "此组件属于Player组件，必须附加至一个APlayer");
            player = entity as MPlayerBeviour;
            player.Move += Move;
            player.Jump += Jump;
            player.WallSlide += WallSlide;
            player.WallJump += WallJump;
            player.Attack += Attack;
            player.CheckUnmovableDurationAfterAttack += CheckUnmoveableDurationAfterAttack;
            player.IsGroundedOrPlatform_Strict += IsGroundedOrPlatform_Strict;
            player.SlowEntityBy += SlowBy;
            player.RecoverEntitySpeed += RecoverSpeed;
            player.DashBeginNotice += SetDashSpeed;
            player.DashMovementUpdate += DashVelocityUpdate;
            player.ToIdle += StandStill;
        }

        protected override void Update()
        {
            base.Update();

            if (jumpCount > 0)
            {
                if (isJumpFinish)
                {
                    if (player.InvokeFunc(player.IsGroundedOrPlatform_Strict) || (canWallSlide && IsTouchWall()))
                    {
                        jumpCount = 0;
                    }
                }
                else
                {
                    if (!player.InvokeFunc(player.IsGroundedOrPlatform_Strict))
                    {
                        isJumpFinish = true;
                    }
                }
            }
        }

        protected void StandStill()
        {
            SetVelocity(Vector2.zero, false);
        }

        protected void Move(float _dir)
        {
            _dir = ToAbsOneOrZero(_dir);
            Vector2 newVelocity = new Vector2(moveSpeed * _dir, rg.velocity.y);
            SetVelocity(newVelocity, true);
        }

        protected void Jump()
        {
            if (jumpCount >= jumpCountMax)
            {
                return;
            }
            Vector2 newVelocity = new Vector2(rg.velocity.x, jumpSpeed);
            SetVelocity(newVelocity, true);
            ++jumpCount;
            isJumpFinish = false;
        }

        protected void WallSlide(float _dir)
        {
            _dir = ToAbsOneOrZero(_dir);
            Vector2 newVelocity;
            if (_dir >= 0)
            {
                newVelocity = new Vector2(rg.velocity.x, -wallSlideSpeed + wallSlideUpAdjustSpeed);
            }
            else
            {
                newVelocity = new Vector2(rg.velocity.x, -wallSlideSpeed + wallSlideDownAdjustSpeed);
            }
            SetVelocity(newVelocity, false);
        }

        protected void WallJump()
        {
            Assert.IsTrue(player.CheckFacingDir != null, "无法获取Entity的朝向，缺少CheckFacingDir服务的提供者");
            Assert.IsTrue(player.CheckFacingDir.GetInvocationList().Length == 1, "CheckFacingDir服务的提供者大于1");
            int facingDir = player.CheckFacingDir.Invoke();
            Vector2 newVelocity = new Vector2(wallJumpHorizontalSpeed * -facingDir, jumpSpeed);
            SetVelocity(newVelocity, true);
            ++jumpCount;
        }

        protected void Attack(int _count)
        {
            float attackDir = facingDir;
            float xInput = player.InvokeFunc(player.CheckHorizonInput);
            if (xInput != 0)
            {
                attackDir = xInput;
            }
            Vector2 newVelocity = new Vector2(attackMovement[_count] * attackDir, rg.velocity.y);
            SetVelocity(newVelocity, true);
            StartCoroutine(AttackMoveableDuration());
        }
        protected IEnumerator AttackMoveableDuration()
        {
            yield return new WaitForSeconds(movableDurationInAttacking);
            SetVelocity(Vector2.zero, false);
        }

        protected float CheckUnmoveableDurationAfterAttack()
        {
            return unmovableDurationAfterAttack;
        }

        protected float ToAbsOneOrZero(float _dir)
        {
            if (_dir < 0)
            {
                _dir = -1;
            }
            if (_dir > 0)
            {
                _dir = 1;
            }
            return _dir;
        }

        protected override bool IsGroundedOrPlatForm()
        {
            Vector2 leftUp = new Vector2(groundCheck.position.x - groundCheckWidth / 2, groundCheck.position.y + groundCheckDistance / 2);
            Vector2 rightDown = new Vector2(groundCheck.position.x + groundCheckWidth / 2, groundCheck.position.y - groundCheckDistance / 2);
            return Physics2D.OverlapArea(leftUp, rightDown, whatIsGround | whatIsPlatform);
        }

        //IsGroundedOrPlatform_Strict更为严格，确保角色的脚确实接触地面，
        //IsGroundedOrPlatform利用体积碰撞检查解决了走楼梯的行为问题，
        //但可能导致角色可以在墙边连续跳跃
        protected virtual bool IsGroundedOrPlatform_Strict()
        {
            return Physics2D.Raycast(groundCheck.position, Vector2.down, strictGroundCheckDistance, whatIsGround | whatIsPlatform);
        }

        protected void SlowBy(float _rate)
        {
            moveSpeed *= (1 - _rate);
            jumpSpeed *= (1 - _rate);
        }

        protected void RecoverSpeed()
        {
            moveSpeed = defaultMoveSpeed;
            jumpSpeed = defaultJumpSpeed;
        }

        protected void SetDashSpeed(float _speed)
        {
            dashSpeed = _speed;
        }
        protected void DashVelocityUpdate()
        {
            SetVelocity(new Vector2(dashSpeed * facingDir, rg.velocity.y), true);
        }

        protected override void OnDrawGizmos()
        {
            base.OnDrawGizmos();
            Gizmos.color = Color.red;
            Vector2 leftUp = new Vector2(groundCheck.position.x - groundCheckWidth / 2, groundCheck.position.y + groundCheckDistance / 2);
            Vector2 rightDown = new Vector2(groundCheck.position.x + groundCheckWidth / 2, groundCheck.position.y - groundCheckDistance / 2);
            Gizmos.DrawWireCube(groundCheck.position, new Vector3(groundCheckWidth, groundCheckDistance));
            Gizmos.DrawLine(groundCheck.position, groundCheck.position + new Vector3(0, -strictGroundCheckDistance, 0));
        }
    }
}

