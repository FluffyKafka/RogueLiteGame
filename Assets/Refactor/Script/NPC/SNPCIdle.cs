using PlayerSystem;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace NPCSystem
{
    internal class SNPCIdle : SNPCStateBase
    {
        public override void Enter()
        {
            base.Enter();
            npc.PlayerInteractNotice += PlayerInteract;
            npc.AnimToIdle();
        }

        public override void Exit()
        {
            base.Exit();
            npc.PlayerInteractNotice -= PlayerInteract;
        }

        public override void Update()
        {
            base.Update();

        }

        protected void PlayerInteract(INPCPlayer _player)
        {
            if(npc.InvokeFunc(npc.CanInteractNotice))
            {
                stateMachine.ChangeState(ENPCStateType.BeforeEffectCommunicate);
            }          
        }
    }
}

