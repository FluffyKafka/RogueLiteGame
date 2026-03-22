using EnemySystem;
using EntitySystem;
using Item;
using ObjectGenerateData;
using StatsData;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Security.Cryptography;
using UIData;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.Assertions;
using UnityEngine.Rendering;

namespace PlayerSystem
{
    public interface IInitPlayer
    {
        public void Init(
            IPlayerInput _inputSource, 
            IPlayerInventory _inventory, 
            IPlayerUI _ui, 
            IPlayerObjectFactory _factory,
            IPlayerSkillManager _skillManager,
            IPlayerAudio _audio,
            IPlayerAudioManager _audioManager,
            IPlayerGameManager _gameManager,
            IPlayerSaveManager _saveManager
            );
    }

    public interface IInputPlayer
    {
        public void HorizonInput(float _input);
        public void VerticalInput(float _input);
        public void JumpInput();
        public void AttackInput();
        public void SkillInputEnd(int _input);
        public void SkillInputBegin(int _input);
        public void InteractToNPCInput();
        public void UIPageSwitchInput(EUIPageType _type);
        public void EndNPCEffectInput();
        public void ObjectInteractInput();
    }

    public interface IEnemyPlayer
    {
        public bool IsDead();
        public Vector3 CheckPosition();
        public Vector3 CheckVelocity();
        public float CheckGravityScale();

        public WReadOnlyDamageData TakeDamage(WReadOnlyDamageData _damageData);
        public Transform CheckTransform();
        public void SetPlayerToBattle(bool _isBattle);
        public void GenerateDropItemAt(ScriptableObject _itemData, Vector3 _postion);

        public void AddSoul(float _soul);
    }

    public interface IAnimPlayer : IAnimEntity
    {
        public void CounterAttackFinish();
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

        public Transform CheckTransform();
    }

    public struct DSkillEntityUIData
    {
        public string id;
        public string name;
        public Sprite icon;
        public string description;
        public float price;
        public List<string> dependIds;
        public List<string> conflictIds;

        public DSkillEntityUIData(
            string id,
            string name,
            Sprite icon,
            string description,
            float price,
            List<string> dependIds,
            List<string> conflictIds)
        {
            this.id = id;
            this.name = name;
            this.icon = icon;
            this.description = description;
            this.price = price;
            this.dependIds = dependIds ?? new List<string>();
            this.conflictIds = conflictIds ?? new List<string>();
        }
    }
    public struct DSkillUnlockDataToUi
    {
        public string skillId;
        public bool isUnlock;
        public DSkillUnlockDataToUi(string _id, bool _isUnlock)
        {
            skillId = _id;
            isUnlock = _isUnlock;
        }
    }
    public struct DSkillForSaleToUi
    {
        public IUISkill skill;
        public float price;

        public DSkillForSaleToUi(IUISkill _skill, float _price)
        {
            skill = _skill;
            price = _price;
        }
    }
    public struct DItemForSaleToUi
    {
        public IItemData item;
        public float price;
        public DItemForSaleToUi(IItemData _item, float _price)
        {
            item = _item;
            price = _price;
        }
    }
    public enum EAudioType
    {
        SFX,
        BGM,
        ENV
    }
    public interface IUISkill
    {
        public Sprite CheckIcon();
        public bool IsSkillHaveCooldown();
        public float CheckCooldownPercent();
        public int CheckInputIndex();
        public bool TryUnlock();
        public float CheckPrice();
        public string CheckName();
        public string CheckDescription();
    }
    public interface IUIEnemy
    {
        public float CheckHealthPercent();
        public string CheckName();
    }
    public interface IUIDialogEntity
    {
        public string CheckName();
        public Sprite CheckIcon();
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
        public List<DSkillEntityUIData> CheckAllSkillEntity();
        public List<DSkillUnlockDataToUi> CheckAllSkillUnlockState();
        public void UpdateAudioVolumeByType(EAudioType _type, float volume);
        public void PauseGame(bool _isPause);
        public List<IUISkill> CheckSkillsHaveCooldownToUi();
        public KeyCode CheckSkillInputSlotKey(int _index);

