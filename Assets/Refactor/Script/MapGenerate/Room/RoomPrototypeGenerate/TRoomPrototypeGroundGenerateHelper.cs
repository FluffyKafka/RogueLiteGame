using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Tilemaps;
using System.Linq;

namespace MapGenerate
{
    internal class TRoomPrototypeGroundGenerateHelper : MonoBehaviour
    {
        [SerializeField] protected Tilemap map;
        [SerializeField] protected TileBase tile;
        [SerializeField] protected BoxCollider2D boundsCollider;

        [Header("生成参数")]
        [SerializeField] private float initialFillProbability = 0.45f;
        [SerializeField] private int randomSeed = 42;
        [SerializeField] private int maxIterTime = 100;

        [Header("规则配置")]
        [SerializeField] private string ruleFolderPath = "Assets/Rules"; // 规则文件夹路径
        [SerializeField] private bool enableRuleValidation = true; // 是否启用规则验证
        [SerializeField] private bool clearInvalidTilesOnStart = false; // 开始时是否清除无效tile

        // 存储当前地图状态
        private bool[,] groundMap;
        private Vector2Int mapSize;
        private Vector2Int mapOffset;
        private bool isInitialized = false;

        // 规则相关
        [SerializeField] private List<DTileSetRuleBase> loadedRules;

        private void Awake()
        {
            if (map == null)
                map = GetComponent<Tilemap>();

            if (boundsCollider == null)
                boundsCollider = GetComponent<BoxCollider2D>();

            // 加载规则
            LoadRulesFromFolder();
        }

        private void Start()
        {
            if (clearInvalidTilesOnStart)
            {
                ClearInvalidTilesByRules();
            }
        }

        // 从指定文件夹加载所有规则
        [ContextMenu("加载规则")]
        public void LoadRulesFromFolder()
        {
            loadedRules.Clear();

#if UNITY_EDITOR
            // 确保文件夹路径存在
            if (!System.IO.Directory.Exists(ruleFolderPath))
            {
                Debug.LogWarning($"规则文件夹不存在: {ruleFolderPath}");
                return;
            }

            // 获取文件夹中所有的DTileSetRuleBase资产
            string[] guids = UnityEditor.AssetDatabase.FindAssets("t:DTileSetRuleBase", new[] { ruleFolderPath });

            foreach (string guid in guids)
            {
                string assetPath = UnityEditor.AssetDatabase.GUIDToAssetPath(guid);
                DTileSetRuleBase rule = UnityEditor.AssetDatabase.LoadAssetAtPath<DTileSetRuleBase>(assetPath);
                if (rule != null && !loadedRules.Contains(rule))
                {
                    loadedRules.Add(rule);
                    Debug.Log($"加载规则: {rule.name} from {assetPath}");
                }
            }

            Debug.Log($"总共加载了 {loadedRules.Count} 条规则");
#else
            Debug.LogWarning("规则加载仅在编辑器中可用");
#endif
        }

        [ContextMenu("执行地形修正")]
        public void GenerateMap()
        {
            int test = maxIterTime;
            int change = 1;
            while(change != 0)
            {
                change = 0;
                change += ClearInvalidTilesByRules();
                change += ExecuteFillIteration();
                --test;
                if(test < 0)
                {
                    Debug.LogError("未能在指定迭代次数内修正地形");
                }
            }
        }

