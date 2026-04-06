using UnityEngine;
using UnityEngine.Tilemaps;
using System.Collections.Generic;

namespace MapGenerate
{
    internal class TRoomDrunken : MonoBehaviour
    {
        [SerializeField] protected Tilemap map;
        [SerializeField] protected TileBase tile;
        [SerializeField] protected BoxCollider2D boundsCollider;

        [Header("醉汉算法参数")]
        [SerializeField] protected int walkersCount = 5;      // 醉汉数量
        [SerializeField] protected int stepsPerWalker = 100;  // 每个醉汉步数
        [SerializeField] protected int startPositions = 1;    // 起始点数量（用于多起点）

        protected BoundsInt roomBounds;
        protected Vector3Int origin;
        protected HashSet<Vector3Int> floorPositions = new HashSet<Vector3Int>();

        void Start()
        {
            GenerateDungeon();
        }

        [ContextMenu("生成地牢")]
        public void GenerateDungeon()
        {
            if (map == null || tile == null || boundsCollider == null)
            {
                Debug.LogError("缺少必要的组件引用！");
                return;
            }

            for(int x = 0; x < startPositions; ++x)
            {
                floorPositions.Clear();
                // 只清除框定区域内的Tile
                ClearTilesInBounds();

                // 获取碰撞器区域边界
                GetBoundsFromCollider();

                // 生成多个醉汉路径
                for (int i = 0; i < walkersCount; i++)
                {
                    DrunkenWalk();
                }

                // 将生成的位置绘制到Tilemap上
                foreach (Vector3Int pos in floorPositions)
                {
                    map.SetTile(pos, tile);
                }

                Debug.Log($"生成了 {floorPositions.Count} 个地板瓦片");
            }          
        }

        protected void GetBoundsFromCollider()
        {
            // 获取BoxCollider2D的世界坐标边界
            Bounds colliderBounds = boundsCollider.bounds;

            // 转换为Tilemap的格子坐标
            Vector3 min = colliderBounds.min;
            Vector3 max = colliderBounds.max;

            Vector3Int minCell = map.WorldToCell(min);
            Vector3Int maxCell = map.WorldToCell(max);

            // 创建整数边界
            roomBounds = new BoundsInt(minCell.x, minCell.y, minCell.z,
                                       maxCell.x - minCell.x + 1,
                                       maxCell.y - minCell.y + 1,
                                       maxCell.z - minCell.z + 1);

            origin = minCell;
        }

        protected void DrunkenWalk()
        {
            // 如果没有已生成的瓦片，从随机位置开始
            Vector3Int currentPos;

            if (floorPositions.Count == 0)
            {
                currentPos = GetRandomPositionInBounds();
                floorPositions.Add(currentPos);
                Debug.Log(currentPos);
            }
            else
            {
                List<Vector3Int> positions = new List<Vector3Int>(floorPositions);
                currentPos = positions[0];
            }

            // 随机行走
            for (int step = 0; step < stepsPerWalker; step++)
            {
                // 随机选择方向（上下左右）
                Vector3Int direction = GetRandomDirection();
                Vector3Int newPos = currentPos + direction;

                // 检查是否在边界内
                if (IsWithinBounds(newPos))
                {
                    currentPos = newPos;
                    floorPositions.Add(currentPos);
                }
                else
                {
                    // 如果走出边界，可以尝试其他方向或保持原位
                    // 这里简单处理：不移动，继续下一步
                    continue;
                }
            }
        }

        protected Vector3Int GetRandomDirection()
        {
            int dir = Random.Range(0, 4);
            switch (dir)
            {
                case 0: return Vector3Int.up;      // 上
                case 1: return Vector3Int.down;    // 下
                case 2: return Vector3Int.left;    // 左
                case 3: return Vector3Int.right;   // 右
                default: return Vector3Int.zero;
            }
        }

        protected Vector3Int GetRandomPositionInBounds()
        {
            int x = Random.Range(roomBounds.xMin, roomBounds.xMax);
            int y = Random.Range(roomBounds.yMin, roomBounds.yMax);
            return new Vector3Int(x, y, 0);
        }

        protected bool IsWithinBounds(Vector3Int pos)
        {
            return pos.x >= roomBounds.xMin && pos.x < roomBounds.xMax &&
                   pos.y >= roomBounds.yMin && pos.y < roomBounds.yMax;
        }

        [ContextMenu("清除框内所有Tile")]
        public void ClearTilesInBounds()
        {
            if (map == null || boundsCollider == null)
            {
                Debug.LogError("缺少必要的组件引用！");
                return;
            }

            // 获取边界
            GetBoundsFromCollider();

            // 清除floorPositions记录
            floorPositions.Clear();

            // 遍历边界内的所有格子并清除
            int clearedCount = 0;
            for (int x = roomBounds.xMin; x < roomBounds.xMax; x++)
            {
                for (int y = roomBounds.yMin; y < roomBounds.yMax; y++)
                {
                    Vector3Int pos = new Vector3Int(x, y, 0);
                    if (map.GetTile(pos) != null)
                    {
                        map.SetTile(pos, null);
                        clearedCount++;
                    }
                }
            }

            Debug.Log($"已清除框定区域内的 {clearedCount} 个瓦片");
        }

        // 可选：在编辑器中显示边界辅助线
        void OnDrawGizmosSelected()
        {
            if (boundsCollider != null)
            {
                Gizmos.color = Color.green;
                Bounds bounds = boundsCollider.bounds;
                Gizmos.DrawWireCube(bounds.center, bounds.size);
            }

            if (map != null && floorPositions != null && Application.isPlaying)
            {
                Gizmos.color = Color.yellow;
                foreach (Vector3Int pos in floorPositions)
                {
                    Vector3 worldPos = map.CellToWorld(pos);
                    Gizmos.DrawWireCube(worldPos + new Vector3(0.5f, 0.5f, 0), Vector3.one * 0.8f);
                }
            }
        }
    }
}