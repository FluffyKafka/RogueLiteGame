using EntityBehaviour;

namespace PlayerBebaviour
{
    internal class SPlayerFall : SPlayerAir
    {
        public SPlayerFall(CEntityStateMachine _stateMachine, MEntityBehaviour _entity) : base(_stateMachine, _entity)
        {
        }

        public override void Enter()
        {
            base.Enter();
        }

        public override void Exit()
        {
            base.Exit();
        }

        public override void Update()
        {
            base.Update();

            if (player.InvokeFunc(player.IsGroundedOrPlatForm))
            {
                playerStateMachine.ChangeState(playerStateMachine.idle);
            }
            else if (player.InvokeFunc(player.IsTouchWall) && playerStateMachine.xInput == player.InvokeFunc(player.CheckFacingDir))
            {
                stateMachine.ChangeState(playerStateMachine.wallSlide);
            }
        }
    }
}
