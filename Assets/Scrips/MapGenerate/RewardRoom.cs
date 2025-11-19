using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RewardRoom : Room
{
    [SerializeField] private Transform rewardTransform;

    protected override void PreGenerateRoom(MapGenerateManager _manager, Line _currentLine, int _index)
    {
        type = RoomType.Reward;

        base.PreGenerateRoom(_manager, _currentLine, _index);
    }

    protected override RoomGenerateStruct GenerateCurrentRoom(MapGenerateManager _manager, Line _currentLine, int _index)
    {
        base.GenerateCurrentRoom(_manager, _currentLine, _index);

        RewardSlot slot = _currentLine.lineEndReward;
        if (!_currentLine.isEndRoom)
        {
            slot = _currentLine.rewards[_currentLine.rewardIndex];
        }

        _manager.GenerateRewardBySlot(slot, rewardTransform);
        return new RoomGenerateStruct(-1, null, null);
    }
}
