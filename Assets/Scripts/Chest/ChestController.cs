using System.Collections.Generic;
using ChestSystem.Chest;
using UnityEngine;

public class ChestController
{
    private List<ChestScriptableObject> chests;
    private ChestPool chestPool;
    private List<ChestView> activeChests = new List<ChestView>();
    private int maxChestSlots = 4;

    public ChestController(List<ChestScriptableObject> chests, ChestPool chestPool)
    {
        this.chests = chests;
        this.chestPool = chestPool;
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
        ChestView chest = chestPool.GetChest();
        chest.gameObject.SetActive(true);
        chest.Initialize(chestData);
        activeChests.Add(chest);

        Debug.Log($"Spawned chest: {chestData.chestType}, Active chests: {activeChests.Count}");
    }

    public void RemoveChest(ChestView chest)
    {
        if (activeChests.Contains(chest))
        {
            activeChests.Remove(chest);
            chestPool.ReturnChestToPool(chest);
        }
    }
}