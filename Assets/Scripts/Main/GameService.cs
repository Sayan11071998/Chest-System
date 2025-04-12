using System.Collections.Generic;
using ChestSystem.Chest;
using ChestSystem.Player;
using ChestSystem.Utilities;
using ChestSystem.UI;
using UnityEngine;

namespace ChestSystem.Main
{
    public class GameService : GenericMonoSingelton<GameService>
    {
        public PlayerService playerService { get; private set; }
        public ChestService chestService { get; private set; }
        public ChestUIManager uiManager { get; private set; }

        [Header("Player")]
        [SerializeField] private PlayerView playerView;
        [SerializeField] private PlayerScriptableObject playerScriptableObject;

        [Header("Chest")]
        [SerializeField] private List<ChestScriptableObject> chests;
        [SerializeField] private ChestService chestServiceComponent;

        [Header("UI")]
        [SerializeField] private ChestUIManager chestUIManagerComponent;

        protected override void Awake()
        {
            base.Awake();

            // Initialize player service
            playerService = new PlayerService(playerView, playerScriptableObject);

            // Initialize chest service
            chestService = chestServiceComponent;
            chestService.Initialize(chests, playerService.PlayerController);

            // Get UI manager
            uiManager = chestUIManagerComponent;
        }
    }
}