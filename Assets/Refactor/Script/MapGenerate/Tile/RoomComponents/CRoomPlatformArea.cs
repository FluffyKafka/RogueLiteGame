using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Tilemaps;

namespace MapGenerate
{
    internal class CRoomPlatformArea : MonoBehaviour, IRoomGeneratorComponents
    {
        [Header("Transform References")]
        public Transform entryPoint;      // 入口标记（矩形的一个对角点）
        public Transform exitPoint;       // 出口标记（矩形的另一个对角点）

        [Header("Tilemap Settings")]
        public Tilemap targetTilemap;     // 要生成平台的Tilemap组件
        public TileBase platformTile;     // 平台使用的Tile

        [Header("Direction Probability")]
        [SerializeField, Range(0f, 1f)]
        private float upProbability = 0.2f;  // 向上的概率，取值范围0-1
        [SerializeField, Range(0f, 1f)]
        private float oneStepProbability = 0.5f;  // 垂直移动1格的概率，取值范围0-1
        [Header("Platform Settings")]
        [SerializeField, Range(1, 10)]
        private int minPlatformLength = 2;  // 最小平台长度（连续格子数）

        private BoundsInt regionBounds;    // 矩形区域边界（tile坐标）
        private Vector2Int minTile;        // 区域最小tile坐标
        private Vector2Int maxTile;        // 区域最大tile坐标
        private int width;                 // 区域宽度（tile数）
        private int height;                // 区域高度（tile数）
        private bool moveUp;               // true: 向上移动, false: 向下移动

        public void Generate()
        {
            GeneratePathPlatforms();
        }

        [ContextMenu("Generate Path Platforms")]
        public void GeneratePathPlatforms()
        {
            ClearRegionTiles();

            if (!ValidateSetup())
                return;

            // 1. 计算矩形区域内的所有tile坐标，构建矩阵
            CalculateRegionBounds();

            // 2. 确定移动方向
            DetermineMoveDirection();

            // 3. 生成从入口到出口的随机路径（tile坐标列表）
            List<Vector2Int> path = GenerateRandomPath();

            if (path == null || path.Count == 0)
            {
                Debug.LogWarning("路径生成失败，未放置任何平台。");
                return;
            }

            // 4. 确保平台满足最小长度要求
            List<Vector2Int> extendedPath = EnsureMinPlatformLength(path);

            if (extendedPath == null || extendedPath.Count == 0)
            {
                Debug.LogWarning("平台长度扩展失败，未放置任何平台。");
                return;
            }

            // 5. 在路径上放置平台tile
            PlacePlatformsOnPath(extendedPath);

            Debug.Log($"成功生成路径，共放置 {extendedPath.Count} 个平台tile。");
        }

