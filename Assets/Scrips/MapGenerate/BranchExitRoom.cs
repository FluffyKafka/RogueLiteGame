using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BranchExitRoom : Room
{
    [SerializeField] private Door door;
    private void Awake()
    {
        door = GetComponentInChildren<Door>();
    }

    protected override RoomGenerateStruct GenerateCurrentRoom(MapGenerateManager _manager, Line _currentLine, int _index)
    {
        door.otherDoor = _currentLine.lineStartDoor.transform;
        return new RoomGenerateStruct(-1, null, null);
    }

    protected override RoomGenerateStruct GenerateNextRoom(MapGenerateManager _manager, Line _currentLine, int _index)
    {
        return new RoomGenerateStruct(-1, null, null);
    }

    protected override void PreGenerateRoom(MapGenerateManager _manager, Line _currentLine, int _index)
    {
        type = RoomType.BranchExit;

        base.PreGenerateRoom(_manager, _currentLine, _index);
    }
}
