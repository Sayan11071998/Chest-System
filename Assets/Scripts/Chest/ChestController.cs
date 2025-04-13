using System.Collections.Generic;
using ChestSystem.Chest;
using ChestSystem.Slot;
using ChestSystem.UI;
using UnityEngine;

public class ChestController
{
    private List<ChestScriptableObject> chests;
    private ChestPool chestPool;
    private EmptySlotPool emptySlotPool;
    private List<ChestView> activeChests = new List<ChestView>();
    private List<EmptySlotView> activeEmptySlots = new List<EmptySlotView>();
    private int maxChestSlots;
    private Transform chestParent;
    private ChestView currentlyUnlockingChest = null;

    public ChestController(List<ChestScriptableObject> chests, ChestPool chestPool, EmptySlotPool emptySlotPool, Transform chestParent, int initialMaxChestSlots)
    {
        this.chests = chests;
        this.chestPool = chestPool;
        this.emptySlotPool = emptySlotPool;
        this.chestParent = chestParent;
        this.maxChestSlots = initialMaxChestSlots;

        CreateInitialEmptySlots();
    }

    private void CreateInitialEmptySlots()
    {
        for (int i = 0; i < maxChestSlots; i++)
            CreateEmptySlot();
    }

    private void CreateEmptySlot()
    {
        EmptySlotView emptySlot = emptySlotPool.GetEmptySlot();
        emptySlot.gameObject.SetActive(true);
        emptySlot.Initialize();

        int lastIndex = chestParent.childCount - 1;
        emptySlot.transform.SetSiblingIndex(lastIndex);
        activeEmptySlots.Add(emptySlot);

        Debug.Log($"Created empty slot. Total slots: {activeEmptySlots.Count}");
    }

    public void IncreaseMaxChestSlots(int amountToIncrease)
    {
        maxChestSlots += amountToIncrease;

        for (int i = 0; i < amountToIncrease; i++)
            CreateEmptySlot();

        Debug.Log($"Max chest slots increased to {maxChestSlots}");
    }

    public void GenerateRandomChest()
    {
        if (chests == null || chests.Count == 0) return;

        if (activeChests.Count >= maxChestSlots)
        {
            Debug.Log("All chest slots are full!");
            return;
        }

        int totalChestGenerationChance = 0;
        foreach (var chestItem in chests)
            totalChestGenerationChance += chestItem.chestGenerationChance;

        int randomValue = Random.Range(0, totalChestGenerationChance);
        foreach (var chestItem in chests)
        {
            if (randomValue < chestItem.chestGenerationChance)
            {
                SpawnChest(chestItem);
                return;
            }
            randomValue -= chestItem.chestGenerationChance;
        }
    }

    private void SpawnChest(ChestScriptableObject chestData)
    {
        if (activeEmptySlots.Count > 0)
        {
            EmptySlotView emptySlot = activeEmptySlots[0];
            activeEmptySlots.RemoveAt(0);

            int siblingIndex = emptySlot.transform.GetSiblingIndex();
            emptySlotPool.ReturnEmptySlotToPool(emptySlot);

            ChestView chest = chestPool.GetChest();
            chest.gameObject.SetActive(true);
            chest.Initialize(chestData);

            chest.transform.SetSiblingIndex(siblingIndex);
            activeChests.Add(chest);

            Debug.Log($"Spawned chest: {chestData.chestType}, Active chests: {activeChests.Count}");
        }
    }

    public bool CanStartUnlocking() => currentlyUnlockingChest == null;

    public void SetUnlockingChest(ChestView chest) => currentlyUnlockingChest = chest;

    public void ChestUnlockCompleted(ChestView chest)
    {
        if (currentlyUnlockingChest == chest)
            currentlyUnlockingChest = null;
    }

    public void CollectChest(ChestView chest, out int coinsAwarded, out int gemsAwarded)
    {
        coinsAwarded = 0;
        gemsAwarded = 0;

        if (activeChests.Contains(chest))
        {
            ChestScriptableObject chestData = GetChestData(chest);
            if (chestData != null)
            {
                coinsAwarded = Random.Range(chestData.minCoinReward, chestData.maxCoinReward + 1);
                gemsAwarded = Random.Range(chestData.minGemReward, chestData.maxGemReward + 1);
            }

            if (currentlyUnlockingChest == chest)
                currentlyUnlockingChest = null;

            RemoveChest(chest);
        }
    }

    private ChestScriptableObject GetChestData(ChestView chest)
    {
        foreach (var chestData in chests)
        {
            if (chestData.chestType.ToString() == chest.name)
                return chestData;
        }
        return null;
    }

    public void RemoveChest(ChestView chest)
    {
        if (activeChests.Contains(chest))
        {
            int siblingIndex = chest.transform.GetSiblingIndex();
            activeChests.Remove(chest);
            chestPool.ReturnChestToPool(chest);

            EmptySlotView emptySlot = emptySlotPool.GetEmptySlot();
            emptySlot.gameObject.SetActive(true);
            emptySlot.Initialize();
            emptySlot.transform.SetSiblingIndex(siblingIndex);
            activeEmptySlots.Add(emptySlot);
        }
    }
}