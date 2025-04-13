using System;
using ChestSystem.Chest;

namespace ChestSystem.Events
{
    public static class ChestEvents
    {
        public static event Action<ChestView> OnChestSpawned;
        public static event Action<ChestView> OnChestUnlockStarted;
        public static event Action<ChestView> OnChestUnlockCompleted;
        public static event Action<ChestView, int, int> OnChestCollected;

        public static event Action<int> OnMaxSlotsIncreased;

        public static void ChestSpawned(ChestView chest) => OnChestSpawned?.Invoke(chest);
        public static void ChestUnlockStarted(ChestView chest) => OnChestUnlockStarted?.Invoke(chest);
        public static void ChestUnlockCompleted(ChestView chest) => OnChestUnlockCompleted?.Invoke(chest);
        public static void ChestCollected(ChestView chest, int coins, int gems) => OnChestCollected?.Invoke(chest, coins, gems);
        public static void MaxSlotsIncreased(int newMaxSlots) => OnMaxSlotsIncreased?.Invoke(newMaxSlots);
    }
}