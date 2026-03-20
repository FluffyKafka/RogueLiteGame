using GameManagerSystem;
using PlayerSystem;
using System;
using System.Collections.Generic;
using UIData;
using UnityEngine;

//需要被第一个Update
namespace InputManager
{
    public interface IInitInputManager
    {
        public void Init(IInputPlayer _player);
    }

    public interface IGameManagerInput
    {
        public void GamePause(bool _isPause);
    }

    internal class MFixInputManager : MonoBehaviour, IInitInputManager, IPlayerInput, IGameManagerInput
    {
        protected IInputPlayer player;

        [Serializable]
        protected class KeyUIPagePair
        {
            public KeyCode key;
            public EUIPageType uiPage;
        }
        [SerializeField] protected List<KeyUIPagePair> uiPageInput;

        protected bool isPasueGame = false;

        public void GamePause(bool _isPause)
        {
            isPasueGame = _isPause;
        }

        public float CheckHorizonInput()
        {
            return UnityEngine.Input.GetAxisRaw("Horizontal");
        }

        public float CheckVerticalInput()
        {
            return UnityEngine.Input.GetAxisRaw("Vertical");
        }

        public void Init(IInputPlayer _player)
        {
            player = _player;
        }

        public Vector3 CheckMousePosition()
        {
            Vector3 mousePos = Input.mousePosition;
            Vector3 worldPos = Camera.main.ScreenToWorldPoint(mousePos);
            return worldPos;
        }

        public KeyCode CheckSkillInputSlotKey(int _index)
        {
            if(_index == 0)
            {
                return KeyCode.LeftShift;
            }
            else if(_index == 1)
            {
                return KeyCode.Mouse1;
            }
            else if(_index == 2)
            {
                return KeyCode.Q;
            }
            return KeyCode.None;
        }

        public KeyCode CheckNPCInteractInputKey()
        {
            return KeyCode.G;
        }

        //注意到更新的顺序，若两个输入事件同时发生，则前一个事件将覆盖下一个事件
        private void Update()
        {
            foreach (var pair in uiPageInput)
            {
                if (Input.GetKeyDown(pair.key))
                {
                    player.UIPageSwitchInput(pair.uiPage);
                }
            }

            if (isPasueGame)
            {
                return;
            }

            if (Input.GetKeyDown(KeyCode.Mouse0))
            {
                player.AttackInput();
            }

            if (Input.GetKeyDown(KeyCode.Space))
            { 
                player.JumpInput();
            }

            if(Input.GetKeyDown(KeyCode.Mouse1))
            {
                player.SkillInputBegin(1);                
            }
            else if(Input.GetKeyUp(KeyCode.Mouse1))
            {
                player.SkillInputEnd(1);
            }

            if (Input.GetKeyDown(KeyCode.LeftShift))
            {
                player.SkillInputBegin(0);
            }
            else if (Input.GetKeyUp(KeyCode.LeftShift))
            {
                player.SkillInputEnd(0);
            }

            if (Input.GetKeyDown(KeyCode.Q))
            {
                player.SkillInputBegin(2);
            }
            else if (Input.GetKeyUp(KeyCode.Q))
            {
                player.SkillInputEnd(2);
            }

            float xInput = Input.GetAxisRaw("Horizontal");
            if (xInput != 0)
            {
                player.HorizonInput(xInput);
            }

            float yInput = Input.GetAxisRaw("Vertical");
            if(yInput != 0)
            {
                player.VerticalInput(yInput);
            }

            if(Input.GetKeyDown(KeyCode.G))
            {
                player.InteractToNPCInput();
            }
        }
    }
}

