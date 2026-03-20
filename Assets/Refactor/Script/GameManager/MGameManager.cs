using InputManager;
using NPCSystem;
using PlayerSystem;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace GameManagerSystem
{   
    public interface IInitGameManager
    {
        public void Init(IGameManagerInput _input, IGameManagerNPCFactory _npcFactory);
    }

    internal class MGameManager : MonoBehaviour, IPlayerGameManager, IInitGameManager
    {
        [SerializeField] protected float pauseTimeSlowRate;
        protected bool isPause = false;

        protected IGameManagerInput input;
        protected IGameManagerNPCFactory npcFactory;

        public void Init(IGameManagerInput _input, IGameManagerNPCFactory _npcFactory)
        {
            input = _input;
            npcFactory = _npcFactory;
        }

        public void Pause(bool _isPause)
        {
            isPause = _isPause;
            if(_isPause)
            {
                Time.timeScale *= pauseTimeSlowRate;
            }
            else
            {
                Time.timeScale = 1;
            }
            PauseGameNotice(_isPause);
        }

        public void PauseRaw(bool _isPause)
        {
            isPause = _isPause;
            if (_isPause)
            {
                Time.timeScale = 0;               
            }
            else
            {
                Time.timeScale = 1;
            }
            PauseGameNotice(_isPause);
        }

        protected void PauseGameNotice(bool _isPause)
        {
            input.GamePause(_isPause);
            npcFactory.GamePause(_isPause, pauseTimeSlowRate);
        }
        
        public bool CheckIsPause()
        {
            return isPause;
        }
    }
}

