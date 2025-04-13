using ChestSystem.Chest;

namespace ChestSystem.Events
{
    public class EventService
    {
        public ChestEventService ChestEvents { get; private set; }

        public EventService()
        {
            ChestEvents = new ChestEventService();
        }
    }

    public class ChestEventService
    {
        public GameEventController<ChestView> OnChestSpawned { get; private set; } = new GameEventController<ChestView>();
        public GameEventController<ChestView> OnChestUnlockStarted { get; private set; } = new GameEventController<ChestView>();
        public GameEventController<ChestView> OnChestUnlockCompleted { get; private set; } = new GameEventController<ChestView>();
        public GameEventController<ChestCollectedEventArgs> OnChestCollected { get; private set; } = new GameEventController<ChestCollectedEventArgs>();
        public GameEventController<int> OnMaxSlotsIncreased { get; private set; } = new GameEventController<int>();
    }

    public class ChestCollectedEventArgs
    {
        public ChestView Chest { get; private set; }
        public int CoinsAwarded { get; private set; }
        public int GemsAwarded { get; private set; }

        public ChestCollectedEventArgs(ChestView chest, int coinsAwarded, int gemsAwarded)
        {
            Chest = chest;
            CoinsAwarded = coinsAwarded;
            GemsAwarded = gemsAwarded;
        }
    }
}