        [ContextMenu("清除无效Tile（按规则）")]
        public int ClearInvalidTilesByRules()
        {
            LoadCurrentMapState();
            if (!enableRuleValidation)
            {
                Debug.Log("规则验证未启用");
                return 0;
            }

            if (loadedRules.Count == 0)
            {
                Debug.LogWarning("没有加载任何规则，请先调用LoadRulesFromFolder()或在Inspector中设置规则文件夹路径");
                return 0;
            }

            if (!isInitialized || groundMap == null)
            {
                LoadCurrentMapState();
                if (groundMap == null)
                {
                    Debug.LogError("无法加载地图状态");
                    return 0;
                }
            }

            int count = 0;
            int validSum = 0;
            bool[,] newMap = (bool[,])groundMap.Clone();

            for (int x = 0; x < mapSize.x; x++)
            {
                for (int y = 0; y < mapSize.y; y++)
                {
                    if (!groundMap[x, y]) continue;

                    // 获取当前位置的邻居信息
                    bool[] prototypeNeighbors = GetPrototypeNeighbors(x, y);

                    // 检查所有规则，如果任何规则返回false，则清除此tile
                    bool isValid = false;                   
                    foreach (var rule in loadedRules)
                    {
                        try
                        {
                            if (rule.CanPlace_Prototype(prototypeNeighbors))
                            {
                                isValid = true;                                
                                break;
                            }
                        }
                        catch (System.Exception e)
                        {
                            Debug.LogError($"规则 {rule.name} 执行出错: {e.Message}");
                            isValid = false;
                            break;
                        }
                    }

                    ++count;
                    newMap[x, y] = false;
                    if (isValid)
                    {
                        ++validSum;
                        newMap[x, y] = true;
                    }
                }
            }
            groundMap = newMap;
            RenderMap();
            return count - validSum;
        }

        // 获取指定位置的原型邻居信息（用于CanPlace_Prototype）
        // 邻居顺序根据你的实际规则定义调整
        // 获取指定位置的原型邻居信息（用于CanPlace_Prototype）
        // 邻居顺序: 从左到右，从上到下
        // 索引布局（3x3网格，中心为当前位置）：
        // 0 1 2
        // 3 4 5
        // 6 7 8
        // 其中索引4是当前位置，但我们只返回周围8个邻居
        private bool[] GetPrototypeNeighbors(int x, int y)
        {
            bool[] neighbors = new bool[8];

            // 左上 (索引0) - 第1行第1列
            neighbors[0] = (x - 1 >= 0 && y + 1 < mapSize.y) && groundMap[x - 1, y + 1];
            // 上 (索引1) - 第1行第2列
            neighbors[1] = (y + 1 < mapSize.y) && groundMap[x, y + 1];
            // 右上 (索引2) - 第1行第3列
            neighbors[2] = (x + 1 < mapSize.x && y + 1 < mapSize.y) && groundMap[x + 1, y + 1];

            // 左 (索引3) - 第2行第1列
            neighbors[3] = (x - 1 >= 0) && groundMap[x - 1, y];
            // 右 (索引4) - 第2行第3列
            neighbors[4] = (x + 1 < mapSize.x) && groundMap[x + 1, y];

            // 左下 (索引5) - 第3行第1列
            neighbors[5] = (x - 1 >= 0 && y - 1 >= 0) && groundMap[x - 1, y - 1];
            // 下 (索引6) - 第3行第2列
            neighbors[6] = (y - 1 >= 0) && groundMap[x, y - 1];
            // 右下 (索引7) - 第3行第3列
            neighbors[7] = (x + 1 < mapSize.x && y - 1 >= 0) && groundMap[x + 1, y - 1];

            return neighbors;
        }

        // 原有的方法保持不变...
        private void CalculateMapBounds()
        {
            if (boundsCollider == null || map == null) return;

            Bounds bounds = boundsCollider.bounds;
            Vector3Int minCell = map.WorldToCell(bounds.min);
            Vector3Int maxCell = map.WorldToCell(bounds.max);

            mapSize = new Vector2Int(maxCell.x - minCell.x + 1, maxCell.y - minCell.y + 1);
            mapOffset = new Vector2Int(minCell.x, minCell.y);

            Debug.Log($"地图范围: {mapSize.x} x {mapSize.y}, 偏移: ({mapOffset.x}, {mapOffset.y})");
        }

        private void LoadCurrentMapState()
        {
            if (map == null || boundsCollider == null) return;

            CalculateMapBounds();
            groundMap = new bool[mapSize.x, mapSize.y];

            for (int x = 0; x < mapSize.x; x++)
            {
                for (int y = 0; y < mapSize.y; y++)
                {
                    Vector3Int tilePosition = new Vector3Int(x + mapOffset.x, y + mapOffset.y, 0);
                    groundMap[x, y] = map.GetTile(tilePosition) != null;
                }
            }

            isInitialized = true;
            Debug.Log($"已加载当前地图状态，当前填充率: {CalculateFillRate():P2}");
        }

