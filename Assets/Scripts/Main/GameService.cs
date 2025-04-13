using System.Collections.Generic;
using ChestSystem.Chest;
using ChestSystem.Player;
using ChestSystem.UI;
using ChestSystem.Utilities;
using ChestSystem.Events;
using UnityEngine;

namespace ChestSystem.Main
{
    public class GameService : GenericMonoSingleton<GameService>
    {
        public PlayerService playerService { get; private set; }
        public ChestService chestService { get; private set; }

        [Header("Player")]
        [SerializeField] private PlayerView playerView;
        [SerializeField] private PlayerScriptableObject playerScriptableObject;

        [Header("Chest")]
        [SerializeField] private List<ChestScriptableObject> chests;
        [SerializeField] private ChestView chestPrefab;
        [SerializeField] private EmptySlotView emptySlotPrefab;
        [SerializeField] private Transform chestScrollContent;
        [SerializeField] private int initialMaxChestSlots = 4;

        protected override void Awake()
        {
            base.Awake();

            // No need to instantiate EventService anymore
            playerService = new PlayerService(playerView, playerScriptableObject);
            chestService = new ChestService(chests, chestPrefab, emptySlotPrefab, chestScrollContent, initialMaxChestSlots);

            SubscribeToEvents();
        }

        private void SubscribeToEvents()
        {
            EventService.Instance.OnChestCollected.AddListener(HandleChestCollected);
        }

        private void HandleChestCollected(ChestCollectedEventArgs args)
        {
            playerService.PlayerController.UpdateCoinCount(playerService.PlayerController.CoinCount + args.CoinsAwarded);
            playerService.PlayerController.UpdateGemsCount(playerService.PlayerController.GemsCount + args.GemsAwarded);

            Debug.Log($"GameService: Player received {args.CoinsAwarded} coins and {args.GemsAwarded} gems from chest");
        }

        private void OnDestroy()
        {
            EventService.Instance.OnChestCollected.RemoveListener(HandleChestCollected);
        }
    }
}