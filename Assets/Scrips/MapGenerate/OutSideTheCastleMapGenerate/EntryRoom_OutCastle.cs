using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EntryRoom_OutCastle : Room_OutCastle
{
    protected override void PreGenerateRoom(MapGenerateManager_OutCastle _manager, Line_OutCastle _currentLine, int _index)
    {
        type = RoomType_OutCastle.Entry;

        base.PreGenerateRoom(_manager, _currentLine, _index);
    }

    protected override RoomGenerateStruct_OutCastle GenerateCurrentRoom(MapGenerateManager_OutCastle _manager, Line_OutCastle _currentLine, int _index)
    {
        return base.GenerateCurrentRoom(_manager, _currentLine, _index);
    }
}
