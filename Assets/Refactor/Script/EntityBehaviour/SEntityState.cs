namespace EntityBehaviour
{
    internal class SEntityState
    {
        protected CEntityStateMachine stateMachine;
        protected MEntityBehaviour entity;

        public SEntityState(CEntityStateMachine _stateMachine, MEntityBehaviour _entity)
        {
            this.stateMachine = _stateMachine;
            entity = _entity;
        }

        public virtual void Enter()
        {

        }

        public virtual void Update()
        {

        }

        public virtual void Exit()
        {

        }
    }
}