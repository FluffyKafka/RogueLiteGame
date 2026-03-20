using PlayerSystem;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace GameManagerSystem
{
    public interface IGameManager
    {
        public void GamePause(bool _isPasue);
    }
    
    public interface IInitGameManager
    {
        public void AddComponentsToPause(IGameManager _component);
    }

    internal class MGameManager : MonoBehaviour, IPlayerGameManager, IInitGameManager
    {
        [SerializeField] protected float pauseTimeSlowRate;
        protected bool isPause = false;
        protected List<IGameManager> componentsToPause = new();

        public void AddComponentsToPause(IGameManager _component)
        {
            componentsToPause.Add(_component);
        }

        public void Pause(bool _isPause)
        {
            isPause = _isPause;
            if(_isPause)
            {
                Time.timeScale *= pauseTimeSlowRate;
                foreach(var component in componentsToPause)
                {
                    component.GamePause(true);
                }
            }
            else
            {
                Time.timeScale = 1;
                foreach (var component in componentsToPause)
                {
                    component.GamePause(false);
                }
            }
        }

        public void PauseRaw(bool _isPause)
        {
            isPause = _isPause;
            if (_isPause)
            {
                Time.timeScale = 0;
                foreach (var component in componentsToPause)
                {
                    component.GamePause(true);
                }
            }
            else
            {
                Time.timeScale = 1;
                foreach (var component in componentsToPause)
                {
                    component.GamePause(false);
                }
            }
        }
        
        public bool CheckIsPause()
        {
            return isPause;
        }
    }
}

