using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[Serializable]
public class AdditionalReward
{
    public List<Transform> rewardTransform;
    public RewardSlot_OutCastle slot;
}

public class BattleRoom_OutCastle : Room_OutCastle
{
    [SerializeField] private List<AdditionalReward> additionalRewards;

    protected override void PreGenerateRoom(MapGenerateManager_OutCastle _manager, Line_OutCastle _currentLine, int _index)
    {
        type = RoomType_OutCastle.Battle;

        base.PreGenerateRoom(_manager, _currentLine, _index);
    }

    protected override RoomGenerateStruct_OutCastle GenerateCurrentRoom(MapGenerateManager_OutCastle _manager, Line_OutCastle _currentLine, int _index)
    {
        base.GenerateCurrentRoom(_manager, _currentLine, _index);

        GenerateEnemy(
            _currentLine.battles[_currentLine.battleIndex].difficulty, 
            _manager.enemyList, 
            _manager.enemyGenerateYOffset
        );

        foreach(var reward in additionalRewards)
        {
            _manager.GenerateRewardBySlot(reward.slot, reward.rewardTransform);
        }

        return new RoomGenerateStruct_OutCastle(-1, null, null);
    }

    protected void GenerateEnemy(float _enemyDifficultyAmount, List<GameObject> _enemyList, float _enemyGenerateYOffset)
    {
        float currentEnemyDifficulty = 0;
        while(currentEnemyDifficulty < _enemyDifficultyAmount)
        {
            Vector2 randomPosition = flatPositions[UnityEngine.Random.Range(0, flatPositions.Count)];
            randomPosition = new Vector2(randomPosition.x, randomPosition.y + _enemyGenerateYOffset);
            GameObject randomEnemy = _enemyList[UnityEngine.Random.Range(0, _enemyList.Count)];

            Enemy newEnemy = 
                Instantiate(randomEnemy, randomPosition, Quaternion.identity).GetComponent<Enemy>();
            currentEnemyDifficulty += newEnemy.difficulty;
        }
    }
}
