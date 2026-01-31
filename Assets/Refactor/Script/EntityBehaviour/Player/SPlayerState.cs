using EntityBehaviour;
using UnityEngine.Assertions;

namespace PlayerBebaviour
{
    internal class SPlayerState : SEntityState
    {
        protected MPlayerBeviour player;
        protected CPlayerStateMachine playerStateMachine;

        public SPlayerState(CEntityStateMachine _stateMachine, MEntityBehaviour _entity) : base(_stateMachine, _entity)
        {
            Assert.IsTrue(_entity is MPlayerBeviour, "此状态属于Player专用状态，不能被用于控制其他Entity");
            player = _entity as MPlayerBeviour;
            Assert.IsTrue(_stateMachine is CPlayerStateMachine, "此状态属于Player专用状态，只能被Player状态机持有");
            playerStateMachine = _stateMachine as CPlayerStateMachine;
        }

        override public void Enter()
        {
            base.Enter();
        }

        override public void Update()
        {
            base.Update();
        }

        override public void Exit()
        {
            base.Exit();
        }
    }
}