        private bool ValidateSetup()
        {
            if (entryPoint == null || exitPoint == null)
            {
                Debug.LogError("入口或出口Transform未赋值！");
                return false;
            }

            if (targetTilemap == null)
            {
                Debug.LogError("目标Tilemap未赋值！");
                return false;
            }

            if (platformTile == null)
            {
                Debug.LogError("平台Tile未赋值！");
                return false;
            }

            if (minPlatformLength < 1)
            {
                Debug.LogWarning("最小平台长度设置为1，将不进行扩展。");
            }

            return true;
        }
        private void CalculateRegionBounds()
        {
            // 获取世界坐标
            Vector3 worldPosEntry = entryPoint.position;
            Vector3 worldPosExit = exitPoint.position;

            // 将世界坐标转换为tile坐标（假设Tilemap的CellSize为1，且原点对齐）
            Vector3Int tileEntry = targetTilemap.WorldToCell(worldPosEntry);
            Vector3Int tileExit = targetTilemap.WorldToCell(worldPosExit);

            // 计算矩形区域的最小和最大tile坐标（X和Y维度，忽略Z）
            int minX = Mathf.Min(tileEntry.x, tileExit.x);
            int minY = Mathf.Min(tileEntry.y, tileExit.y);
            int maxX = Mathf.Max(tileEntry.x, tileExit.x);
            int maxY = Mathf.Max(tileEntry.y, tileExit.y);

            minTile = new Vector2Int(minX, minY);
            maxTile = new Vector2Int(maxX, maxY);

            width = maxX - minX + 1;
            height = maxY - minY + 1;

            // 记录BoundsInt用于可能的可视化或其他用途
            regionBounds = new BoundsInt(minX, minY, 0, width, height, 1);

            Debug.Log($"矩形区域: 从 {minTile} 到 {maxTile}，宽度={width}，高度={height}");
        }
        private void DetermineMoveDirection()
        {
            Vector3Int entryTileAbs = targetTilemap.WorldToCell(entryPoint.position);
            Vector3Int exitTileAbs = targetTilemap.WorldToCell(exitPoint.position);

            if (exitTileAbs.y > entryTileAbs.y)
            {
                moveUp = true;
                Debug.Log("出口在入口上方，将向上生成路径（每次移动1或2格，移动2格时中间格子为空）");
            }
            else if (exitTileAbs.y < entryTileAbs.y)
            {
                moveUp = false;
                Debug.Log("出口在入口下方，将向下生成路径（每次移动1或2格，移动2格时中间格子为空）");
            }
            else
            {
                // 在同一水平线上，默认向上
                moveUp = true;
                Debug.Log("入口和出口在同一水平线上，默认向上生成路径（每次移动1或2格，移动2格时中间格子为空）");
            }
        }
        private bool IsVerticalStepValid(Vector2Int start, Vector2Int direction, int steps)
        {
            Vector2Int currentPos = start;
            for (int i = 1; i <= steps; i++)
            {
                currentPos = start + direction * i;
                if (!IsWithinBounds(currentPos))
                    return false;
            }
            return true;
        }
        private bool IsVerticalStepFree(Vector2Int start, Vector2Int direction, int steps, HashSet<Vector2Int> occupied)
        {
            // 只检查终点位置，中间格子不需要检查是否被占用
            Vector2Int endPos = start + direction * steps;
            return !occupied.Contains(endPos);
        }
        private int GetVerticalSteps(Vector2Int current, Vector2Int exit)
        {
            // 计算到出口的垂直距离
            int verticalDistance;
            if (moveUp)
                verticalDistance = exit.y - current.y;
            else
                verticalDistance = current.y - exit.y;

            // 如果距离为1，只能移动1格
            if (verticalDistance == 1)
                return 1;

            // 如果距离大于等于2，根据概率随机选择1或2格
            if (Random.value < oneStepProbability)
                return 1;
            else
                return 2;
        }
        private List<Vector2Int> GenerateRandomPath()
        {
            // 获取入口和出口的tile坐标
            Vector3Int entryTileAbs = targetTilemap.WorldToCell(entryPoint.position);
            Vector3Int exitTileAbs = targetTilemap.WorldToCell(exitPoint.position);
            Vector2Int entry = new Vector2Int(entryTileAbs.x, entryTileAbs.y);
            Vector2Int exit = new Vector2Int(exitTileAbs.x, exitTileAbs.y);

            // 确保入口和出口都在矩形区域内
            if (!IsWithinBounds(entry) || !IsWithinBounds(exit))
            {
                Debug.LogError("入口或出口的tile坐标不在计算出的矩形区域内！请检查Transform位置。");
                return null;
            }

            // 路径集合（使用HashSet快速判断是否已占用）
            HashSet<Vector2Int> pathSet = new HashSet<Vector2Int>();
            List<Vector2Int> pathList = new List<Vector2Int>();

            Vector2Int current = entry;
            pathSet.Add(current);
            pathList.Add(current);

            int maxAttempts = width * height * 2; // 防止无限循环
            int attempts = 0;

            // 直到当前点到达出口
            while (current != exit && attempts < maxAttempts)
            {
                Vector2Int next;

                // 如果与出口在同一高度，直接向出口水平移动
                if (current.y == exit.y)
                {
                    // 确定水平移动方向
                    if (current.x < exit.x)
                        next = current + Vector2Int.right;
                    else if (current.x > exit.x)
                        next = current + Vector2Int.left;
                    else
                        next = current; // 理论上不应该执行到这里

                    // 检查下一个位置是否在边界内且未被占用
                    if (IsWithinBounds(next) && !pathSet.Contains(next))
                    {
                        current = next;
                        pathSet.Add(current);
                        pathList.Add(current);
                        attempts++;
                        continue;
                    }
                    else
                    {
                        // 如果直接向出口移动被阻塞，则选择其他方向
                        Debug.Log($"向出口水平移动被阻塞，尝试其他方向。当前位置: {current}, 目标位置: {next}");
                        next = GetWeightedRandomNeighbor(current, exit, pathSet);

                        if (next == current) // 无有效邻居
                        {
                            Debug.LogWarning($"无法找到有效移动方向，路径生成失败。当前位置: {current}");
                            return null;
                        }

                        current = next;
                        pathSet.Add(current);
                        pathList.Add(current);
                        attempts++;
                        continue;
                    }
                }

                // 不在同一高度时，正常获取随机邻居
                Vector2Int moveResult = GetWeightedRandomNeighbor(current, exit, pathSet);

                if (moveResult == current) // 无有效邻居
                {
                    Debug.LogWarning($"无法找到有效移动方向，路径生成失败。当前位置: {current}");
                    return null;
                }

                // 检查是否是垂直移动2格
                Vector2Int verticalDirection = moveUp ? Vector2Int.up : Vector2Int.down;
                Vector2Int targetPos = moveResult;

                // 判断移动了多少格
                int verticalStepsMoved = Mathf.Abs(targetPos.y - current.y);

                current = targetPos;
                pathSet.Add(current);
                pathList.Add(current);

                attempts++;
            }

            if (current != exit)
            {
                Debug.LogWarning($"达到最大尝试次数({maxAttempts})，未能到达出口。");
                return null;
            }

            Debug.Log($"路径生成成功，长度: {pathList.Count}");
            return pathList;
        }
        private bool IsWithinBounds(Vector2Int pos)
        {
            return pos.x >= minTile.x && pos.x <= maxTile.x &&
                   pos.y >= minTile.y && pos.y <= maxTile.y;
        }
        private Vector2Int GetWeightedRandomNeighbor(Vector2Int current, Vector2Int exit, HashSet<Vector2Int> occupied)
        {
            // 定义可能的移动
            List<Vector2Int> possibleMoves = new List<Vector2Int>();
            List<float> probabilities = new List<float>();

            // 垂直方向（可能移动1或2格）
            Vector2Int verticalDirection = moveUp ? Vector2Int.up : Vector2Int.down;

            // 获取垂直移动的步数
            int verticalSteps = GetVerticalSteps(current, exit);

            // 检查垂直移动的目标位置是否有效（只检查终点，中间格子不检查）
            if (IsVerticalStepValid(current, verticalDirection, verticalSteps) &&
                IsVerticalStepFree(current, verticalDirection, verticalSteps, occupied))
            {
                // 返回目标位置（如果是移动2格，直接返回2格后的位置，中间格子不添加）
                Vector2Int targetPos = current + verticalDirection * verticalSteps;
                possibleMoves.Add(targetPos - current); // 存储方向向量
                probabilities.Add(upProbability);
            }

            // 水平方向（移动一格）
            Vector2Int leftMove = Vector2Int.left;
            Vector2Int rightMove = Vector2Int.right;

            float leftRightProbability = (1f - upProbability) / 2f;

            // 检查左移
            Vector2Int leftNeighbor = current + leftMove;
            if (IsWithinBounds(leftNeighbor) && !occupied.Contains(leftNeighbor))
            {
                possibleMoves.Add(leftMove);
                probabilities.Add(leftRightProbability);
            }

            // 检查右移
            Vector2Int rightNeighbor = current + rightMove;
            if (IsWithinBounds(rightNeighbor) && !occupied.Contains(rightNeighbor))
            {
                possibleMoves.Add(rightMove);
                probabilities.Add(leftRightProbability);
            }

            // 如果没有可行移动，返回自身（触发失败）
            if (possibleMoves.Count == 0)
                return current;

            // 根据概率权重随机选择移动方向
            Vector2Int selectedMove = GetRandomByProbability(possibleMoves, probabilities);

            // 返回移动后的位置
            return current + selectedMove;
        }
        private Vector2Int GetRandomByProbability(List<Vector2Int> items, List<float> probabilities)
        {
            // 计算总概率
            float totalProbability = 0f;
            foreach (float prob in probabilities)
            {
                totalProbability += prob;
            }

            // 归一化概率并随机选择
            float randomValue = Random.Range(0f, totalProbability);
            float cumulative = 0f;

            for (int i = 0; i < items.Count; i++)
            {
                cumulative += probabilities[i];
                if (randomValue <= cumulative)
                {
                    return items[i];
                }
            }

            // 保底返回第一个
            return items[0];
        }

