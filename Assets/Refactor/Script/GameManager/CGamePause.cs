using NPCSystem;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Windows;

namespace GameManagerSystem
{
    internal class CGamePause : CGameManagerComponentBase
    {
        [SerializeField] protected float pauseTimeSlowRate;

        protected bool isPause = false;

        protected override void Awake()
        {
            base.Awake();
            game.PauseNotice += Pause;
            game.PauseRawNotice += PauseRaw;
            game.CheckIsPauseNotice += CheckIsPause;
            game.CheckPauseAnimSlowRateNotice += CheckPasueAnimSlowRate;
        }

        public void Pause(bool _isPause)
        {
            isPause = _isPause;
            if (_isPause)
            {
                Time.timeScale *= pauseTimeSlowRate;
            }
            else
            {
                Time.timeScale = 1;
            }
            PauseGameToOther(_isPause);
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
            PauseGameToOther(_isPause);
        }

        protected void PauseGameToOther(bool _isPause)
        {
            game.input.GamePause(_isPause);
            game.npcFactory.GamePause(_isPause, pauseTimeSlowRate);
        }

        public bool CheckIsPause()
        {
            return isPause;
        }

        protected float CheckPasueAnimSlowRate()
        {
            return pauseTimeSlowRate;
        }
    }
}

