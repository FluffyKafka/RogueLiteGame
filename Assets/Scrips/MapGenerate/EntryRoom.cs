using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EntryRoom : Room
{
    protected override void PreGenerateRoom(MapGenerateManager _manager, Line _currentLine, int _index)
    {
        type = RoomType.Entry;

        base.PreGenerateRoom(_manager, _currentLine, _index);
    }

    protected override RoomGenerateStruct GenerateCurrentRoom(MapGenerateManager _manager, Line _currentLine, int _index)
    {
        return base.GenerateCurrentRoom(_manager, _currentLine, _index);
    }
}
