using EntitySystem.EntityActor;
using EntitySystem.EntityActor.PlayerActor;
using EntitySystem.EntityComponent.BattleComponent;
using EntitySystem.EntityComponent.MovementComponent;
using EntitySystem.EntityComponent.StateMachineComponent;
using Item;
using StatsData;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Assertions;
using static UnityEditor.Progress;

namespace EntitySystem
{
    namespace EntityActor
    {
        namespace PlayerActor
        {
            public interface IInitPlayer
            {
                public void Init(IPlayerInput _inputSource, IPlayerInventory _inventory, IPlayerUI _ui);
            }

            public interface IInputPlayer
            {
                public void HorizonInput(float _input);
                public void VerticalInput(float _input);
                public void JumpInput();
                public void AttackInput();
            }

            public interface IEnemyPlayer
            {
                public bool IsDead();
                public Vector3 CheckPosition();

                public WReadOnlyDamageData TakeDamage(WReadOnlyDamageData _damageData);
            }

            public interface IAnimPlayer: IAnimEntity
            {

            }

            public interface IInventoryPlayer
            {
                public void AddModifier(WReadOnlyStatsData _data);
                public void RemoveModifier(WReadOnlyStatsData _data);
                public void CraftFailNotice_LackMaterial(IReadOnlyList<IItem> _lack);
                public void StashFullNotice(IItem _itemToFull);
            }

            public interface IUIPlayer
            {
                public bool TryCraft(IEquipmentData _data);
                public void Equip(IEquipment _equip);
                public void UnEquip(IEquipment _Equip);
                public void DropItem(IItem _item);
                public IEquipment CheckEquipmentByType(EEquipmentType _type);
                public IReadOnlyList<IEquipment> CheckEquipmentStash();
                public IReadOnlyList<IItem> CheckMaterialStash();
                public int CheckEquipmentStashMaxSize();
                public int CheckMaterialStashMaxSize();
            }

            internal class APlayer : AEntity, IInitPlayer, IInputPlayer, IAnimPlayer, IEnemyPlayer, IInventoryPlayer, IUIPlayer
            {
                protected IPlayerInput input;
                protected IPlayerInventory inventory;
                protected IPlayerUI ui;

                #region Internal Action
                public Action<float> HorizonInput;
                public Action<float> VerticalInput;
                public Action JumpInput;
                public Action AttackInput;

                public Action Jump;
                public Action<float> Move;
                public Action<float> WallSlide;
                public Action WallJump;
                public Action<float> UpdateYVelocity;

                public Action AttackRaw;
                public Action<int> Attack;

                public Action ToIdle;
                public Action ToMove;
                public Action ToWallSlide;
                #endregion

                #region Internal Func
                public Func<bool> IsGroundedOrPlatform_Strict;//确保只有当Player接触地面时才返回true
                public Func<float> CheckHorizonInput;
                public Func<float> CheckVerticalInput;
                public Func<float> CheckUnmovableDurationAfterAttack;
                #endregion

                #region Init
                void IInitPlayer.Init(IPlayerInput _inputSource, IPlayerInventory _inventory, IPlayerUI _ui)
                {
                    input = _inputSource;
                    inventory = _inventory;
                    ui = _ui;
                }
                #endregion

                #region Input
                void IInputPlayer.HorizonInput(float _input)
                {
                    InvokeAction(HorizonInput, _input);
                }
                void IInputPlayer.VerticalInput(float _input)
                {
                    InvokeAction(VerticalInput, _input);
                }
                void IInputPlayer.JumpInput()
                {                    
                    InvokeAction(JumpInput);
                }
                void IInputPlayer.AttackInput()
                {
                    InvokeAction(AttackInput);
                }
                #endregion

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
                #endregion

                #region Inventory

                public Action<IReadOnlyList<IItem>> CraftFailNotice_LackMaterial;
                public Action<IItem> StashFullNotice;
                void IInventoryPlayer.AddModifier(WReadOnlyStatsData _data)
                {
                    InvokeAction(AddModifier, _data);                 
                }
                void IInventoryPlayer.RemoveModifier(WReadOnlyStatsData _data)
                {
                    InvokeAction(RemoveModifier, _data);
                }
                void IInventoryPlayer.CraftFailNotice_LackMaterial(IReadOnlyList<IItem> _lack)
                {
                    InvokeAction(CraftFailNotice_LackMaterial, _lack);
                }
                void IInventoryPlayer.StashFullNotice(IItem _itemToFull)
                {
                    InvokeAction(StashFullNotice, _itemToFull);
                }
                #endregion

                //暂时直接转发，若有需要再引入事件机制
                #region UI
                bool IUIPlayer.TryCraft(IEquipmentData _data)
                {
                    return inventory.TryCraft(_data);
                }

                void IUIPlayer.Equip(IEquipment _equip)
                {
                    inventory.Equip(_equip);
                }

                void IUIPlayer.UnEquip(IEquipment _equip)
                {
                    inventory.Equip(_equip);
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
                #endregion

                protected override void Awake()
                {
                    base.Awake();

                    Assert.IsTrue(anim is IPlayerAnimation, "Player需要一个IPlayerAnimation的动画组件");
                    IPlayerAnimation playerAnim = anim as IPlayerAnimation;
                    Attack += playerAnim.Attack;
                    WallJump += playerAnim.Air;
                    Jump += playerAnim.Air;
                    UpdateYVelocity += playerAnim.UpdateYVelocity;
                    ToIdle += playerAnim.Idle;
                    ToMove += playerAnim.Move;
                    ToWallSlide += playerAnim.WallSlide;


                    CheckHorizonInput += input.CheckHorizonInput;
                    CheckVerticalInput += input.CheckVerticalInput;

                    //UI
                    CraftFailNotice_LackMaterial += ui.CraftFailNotice_LackMaterial;
                    StashFullNotice += ui.StashFullNotice;
                }
                override protected void ComponentValidCheck()
                {
                    Assert.IsNotNull(GetComponent<CPlayerMovement>(), "缺少玩家运动组件");
                    Assert.IsNotNull(GetComponent<CPlayerBattle>(), "缺少玩家战斗组件");
                    Assert.IsNotNull(GetComponent<CPlayerStateMachine>(), "缺少玩家状态机组件");
                }
            }

            public interface IPlayerAnimation : IEntityAnimation
            {
                public abstract void Idle();
                public abstract void Move();
                public abstract void Attack(int _count);
                public abstract void Air();
                public abstract void UpdateYVelocity(float _yVelocity);
                public abstract void WallSlide();
            }

            public interface IPlayerInput
            {
                public float CheckHorizonInput();
                public float CheckVerticalInput();
            }

            public interface IPlayerEnemy
            {
                public bool IsDead();
                public Vector3 CheckPosition();

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
                public bool TryCraft(IEquipmentData _data);
                public void EffectEquipmentByType(EEquipmentType _type, DEffectExcuteData _data);
            }

            public interface IPlayerUI
            {
                public void StatsChangeNotice(DStatsData _data);
                public void EquipmentChangeNotice(EEquipmentType _type, IEquipment _equip);
                public void EquipmentStashChangeNotice(IReadOnlyList<IEquipment> _stash);
                public void MaterialStashChangeNotice(IReadOnlyList<IItem> _stash);
                public void CraftFailNotice_LackMaterial(IReadOnlyList<IItem> _lack);
                public void StashFullNotice(IItem _itemToFull);
            }
        }        
    }
}