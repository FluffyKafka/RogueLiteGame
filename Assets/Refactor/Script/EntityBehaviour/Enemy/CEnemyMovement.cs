using EntityBehaviour;
using UnityEngine;
using UnityEngine.Assertions;

namespace EnemyBehaviour
{
    internal class CEnemyMovement : CEntityMovement
    {
        protected MEnemyBehaviour enemy;

        [Header("Enemy Regular Movement")]
        [SerializeField] public float moveSpeed;
        protected float defaultMoveSpeed;
        [SerializeField] public float maxIdleDuration;
        [SerializeField] public float minIdleDuration;
        [SerializeField] public float battleMoveSpeed;
        protected float defaultBattleMoveSpeed;

        [Header("Enemy Stunned Movement")]
        [SerializeField] public Vector2 stunDir;

        protected override void Awake()
        {
            base.Awake();

            defaultMoveSpeed = moveSpeed;

            Assert.IsTrue(entity is MEnemyBehaviour, "此为Enemy组件");
            enemy = entity as MEnemyBehaviour;

            enemy.CheckIdleDuration += CheckRandomIdleDuration;
            enemy.MoveForward += MoveForward;
            enemy.MoveToward_Battle += MoveToward_Battle;
            enemy.MoveToward += MoveToward;
            enemy.FacingToPlayer += FacingToPlayer;
            enemy.BeStunned += BeStunned;
            enemy.StandStill += StandStill;

            enemy.SlowEntityBy += SlowBy;
        }

        protected float CheckRandomIdleDuration()
        {
            return Random.Range(minIdleDuration, maxIdleDuration);
        }

        protected void StandStill()
        {
            Vector2 newVelocity = new Vector2(0, rg.velocity.y);
            SetVelocity(newVelocity, false);
        }

        protected void MoveForward(int _dir)
        {
            Vector2 newVelocity = new Vector2(moveSpeed * facingDir * _dir, rg.velocity.y);
            SetVelocity(newVelocity, true);
        }

        protected void MoveToward(int _dir)
        {
            Vector2 newVelocity = new Vector2(moveSpeed * _dir, rg.velocity.y);
            SetVelocity(newVelocity, true);
        }
        protected void MoveToward_Battle(int _dir)
        {
            Vector2 newVelocity = new Vector2(battleMoveSpeed * _dir, rg.velocity.y);
            SetVelocity(newVelocity, true);
        }

        protected void FacingToPlayer()
        {
            //与Player交互逻辑
            if (enemy.InvokeFunc(enemy.CheckPlayerPosition).x > enemy.transform.position.x && isFacingLeft)
            {
                Flip();
            }
            else if (enemy.InvokeFunc(enemy.CheckPlayerPosition).x < enemy.transform.position.x && !isFacingLeft)
            {
                Flip();
            }
        }

        protected void BeStunned()
        {
            Vector2 newVelocity = new Vector2(-facingDir * stunDir.x, stunDir.y);
            SetVelocity(newVelocity, false);
        }

        protected void SlowBy(float _rate)
        {
            moveSpeed *= (1 - _rate);
            battleMoveSpeed *= (1 - _rate);
        }

        protected void RecoverSpeed()
        {
            moveSpeed = defaultMoveSpeed;
            battleMoveSpeed = defaultBattleMoveSpeed;
        }

        protected override void OnDrawGizmos()
        {
            base.OnDrawGizmos();
        }
    }
}

