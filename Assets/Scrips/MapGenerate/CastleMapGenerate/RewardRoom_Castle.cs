using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RewardRoom_Castle : Room_Castle
{
    [SerializeField] protected List<Transform> rewardTransformList;
    public override void GenerateRoom(MapGenerater_Castle _generater)
    {
        base.GenerateRoom(_generater);
        generateReward();
    }

    public void generateReward()
    {
        RewardSlot_Castle slot = generater.rewards[Random.Range(0, generater.rewards.Count)];
        generater.rewards.Remove(slot);

        int rewardTime = UnityEngine.Random.Range(slot.minRewardTime, slot.maxRewardTime);
        for (int i = 0; i < rewardTime; ++i)
        {
            Transform _rewardTransform = rewardTransformList[i];
            float dice = UnityEngine.Random.Range(0, 100);
            float rate = 0;
            if (dice < (rate += slot.witcherRate))
            {
                Instantiate(generater.witcherPrefab, _rewardTransform.position, Quaternion.identity);
                slot.witcherRate = 0;
            }
            else if (dice < (rate += slot.traderRate))
            {
                Instantiate(generater.traderPrefab, _rewardTransform.position, Quaternion.identity);
                slot.traderRate = 0;
            }
            else if (dice < (rate += slot.blackSmithRate))
            {
                Instantiate(generater.blackSmithPrefab, _rewardTransform.position, Quaternion.identity);
                slot.blackSmithRate = 0;
            }
            else
            {
                GenerateRewardBox(slot, _rewardTransform);
            }
        }
    }
    private void GenerateRewardBox(RewardSlot_Castle _slot, Transform _rewardTransform)
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
                        generater.advancedRewardChestPrefab, _rewardTransform.position, Quaternion.identity
                    ).GetComponent<Chest>();
                newChest.SetDrops(drops);
            }
            else
            {
                Chest newChest =
                    Instantiate(
                        generater.primaryRewardChestPrefab, _rewardTransform.position, Quaternion.identity
                    ).GetComponent<Chest>();
                newChest.SetDrops(drops);
            }
        }
    }
    private void GenerateMimic(RewardSlot_Castle _slot, List<Drop> _drops, Transform _rewardTransform)
    {
        _drops.AddRange(GetPrimaryRewards(_slot.rewardAmount));
        if (UnityEngine.Random.Range(0f, 100f) < _slot.mimicAdvancedRewardRate)
        {
            _drops.AddRange(GetAdvancedRewards(_slot.advancedAmount));
        }

        Enemy_Mimic newMimic =
            Instantiate(
                generater.mimicChestPrefab, _rewardTransform.position, Quaternion.identity
            ).GetComponent<Enemy_Mimic>();
        newMimic.SetDrops(_drops);
    }

    public List<Drop> GetPrimaryRewards(int _amount)
    {
        return GetRewards(_amount, generater.primaryRewards);
    }

    public List<Drop> GetAdvancedRewards(int _amount)
    {
        return GetRewards(_amount, generater.advancedRewards);
    }

    private List<Drop> GetRewards(int _amount, List<Drop> _rewardPool)
    {
        float sumWeight = 0f;
        foreach (var item in _rewardPool)
        {
            sumWeight += item.dropChance;
        }

        List<Drop> rewards = new List<Drop>();
        for (int i = 0; i < _amount; i++)
        {
            float dice = UnityEngine.Random.Range(0f, sumWeight);
            float rate = 0f;
            foreach (var item in _rewardPool)
            {
                rate += item.dropChance;
                if (dice < rate)
                {
                    rewards.Add(item);
                    break;
                }
            }
        }

        foreach (var item in rewards)
        {
            item.dropChance = 100f;
        }

        return rewards;
    }
}
