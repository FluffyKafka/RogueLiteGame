using EntitySystem.EntityActor.PlayerActor;
using Item;
using StatsData;
using System;
using System.Collections;
using System.Collections.Generic;
using UIData;
using UnityEngine;

namespace UISystem
{
    public interface IInitUI
    {
        public void Init(IUIPlayer _player);
    }

    internal class MUIManager : ComponentManagerBase, IPlayerUI, IInitUI
    {
        protected IUIPlayer player;

        #region Action
        public Action<IReadOnlyList<IEquipment>> EquipmentStashChange;
        public Action<IReadOnlyList<IItem>> MaterialStashChange;
        public Action<WReadOnlyStatsData> StatsUpdate;
        public Action<EEquipmentType, IEquipment> EquipmentChange;
        public Action<EUIPageType> ChangePageTo;//////////////////////////////
        public Action<IEquipmentData> ShowEquipmentDetail;///////////////////////每个页面需要一个ToolTip管理器以在适当时机唤醒ToolTip
        #endregion

        #region Func
        public Func<int> CheckMaterialStashMaxSize;
        public Func<int> CheckEquipmentStashMaxSize;
        public Func<string, string> Translate;////////改为翻译Enum而不是直接翻译string
        public Func<EStatType, string> TranslateStatType;//拆成这个
        public Func<EEquipmentType, string> TranslateEquipmentType;//和这个
        public Func<EStatType, string> CheckStatDescription;///////////////////////// 
        public Func<EEquipmentType, string> CheckEquipmentTypeDescription;///////////////////////// 
        #endregion

        protected virtual void OnEnable()
        {
            CheckEquipmentStashMaxSize += player.CheckEquipmentStashMaxSize;
            CheckMaterialStashMaxSize += player.CheckMaterialStashMaxSize;
        }

        protected virtual void OnDisable()
        {
            CheckEquipmentStashMaxSize -= player.CheckEquipmentStashMaxSize;
            CheckMaterialStashMaxSize -= player.CheckMaterialStashMaxSize;
        }

        #region Init
        public void Init(IUIPlayer _player)
        {
            player = _player;
        }
        #endregion


        public void CraftFailNotice_LackMaterial(IReadOnlyList<IItem> _lack)
        {
            throw new System.NotImplementedException();
        }

        public void EquipmentChangeNotice(EEquipmentType _type, IEquipment _equip)
        {
            throw new System.NotImplementedException();
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
            throw new System.NotImplementedException();
        }

        public void StatsChangeNotice(WReadOnlyStatsData _data)
        {
            InvokeAction(StatsUpdate, _data);
        }
    }
}