        private List<Vector2Int> EnsureMinPlatformLength(List<Vector2Int> originalPath)
        {
            if (minPlatformLength <= 1 || originalPath == null || originalPath.Count < 2)
                return originalPath;

            // 分析路径，找出所有水平段
            List<List<Vector2Int>> horizontalSegments = new List<List<Vector2Int>>();
            List<Vector2Int> currentSegment = new List<Vector2Int>();

            for (int i = 0; i < originalPath.Count; i++)
            {
                // 添加当前点
                currentSegment.Add(originalPath[i]);

                // 检查是否需要结束当前段
                if (i < originalPath.Count - 1)
                {
                    // 如果下一个点的Y坐标不同，则结束当前水平段
                    if (originalPath[i + 1].y != originalPath[i].y)
                    {
                        horizontalSegments.Add(new List<Vector2Int>(currentSegment));
                        currentSegment.Clear();
                    }
                }
                else
                {
                    horizontalSegments.Add(new List<Vector2Int>(currentSegment));
                }
            }

            // 扩展每个水平段以达到最小长度
            HashSet<Vector2Int> extendedPathSet = new HashSet<Vector2Int>(originalPath);
            List<Vector2Int> extendedPath = new List<Vector2Int>(originalPath);

            foreach (var segment in horizontalSegments)
            {
                Debug.Log(segment.Count);
                if (segment.Count < minPlatformLength)
                {
                    // 需要扩展的格子数
                    int needed = minPlatformLength - segment.Count;
                    int startX = segment[0].x;
                    int endX = segment[segment.Count - 1].x;
                    int y = segment[0].y;

                    // 确定扩展方向（向左或向右扩展）
                    bool canExtendLeft = CanExtendHorizontal(startX - 1, y, needed, extendedPathSet);
                    bool canExtendRight = CanExtendHorizontalRight(endX + 1, y, needed, extendedPathSet);

                    // 根据区域宽度决定扩展方向
                    if (canExtendLeft && canExtendRight)
                    {
                        // 两边都可以扩展，随机选择或平均分配
                        if (Random.value < 0.5f)
                        {
                            ExtendHorizontalLeft(extendedPath, extendedPathSet, startX, y, needed);
                        }
                        else
                        {
                            ExtendHorizontalRight(extendedPath, extendedPathSet, endX, y, needed);
                        }
                    }
                    else if (canExtendLeft)
                    {
                        ExtendHorizontalLeft(extendedPath, extendedPathSet, startX, y, needed);
                    }
                    else if (canExtendRight)
                    {
                        ExtendHorizontalRight(extendedPath, extendedPathSet, endX, y, needed);
                    }
                    else
                    {
                        Debug.LogWarning($"无法扩展水平段从 ({startX}, {y}) 到 ({endX}, {y})，区域边界限制");
                    }
                }
            }

            // 对扩展后的路径进行排序（按x和y坐标排序，确保路径连续性）
            extendedPath.Sort((a, b) => {
                if (a.y != b.y)
                    return a.y.CompareTo(b.y);
                return a.x.CompareTo(b.x);
            });

            Debug.Log($"平台长度扩展完成，原路径长度: {originalPath.Count}, 扩展后长度: {extendedPath.Count}");
            return extendedPath;
        }
        private bool CanExtendHorizontal(int startX, int y, int count, HashSet<Vector2Int> existingPath)
        {
            for (int i = 0; i < count; i++)
            {
                Vector2Int pos = new Vector2Int(startX - i, y);
                if (!IsWithinBounds(pos) || existingPath.Contains(pos))
                    return false;
            }
            return true;
        }

