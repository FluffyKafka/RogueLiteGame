using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.CompilerServices;
using TMPro;
using Unity.VisualScripting;
using UnityEditor.Experimental.GraphView;
using UnityEditor.U2D.Aseprite;
using UnityEngine;
using UnityEngine.Assertions;
using UnityEngine.UIElements;

namespace MapGenerate
{
    internal class CMapGenerator : MonoBehaviour
    {
        [Header("地图参数")]
        [SerializeField] private int width = 20;
        [SerializeField] private int height = 10;
        [SerializeField] private int entryPos = 5;  // 入口Y坐标
        [SerializeField] private int exitPos = 5;   // 出口Y坐标
        [SerializeField] private int mainPathWidth = 3;           // 主路径宽度
        [SerializeField] private int eventRoomCount = 10;          // 事件房数量

        [Header("Generate Dir Weight")]
        [SerializeField] protected float upWeight;
        [SerializeField] protected float downWeight;
        [SerializeField] protected float rightWeight;
        [SerializeField] protected float dirAdjustWeight;
        [SerializeField] protected float inheritAdjustWeight;

        [Header("可视化设置")]
        [SerializeField] private bool showDebugInfo = true;
        [SerializeField] private float cellSize = 1f;
        [SerializeField] private GameObject roomPrefab;           // 房间预制体（可选）
        [SerializeField] private Transform roomsParent;            // 房间父对象

        [Header("随机种子")]
        [SerializeField] private int randomSeed = -1;              // -1表示使用随机种子
        [SerializeField] private bool useRandomSeed = true;

        // 地图数据
        private RoomData[,] mapData;
        private List<RoomData> eventRooms = new List<RoomData>();

        // 随机数生成器
        private System.Random random;

        void Start()
        {
            GenerateMap();
        }


        [ContextMenu("重新生成地图")]
        public void GenerateMap()
        {
            // 初始化随机数生成器
            if (useRandomSeed)
            {
                randomSeed = System.Environment.TickCount;
            }
            random = new System.Random(randomSeed);

            Debug.Log($"使用随机种子: {randomSeed}");

            // 清空旧地图
            ClearMap();

            // 参数校验
            ValidateParameters();

            // 初始化地图
            InitializeMap();

            // 生成主路径和事件房
            GenerateMainPath();

            // 生成连接通道
            GenerateConnections();

            // 可视化地图
            VisualizeMap();
        }

        [ContextMenu("清空地图")]
        public void ClearMap()
        {
            if (roomsParent != null)
            {
                for (int i = roomsParent.childCount - 1; i >= 0; i--)
                {
                    if (Application.isPlaying)
                        Destroy(roomsParent.GetChild(i).gameObject);
                    else
                        DestroyImmediate(roomsParent.GetChild(i).gameObject);
                }
            }

            mapData = null;
            eventRooms.Clear();
        }

        void ValidateParameters()
        {
            if (width <= 0 || height <= 0)
                throw new System.ArgumentException("地图尺寸必须为正数");

            if (entryPos < 0 || entryPos >= height || exitPos < 0 || exitPos >= height)
                throw new System.ArgumentException("出入口位置必须在[0, height)范围内");

            if (mainPathWidth <= 0)
                throw new System.ArgumentException("主路径宽度必须为正数");

            if (eventRoomCount < 0)
                throw new System.ArgumentException("事件房数量不能为负数");
        }

        void InitializeMap()
        {
            mapData = new RoomData[width, height];

            // 设置入口和出口
            mapData[0, entryPos] = new RoomData(0, entryPos, ERoomType.Entry);
            mapData[width - 1, exitPos] = new RoomData(width - 1, exitPos, ERoomType.Exit);
        }

