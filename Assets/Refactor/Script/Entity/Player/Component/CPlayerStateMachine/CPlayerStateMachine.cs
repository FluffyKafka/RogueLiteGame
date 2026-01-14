using EntitySystem.EntityState;
using EntitySystem.EntityState.PlayerState;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace EntitySystem
{
    namespace EntityComponent
    {
        namespace StateMachineComponent
        {
            internal class CPlayerStateMachine : CEntityStateMachine
            {
                #region StateSet
                public SEntityState idle{ get; protected set; }
                [SerializeField] protected string idleAnimName = "Idle";
                public SEntityState move { get; protected set; }
                [SerializeField] protected string moveAnimName = "Move";
                public SEntityState jump { get; protected set; }
                [SerializeField] protected string jumpAnimName = "Air";
                public SEntityState fall { get; protected set; }
                [SerializeField] protected string fallAnimName = "Air";
                #endregion

                protected override void Awake()
                {
                    base.Awake();
                    idle = new SPlayerIdle(this, entity, idleAnimName);
                    move = new SPlayerMove(this, entity, moveAnimName);
                    jump = new SPlayerJump(this, entity, jumpAnimName);
                    fall = new SPlayerFall(this, entity, fallAnimName);
                }

                protected void Start()
                {
                    Initialize(idle);
                }
            }
        }
    }
}