        public void CommunicateFinish();

        public bool CheckCanCraft_Blacksmith();

        public bool CheckIsPause();
        public void ConsumeSoul(float _soul);

        public void NPCEffectFinish();

        public bool CanPurchase_coin(float _coin);
        public void ConsumeCoin(float _coin);
        public void AddItemRaw(IItemData _item);
        public void NPCEffectFail();
        public Transform CheckTransform();
    }

    public interface IStatsPlayer : IStatEntity
    {
        public void StatsChangeNotice();
        public void CurrentHealthChange(float _cur);
        public void CoinChange(float _cur);
        public void SoulChange(float _cur);
    }

    public interface IObjectPlayer : IObjectEntity
    {
        public bool TryTakeItem(IItem _item);
        public void TakeCoin(float _coin);
        public void GenerateDropItemByDataAt(IItemData _data, Vector3 _position);
        public void GenerateCoinAt(float _coin, Vector3 _position);
        public void SaveGame();
    }

    public interface IBehaviourPlayer : IBehaviourEntity
    {
        public void ToJump();
        public void ToWallJump();
        public void ToAttack(int _count);
        public void UpdateYVelocity(float _yVelocity);
        public void ToIdle();
        public void ToMove();
        public void ToExitMove();
        public void ToWallSlide();
        public float CheckHorizonInput();
        public float CheckVerticalInput();

        public bool IsEnemy(GameObject _enemy);
        public bool IsEnemyAlive(GameObject _enemy);
        public WReadOnlyDamageData DamageTo(GameObject _enemy, WReadOnlyDamageData _damage);
        public bool StunCheck(GameObject _enemy);

        public void ToCounterAttackSuccess();
        public void ToCounterAttack();

        public void InteractToNPC(IPlayerNPC _npc);
        public KeyCode CheckNPCInteractInputKey();

        public void GeneratePopUpText(string _text);
        public KeyCode CheckObjectInteractInputKey();
        public void InteractToObject(IPlayerInteractable _object);
    }

    public struct DProjectileAimmingData
    {
        public Vector3 dir;
        public Vector3 launchSpeed;
        public float gravity;
        public DProjectileAimmingData(Vector3 _dir, Vector3 _launchSpeed, float _gravity)
        {
            dir = _dir;
            launchSpeed = _launchSpeed;
            gravity = _gravity;
        }
    }
    public interface ISkillManagerPlayer
    {
        public void DashBegin(float _speed);
        public void DashEnd();

        public bool CanEffectBehaviourSkill();

        public Vector3 CheckMousePosition();
        public Transform CheckPlayerTransform();
        public int CheckPlayerFacingDir();

        public void AimmingBegin();
        public void AimmingUpdate(DProjectileAimmingData _data);
        public void AimmingFinish();
        public GameObject ThrowSword(DProjectileData _data);
        public GameObject ThrowSpinSword(DSpinSwordData _data);
        public GameObject ThrowPierceSword(DProjectileData _data);
        public GameObject ThrowBounceSword(DBounceSwordData _data);
        public void CatchSwordBegin();
        public void CatchSwordFinish();
        public void SwordHitGround(Transform _sword);
        public void SwordHitEnemy(Transform _sword);

        public WReadOnlyDamageData CheckPlayerDamage();

        public void GeneratePlayerClone(DPlayerCloneData _data, Vector3 _position);

        public void CounterAttackBegin();
        public void CounterAttackEnd();
        public void SelfHealByPercent(float _per);
        public void AddStatsModifier(WReadOnlyStatsData _data);
        public void RemoveStatsModifier(WReadOnlyStatsData _data);
        public void SkillUnlockToUi(IUISkill _skill);

        public void GeneratePopUpText(string _text);
    }

    public interface IAudioPlayer
    {
        public Transform CheckTransform();
        public bool CheckIsPlayerInBattle();
        public void UpdateAduioVolumeByTypeToUi(EAudioType _type, float _volume);
    }