        void GenerateMainPath()
        {
            // 生成事件房（要求X坐标各不相同）
            if (eventRoomCount > 0)
            {
                // 收集可用的X坐标（排除入口和出口列）
                List<int> availableX = new List<int>();
                for (int x = 1; x < width - 1; x++)
                {
                    availableX.Add(x);
                }

                // 随机打乱
                availableX = availableX.OrderBy(x => random.Next()).ToList();

                // 选择事件房X坐标
                int actualCount = Mathf.Min(eventRoomCount, availableX.Count);
                List<int> selectedX = availableX.Take(actualCount).OrderBy(x => x).ToList();

                // 在每个选中的X上随机选择Y
                foreach (int x in selectedX)
                {
                    Vector2Int yRange = CheckMainPathYRangeAt(x);
                    int y = Random.Range(yRange.x, yRange.y + 1);

                    mapData[x, y] = new RoomData(x, y, ERoomType.Event);

                    eventRooms.Add(mapData[x, y]);
                }
            }

            // 按X排序事件房
            eventRooms = eventRooms.OrderBy(r => r.x).ToList();

            if (showDebugInfo)
            {
                Debug.Log($"生成了 {eventRooms.Count} 个事件房");
            }
        }
        protected Vector2Int CheckMainPathYRangeAt(int _x)
        {
            // 线性插值计算中心Y坐标
            int centerY = entryPos + (exitPos - entryPos) * _x / (width - 1);

            // 根据宽度确定Y范围
            int halfWidth = mainPathWidth / 2;
            int minY = Mathf.Max(0, centerY - halfWidth);
            int maxY = Mathf.Min(height - 1, centerY + halfWidth);

            // 如果宽度为偶数，调整范围使其对称
            if (mainPathWidth % 2 == 0 && (centerY - minY) < (maxY - centerY))
            {
                minY = Mathf.Max(0, centerY - halfWidth + 1);
            }
            return new Vector2Int(minY, maxY);
        }

        void GenerateConnections()
        {
            if (eventRooms.Count == 0) return;

            eventRooms = eventRooms.OrderBy(r => r.x).ToList();

            // 起点：入口
            RoomData start = mapData[0, entryPos];
            RoomData currentStart = start;

            // 依次连接入口 -> 事件房1 -> 事件房2 -> ...
            foreach (RoomData targetRoom in eventRooms)
            {
                GeneratePathBetween(currentStart, targetRoom);
                currentStart = targetRoom;
            }
     
            RoomData exitRoom = mapData[width - 1, exitPos];
            GeneratePathBetween(currentStart, exitRoom);
        }
        void GeneratePathBetween(RoomData start, RoomData target)
        {
            int maxTestTime = width * height;

            if (start.x == target.x && start.y == target.y) return;            

            Vector2Int current = new Vector2Int(start.x, start.y);
            Vector2Int targetVector = new Vector2Int(target.x, target.y);
            while(current.x != target.x || current.y != target.y)
            {
                --maxTestTime;
                if(maxTestTime <= 0)
                {
                    VisualizeMap();
                    Assert.IsTrue(false);
                }

                if(current.x == target.x)
                {
                    if(current.y < target.y)
                    {
                        ++current.y;                      
                    }
                    else
                    {
                        --current.y;                     
                    }
                    
                }
                else
                {
                    current = GetRandomMove(current, targetVector);
                }

                if(current.x != target.x || current.y != target.y)
                {
                    mapData[current.x, current.y] = new RoomData(current.x, current.y, ERoomType.Passage);
                }
                
            }
        }
        protected Vector2Int GetRandomMove(Vector2Int _cur, Vector2Int _tar)
        {
            Vector2Int up = new Vector2Int(0, 1);
            Vector2Int down = new Vector2Int(0, -1);
            Vector2Int right = new Vector2Int(1, 0);

            float upWeightSum = upWeight;
            float downWeightSum = downWeight;
            float rightWeightSum = rightWeight + dirAdjustWeight;

            if (_cur.y > _tar.y)
            {
                downWeightSum += dirAdjustWeight;
            }
            else if(_cur.y < _tar.y)
            {
                upWeightSum += dirAdjustWeight;
            }
            else
            {
                rightWeightSum += dirAdjustWeight;
            }

            if(IsCellValid_PassageRoom(_cur + up))
            {
                downWeightSum += inheritAdjustWeight;
            }
            else if(IsCellValid_PassageRoom(_cur + down))
            {
                upWeightSum += inheritAdjustWeight;
            }
            else
            {
                rightWeightSum += inheritAdjustWeight;
            }

            bool canUp = false;
            bool canDown = false;
            bool canRight = false;
            float sumWeight = 0;
            if(IsCellValid_PassageRoom(_cur + up))
            {
                sumWeight += upWeightSum;
                canUp = true;
            }
            if(IsCellValid_PassageRoom(_cur + down))
            {
                sumWeight += downWeightSum;
                canDown = true;
            }
            if(IsCellValid_PassageRoom(_cur + right))
            {
                sumWeight += rightWeightSum;
                canRight = true;
            }
            if (!canUp && !canDown && !canRight)
            {
                VisualizeMap();
                Debug.LogError("生成失败");
            }

            float randomValue = Random.Range(0, sumWeight);
            float currentWeight = 0;

            

            if (canUp)
            {
                currentWeight += upWeightSum;
                if (randomValue < currentWeight)
                {
                    Debug.Log("UP   _Up: " + upWeightSum + ", Down: " + downWeightSum + ", Right: " + rightWeightSum + "; Random: " + randomValue + ", Current: " + currentWeight);
                    return _cur + up;
                }
            }

            if (canDown)
            {
                currentWeight += downWeightSum;
                if (randomValue < currentWeight)
                {
                    Debug.Log("Down   _Up: " + upWeightSum + ", Down: " + downWeightSum + ", Right: " + rightWeightSum + "; Random: " + randomValue + ", Current: " + currentWeight);
                    return _cur + down;
                }
            }

            if (canRight)
            {
                currentWeight += rightWeightSum;
                if (randomValue < currentWeight)
                {
                    Debug.Log("Right   _Up: " + upWeightSum + ", Down: " + downWeightSum + ", Right: " + rightWeightSum + "; Random: " + randomValue + ", Current: " + currentWeight);
                    return _cur + right;
                }
            }

            VisualizeMap();
            Assert.IsFalse(true);
            return _cur;
        }
        bool IsCellValid_PassageRoom(Vector2Int _cell)
        {
            return _cell.x >= 0 && _cell.x < width && _cell.y >= 0 && _cell.y < height && IsCellExistPassgae(_cell);
        }
        bool IsCellExistPassgae(Vector2Int _cell)
        {
            return mapData[_cell.x, _cell.y] == null || mapData[_cell.x, _cell.y].type != ERoomType.Passage;
        }

