using JetBrains.Annotations;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Security.Cryptography;
using System.Security.Cryptography.X509Certificates;
using Unity.VisualScripting;
using UnityEditor;
using UnityEngine;

public enum RoomType_OutCastle
{
    Entry,
    Exit,
    Battle,
    Passage,
    Reward,
    Branch,
    BranchExit
}

[Serializable]
public class Access_OutCastle
{
    public Transform transform;
    public bool canEnter = false;
    public bool canExit = false;
}

[Serializable]
public class LineSlot_OutCastle
{
    [Range(0, 100)][SerializeField] public float battleRoomRate;
    [Range(0, 100)][SerializeField] public float rewardRoomRate;
    [Range(0, 100)][SerializeField] public float branchRoomRate;
}

[Serializable]
public class BattleSlot_OutCastle
{
    [SerializeField] public float difficulty; 
}

[Serializable]
public class RewardSlot_OutCastle
{
    [SerializeField] public int minRewardTime = 1;
    [SerializeField] public int maxRewardTime = 3;//每个房间提供三个奖励生成位置
    [SerializeField] public int rewardAmount;
    [SerializeField] public int advancedAmount;
    [Range(0, 100)][SerializeField] public float witcherRate;
    [Range(0, 100)][SerializeField] public float traderRate;
    [Range(0, 100)][SerializeField] public float blackSmithRate;
    [Range(0, 100)][SerializeField] public float advancedRewardRate;
    [Range(0, 100)][SerializeField] public float mimicRate;
    [Range(0, 100)][SerializeField] public float mimicAdvancedRewardRate;
}

[Serializable]
public class BranchSlot_OutCastle
{
    [SerializeField] public bool isRandom;
    [SerializeField] public Line_OutCastle branchLine = null;
}

[Serializable]
public class Line_OutCastle
{
    public RoomType_OutCastle lineEndRoomType;
    [HideInInspector] public bool isEndRoom = false;
    [HideInInspector] public Door lineStartDoor = null;

    public int battleIndex { get; private set; }
    public int rewardIndex { get; private set; }
    public int branchIndex { get; private set; }

    [Header("Room Info")]
    public List<LineSlot_OutCastle> lineRooms;

    [Header("Battle Info")]
    public List<BattleSlot_OutCastle> battles;

    [Header("Reward Info")]
    public List<RewardSlot_OutCastle> rewards;
    public RewardSlot_OutCastle lineEndReward;

    public List<BranchSlot_OutCastle> branches;


    public Line_OutCastle()
    {
        
    }

    public Line_OutCastle GetClone()
    {
        Line_OutCastle newLine = new Line_OutCastle();

        newLine.lineEndRoomType = lineEndRoomType;
        newLine.lineRooms = lineRooms;
        newLine.battles = battles;
        newLine.rewards = rewards;
        newLine.lineEndReward = lineEndReward;
        newLine.branches = branches;

        return newLine;
    }

    public RoomType_OutCastle GetNextRoomType(int _currentRoomIndex, RoomType_OutCastle _currentRoomType)
    {
        if(isEndRoom)
        {
            return RoomType_OutCastle.BranchExit;
        }

        if(_currentRoomType == RoomType_OutCastle.Battle)
        {
            ++battleIndex;
        }
        if (_currentRoomType == RoomType_OutCastle.Reward)
        {
            ++rewardIndex;
        }
        if(_currentRoomType == RoomType_OutCastle.Branch)
        {
            ++branchIndex;
        }

        if (_currentRoomIndex >= lineRooms.Count)
        {
            if (branchIndex < branches.Count)
            {
                return RoomType_OutCastle.Branch;
            }
            if (battleIndex < battles.Count)
            {
                return RoomType_OutCastle.Battle;
            }
            if (rewardIndex < rewards.Count)
            {
                return RoomType_OutCastle.Reward;
            }
            isEndRoom = true;
            Debug.Log(_currentRoomType.ToString() + " " + lineEndRoomType.ToString());
            return lineEndRoomType;
        }

        if (branchIndex >= branches.Count)
        {
            lineRooms[_currentRoomIndex].branchRoomRate = 0;
        }
        if (battleIndex >= battles.Count)
        {
            lineRooms[_currentRoomIndex].battleRoomRate = 0;
        }
        if(rewardIndex >= rewards.Count)
        {
            lineRooms[_currentRoomIndex].rewardRoomRate = 0;
        }

        float rate = lineRooms[_currentRoomIndex].battleRoomRate;
        float dice = UnityEngine.Random.Range(0, 100);

        if (dice < rate)
        {
            return RoomType_OutCastle.Battle;
        }
        else if(dice < (rate += lineRooms[_currentRoomIndex].rewardRoomRate))
        {
            return RoomType_OutCastle.Reward;
        }
        else if(dice < (rate += lineRooms[_currentRoomIndex].branchRoomRate))
        {
            return RoomType_OutCastle.Branch;
        }
        else
        {
            return RoomType_OutCastle.Passage;
        }
    }
}