    public interface INPCPlayer
    {
        public void ShowCraftPage();
        public void Communicate(IDialog _dialog);
        public GameObject GetGameObject();
        public void InteractFinish();
        public List<ScriptableObject> CheckCanUnlockSkillList(float _soul);
        public float CheckSoulAmount();
        public void ShowSkillForSaleWindow(List<DSkillForSaleToUi> _skills);
        public List<IItemData> CheckAllItemCanBeSale();
        public void ShowItemForSaleToUi(List<DItemForSaleToUi> _items);
    }

    public interface IMapPlayer
    {
        public void SetPlayerAtBeginPosition(Vector3 _position);
    }

    internal class APlayer : AEntity, IInitPlayer, IInputPlayer, IAnimPlayer, IEnemyPlayer, IInventoryPlayer, IUIPlayer, IStatsPlayer, IObjectPlayer, IBehaviourPlayer, ISkillManagerPlayer, IAudioPlayer, IUIDialogEntity, INPCPlayer, IMapPlayer
    {
        protected IPlayerInput input;
        protected IPlayerInventory inventory;
        protected IPlayerUI ui;
        protected IPlayerObjectFactory playerObjectFactory;
        protected IPlayerAnimation playerAnim;
        protected IPlayerBehaviour behaviour;
        protected IPlayerSkillManager skillManager;
        protected IPlayerAudio playerAduio;
        protected IPlayerAudioManager audioManager;
        protected IPlayerGameManager gameManager;
        protected IPlayerStats playerStats;
        protected IPlayerSaveManager saveManager;

