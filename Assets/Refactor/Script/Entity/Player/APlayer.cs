using EntitySystem;
using Item;
using StatsData;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Assertions;

namespace PlayerSystem
{
    public interface IInitPlayer
    {
        public void Init(IPlayerInput _inputSource, IPlayerInventory _inventory, IPlayerUI _ui, IPlayerObjectFactory _factory, IPlayerSkillManager _skillManager);
    }

    public interface IInputPlayer
    {
        public void HorizonInput(float _input);
        public void VerticalInput(float _input);
        public void JumpInput();
        public void AttackInput();
        public void SkillInput(int _input);
    }

    public interface IEnemyPlayer
    {
        public bool IsDead();
        public Vector3 CheckPosition();
        public Vector3 CheckVelocity();
        public float CheckGravityScale();

        public WReadOnlyDamageData TakeDamage(WReadOnlyDamageData _damageData);
    }

    public interface IAnimPlayer : IAnimEntity
    {

    }

    //Inventory似乎不应该使用这两个接口：
    //public void AddModifier(WReadOnlyStatsData _data);
    //public void RemoveModifier(WReadOnlyStatsData _data);
    //考虑改名和改变参数类型表示装备被装备的行为，传输IEquipmentData或IEquipment（物品耐久磨损？）
    public interface IInventoryPlayer
    {
        public void AddModifier(WReadOnlyStatsData _data);
        public void RemoveModifier(WReadOnlyStatsData _data);
        public void StashFullNotice(IItem _item);
        public void DiscardItem(IItem _item);
        public void EquipmentChangeNotice(EEquipmentType _type, IEquipment _equip);
        public void EquipmentStashChangeNotice(IReadOnlyList<IEquipment> _stash);
        public void MaterialStashChangeNotice(IReadOnlyList<IItem> _stash);
    }

    public interface IUIPlayer
    {
        public IReadOnlyList<IItemData> TryCraft(IEquipmentData _data);
        public void Equip(IEquipment _equip);
        public void UnEquip(IEquipment _Equip);
        public void DropItem(IItem _item);
        public IEquipment CheckEquipmentByType(EEquipmentType _type);
        public IReadOnlyList<IEquipment> CheckEquipmentStash();
        public IReadOnlyList<IItem> CheckMaterialStash();
        public int CheckEquipmentStashMaxSize();
        public int CheckMaterialStashMaxSize();
        public IReadOnlyList<IEquipmentData> CheckCraftableEquipmentByType(EEquipmentType _type);
        public float TryCheckStat(EStatType _type);
    }

    public interface IStatsPlayer : IStatEntity
    {
        public void StatsChangeNotice();
    }

    public interface IObjectPlayer
    {
        public bool TryTakeItem(IItem _item);
        public WReadOnlyDamageData TakeObjectDamage(WReadOnlyDamageData _damage);
        public Transform CheckTransform();
    }

    public interface IBehaviourPlayer : IBehaviourEntity
    {
        public void ToJump();
        public void ToWallJump();
        public void ToAttack(int _count);
        public void UpdateYVelocity(float _yVelocity);
        public void ToIdle();
        public void ToMove();
        public void ToWallSlide();
        public float CheckHorizonInput();
        public float CheckVerticalInput();

        public bool IsEnemy(GameObject _enemy);
        public bool IsEnemyAlive(GameObject _enemy);
        public WReadOnlyDamageData DamageTo(GameObject _enemy, WReadOnlyDamageData _damage);
        public void StunCheck(GameObject _enemy);
    }

    public interface ISkillManagerPlayer
    {
        public void DashBegin(float _speed);
        public void DashEnd();

        public bool CanEffectBehaviourSkill();
    }

    internal class APlayer : AEntity, IInitPlayer, IInputPlayer, IAnimPlayer, IEnemyPlayer, IInventoryPlayer, IUIPlayer, IStatsPlayer, IObjectPlayer, IBehaviourPlayer, ISkillManagerPlayer
    {
        protected IPlayerInput input;
        protected IPlayerInventory inventory;
        protected IPlayerUI ui;
        protected IPlayerObjectFactory playerObjectFactory;
        protected IPlayerAnimation playerAnim;
        protected IPlayerBehaviour behaviour;
        protected IPlayerSkillManager skillManager;

        //将Behaviour的每个行为对应到具体的Animation的工作目前由Entity完成，这是错误的，Entity只应该进行信息转发，而不应该处理逻辑
        //暂时直接转发，若有需要再引入事件机制
        #region Behaviour
        void IBehaviourPlayer.ToJump()
        {
            playerAnim.Air();
        }

        void IBehaviourPlayer.ToWallJump()
        {
            playerAnim.Air();
        }

        void IBehaviourPlayer.ToAttack(int _count)
        {
            playerAnim.Attack(_count);
        }

        void IBehaviourPlayer.UpdateYVelocity(float _yVelocity)
        {
            playerAnim.UpdateYVelocity(_yVelocity);
        }

        void IBehaviourPlayer.ToIdle()
        {
            playerAnim.Idle();
        }

        void IBehaviourPlayer.ToMove()
        {
            playerAnim.Move();
        }

