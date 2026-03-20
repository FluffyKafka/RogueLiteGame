using EntityBehaviour;

namespace PlayerBebaviour
{
    internal class SPlayerIdle : SPlayerGround
    {
        public SPlayerIdle(CEntityStateMachine _stateMachine, MEntityBehaviour _entity) : base(_stateMachine, _entity)
        {
        }

        public override void Enter()
        {
            base.Enter();
            player.HorizonInput += Move;
            player.InvokeAction(player.ToIdle);
            player.InvokeAction(player.StandStillNotice);
            player.InvokeAction(player.SetGravityToZeroNotice, true);
        }

        public override void Exit()
        {
            base.Exit();
            player.HorizonInput -= Move;
            player.InvokeAction(player.SetGravityToZeroNotice, false);
        }

        public override void Update()
        {
            base.Update();
            player.InvokeAction(player.StandStillNotice);
        }

        protected void Move(float _speed)
        {
            stateMachine.ChangeState(playerStateMachine.move);
        }
    }
}