        private bool CanExtendHorizontalRight(int startX, int y, int count, HashSet<Vector2Int> existingPath)
        {
            for (int i = 0; i < count; i++)
            {
                Vector2Int pos = new Vector2Int(startX + i, y);
                if (!IsWithinBounds(pos) || existingPath.Contains(pos))
                    return false;
            }
            return true;
        }

        private void ExtendHorizontalLeft(List<Vector2Int> path, HashSet<Vector2Int> pathSet, int startX, int y, int count)
        {
            for (int i = 1; i <= count; i++)
            {
                Vector2Int newPos = new Vector2Int(startX - i, y);
                if (IsWithinBounds(newPos) && !pathSet.Contains(newPos))
                {
                    path.Add(newPos);
                    pathSet.Add(newPos);
                }
            }
        }

        private void ExtendHorizontalRight(List<Vector2Int> path, HashSet<Vector2Int> pathSet, int endX, int y, int count)
        {
            for (int i = 1; i <= count; i++)
            {
                Vector2Int newPos = new Vector2Int(endX + i, y);
                if (IsWithinBounds(newPos) && !pathSet.Contains(newPos))
                {
                    path.Add(newPos);
                    pathSet.Add(newPos);
                }
            }
        }

        private void PlacePlatformsOnPath(List<Vector2Int> path)
        {
            // 可选：清空区域内的原有tile（如需重新生成，取消注释）
            // ClearRegionTiles();

            foreach (Vector2Int tilePos in path)
            {
                Vector3Int cellPos = new Vector3Int(tilePos.x, tilePos.y, 0);
                targetTilemap.SetTile(cellPos, platformTile);
            }
        }

