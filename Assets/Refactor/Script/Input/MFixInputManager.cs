using PlayerSystem;
using UnityEngine;

//需要被第一个Update
namespace InputManager
{
    public interface IInitInputManager
    {
        public void Init(IInputPlayer _player);
    }

    internal class MFixInputManager : MonoBehaviour, IInitInputManager, IPlayerInput
    {
        [SerializeField] protected IInputPlayer player;

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

        //注意到更新的顺序，若两个输入事件同时发生，则前一个事件将覆盖下一个事件
        private void Update()
        {
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
        }
    }
}

