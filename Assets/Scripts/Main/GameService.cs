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
        [SerializeField] private PlayerScriptableObject playerScriptableObject;

        [Header("Chest")]
        [SerializeField] private List<ChestScriptableObject> chests;
        [SerializeField] private ChestView chestPrefab;
        [SerializeField] private Transform chestScrollContent;

        protected override void Awake()
        {
            base.Awake();

            playerService = new PlayerService(playerView, playerScriptableObject);
            chestService = new ChestService(chests, chestPrefab, chestScrollContent);
        }
    }
}