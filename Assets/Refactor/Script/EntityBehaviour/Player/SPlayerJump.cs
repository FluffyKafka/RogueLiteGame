using EntityBehaviour;

namespace PlayerBebaviour
{
    internal class SPlayerJump : SPlayerAir
    {
        protected bool isFinishJump = false;
        public SPlayerJump(CEntityStateMachine _stateMachine, MEntityBehaviour _entity) : base(_stateMachine, _entity)
        {
        }

        public override void Enter()
        {
            base.Enter();
            player.InvokeAction(player.Jump);
            isFinishJump = false;
        }

        public override void Exit()
        {
            base.Exit();
        }

        public override void Update()
        {
            base.Update();
            if (player.InvokeFunc(player.IsFall))
            {
                playerStateMachine.ChangeState(playerStateMachine.fall);
                return;
            }

            if (player.InvokeFunc(player.IsGroundedOrPlatForm))
            {
                if (isFinishJump)
                {
                    playerStateMachine.ChangeState(playerStateMachine.idle);
                }
            }
            else
            {
                if (!isFinishJump)
                {
                    isFinishJump = true;
                }
            }
        }
    }
}