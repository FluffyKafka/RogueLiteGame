using Item;
using PlayerSystem;
using StatsData;
using System;
using System.Collections;
using System.Collections.Generic;
using UIData;
using UnityEngine;
using UnityEngine.Windows;

namespace UISystem
{
    public interface IInitUI
    {
        public void Init(IUIPlayer _player, IUIAudio _audio);
    }

    internal class MUIManager : ComponentManagerBase, IPlayerUI, IInitUI
    {
        protected IUIPlayer player;
        protected IUIAudio audioSystem;

        #region Action
        public Action<IEquipment> Equip;
        public Action<IEquipment> UnEquip;
        public Action<IItem> DropItem;
        public Action<IReadOnlyList<IEquipment>> EquipmentStashChange;
        public Action<IReadOnlyList<IItem>> MaterialStashChange;
        public Action UpdateStats;
        public Action<EEquipmentType, IEquipment> EquipmentChange;
        public Action<EUIPageType> ChangePageTo;
        public Action<IEquipmentData> ShowEquipmentDetail;
        public Action<IItemData> ShowMaterialDetail;
        public Action<EStatType> ShowStatsDetail;
        public Action<IEquipmentData> ShowCraftWindow;
        public Action<string> ShowWarning;
        public Action HideTooltip;
        public Action<DSkillDetail> ShowSkillDetailNotice;
        public Action<EAudioType, float> AudioVolumeUpdateNotice;
        public Action<float> CurrentHealthChangeNotice;
        public Action<float> CoinChangeNotice;
        public Action<float> SoulChangeNotice;
        public Action<IUISkill> SkillUnlockNotice;
        public Action<IUIEnemy> SetCurrentEnemyNotice;
        public Action<IDialog> ShowCommunicateWindowNotice;
        public Action<List<DSkillForSaleToUi>> ShowSkillForSaleWindowNotice;
        public Action<List<DItemForSaleToUi>> ShowItemForSaleWindowNotice;
        #endregion

        #region Func
        public Func<int> CheckMaterialStashMaxSize;
        public Func<int> CheckEquipmentStashMaxSize;
        public Func<string, string> Translate;
        public Func<string, string> CheckKeyWordStatDescription;
        public Func<IEquipmentData, IReadOnlyList<IItemData>> TryCraft;
        public Func<EEquipmentType, IReadOnlyList<IEquipmentData>> CheckCraftableEquipmentByType;
        public Func<EStatType, float> TryCheckStat;
        public Func<IReadOnlyList<IItem>> CheckMaterialStash;
        public Func<IReadOnlyList<IEquipment>> CheckEquipmentStash;
        public Func<EEquipmentType, IEquipment> CheckEquipmentByType;
        #endregion

        #region Pages
        [Serializable]
        protected class DPage
        {
            public EUIPageType type;
            public GameObject gameObject;
        }
        [SerializeField] protected List<DPage> pages;
        [SerializeField] protected EUIPageType initPage;
        protected EUIPageType currentPageType;
        #endregion


        protected void Awake()
        {
            CheckEquipmentStashMaxSize += player.CheckEquipmentStashMaxSize;
            CheckMaterialStashMaxSize += player.CheckMaterialStashMaxSize;
            ChangePageTo += ChangePageToByType;
            Equip += player.Equip;
            UnEquip += player.UnEquip;
            DropItem += player.DropItem;
            TryCraft += player.TryCraft;
            CheckCraftableEquipmentByType += player.CheckCraftableEquipmentByType;
            TryCheckStat += player.TryCheckStat;
            CheckEquipmentByType += player.CheckEquipmentByType;
            CheckEquipmentStash += player.CheckEquipmentStash;
            CheckMaterialStash += player.CheckMaterialStash;
        }

        protected void Start()
        {
            ChangePageToByType(initPage);
        }

        protected void Update()
        {
            if(currentPageType != EUIPageType.InGame && !CheckIsPause())
            {
                PauseGame(true);
            }


        }

        #region Init
        public void Init(IUIPlayer _player, IUIAudio _audio)
        {
            player = _player;
            audioSystem = _audio;
        }
        #endregion

        #region Self
        protected void ChangePageToByType(EUIPageType _type)
        {
            if(currentPageType == _type)
            {
                return;
            }

            currentPageType = _type;
            foreach(var page in pages)
            {
                if(page.type != _type)
                {
                    page.gameObject.SetActive(false);
                }
                else
                {
                    page.gameObject.SetActive(true);
                }
            }

            if(currentPageType == EUIPageType.InGame)
            {
                PauseGame(false);
            }
        }
        #endregion

        public void EquipmentChangeNotice(EEquipmentType _type, IEquipment _equip)
        {
            InvokeAction(EquipmentChange, _type, _equip);
        }

        public void EquipmentStashChangeNotice(IReadOnlyList<IEquipment> _stash)
        {
            InvokeAction(EquipmentStashChange, _stash);
        }

        public void MaterialStashChangeNotice(IReadOnlyList<IItem> _stash)
        {
            InvokeAction(MaterialStashChange, _stash);
        }

        public void StashFullNotice(IItem _itemToFull)
        {
            throw new System.NotImplementedException();//////////////////////////////////////////////////////////////////////////////////
        }

        public void StatsChangeNotice()
        {
            InvokeAction(UpdateStats);
        }

