using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BranchExitRoom_OutCastle : Room_OutCastle
{
    [SerializeField] private Door door;
    private void Awake()
    {
        door = GetComponentInChildren<Door>();
    }

    protected override RoomGenerateStruct_OutCastle GenerateCurrentRoom(MapGenerateManager_OutCastle _manager, Line_OutCastle _currentLine, int _index)
    {
        door.otherDoor = _currentLine.lineStartDoor.transform;
        return new RoomGenerateStruct_OutCastle(-1, null, null);
    }

    protected override RoomGenerateStruct_OutCastle GenerateNextRoom(MapGenerateManager_OutCastle _manager, Line_OutCastle _currentLine, int _index)
    {
        return new RoomGenerateStruct_OutCastle(-1, null, null);
    }

    protected override void PreGenerateRoom(MapGenerateManager_OutCastle _manager, Line_OutCastle _currentLine, int _index)
    {
        type = RoomType_OutCastle.BranchExit;

        base.PreGenerateRoom(_manager, _currentLine, _index);
    }
}
