using EntitySystem.EntityActor.PlayerActor;
using EntitySystem.EntityState;
using EntitySystem.EntityState.PlayerState;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Assertions;

namespace EntitySystem
{
    namespace EntityComponent
    {
        namespace StateMachineComponent
        {
            internal class CPlayerStateMachine : CEntityStateMachine
            {
                #region StateSet
                public SEntityState idle { get; protected set; }
                [SerializeField] protected string idleAnimName = "Idle";
                public SEntityState move { get; protected set; }
                [SerializeField] protected string moveAnimName = "Move";
                public SEntityState jump { get; protected set; }
                [SerializeField] protected string jumpAnimName = "Air";
                public SEntityState fall { get; protected set; }
                [SerializeField] protected string fallAnimName = "Air";
                public SEntityState wallSlide { get; protected set; }
                [SerializeField] protected string wallSlideAnimName = "WallSlide";
                public SEntityState wallJump { get; protected set; }
                [SerializeField] protected string wallJumpAnimName = "Air";
                public SEntityState primaryAttack { get; protected set; }
                [SerializeField] protected string primaryAttackAnimName = "Attack";
                #endregion

                #region InputHandle
                public float xInput { get; protected set; } = 0;
                public float yInput { get; protected set; } = 0;
                #endregion

                protected APlayer player;

                protected bool isPlayerShouldBeIdle = false;

                protected override void Awake()
                {
                    base.Awake();

                    Assert.IsTrue(entity is APlayer, "Player状态机组件需要被附加至一个Player实体");
                    player = entity as APlayer;

                    idle = new SPlayerIdle(this, entity, idleAnimName);
                    move = new SPlayerMove(this, entity, moveAnimName);
                    jump = new SPlayerJump(this, entity, jumpAnimName);
                    fall = new SPlayerFall(this, entity, fallAnimName);
                    wallSlide = new SPlayerWallSlide(this, entity, wallSlideAnimName);
                    wallJump = new SPlayerWallJump(this, entity, wallJumpAnimName);
                    primaryAttack = new SPlayerPrimaryAttack(this, entity, primaryAttackAnimName);
                }

                protected override void Start()
                {
                    base.Start();
                    Initialize(idle);
                }

                protected override void Update()
                {
                    xInput = player.InvokeFunc(player.CheckHorizonInput);
                    yInput = player.InvokeFunc(player.CheckVerticalInput);
                    base.Update();
                }

                public override void ChangeState(SEntityState _newState)
                {
                    if (isPlayerShouldBeIdle && currentState == idle)
                    {
                        return;
                    }

                    base.ChangeState(_newState);
                }

                public void BusyFor(float _duration)
                {
                    StartCoroutine(BusyForHelper(_duration));
                }
                protected IEnumerator BusyForHelper(float _duration)
                {
                    isPlayerShouldBeIdle = true;
                    yield return new WaitForSeconds(_duration);
                    isPlayerShouldBeIdle = false;
                }
            }
        }
    }
}

