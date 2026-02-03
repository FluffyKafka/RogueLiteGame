using EntityBehaviour;

namespace PlayerBebaviour
{
    internal class SPlayerWallJump : SPlayerAir
    {
        public SPlayerWallJump(CEntityStateMachine _stateMachine, MEntityBehaviour _entity) : base(_stateMachine, _entity)
        {
        }

        public override void Enter()
        {
            base.Enter();
            player.InvokeAction(player.WallJump);
            canMove = false;
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
                stateMachine.ChangeState(playerStateMachine.fall);
            }
        }
    }
}