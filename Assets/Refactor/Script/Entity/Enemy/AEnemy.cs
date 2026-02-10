using EntitySystem;
using PlayerSystem;
using StatsData;
using UnityEngine;
using UnityEngine.Assertions;

namespace EnemySystem
{
    public interface IBehaviourEnemy: IBehaviourEntity
    {
        public void ToAttack();
        public void BeStunned();
        public void StunnedFinish();
        public void ToIdle();
        public void ToMove();
        public void ToPullBack();
        public void ToPullBackJump();
        public void ToFall();

        public bool IsPlayer(GameObject _object);
        public Vector3 CheckPlayerPosition();
        public Vector3 CheckPlayerVelocity();
        public float CheckPlayerGravityScale();
        public WReadOnlyDamageData DamageTo(GameObject _player, WReadOnlyDamageData _damage);
        public bool IsPlayerAlive();
        public bool IsPlayerAlive(GameObject _player);
        public float CheckArrowGravity();
        public void GenerateArrowAt(DArrowData _data, Vector3 _position);
    }

    public interface IAnimEnemy : IAnimEntity
    {
        public abstract void OpenStun(bool _isOpen);       
    }

    public interface IObjectEnemy
    {
        public WReadOnlyDamageData TakeObjectDamage(WReadOnlyDamageData _damage);
        public Transform CheckTransform();
    }

    internal class AEnemy : AEntity, IAnimEnemy, IPlayerEnemy, IObjectEnemy, IBehaviourEnemy
    {
        protected IEnemyPlayer player;
        protected IEnemyAnimation enemyAnim;
        protected IEnemyBehaviour behaviour;

        protected override void Awake()
        {
            base.Awake();

            Assert.IsTrue(anim is IEnemyAnimation);
            enemyAnim = anim as IEnemyAnimation;

            behaviour = GetComponent<IEnemyBehaviour>();
            AttackFinish += behaviour.AttackFinish;
            AttackDamageTrigger += behaviour.AttackDamageTrigger;
            CanBeDamage += behaviour.CanBeDamage;
        }
        protected virtual void Start()
        {
            if (player == null)
            {
                FEnemyFactory.GetInstance_TestMode().InitEnemyNotGenerateByFactory_TestMode(this);
            }

        }

        #region Init
        public void Init(IEnemyPlayer _player, IEnemyObjectFactory _objectFactory)
        {
            player = _player;
            objectFactory = _objectFactory;
            enemyObjectFactory = _objectFactory;
        }
        #endregion

        #region Behaviour
        public void ToAttack()
        {
            enemyAnim.Attack();
        }

        void IBehaviourEnemy.BeStunned()
        {
            enemyAnim.BeStunned();
        }

        public void StunnedFinish()
        {
            enemyAnim.StunFinish();
        }

        void IBehaviourEnemy.ToIdle()
        {
            enemyAnim.Idle();
        }

        void IBehaviourEnemy.ToMove()
        {
            enemyAnim.Move();
        }

        void IBehaviourEnemy.ToPullBack()
        {
            enemyAnim.PullBack();
        }

        void IBehaviourEnemy.ToPullBackJump()
        {
            enemyAnim.PullBackJump();
        }

        void IBehaviourEnemy.ToFall()
        {
            enemyAnim.Fall();
        }

        public bool IsPlayer(GameObject _object)
        {
            return _object.GetComponent<IEnemyPlayer>() != null;
        }

        public Vector3 CheckPlayerPosition()
        {
            return player.CheckPosition();
        }

        public Vector3 CheckPlayerVelocity()
        {
            return player.CheckVelocity();
        }

        public float CheckPlayerGravityScale()
        {
            return player.CheckGravityScale();
        }

        public WReadOnlyDamageData DamageTo(GameObject _player, WReadOnlyDamageData _damage)
        {
            return _player.GetComponent<IEnemyPlayer>().TakeDamage(_damage);
        }
        public bool IsPlayerAlive()
        {
            return !player.IsDead();
        }
        public bool IsPlayerAlive(GameObject _player)
        {
            return !_player.GetComponent<IEnemyPlayer>().IsDead();
        }

        public float CheckArrowGravity()
        {
            return enemyObjectFactory.CheckArrowGravityScale();
        }
        public void GenerateArrowAt(DArrowData _data, Vector3 _position)
        {
            enemyObjectFactory.GenerateArrow(_data, _position);
        }

        #endregion

        #region Animation
        void IAnimEntity.AttackDamageTrigger()
        {
            InvokeAction(AttackDamageTrigger);
        }

        void IAnimEnemy.OpenStun(bool _isOpen)
        {
            behaviour.OpenStun(_isOpen);
        }

        void IAnimEntity.AttackFinish()
        {
            InvokeAction(AttackFinish);
        }
        #endregion

        #region Player
        bool IPlayerEnemy.IsDead()
        {
            return isDead;
        }
        WReadOnlyDamageData IPlayerEnemy.TakeDamage(WReadOnlyDamageData _damageData)
        {
            WReadOnlyDamageData damage = InvokeFunc(CalculateDamageTaken, _damageData);
            InvokeAction(TakeDamage, damage);
            return damage;
        }
        void IPlayerEnemy.StunCheck()
        {
            behaviour.StunCheck();
        }
        #endregion

        #region Object
        protected IEnemyObjectFactory enemyObjectFactory;
        public WReadOnlyDamageData TakeObjectDamage(WReadOnlyDamageData _damage)
        {
            WReadOnlyDamageData damage = InvokeFunc(CalculateDamageTaken, _damage);
            InvokeAction(TakeDamage, damage);
            return damage;
        }
        public Transform CheckTransform()
        {
            return transform;
        }
        #endregion

    }

    public interface IEnemyBehaviour : IEntityBehaviour
    {
        public void OpenStun(bool _isOpen);
        public void StunCheck();
    }

    public interface IEnemyAnimation : IEntityAnimation
    {
        public void Idle();
        public void Move();
        public void Attack();
        public void PullBack();
        public void PullBackJump();
        public void Fall();
    }
    public interface IEnemyObjectFactory: IEntityObjectFactory
    {
        public void GenerateArrow(DArrowData _data, Vector3 _position);
        public float CheckArrowGravityScale();
    }
}


