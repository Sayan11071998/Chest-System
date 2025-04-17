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

        public EventController<ChestView> OnChestSpawned { get; private set; }
        public EventController<ChestView> OnChestUnlockStarted { get; private set; }
        public EventController<ChestView> OnChestUnlockCompleted { get; private set; }
        public EventController<ChestView, int, int> OnChestCollected { get; private set; }
        public EventController<int> OnMaxSlotsIncreased { get; private set; }

        public EventController OnUIButtonClick { get; private set; }
        public EventController OnNotificationShow { get; private set; }
        public EventController OnNotificationClose { get; private set; }
        public EventController OnGemsSpend { get; private set; }
        public EventController OnCommandUndo { get; private set; }

        public EventService()
        {
            OnChestSpawned = new EventController<ChestView>();
            OnChestUnlockStarted = new EventController<ChestView>();
            OnChestUnlockCompleted = new EventController<ChestView>();
            OnChestCollected = new EventController<ChestView, int, int>();
            OnMaxSlotsIncreased = new EventController<int>();

            OnUIButtonClick = new EventController();
            OnNotificationShow = new EventController();
            OnNotificationClose = new EventController();
            OnGemsSpend = new EventController();
            OnCommandUndo = new EventController();
        }
    }
}