public struct RoomGenerateStruct_OutCastle
{
    public int index;
    public Room_OutCastle room;
    public Line_OutCastle line;
    public RoomGenerateStruct_OutCastle(int _index, Room_OutCastle _room, Line_OutCastle _line)
    {
        index = _index;
        room = _room;
        line = _line;
    }
}

//每个场景考虑使用自己的地图生成器
public class MapGenerateManager_OutCastle : MonoBehaviour
{
    [Header("Room Prefabs")]
    [SerializeField] public List<GameObject> entryRoomPrefabs;
    [SerializeField] public List<GameObject> exitRoomPrefabs;
    [SerializeField] public List<GameObject> battleRoomPrefabs;
    [SerializeField] public List<GameObject> passageRoomPrefabs;
    [SerializeField] public List<GameObject> rewardRoomPrefabs;
    [SerializeField] public List<GameObject> branchRoomPrefabs;
    [SerializeField] public List<GameObject> branchExitRoomPrefabs;

    [Header("Line info")]
    [SerializeField] public Line_OutCastle mainLine = new Line_OutCastle();
    [SerializeField] public List<Line_OutCastle> branchLines = new List<Line_OutCastle>();

    [Header("Map Info")]
    [SerializeField] private Transform startTransform;
    [SerializeField] public List<GameObject> enemyList;
    [SerializeField] public float enemyGenerateYOffset = 1f;

    [Header("Reward Info")]
    [SerializeField] private List<Drop> primaryRewards;
    [SerializeField] private List<Drop> advancedRewards;
    [SerializeField] public GameObject primaryRewardChestPrefab;
    [SerializeField] public GameObject advancedRewardChestPrefab;
    [SerializeField] public GameObject mimicChestPrefab;
    [SerializeField] public GameObject traderPrefab;
    [SerializeField] public GameObject blackSmithPrefab;
    [SerializeField] public GameObject witcherPrefab;
    public float advancedRewardPrice;

    [Header("Room Decoration Info")]
    [SerializeField] public GameObject decorationPrefab;
    [SerializeField] public List<Sprite> decorations;

    [Header("Branch Info")]
    [SerializeField] public float branchYOffset;


    private List<RoomGenerateStruct_OutCastle> roomsToGenerate;

    private void Start()
    {
        int startRoomIndex = UnityEngine.Random.Range(0, entryRoomPrefabs.Count);

        Room_OutCastle startRoomTemp = entryRoomPrefabs[startRoomIndex].GetComponent<Room_OutCastle>();
        Vector3 startRoomLoc = startTransform.position - startRoomTemp.leftAccess.transform.position;

        Room_OutCastle currentRoom = Instantiate(entryRoomPrefabs[startRoomIndex], startRoomLoc, Quaternion.identity).GetComponent<Room_OutCastle>();
        RoomGenerateStruct_OutCastle roomGenerateStruct = new RoomGenerateStruct_OutCastle(0, currentRoom, mainLine);
        roomsToGenerate = new List<RoomGenerateStruct_OutCastle>();
        roomsToGenerate.Add(roomGenerateStruct);
        while (roomsToGenerate.Count != 0)
        {
            RoomGenerateStruct_OutCastle newRoomStruct = roomsToGenerate[roomsToGenerate.Count - 1];
            roomsToGenerate.RemoveAt(roomsToGenerate.Count - 1);

            List<RoomGenerateStruct_OutCastle> rooms = newRoomStruct.room.GenerateRoom(this, newRoomStruct.line, newRoomStruct.index);
            foreach(RoomGenerateStruct_OutCastle roomStruct in rooms)
            {
                roomsToGenerate.Add(roomStruct);
            }
        }
        

        GameManager.instance.CheckPointLoad();
    }

    public List<Drop> GetPrimaryRewards(int _amount)
    {
        return GetRewards(_amount, primaryRewards);
    }

    public List<Drop> GetAdvancedRewards(int _amount)
    {
        return GetRewards(_amount, advancedRewards);
    }

