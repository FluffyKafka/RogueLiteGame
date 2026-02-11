
using UnityEngine;

namespace EnemyBehaviour
{
    internal class SArcherPullBackJump : SArcherBattle
    {        
        public SArcherPullBackJump(CArcherStateMachine _stateMachine, MEnemyBehaviour _entity) : base(_stateMachine, _entity)
        {
        }

        protected bool isJumpFinish = false;

        public override void Enter()
        {
            base.Enter();
            enemy.InvokeAction(enemy.EffectPullBackJump);
            enemy.InvokeAction(enemy.ToPullbackJump);
        }

        public override void Exit()
        {
            base.Exit();
        }

        public override void Update()
        {
            base.Update();

            if(enemy.InvokeFunc(enemy.IsFall))
            {
                enemy.InvokeAction(enemy.ToFall);
            }

            if(enemy.InvokeFunc(enemy.IsGroundedOrPlatForm))
            {
                if (!isJumpFinish)
                {
                    isJumpFinish = true;
                }
                else
                { 
                    enemyStateMachine.ChangeState(enemyStateMachine.battleIdle);
                }
            }
        }
    }
}
