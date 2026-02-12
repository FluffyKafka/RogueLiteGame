using EntitySystem;
using PlayerSystem;
using StatsData;
using System;
using UnityEngine;
using UnityEngine.Assertions;

namespace EnemySystem
{
    public enum EEnemyType
    {
        Skeleton,
        Archer,
        Necromancer,
        Slime,
        SubSlime,
        MinSlime
    }
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
        public void ToControll();

        public bool IsPlayer(GameObject _object);
        public Vector3 CheckPlayerPosition();
        public Vector3 CheckPlayerVelocity();
        public float CheckPlayerGravityScale();
        public Transform CheckPlayerTransform();
        public WReadOnlyDamageData DamageToPlayer(GameObject _player, WReadOnlyDamageData _damage);
        public bool IsPlayerAlive();
        public bool IsPlayerAlive(GameObject _player);
        public float CheckArrowGravity();
        public void GenerateArrowAt(DProjectileData _data, Vector3 _position);
        public void GenerateSkullAmmoAt(DAmmoData _data, Vector3 _position);
        public GameObject GenerateEnemyByTypeAt(EEnemyType _enemyType, Vector3 _position);

        public WReadOnlyDamageData DamageToEnemy(GameObject _enemy, WReadOnlyDamageData _damage);

        public void ToSelfExplode();
        public void ToSelfExplodeHolding();
    }

    public interface IAnimEnemy : IAnimEntity
    {
        public abstract void OpenStun(bool _isOpen);
        public abstract void OnSelfExplodeDamageTrigger();
        public abstract void OnSelfExplodeFinish();
    }

    public interface IObjectEnemy : IObjectEntity
    {

    }

    internal class AEnemy : AEntity, IAnimEnemy, IPlayerEnemy, IObjectEnemy, IBehaviourEnemy
    {
        protected IEnemyPlayer player;
        protected IEnemyAnimation enemyAnim;
        protected IEnemyBehaviour behaviour;
        protected IEnemyFactory factory;

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
        public void Init(IEnemyPlayer _player, IEnemyObjectFactory _objectFactory, IEnemyFactory _factory)
        {
            player = _player;
            objectFactory = _objectFactory;
            enemyObjectFactory = _objectFactory;
            factory = _factory;
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

        void IBehaviourEnemy.ToControll()
        {
            enemyAnim.Controll();
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

        public Transform CheckPlayerTransform()
        {
            return player.CheckTransform();
        }

        public WReadOnlyDamageData DamageToPlayer(GameObject _player, WReadOnlyDamageData _damage)
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
        public void GenerateArrowAt(DProjectileData _data, Vector3 _position)
        {
            enemyObjectFactory.GenerateArrow(_data, _position);
        }

        public void GenerateSkullAmmoAt(DAmmoData _data, Vector3 _position)
        {
            _data.originEntity = this; 
            enemyObjectFactory.GenerateSkullAmmo(_data, _position);
        }

        public GameObject GenerateEnemyByTypeAt(EEnemyType _type, Vector3 _position)
        {
            return factory.GenerateEnemyByTypeAt(_type, _position);
        }

        public WReadOnlyDamageData DamageToEnemy(GameObject _enemy, WReadOnlyDamageData _damage)
        {
            return _enemy.GetComponent<IPlayerEnemy>().TakeDamage(_damage);
        }

        public void ToSelfExplode()
        {
            enemyAnim.ToSelfExplode();
        }

        public void ToSelfExplodeHolding()
        {
            enemyAnim.ToSelfExplodeHolding();
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

        void IAnimEnemy.OnSelfExplodeDamageTrigger()
        {
            behaviour.OnSelfExplodeDamageTrigger();
        }

        void IAnimEnemy.OnSelfExplodeFinish()
        {
            behaviour.OnSelfExplodeFinish();
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

        public void ObjectFinish()
        {
            behaviour.ObjectFinish();
        }
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
        public void ObjectFinish();
        public void OnSelfExplodeDamageTrigger();
        public void OnSelfExplodeFinish();
    }

    public interface IEnemyAnimation : IEntityAnimation
    {
        public void Idle();
        public void Move();
        public void Attack();
        public void PullBack();
        public void PullBackJump();
        public void Fall();
        public void Controll();
        public void ToSelfExplode();
        public void ToSelfExplodeHolding();
    }

    public interface IEnemyObjectFactory: IEntityObjectFactory
    {
        public void GenerateArrow(DProjectileData _data, Vector3 _position);
        public float CheckArrowGravityScale();

        public void GenerateSkullAmmo(DAmmoData _data, Vector3 _position);
    }

    public interface IEnemyFactory
    {
        public GameObject GenerateEnemyByTypeAt(EEnemyType _type, Vector3 _position);
    }
}


