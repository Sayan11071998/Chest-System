using ChestSystem.Chest;

namespace ChestSystem.Events
{
    public class EventService
    {
        private static EventService instance;
        public static EventService Instance
        {
            get
            {
                if (instance == null)
                {
                    instance = new EventService();
                }
                return instance;
            }
        }

        public GameEventController<ChestView> OnChestSpawned { get; private set; }
        public GameEventController<ChestView> OnChestUnlockStarted { get; private set; }
        public GameEventController<ChestView> OnChestUnlockCompleted { get; private set; }
        public GameEventController<ChestCollectedEventArgs> OnChestCollected { get; private set; }
        public GameEventController<int> OnMaxSlotsIncreased { get; private set; }

        public EventService()
        {
            OnChestSpawned = new GameEventController<ChestView>();
            OnChestUnlockStarted = new GameEventController<ChestView>();
            OnChestUnlockCompleted = new GameEventController<ChestView>();
            OnChestCollected = new GameEventController<ChestCollectedEventArgs>();
            OnMaxSlotsIncreased = new GameEventController<int>();
        }
    }
}