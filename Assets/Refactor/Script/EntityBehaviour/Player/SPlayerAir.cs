using EntityBehaviour;

namespace PlayerBebaviour
{
    internal class SPlayerAir : SPlayerState
    {
        protected bool canMove = true;
        public SPlayerAir(CEntityStateMachine _stateMachine, MEntityBehaviour _entity) : base(_stateMachine, _entity)
        {
        }

        public override void Enter()
        {
            base.Enter();
            player.JumpInput += Jump;
            player.AttackInput += Attack;
        }

        public override void Exit()
        {
            base.Exit();
            player.JumpInput -= Jump;
            player.AttackInput -= Attack;
        }

        public override void Update()
        {
            base.Update();
            player.InvokeAction(player.UpdateYVelocity, player.InvokeFunc(player.CheckYVelocity));
            if (canMove)
            {
                player.InvokeAction(player.Move, playerStateMachine.xInput);
            }
        }

        protected void Jump()
        {
            playerStateMachine.ChangeState(playerStateMachine.jump);
        }
        protected void Attack()
        {
            playerStateMachine.ChangeState(playerStateMachine.primaryAttack);
        }
    }
}

