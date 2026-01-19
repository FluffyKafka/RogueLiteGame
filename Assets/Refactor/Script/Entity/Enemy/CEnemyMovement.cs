using EntitySystem.EntityActor.EnemyActor;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Assertions;

namespace EntitySystem
{
    namespace EntityComponent
    {
        namespace MovementComponent
        {
            internal class CEnemyMovement : CEntityMovement
            {
                protected AEnemy enemy;

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

                    Assert.IsTrue(entity is AEnemy, "此为Enemy组件");
                    enemy = entity as AEnemy;

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
                    if(enemy.player.CheckPosition().x > enemy.transform.position.x && isFacingLeft)
                    {
                        Flip();
                    }
                    else if(enemy.player.CheckPosition().x < enemy.transform.position.x && !isFacingLeft)
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
    }
}

