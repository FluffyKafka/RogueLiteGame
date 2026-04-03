using InputManager;
using NPCSystem;
using PlayerSystem;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using static GameManagerSystem.ISaveGameManager;

namespace GameManagerSystem
{   
    public interface IInitGameManager
    {
        public void Init(IGameManagerInput _input, IGameManagerNPCFactory _npcFactory);
    }

    public interface ISaveGameManager
    {
        public class DGameManagerSaveData
        {
            public string sceneName = string.Empty;
        }
        public void Save(ref DGameManagerSaveData _data);
        public void Load(DGameManagerSaveData _data);
    }

    internal class MGameManager : ComponentManagerBase, IPlayerGameManager, IInitGameManager, ISaveGameManager
    {
        #region ActionAndFunc
        public Action<bool> PauseNotice;
        public Action<bool> PauseRawNotice;
        public Func<bool> CheckIsPauseNotice;
        public Func<float> CheckPauseAnimSlowRateNotice;

        public Action<string> SwitchSceneToNotice;
        public Func<string> CheckCurrentSceneNameNotice;
        #endregion

        public IGameManagerInput input;
        public IGameManagerNPCFactory npcFactory;

        public void Init(IGameManagerInput _input, IGameManagerNPCFactory _npcFactory)
        {
            input = _input;
            npcFactory = _npcFactory;
        }

        public void Pause(bool _isPause)
        {
            InvokeAction(PauseNotice, _isPause);
        }
        public void PauseRaw(bool _isPause)
        {
            InvokeAction(PauseRawNotice, _isPause);
        }       
        public bool CheckIsPause()
        {
            return InvokeFunc(CheckIsPauseNotice);
        }
        public float CheckPauseAnimSlowRate()
        {
            return InvokeFunc(CheckPauseAnimSlowRateNotice);
        }

        public void SwitchSceneTo(string _sceneName)
        {
            InvokeAction(SwitchSceneToNotice, _sceneName);
        }

        public void Save(ref DGameManagerSaveData _data)
        {
            _data.sceneName = InvokeFunc(CheckCurrentSceneNameNotice);
        }
        public void Load(DGameManagerSaveData _data)
        {
            if(_data.sceneName != string.Empty && _data.sceneName != InvokeFunc(CheckCurrentSceneNameNotice))
            {
                SwitchSceneTo(_data.sceneName);
            }
        }
    }

    internal class CGameManagerComponentBase: MonoBehaviour
    {
        protected MGameManager game;

        protected virtual void Awake()
        {
            game = GetComponent<MGameManager>();
        }
    }
}

