using SaveSystem;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace UISystem
{
    public interface IInitMenuUI
    {
        public void Init(IMenuSaveManager _save, IMenuGameManager _game, IMenuAudio _audio);
    }
    internal class CMainMenu : MonoBehaviour, IInitMenuUI
    {
        [SerializeField] protected Button continueButton;
        [SerializeField] protected Button newGameButton;
        [SerializeField] protected Button exitButton;

        protected IMenuSaveManager save;
        protected IMenuGameManager game;
        protected IMenuAudio audioSystem;

        public void Init(IMenuSaveManager _save, IMenuGameManager _game, IMenuAudio _audio)
        {
            save = _save;
            game = _game;
            audioSystem = _audio;

            continueButton?.gameObject.SetActive(save.HasOldGame());
            continueButton?.onClick.AddListener(ContinueGame);
            continueButton?.onClick.AddListener(() => audioSystem.ButtonClick(transform, true));
            newGameButton?.onClick.AddListener(NewGame);
            newGameButton?.onClick.AddListener(() => audioSystem.ButtonClick(transform, true));
            exitButton?.onClick.AddListener(Application.Quit);
            exitButton?.onClick.AddListener(() => audioSystem.ButtonClick(transform, true));
        }

        protected void ContinueGame()
        {
            string scene = save.CheckContinueSceneName();
            game.SwitchSceneTo(scene);
        }

        protected void NewGame()
        {
            save.NewGame();
            string scene = save.CheckContinueSceneName();
            game.SwitchSceneTo(scene);
        }
    }

    public interface IMenuSaveManager
    {
        public void NewGame();
        public string CheckContinueSceneName();
        public bool HasOldGame();
    }
    public interface IMenuGameManager
    {
        public void SwitchSceneTo(string _name);
    }
    public interface IMenuAudio
    {
        public void ButtonClick(Transform button, bool _isPlay);
    }
}

