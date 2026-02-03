using EntityBehaviour;

namespace PlayerBebaviour
{
    internal class SPlayerMove : SPlayerGround
    {
        public SPlayerMove(CEntityStateMachine _stateMachine, MEntityBehaviour _entity) : base(_stateMachine, _entity)
        {
        }

        public override void Enter()
        {
            base.Enter();
            player.InvokeAction(player.ToMove);
        }

        public override void Exit()
        {
            base.Exit();
        }

        public override void Update()
        {
            base.Update();
            player.InvokeAction(player.Move, playerStateMachine.xInput);
            if (playerStateMachine.xInput == 0)
            {
                playerStateMachine.ChangeState(playerStateMachine.idle);
            }
        }
    }
}
