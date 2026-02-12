using EntityBehaviour;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace EnemyBehaviour
{
    internal class SBombManExplodeHolding : SBombManState
    {
        public SBombManExplodeHolding(CEntityStateMachine _stateMachine, MEntityBehaviour _entity) : base(_stateMachine, _entity)
        {
        }

        protected Coroutine explodeTimer;

        public override void Enter()
        {
            base.Enter();
            enemy.BeStunned += OnStun;
            explodeTimer = enemyStateMachine.StartCoroutine(ExplodeAfter());
            enemy.InvokeAction(enemy.ToSelfExplodeHolding);
        }

        public override void Exit()
        {
            base.Exit();
            enemy.BeStunned -= OnStun;
            enemyStateMachine.StopCoroutine(explodeTimer);
        }

        public override void Update()
        {
            base.Update();
        }

        protected void OnStun()
        {
            enemyStateMachine.ChangeState(enemyStateMachine.stunnedExplode);
        }

        protected IEnumerator ExplodeAfter()
        {
            yield return new WaitForSeconds(enemy.InvokeFunc(enemy.CheckSelfExplodeHoldingDurationNotice));
            enemyStateMachine.ChangeState(enemyStateMachine.explode);
        }
    }
}