        [ContextMenu("执行填补迭代")]
        public int ExecuteFillIteration()
        {
            LoadCurrentMapState();
            if (!isInitialized || groundMap == null)
            {
                Debug.Log("未初始化，正在从当前Tilemap加载状态...");
                LoadCurrentMapState();
                if (groundMap == null)
                {
                    Debug.LogError("无法加载地图状态，请确保BoxCollider2D和Tilemap配置正确！");
                    return 0;
                }
            }

            int filledCount = 0;
            bool[,] newMap = (bool[,])groundMap.Clone();

            for (int x = 1; x < mapSize.x - 1; x++)
            {
                for (int y = 1; y < mapSize.y - 1; y++)
                {
                    if (groundMap[x, y]) continue;

                    if (ShouldFill(x, y))
                    {
                        newMap[x, y] = true;
                        filledCount++;
                    }
                }
            }

            groundMap = newMap;
            RenderMap();
            Debug.Log($"填补迭代完成，新增 {filledCount} 个tile，当前填充率: {CalculateFillRate():P2}");
            return filledCount;
        }

        [ContextMenu("清空框内全部tile")]
        public void ClearAllTilesInBounds()
        {
            if (map == null || boundsCollider == null)
            {
                Debug.LogError("请确保Tilemap和BoxCollider2D组件都已赋值！");
                return;
            }

            CalculateMapBounds();

            if (groundMap != null)
            {
                groundMap = null;
                isInitialized = false;
            }

            int clearedCount = 0;
            for (int x = 0; x < mapSize.x; x++)
            {
                for (int y = 0; y < mapSize.y; y++)
                {
                    Vector3Int tilePosition = new Vector3Int(x + mapOffset.x, y + mapOffset.y, 0);
                    if (map.GetTile(tilePosition) != null)
                    {
                        map.SetTile(tilePosition, null);
                        clearedCount++;
                    }
                }
            }

            Debug.Log($"已清空框定区域内的 {clearedCount} 个tile");
        }

        private bool ShouldFill(int x, int y)
        {
            if (x > 0 && x < mapSize.x - 1 && y > 0 && y < mapSize.y - 1)
            {
                if (groundMap[x - 1, y] && groundMap[x + 1, y])
                    return true;
                if (groundMap[x, y - 1] && groundMap[x, y + 1])
                    return true;
            }
            return false;
        }

        private void RenderMap()
        {
            if (map == null || tile == null) return;

            for (int x = 0; x < mapSize.x; x++)
            {
                for (int y = 0; y < mapSize.y; y++)
                {
                    Vector3Int tilePosition = new Vector3Int(x + mapOffset.x, y + mapOffset.y, 0);
                    if (groundMap[x, y])
                    {
                        map.SetTile(tilePosition, tile);
                    }
                    else
                    {
                        map.SetTile(tilePosition, null);
                    }
                }
            }
        }

        private float CalculateFillRate()
        {
            if (groundMap == null) return 0;

            int filledCount = 0;
            for (int x = 0; x < mapSize.x; x++)
            {
                for (int y = 0; y < mapSize.y; y++)
                {
                    if (groundMap[x, y]) filledCount++;
                }
            }

            return (float)filledCount / (mapSize.x * mapSize.y);
        }

        public Vector2Int GetMapSize() => mapSize;
        public Vector2Int GetMapOffset() => mapOffset;

        [ContextMenu("更新边界")]
        public void UpdateBounds()
        {
            if (boundsCollider != null)
            {
                CalculateMapBounds();
                if (groundMap != null)
                {
                    RenderMap();
                }
            }
        }

#if UNITY_EDITOR
        private void OnDrawGizmosSelected()
        {
            if (boundsCollider != null)
            {
                Gizmos.color = Color.green;
                Gizmos.DrawWireCube(boundsCollider.bounds.center, boundsCollider.bounds.size);
            }
        }
#endif
    }
}