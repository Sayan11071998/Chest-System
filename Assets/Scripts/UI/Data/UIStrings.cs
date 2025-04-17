namespace ChestSystem.UI.Data
{
    public static class UIStrings
    {
        #region Chest Status Labels
        public const string Locked = "LOCKED";
        public const string Unlocking = "UNLOCKING";
        public const string Unlocked = "UNLOCKED";
        public const string Collected = "COLLECTED";
        public const string Ready = "Ready!";
        #endregion

        #region Notification Popup Lebels
        public const string ChestLocked = "CHEST LOCKED";
        public const string InstantUnlockChest = "INSTANT UNLOCK - {0} CHEST";
        public const string ChestSlotsFull = "CHEST SLOTS FULL";
        #endregion

        #region Notification Popup Messages
        public const string AnotherChestIsAlreadyBeingUnlocked = "Another chest is already being unlocked!";
        public const string ChestRewards = "{0} CHEST REWARDS";
        public const string AboutToCollect = "You are about to collect:\n\n{0} coins\n{1} gems\n\nTap to collect!";
        public const string WouldLikeInstantUnlock = "Would you like to instantly unlock this {0} chest for {1} gems?\n\nYou have: {2} gems\nCost: {1} gems\n\nTap to confirm!";
        public const string DoNotHaveEnoughGems = "You don't have enough gems to instantly unlock this chest.\n\nYou have: {0} gems\nRequired: {1} gems\n\nWait for the timer or get more gems!";
        public const string AllChestsAreCurrentlyFull = "All chest slots are currently full. Add more slots or open existing chests first.";
        #endregion

        #region Notification Popup Button Lebels
        public const string Close = "CLOSE";
        public const string Collect = "COLLECT";
        public const string Confirm = "CONFIRM";
        public const string Okay = "OKAY";
        #endregion
    }
}