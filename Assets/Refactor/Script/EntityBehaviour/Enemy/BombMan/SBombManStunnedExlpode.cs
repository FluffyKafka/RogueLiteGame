using EntityBehaviour;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace EnemyBehaviour
{
    internal class SBombManStunnedExlpode : SBombManState
    {
        public SBombManStunnedExlpode(CEntityStateMachine _stateMachine, MEntityBehaviour _entity) : base(_stateMachine, _entity)
        {
        }

        public override void Enter()
        {
            base.Enter();
            enemy.InvokeAction(enemy.SelfExplodeNotice_isReflect, true);
            enemy.InvokeAction(enemy.ToSelfExplode);
            enemy.OnSelfExplodeFinish += ExplodeFinish;
        }

        public override void Exit()
        {
            base.Exit();
            enemy.OnSelfExplodeFinish -= ExplodeFinish;
        }

        public override void Update()
        {
            base.Update();
        }
        protected void ExplodeFinish()
        {
            enemyStateMachine.ChangeState(enemyStateMachine.dead);
        }
    }
}

