using EnemySystem;
using InventorySystem;
using Item;
using NPCSystem;
using ObjectController;
using PlayerSystem;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using static MapGenerate.IRoomGenerator;

namespace MapGenerate
{
    internal enum ETileMapType
    {
        Ground,
        BackGround,
        Platform
    }

    public interface IInitMapGenerater
    {
        public void Init(
            IMapPlayer _player, IMapInventory _inventory,
            IMapEnemyFactory _enemyFactory, IMapNPCFactory _npcFactory,
            IMapObjectFactroy _objectFactory, IMapInput _input
            );
    }

    internal class MMapGenerateManager : MonoBehaviour, IInitMapGenerater
    {
        [SerializeField] protected bool isGenerate;
        [SerializeField] protected float difficulty;

        protected IMapPlayer player;
        protected IMapInventory inventory;
        protected IMapEnemyFactory enemyFactory;
        protected IMapNPCFactory npcFactory;
        protected IMapObjectFactroy objectFactory;
        protected IMapInput input;

        public void Init(
            IMapPlayer _player, IMapInventory _inventory,
            IMapEnemyFactory _enemyFactory, IMapNPCFactory _npcFactory,
            IMapObjectFactroy _objectFactory, IMapInput _input
            )
        {
            player = _player;
            inventory = _inventory;
            enemyFactory = _enemyFactory;
            npcFactory = _npcFactory;
            objectFactory = _objectFactory;
            input = _input;  
        }

        private void Start()
        {
            if (isGenerate)
            {
                GetComponent<IMapGenerator>().GenerateMap();
            }
        }

        public void GenerateEnemyAt(EEnemyType type, Vector3 _position)
        {
            enemyFactory.GetEmemyGameObjectAt(type, _position);
        }

        public void GenerateNPCAt(ENPCType _type, Vector3 _position)
        {
            npcFactory.GenerateNPCByTypeAt(_type, _position);
        }

        public void GenerateRewardBoxAt(List<IItemData> _rewards, float _coin, Vector3 _position, bool _isAdvanced)
        {
            if(!_isAdvanced)
            {
                objectFactory.GeneratePrimaryRewardBox(_rewards, _coin, _position);
            }
            else
            {
                objectFactory.GenerateAdvanceRewardBox(_rewards, _coin, _position);
            }
        }

        public void GenerateRoomFromData(DMapRoomInfo _data, int passageRoomCount)
        {
            DRoomGenerateInfo info = new();
            info.haveUpWall = !_data.up;
            info.haveDownWall = !_data.down;
            info.haveLeftWall = !_data.left;
            info.haveRightWall = !_data.right;
            info.isBranchEntry = _data.isBranchEntry;
            info.isBranchEnd = _data.isBranchEnd;
            info.roomType = _data.type;
            info.roomIndex.x = _data.x;
            info.roomIndex.y = _data.y;
            if(_data.type == ERoomType.Passage)
            {
                info.enemyDifficulty = difficulty / passageRoomCount;
            }
            else
            {
                info.enemyDifficulty = -1;
            }
            GetComponent<IRoomGenerator>().GenerateRoomFromData(info);
        }

        public void GenerateDeliverPointAt(Vector3 _position)
        {
            Debug.Log("GenerateDeliverPoints");
            objectFactory.GenerateDeliverPointAt(_position);
        }

        public bool IsAnyKeyInput()
        {
            return input.IsAnyKeyInput();
        }
    }

    internal interface IRoomGenerator
    {
        public void GenerateRoomFromData(DRoomGenerateInfo _data);
    }

    internal interface IMapGenerator
    {
        public void GenerateMap();
    }

    internal enum EDirection
    {
        Up,
        Down,
        Left,
        Right
    }
    internal enum ERoomType
    {
        Event,
        Passage,
        Entry,
        Exit
    }

    internal class DRoomGenerateInfo
    {
        public bool haveLeftWall;
        public bool haveUpWall;
        public bool haveDownWall;
        public bool haveRightWall;
        public ERoomType roomType;
        public Vector2 roomIndex;
        public float enemyDifficulty;
        public bool isBranchEntry;
        public bool isBranchEnd;
    }

    [System.Serializable]
    internal class DMapRoomInfo
    {
        public int x, y;
        public bool up, down, left, right;
        public bool isBranchEntry = false;
        public bool isBranchEnd = false;
        public ERoomType type;
        public DMapRoomInfo(int _x, int _y, ERoomType _type)
        {
            x = _x;
            y = _y;
            type = _type;
            up = down = left = right = false;
        }

        public void SetDirection(EDirection _dir, bool _isConnect)
        {
            switch (_dir)
            {
                case EDirection.Up:
                    up = _isConnect; return;
                case EDirection.Down:
                    down = _isConnect; return;
                case EDirection.Left:
                    left = _isConnect; return;
                case EDirection.Right:
                    right = _isConnect; return;
            }
        }     
    }

    public interface IMapInput
    {
        public bool IsAnyKeyInput();
    }
}
