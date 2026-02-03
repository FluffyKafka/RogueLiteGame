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
        }

        public override void Exit()
        {
            base.Exit();
            player.HorizonInput -= Move;
        }

        public override void Update()
        {
            base.Update();
        }

        protected void Move(float _speed)
        {
            stateMachine.ChangeState(playerStateMachine.move);
        }
    }
}