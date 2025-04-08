using System.Collections.Generic;
using ChestSystem.Chest;
using ChestSystem.Player;
using ChestSystem.Utilities;
using UnityEngine;

namespace ChestSystem.Main
{
    public class GameService : GenericMonoSingelton<GameService>
    {
        public PlayerService playerService { get; private set; }
        public ChestService chestService { get; private set; }

        [Header("Player")]
        [SerializeField] private PlayerView playerView;

        [Header("Chest")]
        [SerializeField] private List<ChestScriptableObject> chests;
        // [SerializeField] private ChestView chestView;

        protected override void Awake()
        {
            base.Awake();

            playerService = new PlayerService(playerView);
            chestService = new ChestService(chests);
        }
    }
}