        public List<DSkillEntityUIData> CheckAllSkillEntity()
        {
            return player.CheckAllSkillEntity();
        }
        public List<DSkillUnlockDataToUi> CheckAllSkillUnlockState()
        {
            return player.CheckAllSkillUnlockState();
        }

        public void UpdateAudioVolumeByType(EAudioType _type, float _volume)
        {
            player.UpdateAudioVolumeByType(_type, _volume);
        }

        public void AudioVolumeUpdate(EAudioType _type, float _volume)
        {
            InvokeAction(AudioVolumeUpdateNotice, _type, _volume);
        }

        public void CurrentHealthChange(float _cur)
        {
            InvokeAction(CurrentHealthChangeNotice, _cur);
        }
        public void CoinChange(float _cur)
        {
            InvokeAction(CoinChangeNotice, _cur);
        }
        public void SoulChange(float _cur)
        {
            InvokeAction(SoulChangeNotice, _cur);
        }

        public List<IUISkill> CheckSkillsUnlockedHaveCooldown()
        {
            return player.CheckSkillsHaveCooldownToUi();
        }
        public void SkillUnlock(IUISkill _skill)
        {
            InvokeAction(SkillUnlockNotice, _skill);
        }
        
        public KeyCode CheckSkillInputSlotKey(int _index)
        {
            return player.CheckSkillInputSlotKey(_index);
        }

        public void SetCurrentEnemy(IUIEnemy _enemy)
        {
            InvokeAction(SetCurrentEnemyNotice, _enemy);
        }

        public void ShowCraftPage()
        {
            ChangePageToByType(EUIPageType.Craft);
        }

        public void PauseGame(bool _isPause)
        {
            player.PauseGame(_isPause);
        }

        public void ShowCommunicateWindow(IDialog _dialog)
        {
            InvokeAction(ShowCommunicateWindowNotice, _dialog);
        }
        public void CommunicateFinish()
        {
            player.CommunicateFinish();
        }

        public bool CheckCanCraft_BlackSmith()
        {
            return player.CheckCanCraft_Blacksmith();
        }

        public bool CheckIsPause()
        {
            return player.CheckIsPause();
        }

        public void UIPageSwitchTo(EUIPageType _type)
        {
            ChangePageToByType(_type);
        }

        public void ConsumeSoul(float _soul)
        {
            player.ConsumeSoul(_soul);
        }

        public void NPCEffectFinish()
        {
            player.NPCEffectFinish();
        }

        public void ShowSkillForSaleWindow(List<DSkillForSaleToUi> _skills)
        {
            InvokeAction(ShowSkillForSaleWindowNotice, _skills);
        }

        public bool CanPurchase_coin(float _coin)
        {
            return player.CanPurchase_coin(_coin);
        }
        public void ConsumeCoin(float _coin)
        {
            player.ConsumeCoin(_coin);
        }

        public void AddItemRaw(IItemData _item)
        {
            player.AddItemRaw(_item);
        }

        public void ShowItemForSaleWindow(List<DItemForSaleToUi> _itemList)
        {
            InvokeAction(ShowItemForSaleWindowNotice, _itemList);
        }

        public void NPCEffectFail()
        {
            player.NPCEffectFail();
        }

        #region Audio
        public void PlayButtonClickSFX(bool _isPlay)
        {
            audioSystem.ButtonClick(player.GetTransform(), _isPlay);
        }

        public void PlayCraftSFX(bool _isPlay)
        {
            audioSystem.Craft(player.GetTransform(), _isPlay);
        }

        public void PlayEquipSFX(bool _isPlay)
        {
            audioSystem.Equip(player.GetTransform(), _isPlay);
        }

        public void PlayBuySFX(bool _isPlay)
        {
            audioSystem.Buy(player.GetTransform(), _isPlay);
        }

        public void PlayUpgradeSFX(bool _isPlay)
        {
            audioSystem.Upgrade(player.GetTransform(), _isPlay);
        }

        public void PlayDiscardInventorySFX(bool _isPlay)
        {
            audioSystem.DiscardInventory(player.GetTransform(), _isPlay);
        }

        public void PlayCommunicatingSFX(bool _isPlay)
        {
            audioSystem.Communicating(player.GetTransform(), _isPlay);
        }
        #endregion

        public bool IsMapDragBeginInput()
        {
            return player.IsMapDragBeginInput();
        }
        public bool IsMapDragInput()
        {
            return player.IsMapDragInput();
        }
        public float CheckZoomInput()
        {
            return player.CheckZoomInput();
        }
        public Vector3 CheckMousePosition(bool _isRaw = false)
        {
            return player.CheckMousePosition(_isRaw);
        }

        public Transform CheckPlayerTransform()
        {
            return player.GetTransform();
        }
        
        public float CheckSceneLoadRate()
        {
            return player.CheckSceneLoadRate();
        }
    }
}

public interface IUIAudio
{
    public void ButtonClick(Transform _sourceTransform, bool _play = true);
    public void Craft(Transform _sourceTransform, bool _play = true);
    public void Equip(Transform _sourceTransform, bool _play = true);
    public void Buy(Transform _sourceTransform, bool _play = true);
    public void Upgrade(Transform _sourceTransform, bool _play = true);
    public void DiscardInventory(Transform _sourceTransform, bool _play = true);
    public void Communicating(Transform _sourceTransform, bool _play = true);
}