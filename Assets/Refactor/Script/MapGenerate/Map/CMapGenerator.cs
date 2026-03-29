using System.Collections.Generic;
using System.Linq;
using TMPro;
using UnityEngine;
using UnityEngine.Assertions;

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

        [Header("支线路径参数")]
        [SerializeField] private int branchPathCount = 3;          // 支线路径数量
        [SerializeField] private int branchPathLength = 5;         // 支线路径预期长度
        [SerializeField] private int branchPathLengthOffset = 2;   // 支线路径长度偏移量

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
        [SerializeField] private bool showConnectionLines = true;  // 是否显示连接线
        [SerializeField] private Color connectionColor = Color.white; // 连接线颜色
        [SerializeField] private float connectionLineWidth = 0.1f;    // 连接线宽度

        [Header("随机种子")]
        [SerializeField] private int randomSeed = -1;              // -1表示使用随机种子
        [SerializeField] private bool useRandomSeed = true;

        // 地图数据
        private RoomData[,] mapData;
        private List<RoomData> eventRooms = new List<RoomData>();
        private List<LineRenderer> connectionLines = new List<LineRenderer>();

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

            // 生成支线路径
            GenerateBranchPaths();

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

            // 清除连接线
            foreach (var line in connectionLines)
            {
                if (line != null)
                {
                    if (Application.isPlaying)
                        Destroy(line.gameObject);
                    else
                        DestroyImmediate(line.gameObject);
                }
            }
            connectionLines.Clear();

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

            if (branchPathCount < 0)
                throw new System.ArgumentException("支线路径数量不能为负数");
        }

        void InitializeMap()
        {
            mapData = new RoomData[width, height];

            // 设置入口和出口
            mapData[0, entryPos] = new RoomData(0, entryPos, ERoomType.Entry);
            mapData[width - 1, exitPos] = new RoomData(width - 1, exitPos, ERoomType.Exit);
        }

        protected void SetRoomConnection(Vector2Int _roomPosition, EDirection _dir)
        {
            switch (_dir)
            {
                case EDirection.Up:
                    mapData[_roomPosition.x, _roomPosition.y].up = true;
                    mapData[_roomPosition.x, _roomPosition.y + 1].down = true;
                    return;
                case EDirection.Down:
                    mapData[_roomPosition.x, _roomPosition.y].down = true;
                    mapData[_roomPosition.x, _roomPosition.y - 1].up = true;
                    return;
                case EDirection.Left:
                    mapData[_roomPosition.x, _roomPosition.y].left = true;
                    mapData[_roomPosition.x - 1, _roomPosition.y].right = true;
                    return;
                case EDirection.Right:
                    mapData[_roomPosition.x, _roomPosition.y].right = true;
                    mapData[_roomPosition.x + 1, _roomPosition.y].left = true;
                    return;
            }
        }

        #region 主路经生成
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
            EDirection dir;
            while (current.x != target.x || current.y != target.y)
            {
                --maxTestTime;
                if (maxTestTime <= 0)
                {
                    VisualizeMap();
                    Assert.IsTrue(false);
                }

                if (current.x == target.x)
                {
                    if (current.y < target.y)
                    {
                        ++current.y;
                        dir = EDirection.Down;
                    }
                    else
                    {
                        --current.y;
                        dir = EDirection.Up;
                    }

                }
                else
                {
                    dir = GetRandomMove(ref current, targetVector);
                }

                if (current.x != target.x || current.y != target.y)
                {
                    mapData[current.x, current.y] = new RoomData(current.x, current.y, ERoomType.Passage);
                }
                SetRoomConnection(current, dir);
            }

        }
        protected EDirection GetRandomMove(ref Vector2Int _cur, Vector2Int _tar)
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
            else if (_cur.y < _tar.y)
            {
                upWeightSum += dirAdjustWeight;
            }
            else
            {
                rightWeightSum += dirAdjustWeight;
            }

            if (IsCellValid_PassageRoom(_cur + up))
            {
                downWeightSum += inheritAdjustWeight;
            }
            else if (IsCellValid_PassageRoom(_cur + down))
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
            if (IsCellValid_PassageRoom(_cur + up))
            {
                sumWeight += upWeightSum;
                canUp = true;
            }
            if (IsCellValid_PassageRoom(_cur + down))
            {
                sumWeight += downWeightSum;
                canDown = true;
            }
            if (IsCellValid_PassageRoom(_cur + right))
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
                    _cur += up;
                    return EDirection.Down;
                }
            }

            if (canDown)
            {
                currentWeight += downWeightSum;
                if (randomValue < currentWeight)
                {
                    Debug.Log("Down   _Up: " + upWeightSum + ", Down: " + downWeightSum + ", Right: " + rightWeightSum + "; Random: " + randomValue + ", Current: " + currentWeight);
                    _cur += down;
                    return EDirection.Up;
                }
            }

            if (canRight)
            {
                currentWeight += rightWeightSum;
                if (randomValue < currentWeight)
                {
                    Debug.Log("Right   _Up: " + upWeightSum + ", Down: " + downWeightSum + ", Right: " + rightWeightSum + "; Random: " + randomValue + ", Current: " + currentWeight);
                    _cur += right;
                    return EDirection.Left;
                }
            }

            VisualizeMap();
            Assert.IsFalse(true);
            return EDirection.Right;
        }
        bool IsCellValid_PassageRoom(Vector2Int _cell)
        {
            return _cell.x >= 0 && _cell.x < width && _cell.y >= 0 && _cell.y < height && IsCellExistPassgae(_cell);
        }
        bool IsCellExistPassgae(Vector2Int _cell)
        {
            return mapData[_cell.x, _cell.y] == null || mapData[_cell.x, _cell.y].type != ERoomType.Passage;
        }

        #endregion

        #region 支线路径生成
        void GenerateBranchPaths()
        {
            if (branchPathCount <= 0) return;

            // 收集所有主路径上的格子（包括入口、事件房、出口和普通通道）
            List<Vector2Int> mainPathCells = CollectMainPathCells();

            if (showDebugInfo)
            {
                Debug.Log($"主路径格子数量: {mainPathCells.Count}");
            }

            int generatedCount = 0;
            int maxAttempts = branchPathCount * 10; // 最大尝试次数
            int attempts = 0;

            while (generatedCount < branchPathCount && attempts < maxAttempts)
            {
                attempts++;

                // 随机选择一个主路径格子作为起点
                Vector2Int startCell = mainPathCells[random.Next(mainPathCells.Count)];

                // 计算目标长度
                int minLength = Mathf.Max(1, branchPathLength - branchPathLengthOffset);
                int maxLength = branchPathLength + branchPathLengthOffset;
                int targetLength = random.Next(minLength, maxLength + 1);

                // 使用回溯法生成支线路径
                List<Vector2Int> branchPath = TryGenerateBranchPathWithBacktracking(startCell, targetLength, minLength);

                if (branchPath != null)
                {
                    mapData[branchPath[0].x, branchPath[0].y].isBranchEntry = true;
                    for (int i = 1; i < branchPath.Count; ++i)
                    {
                        Vector2Int cell = branchPath[i];
                        Vector2Int last = branchPath[i - 1];
                        mapData[cell.x, cell.y] = new RoomData(cell.x, cell.y, ERoomType.Passage);

                        EDirection dir;
                        if (last.x < cell.x)
                        {
                            dir = EDirection.Left;
                        }
                        else if (last.x > cell.x)
                        {
                            dir = EDirection.Right;
                        }
                        else if (last.y > cell.y)
                        {
                            dir = EDirection.Up;
                        }
                        else
                        {
                            dir = EDirection.Down;
                        }
                        SetRoomConnection(cell, dir);
                    }
                    Vector2Int endCell = branchPath[branchPath.Count - 1];
                    mapData[endCell.x, endCell.y].type = ERoomType.Event;

                    generatedCount++;

                    if (showDebugInfo)
                    {
                        Debug.Log($"生成支线路径 {generatedCount}/{branchPathCount}，起点: ({startCell.x}, {startCell.y})，长度: {branchPath.Count}/{targetLength}");
                    }
                }
            }

            if (showDebugInfo)
            {
                Debug.Log($"支线路径生成完成: 成功生成 {generatedCount}/{branchPathCount} 条，尝试次数: {attempts}");
            }
        }

        List<Vector2Int> CollectMainPathCells()
        {
            List<Vector2Int> mainPathCells = new();

            // 遍历所有格子，添加通道房间（这些是主路径上的连接房间）
            for (int x = 0; x < width; x++)
            {
                for (int y = 0; y < height; y++)
                {
                    if (mapData[x, y] != null && mapData[x, y].type == ERoomType.Passage)
                    {
                        mainPathCells.Add(new Vector2Int(x, y));
                    }
                }
            }

            return mainPathCells;
        }

        List<Vector2Int> TryGenerateBranchPathWithBacktracking(Vector2Int start, int targetLength, int minLength)
        {
            List<Vector2Int> bestPath = null;
            List<Vector2Int> minPath = null;

            Vector2Int[] directions = new Vector2Int[]
            {
                new Vector2Int(0, 1),   // 上
                new Vector2Int(0, -1),  // 下
                new Vector2Int(1, 0),   // 右
                new Vector2Int(-1, 0)   // 左
            };

            HashSet<Vector2Int> visited = new HashSet<Vector2Int>();
            List<Vector2Int> currentPath = new List<Vector2Int>();
            currentPath.Add(start);
            visited.Add(start);

            // 使用深度优先搜索生成路径
            DFSBranchPath(start, targetLength, minLength, directions, visited, currentPath, ref bestPath, ref minPath);

            if (bestPath != null)
            {
                return bestPath;
            }
            else
            {
                return minPath;
            }
        }

        void DFSBranchPath(Vector2Int current, int targetLength, int minLength, Vector2Int[] directions,
            HashSet<Vector2Int> visited, List<Vector2Int> currentPath,
            ref List<Vector2Int> bestPath, ref List<Vector2Int> minPath)
        {
            // 如果当前路径长度已经达到目标长度或超过
            if (currentPath.Count >= targetLength)
            {
                if (bestPath == null || currentPath.Count > bestPath.Count)
                {
                    bestPath = new List<Vector2Int>(currentPath);
                }
                return;
            }

            if (currentPath.Count >= minLength)
            {
                if (minPath == null)
                {
                    minPath = new List<Vector2Int>(currentPath);
                }
            }

            // 随机打乱方向顺序
            directions = directions.OrderBy(d => random.Next()).ToArray();
            // 尝试所有方向
            foreach (Vector2Int dir in directions)
            {
                Vector2Int next = current + dir;

                // 检查下一个格子是否有效
                if (IsValidBranchCell(next, visited))
                {
                    visited.Add(next);
                    currentPath.Add(next);

                    DFSBranchPath(next, targetLength, minLength, directions, visited, currentPath, ref bestPath, ref minPath);

                    // 如果已经找到足够好的路径，可以提前结束
                    if (bestPath != null && bestPath.Count >= targetLength)
                    {
                        return;
                    }

                    // 回溯
                    currentPath.RemoveAt(currentPath.Count - 1);
                    visited.Remove(next);
                }
            }
        }

        bool IsValidBranchCell(Vector2Int cell, HashSet<Vector2Int> visited)
        {
            // 检查边界
            if (cell.x < 0 || cell.x >= width || cell.y < 0 || cell.y >= height)
                return false;

            // 检查是否已访问
            if (visited.Contains(cell))
                return false;

            // 检查是否已被占用
            if (mapData[cell.x, cell.y] != null)
            {
                return false;
            }

            return true;
        }
        #endregion

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

                        if (room != null)
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

                // 生成连接线
                if (showConnectionLines)
                {
                    GenerateConnectionLines();
                }
            }
        }

        void GenerateConnectionLines()
        {
            // 清除旧的连接线
            foreach (var line in connectionLines)
            {
                if (line != null)
                {
                    if (Application.isPlaying)
                        Destroy(line.gameObject);
                    else
                        DestroyImmediate(line.gameObject);
                }
            }
            connectionLines.Clear();

            // 遍历所有房间，根据连接方向生成线
            for (int x = 0; x < width; x++)
            {
                for (int y = 0; y < height; y++)
                {
                    RoomData room = mapData[x, y];
                    if (room == null) continue;

                    Vector3 startPos = new Vector3(x * cellSize, y * cellSize, -0.1f);
                    Vector3 endPos = Vector3.zero;
                    // 检查四个方向并生成连接线
                    if (room.up && y + 1 < height && mapData[x, y + 1] != null)
                    {
                        endPos = new Vector3(x * cellSize, (y + 1) * cellSize, -0.1f);
                        CreateConnectionLine(startPos, (endPos + startPos) / 2);
                    }

                    if (room.down && y - 1 >= 0 && mapData[x, y - 1] != null)
                    {
                        endPos = new Vector3(x * cellSize, (y - 1) * cellSize, -0.1f);
                        CreateConnectionLine(startPos, (endPos + startPos) / 2);
                    }

                    if (room.left && x - 1 >= 0 && mapData[x - 1, y] != null)
                    {
                        endPos = new Vector3((x - 1) * cellSize, y * cellSize, -0.1f);
                        CreateConnectionLine(startPos, (endPos + startPos) / 2);
                    }

                    if (room.right && x + 1 < width && mapData[x + 1, y] != null)
                    {
                        endPos = new Vector3((x + 1) * cellSize, y * cellSize, -0.1f);
                        CreateConnectionLine(startPos, (endPos + startPos) / 2);
                    }           
                }
            }
        }

        void CreateConnectionLine(Vector3 start, Vector3 end)
        {
            GameObject lineObj = new GameObject("ConnectionLine");
            lineObj.transform.SetParent(roomsParent);

            LineRenderer lineRenderer = lineObj.AddComponent<LineRenderer>();
            lineRenderer.startWidth = connectionLineWidth;
            lineRenderer.endWidth = connectionLineWidth;
            lineRenderer.material = new Material(Shader.Find("Sprites/Default"));
            lineRenderer.startColor = connectionColor;
            lineRenderer.endColor = connectionColor;
            lineRenderer.positionCount = 2;
            lineRenderer.SetPosition(0, start);
            lineRenderer.SetPosition(1, end);
            lineRenderer.sortingLayerName = "UI";

            connectionLines.Add(lineRenderer);
        }
    }

    internal class RoomVisual : MonoBehaviour
    {
        private RoomData roomData;
        private TextMeshPro textMesh;
        private Renderer objectRenderer;

        // 用于显示连接方向的小箭头或标记
        [SerializeField] private GameObject upArrow;
        [SerializeField] private GameObject downArrow;
        [SerializeField] private GameObject leftArrow;
        [SerializeField] private GameObject rightArrow;

        void Awake()
        {
            objectRenderer = GetComponent<Renderer>();
            textMesh = GetComponentInChildren<TextMeshPro>();

            if (textMesh == null)
            {
                // 尝试添加TextMeshPro
                GameObject textObj = new GameObject("RoomText");
                textObj.transform.SetParent(transform);
                textObj.transform.localPosition = new Vector3(0, 0.6f, 0);
                textMesh = textObj.AddComponent<TextMeshPro>();
                textMesh.fontSize = 0.3f;
                textMesh.alignment = TextAlignmentOptions.Center;
            }

            // 创建方向指示器
            CreateDirectionIndicators();
        }

        void CreateDirectionIndicators()
        {
            // 创建向上的箭头指示器
            upArrow = CreateArrowIndicator(Vector3.up * 0.4f, Quaternion.identity, "UpArrow");
            downArrow = CreateArrowIndicator(Vector3.down * 0.4f, Quaternion.Euler(0, 0, 180), "DownArrow");
            leftArrow = CreateArrowIndicator(Vector3.left * 0.5f, Quaternion.Euler(0, 0, 90), "LeftArrow");
            rightArrow = CreateArrowIndicator(Vector3.right * 0.5f, Quaternion.Euler(0, 0, -90), "RightArrow");

            // 默认隐藏所有箭头
            SetArrowsActive(false);
        }

        GameObject CreateArrowIndicator(Vector3 localPosition, Quaternion rotation, string name)
        {
            GameObject arrowObj = new GameObject(name);
            arrowObj.transform.SetParent(transform);
            arrowObj.transform.localPosition = localPosition;
            arrowObj.transform.localRotation = rotation;

            // 创建简单的箭头网格（三角形）
            MeshFilter meshFilter = arrowObj.AddComponent<MeshFilter>();
            MeshRenderer meshRenderer = arrowObj.AddComponent<MeshRenderer>();

            // 创建三角形网格
            Mesh mesh = new Mesh();
            Vector3[] vertices = new Vector3[]
            {
                new Vector3(0, 0.1f, 0),
                new Vector3(-0.05f, 0, 0),
                new Vector3(0.05f, 0, 0)
            };

            int[] triangles = new int[] { 0, 1, 2 };
            mesh.vertices = vertices;
            mesh.triangles = triangles;
            mesh.RecalculateNormals();

            meshFilter.mesh = mesh;
            meshRenderer.material = new Material(Shader.Find("Sprites/Default"));
            meshRenderer.material.color = Color.yellow;

            return arrowObj;
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

            // 根据连接方向显示箭头
            UpdateDirectionArrows();
        }

        void UpdateDirectionArrows()
        {
            if (roomData == null) return;

            // 显示有连接的方向的箭头
            if (upArrow != null) upArrow.SetActive(roomData.up);
            if (downArrow != null) downArrow.SetActive(roomData.down);
            if (leftArrow != null) leftArrow.SetActive(roomData.left);
            if (rightArrow != null) rightArrow.SetActive(roomData.right);
        }

        void SetArrowsActive(bool active)
        {
            if (upArrow != null) upArrow.SetActive(active);
            if (downArrow != null) downArrow.SetActive(active);
            if (leftArrow != null) leftArrow.SetActive(active);
            if (rightArrow != null) rightArrow.SetActive(active);
        }

        void OnMouseEnter()
        {
            if (roomData != null)
            {
                Debug.Log($"房间 ({roomData.x}, {roomData.y}) - {roomData.GetTypeName()}\n" +
                         $"连接方向: {(roomData.up ? "↑ " : "")}{(roomData.down ? "↓ " : "")}" +
                         $"{(roomData.left ? "← " : "")}{(roomData.right ? "→ " : "")}");
            }
        }

        void OnMouseOver()
        {
            // 可以在鼠标悬停时高亮显示连接
            if (roomData != null)
            {
                // 这里可以添加高亮效果
            }
        }

        void OnMouseExit()
        {
            // 恢复原样
        }
    }

    [System.Serializable]
    internal class RoomData
    {
        public int x, y;
        public bool up, down, left, right;
        public bool isBranchEntry = false;
        public ERoomType type;
        public RoomData(int _x, int _y, ERoomType _type)
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

        public Color GetColor()
        {
            switch (type)
            {
                case ERoomType.Passage: 
                    return isBranchEntry ? Color.black : Color.gray; // 普通房间
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

    internal enum EDirection
    {
        Up,
        Down,
        Left,
        Right
    }
}