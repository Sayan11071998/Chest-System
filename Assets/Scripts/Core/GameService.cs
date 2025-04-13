using System.Collections.Generic;
using ChestSystem.Chest;
using ChestSystem.Player.Core;
using ChestSystem.Player.Data;
using ChestSystem.Player.UI;
using ChestSystem.UI.Components;
using ChestSystem.Utilities;
using UnityEngine;

namespace ChestSystem.Core
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

            playerService = new PlayerService(playerView, playerScriptableObject);
            chestService = new ChestService(chests, chestPrefab, emptySlotPrefab, chestScrollContent, initialMaxChestSlots);
        }
    }
}