        void IBehaviourPlayer.ToWallSlide()
        {
            playerAnim.WallSlide();
        }

        float IBehaviourPlayer.CheckHorizonInput()
        {
            return input.CheckHorizonInput();
        }

        float IBehaviourPlayer.CheckVerticalInput()
        {
            return input.CheckVerticalInput();
        }

        public bool IsEnemy(GameObject _enemy)
        {
            return _enemy.GetComponent<IPlayerEnemy>() != null;
        }

        public bool IsEnemyAlive(GameObject _enemy)
        {
            return !_enemy.GetComponent<IPlayerEnemy>().IsDead();
        }

        public WReadOnlyDamageData DamageTo(GameObject _enemy, WReadOnlyDamageData _damage)
        {
            return _enemy.GetComponent<IPlayerEnemy>().TakeDamage(_damage);
        }

        public void StunCheck(GameObject _enemy)
        {
            _enemy.GetComponent<IPlayerEnemy>().StunCheck();
        }
        #endregion

        //暂时直接转发，若有需要再引入事件机制
        #region Init
        void IInitPlayer.Init(IPlayerInput _inputSource, IPlayerInventory _inventory, IPlayerUI _ui, IPlayerObjectFactory _factory, IPlayerSkillManager _skillManager)
        {
            input = _inputSource;
            inventory = _inventory;
            ui = _ui;
            playerObjectFactory = _factory;
            objectFactory = _factory;
            skillManager = _skillManager;
        }
        #endregion

        //暂时直接转发，若有需要再引入事件机制
        #region Input
        void IInputPlayer.HorizonInput(float _input)
        {
            behaviour.HorizonInput(_input);
        }
        void IInputPlayer.VerticalInput(float _input)
        {
            behaviour.VerticalInput(_input);
        }
        void IInputPlayer.JumpInput()
        {
            behaviour.JumpInput();
        }
        void IInputPlayer.AttackInput()
        {
            behaviour.AttackInput();
        }
        void IInputPlayer.SkillInput(int _input)
        {
            skillManager.SkillInput(_input);
        }
        #endregion

        //暂时直接转发，若有需要再引入事件机制
        #region Enemy
        bool IEnemyPlayer.IsDead()
        {
            return isDead;
        }
        Vector3 IEnemyPlayer.CheckPosition()
        {
            return transform.position;
        }
        WReadOnlyDamageData IEnemyPlayer.TakeDamage(WReadOnlyDamageData _damageData)
        {
            WReadOnlyDamageData damage = InvokeFunc(CalculateDamageTaken, _damageData);
            InvokeAction(TakeDamage, damage);
            return damage;
        }

        Vector3 IEnemyPlayer.CheckVelocity()
        {
            return GetComponent<Rigidbody2D>().velocity;
        }

        float IEnemyPlayer.CheckGravityScale()
        {
            return GetComponent<Rigidbody2D>().gravityScale;
        }
        #endregion

        //暂时直接转发，若有需要再引入事件机制
        #region Inventory
        public Action<IItem> StashFullNotice;
        public Action<IItem> DiscardItemNotice;
        void IInventoryPlayer.AddModifier(WReadOnlyStatsData _data)
        {
            InvokeAction(AddModifier, _data);
        }
        void IInventoryPlayer.RemoveModifier(WReadOnlyStatsData _data)
        {
            InvokeAction(RemoveModifier, _data);
        }

        void IInventoryPlayer.EquipmentChangeNotice(EEquipmentType _type, IEquipment _equip)
        {
            ui.EquipmentChangeNotice(_type, _equip);
        }
        void IInventoryPlayer.EquipmentStashChangeNotice(IReadOnlyList<IEquipment> _stash)
        {
            ui.EquipmentStashChangeNotice(_stash);
        }
        void IInventoryPlayer.MaterialStashChangeNotice(IReadOnlyList<IItem> _stash)
        {
            ui.MaterialStashChangeNotice(_stash);
        }

        void IInventoryPlayer.StashFullNotice(IItem _itemToFull)
        {
            InvokeAction(StashFullNotice, _itemToFull);
        }
        void IInventoryPlayer.DiscardItem(IItem _item)
        {
            InvokeAction(DiscardItemNotice, _item);
        }
        #endregion

        //暂时直接转发，若有需要再引入事件机制
        #region Stats
        void IStatsPlayer.StatsChangeNotice()
        {
            ui.StatsChangeNotice();
        }
        #endregion

        //暂时直接转发，若有需要再引入事件机制
        #region UI
        IReadOnlyList<IItemData> IUIPlayer.TryCraft(IEquipmentData _data)
        {
            return inventory.TryCraft(_data);
        }

        void IUIPlayer.Equip(IEquipment _equip)
        {
            inventory.Equip(_equip);
        }

        void IUIPlayer.UnEquip(IEquipment _equip)
        {
            inventory.UnEquip(_equip);
        }

        void IUIPlayer.DropItem(IItem _item)
        {
            inventory.DropFromStash(_item);
        }

        IEquipment IUIPlayer.CheckEquipmentByType(EEquipmentType _type)
        {
            return inventory.CheckEquipmentByType(_type);
        }

