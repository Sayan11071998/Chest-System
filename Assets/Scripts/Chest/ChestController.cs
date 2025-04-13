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

    private ChestSlotManager slotManager;
    private ChestGenerator chestGenerator;
    private ChestRewardCalculator rewardCalculator;

    public ChestController(List<ChestScriptableObject> chests, ChestPool chestPool, EmptySlotPool emptySlotPool, Transform chestParent, int initialMaxChestSlots)
    {
        this.chests = chests;
        this.chestPool = chestPool;
        this.emptySlotPool = emptySlotPool;
        this.chestParent = chestParent;
        this.maxChestSlots = initialMaxChestSlots;

        slotManager = new ChestSlotManager(this, emptySlotPool, chestParent);
        chestGenerator = new ChestGenerator(this, chests, chestPool);
        rewardCalculator = new ChestRewardCalculator(chests);

        slotManager.CreateInitialEmptySlots(initialMaxChestSlots);
    }

    public List<ChestView> ActiveChests => activeChests;
    public List<EmptySlotView> ActiveEmptySlots => activeEmptySlots;
    public int MaxChestSlots => maxChestSlots;
    public ChestSlotManager SlotManager => slotManager;

    public void IncreaseMaxChestSlots(int amountToIncrease)
    {
        maxChestSlots += amountToIncrease;
        slotManager.AddNewEmptySlots(amountToIncrease);
        Debug.Log($"Max chest slots increased to {maxChestSlots}");
    }

    public void GenerateRandomChest()
    {
        if (activeChests.Count >= maxChestSlots)
        {
            Debug.Log("All chest slots are full!");
            return;
        }

        chestGenerator.GenerateRandomChest();
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
            rewardCalculator.CalculateRewards(chest, out coinsAwarded, out gemsAwarded);

            if (currentlyUnlockingChest == chest)
                currentlyUnlockingChest = null;

            slotManager.ReplaceChestWithEmptySlot(chest);
        }
    }

    public void AddChest(ChestView chest) => activeChests.Add(chest);

    public void RemoveChest(ChestView chest)
    {
        activeChests.Remove(chest);
        chestPool.ReturnChestToPool(chest);
    }

    public void AddEmptySlot(EmptySlotView slot) => activeEmptySlots.Add(slot);
    public void RemoveEmptySlot(EmptySlotView slot) => activeEmptySlots.Remove(slot);
}