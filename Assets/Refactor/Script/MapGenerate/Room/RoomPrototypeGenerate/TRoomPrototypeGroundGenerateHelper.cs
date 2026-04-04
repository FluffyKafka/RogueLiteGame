using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Tilemaps;

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

        // 存储当前地图状态
        private bool[,] groundMap;
        private Vector2Int mapSize;
        private Vector2Int mapOffset;

        private void Awake()
        {
            if (map == null)
                map = GetComponent<Tilemap>();

            if (boundsCollider == null)
                boundsCollider = GetComponent<BoxCollider2D>();
        }

        private void CalculateMapBounds()
        {
            if (boundsCollider == null || map == null) return;

            // 获取BoxCollider2D的边界
            Bounds bounds = boundsCollider.bounds;

            // 将世界坐标转换为Tilemap的单元格坐标
            Vector3Int minCell = map.WorldToCell(bounds.min);
            Vector3Int maxCell = map.WorldToCell(bounds.max);

            // 计算地图尺寸
            mapSize = new Vector2Int(maxCell.x - minCell.x + 1, maxCell.y - minCell.y + 1);
            mapOffset = new Vector2Int(minCell.x, minCell.y);

            Debug.Log($"地图范围: {mapSize.x} x {mapSize.y}, 偏移: ({mapOffset.x}, {mapOffset.y})");
        }

        [ContextMenu("初始化")]
        public void Initialize()
        {
            if (map == null || boundsCollider == null)
            {
                Debug.LogError("请确保Tilemap和BoxCollider2D组件都已赋值！");
                return;
            }

            // 计算地图边界
            CalculateMapBounds();

            // 初始化地图数组
            groundMap = new bool[mapSize.x, mapSize.y];

            // 设置随机种子
            Random.InitState(randomSeed);

            // 随机填充（考虑边界，避免边缘检测时越界）
            for (int x = 1; x < mapSize.x - 1; x++)
            {
                for (int y = 1; y < mapSize.y - 1; y++)
                {
                    groundMap[x, y] = Random.value < initialFillProbability;
                }
            }

            // 渲染地图
            RenderMap();

            Debug.Log($"初始化完成，填充率: {CalculateFillRate():P2}");
        }

        [ContextMenu("执行填补迭代")]
        public void ExecuteFillIteration()
        {
            if (groundMap == null)
            {
                Debug.LogError("请先初始化地图！");
                return;
            }

            int filledCount = 0;
            bool[,] newMap = (bool[,])groundMap.Clone();

            // 遍历每个位置
            for (int x = 1; x < mapSize.x - 1; x++)
            {
                for (int y = 1; y < mapSize.y - 1; y++)
                {
                    // 跳过已经是tile的位置
                    if (groundMap[x, y]) continue;

                    // 检查是否符合填补规则：两个tile中间有一个空位置
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
        }

        [ContextMenu("执行消除迭代")]
        public void ExecuteEliminateIteration()
        {
            if (groundMap == null)
            {
                Debug.LogError("请先初始化地图！");
                return;
            }

            int eliminatedCount = 0;
            bool[,] newMap = (bool[,])groundMap.Clone();

            // 遍历每个位置
            for (int x = 1; x < mapSize.x - 1; x++)
            {
                for (int y = 1; y < mapSize.y - 1; y++)
                {
                    // 只检查有tile的位置
                    if (!groundMap[x, y]) continue;

                    // 检查是否符合消除规则：两个空位置中间的tile
                    if (ShouldEliminate(x, y))
                    {
                        newMap[x, y] = false;
                        eliminatedCount++;
                    }
                }
            }

            groundMap = newMap;
            RenderMap();

            Debug.Log($"消除迭代完成，消除 {eliminatedCount} 个tile，当前填充率: {CalculateFillRate():P2}");
        }

        [ContextMenu("清空全部tile")]
        public void ClearAllTiles()
        {
            if (map == null) return;

            for (int x = 0; x < mapSize.x - 1; x++)
            {
                for (int y = 0; y < mapSize.y - 1; y++)
                {
                    Vector3Int tilePosition = new Vector3Int(x + mapOffset.x, y + mapOffset.y, 0);
                    map.SetTile(tilePosition, null);
                }
            }
            if (groundMap != null)
            {
                System.Array.Clear(groundMap, 0, groundMap.Length);
            }

            Debug.Log("已清空全部tile");
        }

        // 检查是否应该填补位置 (x, y)
        private bool ShouldFill(int x, int y)
        {
            // 检查水平方向：左边和右边都有tile
            if (x > 0 && x < mapSize.x - 1 && y > 0 && y < mapSize.y - 1)
            {
                if (groundMap[x - 1, y] && groundMap[x + 1, y])
                    return true;
                if (groundMap[x, y - 1] && groundMap[x, y + 1])
                    return true;
            }

            return false;
        }

        // 检查是否应该消除位置 (x, y)
        private bool ShouldEliminate(int x, int y)
        {
            // 检查水平方向：左右都是空
            if (x > 0 && x < mapSize.x - 1 && y > 0 && y < mapSize.y - 1)
            {
                if ((!groundMap[x - 1, y] && !groundMap[x + 1, y]) || (!groundMap[x, y - 1] && !groundMap[x, y + 1]))
                    return true;
            }

            return false;
        }

        // 渲染地图到Tilemap
        private void RenderMap()
        {
            if (map == null || tile == null) return;

            for (int x = 0; x < mapSize.x - 1; x++)
            {
                for (int y = 0; y < mapSize.y - 1; y++)
                {
                    Vector3Int tilePosition = new Vector3Int(x + mapOffset.x, y + mapOffset.y, 0);                
                    if (groundMap[x, y])
                    {
                        // 计算世界坐标位置              
                        map.SetTile(tilePosition, tile);
                    }
                    else
                    {
                        map.SetTile(tilePosition, null);
                    }
                }
            }
        }

        // 计算当前填充率（用于调试）
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

        // 公共方法：获取地图尺寸和偏移
        public Vector2Int GetMapSize() => mapSize;
        public Vector2Int GetMapOffset() => mapOffset;

        // 公共方法：更新BoxCollider2D边界（如果运行时修改了Collider大小）
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
        // 编辑器辅助功能：可视化BoxCollider2D边界
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