using System.Collections.Generic;
using ChestSystem.Audio;
using ChestSystem.Chest.Core;
using ChestSystem.Chest;
using ChestSystem.Chest.UI;
using ChestSystem.Player.Core;
using ChestSystem.Player.Data;
using ChestSystem.Player.UI;
using ChestSystem.UI.Components;
using ChestSystem.Utilities;
using UnityEngine;
using ChestSystem.Sound;
using ChestSystem.Events;

namespace ChestSystem.Core
{
    public class GameService : GenericMonoSingleton<GameService>
    {
        public PlayerService playerService { get; private set; }
        public ChestService chestService { get; private set; }
        public SoundService soundService { get; private set; }

        [Header("Player")]
        [SerializeField] private PlayerView playerView;
        [SerializeField] private PlayerScriptableObject playerScriptableObject;

        [Header("Chest")]
        [SerializeField] private List<ChestScriptableObject> chests;
        [SerializeField] private ChestView chestPrefab;
        [SerializeField] private EmptySlotView emptySlotPrefab;
        [SerializeField] private Transform chestScrollContent;
        [SerializeField] private int initialMaxChestSlots = 4;

        [Header("Sound")]
        [SerializeField] private SoundScriptableObject soundScriptableObject;
        [SerializeField] private AudioSource soundEffectsSource;
        [SerializeField] private AudioSource backgroundMusicSource;

        protected override void Awake()
        {
            base.Awake();

            playerService = new PlayerService(playerView, playerScriptableObject);
            chestService = new ChestService(chests, chestPrefab, emptySlotPrefab, chestScrollContent, initialMaxChestSlots);
            soundService = new SoundService(soundScriptableObject, soundEffectsSource, backgroundMusicSource);

            RegisterSoundEventListeners();
        }

        private void RegisterSoundEventListeners()
        {
            EventService.Instance.OnChestSpawned.AddListener(chest => soundService.PlaySoundEffects(SoundType.CHEST_CLICK));
            EventService.Instance.OnChestUnlockStarted.AddListener(chest => soundService.PlaySoundEffects(SoundType.CHEST_UNLOCK_START));
            EventService.Instance.OnChestUnlockCompleted.AddListener(chest => soundService.PlaySoundEffects(SoundType.CHEST_UNLOCK_COMPLETE));
            EventService.Instance.OnChestCollected.AddListener((chest, coins, gems) => soundService.PlaySoundEffects(SoundType.CHEST_COLLECT));
            EventService.Instance.OnMaxSlotsIncreased.AddListener(slots => soundService.PlaySoundEffects(SoundType.ADD_SLOT));

            EventService.Instance.OnUIButtonClick.AddListener(() => soundService.PlaySoundEffects(SoundType.UI_BUTTON_CLICK));
            EventService.Instance.OnNotificationShow.AddListener(() => soundService.PlaySoundEffects(SoundType.NOTIFICATION_SHOW));
            EventService.Instance.OnNotificationClose.AddListener(() => soundService.PlaySoundEffects(SoundType.NOTIFICATION_CLOSE));
            EventService.Instance.OnGemsSpend.AddListener(() => soundService.PlaySoundEffects(SoundType.GEMS_SPEND));
        }

        private void OnDestroy()
        {
            if (EventService.Instance != null)
            {
                EventService.Instance.OnChestSpawned.RemoveListener(chest => soundService.PlaySoundEffects(SoundType.CHEST_CLICK));
                EventService.Instance.OnChestUnlockStarted.RemoveListener(chest => soundService.PlaySoundEffects(SoundType.CHEST_UNLOCK_START));
                EventService.Instance.OnChestUnlockCompleted.RemoveListener(chest => soundService.PlaySoundEffects(SoundType.CHEST_UNLOCK_COMPLETE));
                EventService.Instance.OnChestCollected.RemoveListener((chest, coins, gems) => soundService.PlaySoundEffects(SoundType.CHEST_COLLECT));
                EventService.Instance.OnMaxSlotsIncreased.RemoveListener(slots => soundService.PlaySoundEffects(SoundType.ADD_SLOT));

                EventService.Instance.OnUIButtonClick.RemoveListener(() => soundService.PlaySoundEffects(SoundType.UI_BUTTON_CLICK));
                EventService.Instance.OnNotificationShow.RemoveListener(() => soundService.PlaySoundEffects(SoundType.NOTIFICATION_SHOW));
                EventService.Instance.OnNotificationClose.RemoveListener(() => soundService.PlaySoundEffects(SoundType.NOTIFICATION_CLOSE));
                EventService.Instance.OnGemsSpend.RemoveListener(() => soundService.PlaySoundEffects(SoundType.GEMS_SPEND));
            }
        }
    }
}