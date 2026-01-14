using EntitySystem.EntityActor.PlayerActor;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Assertions;
using UnityEngine.Windows;

//需要被第一个Update
namespace InputManager
{
    public interface IInputManagerInit
    {
        public void InitInputManager(IPlayerInput _player);
    }

    internal class MFixInputManager : MonoBehaviour, IInputManagerInit
    {
        [SerializeField] protected IPlayerInput player;

        public void InitInputManager(IPlayerInput _player)
        {
            Assert.IsTrue(_player is IPlayerInput);
            player = _player as IPlayerInput;
        }

        //注意到更新的顺序，若两个输入事件同时发生，则前一个事件将覆盖下一个事件
        private void Update()
        {
            if(UnityEngine.Input.GetKeyDown(KeyCode.Space))
            {
                player.JumpInput();
            }

            float xInput = UnityEngine.Input.GetAxisRaw("Horizontal");
            if (xInput != 0)
            {
                player.HorizonInput(xInput);
            }
        }
    }
}

