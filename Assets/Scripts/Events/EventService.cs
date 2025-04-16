using ChestSystem.Chest.UI;

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
                    instance = new EventService();
                return instance;
            }
        }

        public GameEventController<ChestView> OnChestSpawned { get; private set; }
        public GameEventController<ChestView> OnChestUnlockStarted { get; private set; }
        public GameEventController<ChestView> OnChestUnlockCompleted { get; private set; }
        public EventController<ChestView, int, int> OnChestCollected { get; private set; }
        public GameEventController<int> OnMaxSlotsIncreased { get; private set; }

        public GameEventController OnUIButtonClick { get; private set; }
        public GameEventController OnNotificationShow { get; private set; }
        public GameEventController OnNotificationClose { get; private set; }
        public GameEventController OnGemsSpend { get; private set; }

        public EventService()
        {
            OnChestSpawned = new GameEventController<ChestView>();
            OnChestUnlockStarted = new GameEventController<ChestView>();
            OnChestUnlockCompleted = new GameEventController<ChestView>();
            OnChestCollected = new EventController<ChestView, int, int>();
            OnMaxSlotsIncreased = new GameEventController<int>();

            OnUIButtonClick = new GameEventController();
            OnNotificationShow = new GameEventController();
            OnNotificationClose = new GameEventController();
            OnGemsSpend = new GameEventController();
        }
    }
}