        #region Behaviour
        void IBehaviourPlayer.ToJump()
        {
            playerAnim.Air();
            playerAduio.Jump(transform);
        }
        void IBehaviourPlayer.ToWallJump()
        {
            playerAnim.Air();
            playerAduio.Jump(transform);
        }
        void IBehaviourPlayer.ToAttack(int _count)
        {
            playerAnim.Attack(_count);
            playerAduio.Attack(_count, transform);
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
            playerAduio.Ground(transform);
        }
        void IBehaviourPlayer.ToExitMove()
        {
            playerAduio.Ground(transform, false);
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

        public bool StunCheck(GameObject _enemy)
        {
            return _enemy.GetComponent<IPlayerEnemy>().StunCheck();
        }

        public void ToCounterAttack()
        {
            playerAnim.CounterAttack();
        }

        public void ToCounterAttackSuccess()
        {
            skillManager.CounterAttackSuccess();
            playerAnim.CounterAttackSuccess();
            playerAduio.CounterAttackSuccess(transform);
        }
        public void CounterAttackFinish()
        {
            behaviour.CounterAttackSuccessFinish();
        }

        public void SelfHealByPercent(float _per)
        {
            stats.SelfHealByPercent(_per);
        }
        public void AddStatsModifier(WReadOnlyStatsData _data)
        {
            stats.AddStatModifier(_data);
        }
        public void RemoveStatsModifier(WReadOnlyStatsData _data)
        {
            stats.RemoveStatModifier(_data);
        }

        public void InteractToNPC(IPlayerNPC _npc)
        {
            _npc.Interact(this);
        }

        public void InteractToNPCInput()
        {
            behaviour.InteractToNPCInput();
        }

        public KeyCode CheckNPCInteractInputKey()
        {
            return input.CheckNPCInteractInputKey();
        }

        public void GeneratePopUpText(string _text)
        {
            playerObjectFactory.GeneratePopUpText(_text, transform.position);
        }

        public KeyCode CheckObjectInteractInputKey()
        {
            return input.CheckObjectInteractInputKey();
        }

        public void InteractToObject(IPlayerInteractable _object)
        {
            _object.Interact(this);
        }
        #endregion

        #region Init
        void IInitPlayer.Init(
            IPlayerInput _inputSource, 
            IPlayerInventory _inventory, 
            IPlayerUI _ui, 
            IPlayerObjectFactory _factory, 
            IPlayerSkillManager _skillManager, 
            IPlayerAudio _audio, 
            IPlayerAudioManager _audioManager, 
            IPlayerGameManager _gameManager, 
            IPlayerSaveManager _saveManager
            )
        {
            input = _inputSource;
            inventory = _inventory;
            ui = _ui;
            playerObjectFactory = _factory;
            objectFactory = _factory;
            skillManager = _skillManager;
            playerAduio = _audio;
            audioManager = _audioManager;
            gameManager = _gameManager;
            playerStats = GetComponentInChildren<IPlayerStats>();
            saveManager = _saveManager;
        }
        #endregion

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
        void IInputPlayer.SkillInputEnd(int _input)
        {
            skillManager.SkillInputEnd(_input);
        }
        void IInputPlayer.SkillInputBegin(int _input)
        {
            skillManager.SkillInputBegin(_input);
        }
        public void UIPageSwitchInput(EUIPageType _type)
        {
            ui.UIPageSwitchTo(_type);
        }

        public void EndNPCEffectInput()
        {
            behaviour.NPCEffectFinish();
        }

        public void ObjectInteractInput()
        {
            behaviour.ObjectInteractInput();
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
            if (damage.data.physical > 0 || damage.data.magical > 0)
            {
                playerAduio.PlayerTakeHit(transform);
                float physcis = damage.data.physical < 0 ? 0 : damage.data.physical;
                float magical = damage.data.magical < 0 ? 0 : damage.data.magical;
                playerObjectFactory.GeneratePopUpText((physcis + magical).ToString(), transform.position);
            }
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

        Transform IEnemyPlayer.CheckTransform()
        {
            return transform;
        }

        public void SetPlayerToBattle(bool _isBattle)
        {
            behaviour.SetPlayerToBattle(_isBattle);
        }

        public void GenerateDropItemAt(ScriptableObject _itemData, Vector3 _position)
        {
            IItem item = inventory.GenerateItemByData(_itemData as IItemData);
            playerObjectFactory.GenerateDropItemObject(item, _position);
        }

        public void AddSoul(float _soul)
        {
            playerStats.AddSoul(_soul);
        }
        #endregion

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

        #region Stats
        void IStatsPlayer.StatsChangeNotice()
        {
            ui.StatsChangeNotice();
        }

        public void CurrentHealthChange(float _cur)
        {
            ui.CurrentHealthChange(_cur);
        }
        public void CoinChange(float _cur)
        {
            ui.CoinChange(_cur);
        }
        public void SoulChange(float _cur)
        {
            ui.SoulChange(_cur);
        }
        #endregion

        #region UI

        public void CommunicateFinish()
        {
            behaviour.CommunicateFinish();
        }

        public string CheckName()
        {
            return entityName;
        }
        public Sprite CheckIcon()
        {
            return entityIcon;
        }

        public KeyCode CheckSkillInputSlotKey(int _index)
        {
            return input.CheckSkillInputSlotKey(_index);
        }

        public List<IUISkill> CheckSkillsHaveCooldownToUi()
        {
            return skillManager.CheckSkillsHaveCooldownToUi();
        }

        public void PauseGame(bool _isPause)
        {
            gameManager.Pause(_isPause);
        }

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
        List<DSkillEntityUIData> IUIPlayer.CheckAllSkillEntity()
        {
            return skillManager.ShowAllSkillEntityToUi();
        }
        public List<DSkillUnlockDataToUi> CheckAllSkillUnlockState()
        {
            return skillManager.CheckAllSkillUnlockState();
        }

        public void UpdateAudioVolumeByType(EAudioType _type, float _volume)
        {
            audioManager.UpdateAudioVolumeByType(_type, _volume);
        }

        public bool CheckCanCraft_Blacksmith()
        {
            return inventory.CheckCanCraft_Blacksmith();
        }

        public bool CheckIsPause()
        {
            return gameManager.CheckIsPause();
        }

        public void ConsumeSoul(float _soul)
        {
            playerStats.ConsumeSoul(_soul);
        }

        public void NPCEffectFinish()
        {
            behaviour.NPCEffectFinish();
        }

        public bool CanPurchase_coin(float _coin)
        {
            return playerStats.CanPurchase_coin(_coin);
        }
        public void ConsumeCoin(float _coin)
        {
            playerStats.ConsumeCoin(_coin);
        }

        public void AddItemRaw(IItemData _item)
        {
            inventory.AddItemRaw(_item);
        }

        public void NPCEffectFail()
        {
            behaviour.NPCEffectFail();
        }
        #endregion

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

        public void ObjectFinish(Transform _object)
        {
            //暂时空置
        }

        public void TakeCoin(float _coin)
        {
            playerStats.AddCoin(_coin);
        }

        public void GenerateDropItemByDataAt(IItemData _data, Vector3 _position)
        {
            IItem item = inventory.GenerateItemByData(_data);
            playerObjectFactory.GenerateDropItemObject(item, _position);
        }
        public void GenerateCoinAt(float _coin, Vector3 _position)
        {
            playerObjectFactory.GenerateCoin(_coin, _position);
        }

        public void SaveGame()
        {
            saveManager.SaveGame();
        }
        #endregion

        #region Skill

        public void SkillUnlockToUi(IUISkill _skill)
        {
            ui.SkillUnlock(_skill);
        }

        public void DashBegin(float _speed)
        {
            behaviour.DashBegin(_speed);
            playerAnim.DashBegin();
            playerAduio.Dash(transform);
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

        public Vector3 CheckMousePosition()
        {
            return input.CheckMousePosition();
        }

        public Transform CheckPlayerTransform()
        {
            return transform;
        }
        public int CheckPlayerFacingDir()
        {
            return behaviour.CheckFacingDir();
        }

        public void CatchSwordBegin()
        {
            behaviour.CatchSwordBegin();
            playerAnim.CatchSwordBegin();
            playerAduio.SwordCatch(transform);
        }
        public void CatchSwordFinish()
        {
            behaviour.CatchSwordFinish();
            playerAnim.CatchSwordFinish();
        }
        public void AimmingBegin()
        {
            behaviour.AimmingBegin();
            playerAnim.AimmingBegin();
        }
        public void AimmingUpdate(DProjectileAimmingData _data)
        {
            behaviour.AimmingUpdate(_data);
            playerAnim.AimmingUpdate(_data);
        }
        public void AimmingFinish()
        {
            behaviour.AimmingFinish();
            playerAnim.AimmingFinish();            
        }

        public GameObject ThrowSword(DProjectileData _data)
        {
            playerAduio.SwordThrow(transform);
            return playerObjectFactory.GenerateSword(_data, transform.position);
        }
        public GameObject ThrowSpinSword(DSpinSwordData _data)
        {
            playerAduio.SwordThrow(transform);
            return playerObjectFactory.GenerateSpinSword(_data, transform.position);
        }
        public GameObject ThrowPierceSword(DProjectileData _data)
        {
            playerAduio.SwordThrow(transform);
            return playerObjectFactory.GeneratePierceSword(_data, transform.position);
        }
        public GameObject ThrowBounceSword(DBounceSwordData _data)
        {
            playerAduio.SwordThrow(transform);
            return playerObjectFactory.GenerateBounceSword(_data, transform.position);
        }
        public void SwordHitGround(Transform _sword)
        {
            playerAduio.SwordGround(_sword);
        }
        public void SwordHitEnemy(Transform _sword)
        {
            playerAduio.SwordHit(_sword);
        }

        public WReadOnlyDamageData CheckPlayerDamage()
        {
            return stats.GetPrimaryAttackData();
        }

        public void GeneratePlayerClone(DPlayerCloneData _data, Vector3 _position)
        {
            playerObjectFactory.GeneratePlayerClone(_data, _position);
        }

        public void CounterAttackBegin()
        {
            playerAduio.CounterAttack(transform);
            behaviour.CounterAttackBegin();
        }
        public void CounterAttackEnd()
        {
            behaviour.CounterAttackEnd();
        }
        #endregion

        #region Audio
        public bool CheckIsPlayerInBattle()
        {
            return behaviour.CheckIsPlayerInBattle();
        }
        public void UpdateAduioVolumeByTypeToUi(EAudioType _type, float _volume)
        {
            ui.AudioVolumeUpdate(_type, _volume);
        }
        #endregion

        #region NPC
        public GameObject GetGameObject()
        {
            return gameObject;
        }
        public void ShowCraftPage()
        {
            ui.ShowCraftPage();
        }
        public void Communicate(IDialog _dialog)
        {
            ui.ShowCommunicateWindow(_dialog);
        }
        public void InteractFinish()
        {
            behaviour.InteractFinish();
        }
        public float CheckSoulAmount()
        {
            return playerStats.CheckSoulAmount();
        }

        public List<ScriptableObject> CheckCanUnlockSkillList(float _soul)
        {
            return skillManager.CheckCanUnlockSkillList(_soul);
        }

        public void ShowSkillForSaleWindow(List<DSkillForSaleToUi> _skills)
        {
            ui.ShowSkillForSaleWindow(_skills);
        }

        public List<IItemData> CheckAllItemCanBeSale()
        {
            return inventory.CheckAllItemsCanBeSale();
        }

        public void ShowItemForSaleToUi(List<DItemForSaleToUi> _items)
        {
            ui.ShowItemForSaleWindow(_items);
        }
        #endregion

        #region Map
        public void SetPlayerAtBeginPosition(Vector3 _position)
        {
            transform.position = _position;
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
        public void SkillInputEnd(int _input);
        public void SkillInputBegin(int _input);

        public void CounterAttackSuccess();

        public List<DSkillEntityUIData> ShowAllSkillEntityToUi();
        public List<DSkillUnlockDataToUi> CheckAllSkillUnlockState();
        public List<IUISkill> CheckSkillsHaveCooldownToUi();

        public List<ScriptableObject> CheckCanUnlockSkillList(float _soul);
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

        public void AimmingBegin();
        public void AimmingUpdate(DProjectileAimmingData _data);
        public void AimmingFinish();
        public void CatchSwordBegin();
        public void CatchSwordFinish();

        public void CounterAttackBegin();
        public void CounterAttackEnd();
        public void CounterAttackSuccessFinish();

        public void SetPlayerToBattle(bool _isBattle);
        public bool CheckIsPlayerInBattle();

        public void InteractToNPCInput();
        public void CommunicateFinish();
        public void InteractFinish();
        public void NPCEffectFinish();
        public void NPCEffectFail();

        public void ObjectInteractInput();
    }

    public interface IPlayerEnterable : IEntityObject
    {
        public void Enter(IObjectPlayer _player);
    }
    public interface IPlayerInteractable : IEntityObject
    {
        public bool CanInteract();
        public void Interact(IObjectPlayer _player);
    }
    public interface IPlayerReflectable : IEntityObject
    {
        public void Reflect(IObjectPlayer _player);
    }
    public interface IPlayerObjectFactory: IEntityObjectFactory
    {
        public void GenerateDropItemObject(IItem _data, Vector3 _position);
        public GameObject GenerateSword(DProjectileData _data, Vector3 _position);
        public GameObject GenerateSpinSword(DSpinSwordData _data, Vector3 _position);
        public GameObject GeneratePierceSword(DProjectileData _data, Vector3 _position);
        public GameObject GenerateBounceSword(DBounceSwordData _data, Vector3 _position);
        public void GeneratePlayerClone(DPlayerCloneData _data, Vector3 _position);
        public void GeneratePopUpText(string _data, Vector3 _position);
        public void GenerateCoin(float _coin, Vector3 _position);
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

        public abstract void AimmingBegin();
        public abstract void AimmingUpdate(DProjectileAimmingData _data);
        public abstract void AimmingFinish();
        public abstract void CatchSwordBegin();
        public abstract void CatchSwordFinish();

        public abstract void CounterAttack();
        public abstract void CounterAttackSuccess();
    }

    public interface IPlayerInput
    {
        public float CheckHorizonInput();
        public float CheckVerticalInput();

        public Vector3 CheckMousePosition();

        public KeyCode CheckSkillInputSlotKey(int _index);
        public KeyCode CheckNPCInteractInputKey();
        public KeyCode CheckObjectInteractInputKey();
    }

    public interface IPlayerEnemy
    {
        public bool IsDead();

        public WReadOnlyDamageData TakeDamage(WReadOnlyDamageData _damageData);

        public bool StunCheck();
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
        public bool CheckCanCraft_Blacksmith();
        public List<IItemData> CheckAllItemsCanBeSale();
        public void AddItemRaw(IItemData _item);
        public IItem GenerateItemByData(IItemData _data);
    }

    public interface IPlayerUI
    {
        public void StatsChangeNotice();
        public void EquipmentChangeNotice(EEquipmentType _type, IEquipment _equip);
        public void EquipmentStashChangeNotice(IReadOnlyList<IEquipment> _stash);
        public void MaterialStashChangeNotice(IReadOnlyList<IItem> _stash);
        public void StashFullNotice(IItem _itemToFull);

        public void AudioVolumeUpdate(EAudioType _type, float _volume);

        public void CurrentHealthChange(float _cur);
        public void CoinChange(float _cur);
        public void SoulChange(float _cur);

        public void SkillUnlock(IUISkill _skill);
        public void SetCurrentEnemy(IUIEnemy _enemy);

        public void ShowCraftPage();
        public void ShowCommunicateWindow(IDialog _dialog);
        public void UIPageSwitchTo(EUIPageType _type);

        public void ShowSkillForSaleWindow(List<DSkillForSaleToUi> _skill);
        public void ShowItemForSaleWindow(List<DItemForSaleToUi> _item);
    }

    public interface IPlayerAudio
    {
        public void Attack(int _count, Transform _sourceTransform, bool _play = true);
        public void Ground(Transform _sourceTransform, bool _play = true);
        public void Jump(Transform _sourceTransform, bool _play = true);
        public void Dash(Transform _sourceTransform, bool _play = true);
        public void SwordThrow(Transform _sourceTransform, bool _play = true);
        public void SwordGround(Transform _sourceTransform, bool _play = true);
        public void SwordCatch(Transform _sourceTransform, bool _play = true);
        public void CounterAttack(Transform _sourceTransform, bool _play = true);
        public void CounterAttackSuccess(Transform _sourceTransform, bool _play = true);
        public void BlackHoleLoop(Transform _sourceTransform, bool _play = true);
        public void CrystalPlace(Transform _sourceTransform, bool _play = true);
        public void CrystalFlashBack(Transform _sourceTransform, bool _play = true);
        public void CrystalExplode(Transform _sourceTransform, bool _play = true);
        public void EvasionSuccess(Transform _sourceTransform, bool _play = true);
        public void PlayerTakeHit(Transform _sourceTransform, bool _play = true);
        public void SwordHit(Transform _sourceTransform, bool _play = true);
    }

    public interface IPlayerAudioManager
    {
        public void UpdateAudioVolumeByType(EAudioType _type, float _volume);
    }

    public interface IPlayerGameManager
    {
        public void Pause(bool _isPause);
        public void PauseRaw(bool _isPause);
        public bool CheckIsPause();
    }

    public interface IPlayerStats: IEntityStats
    {
        public float CheckSoulAmount();
        public void ConsumeSoul(float _soul);
        public bool CanPurchase_coin(float _coin);
        public void ConsumeCoin(float _coin);
        public void AddSoul(float _soul);
        public void AddCoin(float _coin);
    }

    public enum ENPCType
    {
        BlackSmith,
        Trader,
        Witch
    }
    public interface IPlayerNPC
    {
        public ENPCType CheckType();
        public void Interact(INPCPlayer _player);
        public void CommunicateFinish();
        public void EffectFinish();
        public void EffectFail();
    }

    public interface IDialog
    {
        [Serializable]
        public class DSentence
        {
            public int dialogEntityIndex;
            public string text;
        }

        public GameObject CheckEntityByIndex(int _index);
        public List<DSentence> CheckDialog();
        public void SetDialogIndex(int _index, GameObject _entity);
    }

    public interface IPlayerSaveManager
    {
        public void SaveGame();
    }
}
