using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RewardRoom_OutCastle : Room_OutCastle
{
    [SerializeField] private List<Transform> rewardTransform;

    protected override void PreGenerateRoom(MapGenerateManager_OutCastle _manager, Line_OutCastle _currentLine, int _index)
    {
        type = RoomType_OutCastle.Reward;

        base.PreGenerateRoom(_manager, _currentLine, _index);
    }

    protected override RoomGenerateStruct_OutCastle GenerateCurrentRoom(MapGenerateManager_OutCastle _manager, Line_OutCastle _currentLine, int _index)
    {
        base.GenerateCurrentRoom(_manager, _currentLine, _index);

        RewardSlot_OutCastle slot = _currentLine.lineEndReward;
        if (!_currentLine.isEndRoom)
        {
            slot = _currentLine.rewards[_currentLine.rewardIndex];
        }

        _manager.GenerateRewardBySlot(slot, rewardTransform);
        return new RoomGenerateStruct_OutCastle(-1, null, null);
    }
}
