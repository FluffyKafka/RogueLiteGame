using PlayerSystem;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace GameManagerSystem
{
    internal class MGameManager : MonoBehaviour, IPlayerGameManager
    {
        [SerializeField] protected float pauseTimeSlowRate;
         
        public void Pause(bool _isPause)
        {
            if(_isPause)
            {
                Time.timeScale *= pauseTimeSlowRate;
            }
            else
            {
                Time.timeScale /= pauseTimeSlowRate;
            }
        }

        public void PauseRaw(bool _isPause)
        {
            if (_isPause)
            {
                Time.timeScale = 0;
            }
            else
            {
                Time.timeScale = 1;
            }
        }

    }
}