        void VisualizeMap()
        {
            if (mapData == null) return;

            // 如果提供了预制体，则生成3D对象
            if (roomPrefab != null && roomsParent != null)
            {
                for (int y = 0; y < height; y++)
                {
                    for (int x = 0; x < width; x++)
                    {
                        RoomData room = mapData[x, y];

                        if(room != null)
                        {
                            Vector3 position = new Vector3(x * cellSize, y * cellSize);
                            GameObject roomObj = Instantiate(roomPrefab, position, Quaternion.identity, roomsParent);

                            // 设置房间颜色
                            Renderer renderer = roomObj.GetComponent<Renderer>();
                            if (renderer != null)
                            {
                                renderer.material.color = room.GetColor();
                            }

                            // 添加房间数据组件
                            RoomVisual visual = roomObj.GetComponent<RoomVisual>();
                            if (visual == null)
                                visual = roomObj.AddComponent<RoomVisual>();
                            visual.SetRoomData(room);
                        }
                    }
                }
            }
        }
    }
    internal class RoomVisual : MonoBehaviour
    {
        private RoomData roomData;
        private TextMeshPro textMesh;
        private Renderer objectRenderer;

        void Awake()
        {
            objectRenderer = GetComponent<Renderer>();
            textMesh = GetComponentInChildren<TextMeshPro>();

            if (textMesh == null)
            {
                // 尝试添加TextMeshPro
                GameObject textObj = new GameObject("RoomText");
                textObj.transform.SetParent(transform);
                textObj.transform.localPosition = new Vector3(0, 0.5f, 0);
                textMesh = textObj.AddComponent<TextMeshPro>();
                textMesh.fontSize = 0.3f;
                textMesh.alignment = TextAlignmentOptions.Center;
            }
        }

        public void SetRoomData(RoomData data)
        {
            roomData = data;
            UpdateVisual();
        }

        void UpdateVisual()
        {
            if (objectRenderer != null)
            {
                objectRenderer.material.color = roomData.GetColor();
            }

            if (textMesh != null)
            {
                textMesh.text = roomData.GetTypeName();
            }
        }

        void OnMouseEnter()
        {
            if (roomData != null)
            {
                Debug.Log($"房间 ({roomData.x}, {roomData.y}) - {roomData.GetTypeName()}");
            }
        }
    }

    [System.Serializable]
    internal class RoomData
    {
        public int x, y;
        public ERoomType type;
        public RoomData(int _x, int _y, ERoomType _type)
        {
            x = _x;
            y = _y;
            type = _type;
        }

        public Color GetColor()
        {
            switch (type)
            {
                case ERoomType.Passage: return Color.gray; // 普通房间
                case ERoomType.Event: return Color.red;      // 事件房
                case ERoomType.Entry: return Color.green;    // 入口
                case ERoomType.Exit: return Color.blue;     // 出口
                default: return Color.white;
            }
        }

        public string GetTypeName()
        {
            switch (type)
            {
                case ERoomType.Passage: return "普通";
                case ERoomType.Event: return "事件房";
                case ERoomType.Entry: return "入口";
                case ERoomType.Exit: return "出口";
                default: return "未知";
            }
        }
    }

    internal enum ERoomType
    {
        Event,
        Passage,
        Entry,
        Exit
    }
}




