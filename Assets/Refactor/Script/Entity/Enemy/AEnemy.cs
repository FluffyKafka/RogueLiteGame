using EntitySystem;
using PlayerSystem;
using StatsData;
using UnityEngine;
using UnityEngine.Assertions;

namespace EnemySystem
{
    public interface IBehaviourEnemy
    {
        public void ToAttack();
        public void BeStunned();
        public void StunnedFinish();
        public void ToIdle();
        public void ToMove();

        public bool IsPlayer(GameObject _object);
        public Vector3 CheckPlayerPosition();
        public WReadOnlyDamageData DamageTo(GameObject _player, WReadOnlyDamageData _damage);
        public bool IsPlayerAlive();
        public bool IsPlayerAlive(GameObject _player);
    }

    public interface IAnimEnemy : IAnimEntity
    {
        public abstract void OpenStun(bool _isOpen);
    }

    public interface IObjectEnemy
    {

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
        }
        protected virtual void Start()
        {
            if (player == null)
            {
                MEnemyFactory.GetInstance_TestMode().InitEnemyNotGenerateByFactory_TestMode(this);
            }

        }

        #region Init
        public void Init(IEnemyPlayer _player)
        {
            player = _player;
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

        public bool IsPlayer(GameObject _object)
        {
            return _object.GetComponent<IEnemyPlayer>() != null;
        }

        public Vector3 CheckPlayerPosition()
        {
            return player.CheckPosition();
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

    }

    public interface IEnemyBehaviour
    {
        public void OpenStun(bool _isOpen);
        public void StunCheck();
    }

    public interface IEnemyAnimation : IEntityAnimation
    {
        public void Idle();
        public void Move();
        public void Attack();
    }
    public interface IEnemyObjectFactory
    {

    }
}


