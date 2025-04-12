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

    public ChestController(List<ChestScriptableObject> chests, ChestPool chestPool, EmptySlotPool emptySlotPool, int initialMaxChestSlots)
    {
        this.chests = chests;
        this.chestPool = chestPool;
        this.emptySlotPool = emptySlotPool;
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
            emptySlotPool.ReturnEmptySlotToPool(emptySlot);

            ChestView chest = chestPool.GetChest();
            chest.gameObject.SetActive(true);
            chest.Initialize(chestData);
            chest.transform.SetSiblingIndex(emptySlot.transform.GetSiblingIndex());
            activeChests.Add(chest);

            Debug.Log($"Spawned chest: {chestData.chestType}, Active chests: {activeChests.Count}");
        }
    }

    public void RemoveChest(ChestView chest)
    {
        if (activeChests.Contains(chest))
        {
            activeChests.Remove(chest);
            chestPool.ReturnChestToPool(chest);

            CreateEmptySlot();
        }
    }
}