        IReadOnlyList<IEquipment> IUIPlayer.CheckEquipmentStash()
        {
            return inventory.CheckEquipmentStash();
        }

        IReadOnlyList<IItem> IUIPlayer.CheckMaterialStash()
        {
            return inventory.CheckMaterialStash();
        }

        int IUIPlayer.CheckEquipmentStashMaxSize()
        {
            return inventory.CheckEquipmentStashMaxSize();
        }

        int IUIPlayer.CheckMaterialStashMaxSize()
        {
            return inventory.CheckMaterialStashMaxSize();
        }

        IReadOnlyList<IEquipmentData> IUIPlayer.CheckCraftableEquipmentByType(EEquipmentType _type)
        {
            return inventory.CheckCraftableEquipmentByType(_type);
        }

        public float TryCheckStat(EStatType _type)
        {
            return stats.TryCheckStat(_type);
        }
        #endregion

        //暂时直接转发，若有需要再引入事件机制
        #region ObjectController
        public bool TryTakeItem(IItem _item)
        {
            return inventory.TryTakeItem(_item);
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

        #region Skill
        public void DashBegin(float _speed)
        {
            behaviour.DashBegin(_speed);
            playerAnim.DashBegin();
        }

        public void DashEnd()
        {
            behaviour.DashEnd();
            playerAnim.DashEnd();
        }

        public bool CanEffectBehaviourSkill()
        {
            return behaviour.CanEffectBehaviourSkill();
        }
        #endregion

        protected override void Awake()
        {
            base.Awake();

            Assert.IsTrue(anim is IPlayerAnimation, "Player需要一个IPlayerAnimation的动画组件");
            playerAnim = anim as IPlayerAnimation;

            behaviour = GetComponentInChildren<IPlayerBehaviour>();
            AttackFinish += behaviour.AttackFinish;
            AttackDamageTrigger += behaviour.AttackDamageTrigger;
            CanBeDamage += behaviour.CanBeDamage;

            //Inventory
            StashFullNotice += ui.StashFullNotice;
            StashFullNotice += (IItem _item) => { playerObjectFactory.GenerateDropItemObject(_item, transform.position); };
            DiscardItemNotice += (IItem _item) => { playerObjectFactory.GenerateDropItemObject(_item, transform.position); };
        }
    }

    public interface IPlayerSkillManager
    {
        public void SkillInput(int _input);
    }

    public interface IPlayerBehaviour : IEntityBehaviour
    {
        public void HorizonInput(float _xInput);
        public void VerticalInput(float _yInput);
        public void JumpInput();
        public void AttackInput();
        public void DashBegin(float _speed);
        public void DashEnd();
        public bool CanEffectBehaviourSkill();
    }

    public interface IPlayerEnterable : IEntityObject
    {
        public void Enter(IObjectPlayer _player);
    }
    public interface IPlayerInteractable : IEntityObject
    {
        public void Interact(IObjectPlayer _player);
    }
    public interface IPlayerReflectable : IEntityObject
    {
        public void Reflect(IObjectPlayer _player);
    }
    public interface IPlayerObjectFactory: IEntityObjectFactory
    {
        public void GenerateDropItemObject(IItem _data, Vector3 position);
    }

    public interface IPlayerAnimation : IEntityAnimation
    {
        public abstract void Idle();
        public abstract void Move();
        public abstract void Attack(int _count);
        public abstract void Air();
        public abstract void UpdateYVelocity(float _yVelocity);
        public abstract void WallSlide();
        public abstract void DashBegin();
        public abstract void DashEnd();
    }

    public interface IPlayerInput
    {
        public float CheckHorizonInput();
        public float CheckVerticalInput();
    }

    public interface IPlayerEnemy
    {
        public bool IsDead();

        public WReadOnlyDamageData TakeDamage(WReadOnlyDamageData _damageData);

        public void StunCheck();
    }

    public interface IPlayerInventory
    {
        public void Equip(IEquipment _newEquip);
        public void UnEquip(IEquipment _equip);
        public IEquipment CheckEquipmentByType(EEquipmentType _type);
        public IReadOnlyList<IEquipment> CheckEquipmentStash();
        public int CheckEquipmentStashMaxSize();
        public IReadOnlyList<IItem> CheckMaterialStash();
        public int CheckMaterialStashMaxSize();
        public void DropFromStash(IItem _data);
        public IReadOnlyList<IItemData> TryCraft(IEquipmentData _data);
        public void EffectEquipmentByType(EEquipmentType _type, DEffectExcuteData _data);
        public IReadOnlyList<IEquipmentData> CheckCraftableEquipmentByType(EEquipmentType _type);
        public bool TryTakeItem(IItem _item);
    }

    public interface IPlayerUI
    {
        public void StatsChangeNotice();
        public void EquipmentChangeNotice(EEquipmentType _type, IEquipment _equip);
        public void EquipmentStashChangeNotice(IReadOnlyList<IEquipment> _stash);
        public void MaterialStashChangeNotice(IReadOnlyList<IItem> _stash);
        public void StashFullNotice(IItem _itemToFull);
    }
}