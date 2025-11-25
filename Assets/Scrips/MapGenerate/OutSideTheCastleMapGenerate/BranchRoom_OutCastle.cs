using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class BranchRoom_OutCastle : Room_OutCastle
{
    [SerializeField] private Door door;
    private bool isBranch = false;

    private void Awake()
    {
        door = GetComponentInChildren<Door>();
    }

    public void SetupBranch(Door _door)
    {
        isBranch = true;
        door.otherDoor = _door.transform;
    }

    public Transform GetBranchDoorTransform()
    {
        return door.transform;
    }

    protected override RoomGenerateStruct_OutCastle GenerateCurrentRoom(MapGenerateManager_OutCastle _manager, Line_OutCastle _currentLine, int _index)
    {
        if(!isBranch)
        {
            GameObject randomBranchRoomPrefab =
                _manager.branchRoomPrefabs[Random.Range(0, _manager.branchRoomPrefabs.Count)];
            Vector3 newBranchPosition = 
                transform.position + new Vector3(0, _manager.branchYOffset * (_currentLine.branchIndex + 1));

            BranchRoom_OutCastle newBranchRoom =
                Instantiate(randomBranchRoomPrefab, newBranchPosition, Quaternion.identity).GetComponent<BranchRoom_OutCastle>();

            newBranchRoom.SetupBranch(door);
            door.otherDoor = newBranchRoom.GetBranchDoorTransform();

            Line_OutCastle newBranchLine = null;
            if (_currentLine.branches[_currentLine.branchIndex].isRandom)
            {
                newBranchLine = _manager.branchLines[Random.Range(0, _manager.branchLines.Count)].GetClone();
            }
            else
            {
                newBranchLine = _currentLine.branches[_currentLine.branchIndex].branchLine.GetClone();
            }
            newBranchLine.lineStartDoor = door;

            return new RoomGenerateStruct_OutCastle(0, newBranchRoom, newBranchLine);
        }
        return new RoomGenerateStruct_OutCastle(-1, null, null);
    }

    protected override RoomGenerateStruct_OutCastle GenerateNextRoom(MapGenerateManager_OutCastle _manager, Line_OutCastle _currentLine, int _index)
    {
        return base.GenerateNextRoom(_manager, _currentLine, _index);
    }

    protected override void PreGenerateRoom(MapGenerateManager_OutCastle _manager, Line_OutCastle _currentLine, int _index)
    {
        type = RoomType_OutCastle.Branch;

        base.PreGenerateRoom(_manager, _currentLine, _index);
    }
}
