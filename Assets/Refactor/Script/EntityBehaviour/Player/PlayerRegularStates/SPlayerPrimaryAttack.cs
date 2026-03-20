using EntityBehaviour;

namespace PlayerBebaviour
{
    internal class SPlayerPrimaryAttack : SPlayerRegularState
    {
        public SPlayerPrimaryAttack(CEntityStateMachine _stateMachine, MEntityBehaviour _entity) : base(_stateMachine, _entity)
        {
        }

        public override void Enter()
        {
            base.Enter();
            player.InvokeAction(player.AttackRaw);
            player.AttackFinish += OnAttackFinish;
            player.InvokeAction(player.SetGravityToZeroNotice, true);
        }

        public override void Exit()
        {
            base.Exit();
            playerStateMachine.BusyFor(player.InvokeFunc(player.CheckUnmovableDurationAfterAttack));
            player.AttackFinish -= OnAttackFinish;
            player.InvokeAction(player.SetGravityToZeroNotice, false);
        }

        public override void Update()
        {
            base.Update();
        }

        protected void OnAttackFinish()
        {
            playerStateMachine.ChangeState(playerStateMachine.idle);
        }
    }
}