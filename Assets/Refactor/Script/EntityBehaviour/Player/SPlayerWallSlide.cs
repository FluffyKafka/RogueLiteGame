using EntityBehaviour;

namespace PlayerBebaviour
{
    internal class SPlayerWallSlide : SPlayerState
    {

        public SPlayerWallSlide(CEntityStateMachine _stateMachine, MEntityBehaviour _entity) : base(_stateMachine, _entity)
        {
        }

        public override void Enter()
        {
            base.Enter();
            player.JumpInput += OnJump;
            player.InvokeAction(player.ToWallSlide);
        }

        public override void Exit()
        {
            base.Exit();
            player.JumpInput -= OnJump;
        }

        public override void Update()
        {
            base.Update();
            player.WallSlide?.Invoke(playerStateMachine.yInput);

            bool IsGroundedOrPlatForm = player.InvokeFunc(player.IsGroundedOrPlatForm);
            bool isTouchWall = player.InvokeFunc(player.IsTouchWall);
            int facingDir = player.InvokeFunc(player.CheckFacingDir);

            if (IsGroundedOrPlatForm || !isTouchWall)
            {
                stateMachine.ChangeState(playerStateMachine.idle);
            }
            else if (playerStateMachine.xInput != facingDir)
            {
                stateMachine.ChangeState(playerStateMachine.fall);
            }
        }

        public void OnJump()
        {
            playerStateMachine.ChangeState(playerStateMachine.wallJump);
        }

    }
}