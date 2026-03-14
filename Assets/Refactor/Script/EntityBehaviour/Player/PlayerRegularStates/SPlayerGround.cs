using EntityBehaviour;

namespace PlayerBebaviour
{
    internal class SPlayerGround : SPlayerRegularState
    {
        public SPlayerGround(CEntityStateMachine _stateMachine, MEntityBehaviour _entity) : base(_stateMachine, _entity)
        {
        }

        public override void Enter()
        {
            base.Enter();
            player.JumpInput += Jump;
            player.AttackInput += Attack;
            player.InteractToNPCInputNotice += TryInteractToNPC;
        }

        public override void Exit()
        {
            base.Exit();
            player.JumpInput -= Jump;
            player.AttackInput -= Attack;
            player.InteractToNPCInputNotice -= TryInteractToNPC;
        }

        public override void Update()
        {
            base.Update();
        }

        protected void Jump()
        {
            playerStateMachine.ChangeState(playerStateMachine.jump);
        }
        protected void Attack()
        {
            playerStateMachine.ChangeState(playerStateMachine.primaryAttack);
        }
        protected void TryInteractToNPC()
        {
            bool isInBattle = player.InvokeFunc(player.CheckIsPlayerInBattleNotice);
            if(!isInBattle)
            {
                player.InvokeAction(player.InteractToNPCNotice);
            }
        }
    }
}
