using EntityBehaviour;

namespace PlayerBebaviour
{
    internal class SPlayerGround : SPlayerState
    {
        public SPlayerGround(CEntityStateMachine _stateMachine, MEntityBehaviour _entity) : base(_stateMachine, _entity)
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