    private List<Drop> GetRewards(int _amount, List<Drop> _rewardPool) 
    {
        float sumWeight = 0f;
        foreach(var item in _rewardPool)
        {
            sumWeight += item.dropChance;
        }

        List<Drop> rewards = new List<Drop>();
        for(int i = 0; i < _amount; i++)
        {
            float dice = UnityEngine.Random.Range(0f, sumWeight);
            float rate = 0f;
            foreach (var item in _rewardPool)
            {
                rate += item.dropChance;
                if(dice < rate)
                {
                    rewards.Add(item); 
                    break;
                }
            }
        }

        foreach(var item in rewards)
        {
            item.dropChance = 100f;
        }

        return rewards;
    }

    public void GenerateRewardBySlot(RewardSlot_OutCastle _slot, List<Transform> _rewardTransformList)
    {
        int rewardTime = UnityEngine.Random.Range(_slot.minRewardTime, _slot.maxRewardTime);
        for(int i = 0; i < rewardTime; ++i)
        {
            Transform _rewardTransform = _rewardTransformList[i];
            float dice = UnityEngine.Random.Range(0, 100);
            float rate = 0;
            if (dice < (rate += _slot.witcherRate))
            {
                Instantiate(witcherPrefab, _rewardTransform.position, Quaternion.identity);
                _slot.witcherRate = 0;
            }
            else if (dice < (rate += _slot.traderRate))
            {
                Instantiate(traderPrefab, _rewardTransform.position, Quaternion.identity);
                _slot.traderRate = 0;
            }
            else if (dice < (rate += _slot.blackSmithRate))
            {
                Instantiate(blackSmithPrefab, _rewardTransform.position, Quaternion.identity);
                _slot.blackSmithRate = 0;
            }
            else
            {
                GenerateRewardBox(_slot, _rewardTransform);
            }
        }
        
    }
    private void GenerateRewardBox(RewardSlot_OutCastle _slot, Transform _rewardTransform)
    {
        List<Drop> drops = new List<Drop>();
        if (UnityEngine.Random.Range(0f, 100f) < _slot.mimicRate)
        {
            GenerateMimic(_slot, drops, _rewardTransform);
        }
        else
        {
            drops.AddRange(GetPrimaryRewards(_slot.rewardAmount));
            if (UnityEngine.Random.Range(0f, 100f) < _slot.advancedRewardRate)
            {
                drops.AddRange(GetAdvancedRewards(_slot.advancedAmount));
                Chest newChest =
                    Instantiate(
                        advancedRewardChestPrefab, _rewardTransform.position, Quaternion.identity
                    ).GetComponent<Chest>();
                newChest.SetDrops(drops);
            }
            else
            {
                Chest newChest =
                    Instantiate(
                        primaryRewardChestPrefab, _rewardTransform.position, Quaternion.identity
                    ).GetComponent<Chest>();
                newChest.SetDrops(drops);
            }
        }
    }
    private void GenerateMimic(RewardSlot_OutCastle _slot, List<Drop> _drops, Transform _rewardTransform)
    {
        _drops.AddRange(GetPrimaryRewards(_slot.rewardAmount));
        if (UnityEngine.Random.Range(0f, 100f) < _slot.mimicAdvancedRewardRate)
        {
            _drops.AddRange(GetAdvancedRewards(_slot.advancedAmount));
        }

        Enemy_Mimic newMimic =
            Instantiate(
                mimicChestPrefab, _rewardTransform.position, Quaternion.identity
            ).GetComponent<Enemy_Mimic>();
        newMimic.SetDrops(_drops);
    }

#if UNITY_EDITOR
    [ContextMenu("Fill Up Rewards")]
    private void GetItemDatabase()
    {
        string[] assetNames = AssetDatabase.FindAssets("t:ItemData", new[] { "Assets/Scrips/Item/ItemData" });

        foreach (string SOName in assetNames)
        {
            var SOPath = AssetDatabase.GUIDToAssetPath(SOName);
            var itemData = AssetDatabase.LoadAssetAtPath<ItemData>(SOPath);

            if (itemData.price < advancedRewardPrice)
            {
                primaryRewards.Add(new Drop(itemData, 100 * (itemData.price / ((advancedRewardPrice / 2) + itemData.price))));
            }
            else
            {
                advancedRewards.Add(new Drop(itemData, 100 * (itemData.price / (advancedRewardPrice + itemData.price))));
            }
        }
    }
#endif
}