        [ContextMenu("Clear Region Tiles")]
        public void ClearRegionTiles()
        {
            if (!ValidateSetup())
                return;

            CalculateRegionBounds(); // 确保bounds已计算

            for (int x = minTile.x; x <= maxTile.x; x++)
            {
                for (int y = minTile.y; y <= maxTile.y; y++)
                {
                    targetTilemap.SetTile(new Vector3Int(x, y, 0), null);
                }
            }
            Debug.Log("已清除矩形区域内所有tile。");
        }

        private void OnDrawGizmosSelected()
        {
            if (entryPoint == null || exitPoint == null) return;
            if (targetTilemap == null) return;

            // 计算矩形区域的世界坐标角点
            Vector3 worldEntry = entryPoint.position;
            Vector3 worldExit = exitPoint.position;

            Vector3 minWorld = new Vector3(Mathf.Min(worldEntry.x, worldExit.x), Mathf.Min(worldEntry.y, worldExit.y), 0);
            Vector3 maxWorld = new Vector3(Mathf.Max(worldEntry.x, worldExit.x), Mathf.Max(worldEntry.y, worldExit.y), 0);

            Vector3 size = maxWorld - minWorld;

            Gizmos.color = Color.green;
            Gizmos.DrawWireCube(minWorld + size * 0.5f, size);

            // 标记入口和出口
            Gizmos.color = Color.blue;
            Gizmos.DrawSphere(entryPoint.position, 0.3f);
            Gizmos.color = Color.red;
            Gizmos.DrawSphere(exitPoint.position, 0.3f);

            // 在入口和出口之间绘制方向提示线
            Gizmos.color = Color.yellow;
            Gizmos.DrawLine(entryPoint.position, exitPoint.position);
        }
    }
}

