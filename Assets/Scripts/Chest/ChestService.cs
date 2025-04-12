using System.Collections.Generic;
using ChestSystem.Player;
using UnityEngine;

namespace ChestSystem.Chest
{
    public class ChestService : MonoBehaviour
    {
        public ChestController chestController { get; private set; }

        [Header("Chest Slots")]
        [SerializeField] private Transform chestSlotsParent;
        [SerializeField] private int maxSlots = 4;

        [Header("Chest Prefabs")]
        [SerializeField] private GameObject commonChestPrefab;
        [SerializeField] private GameObject rareChestPrefab;
        [SerializeField] private GameObject epicChestPrefab;
        [SerializeField] private GameObject legendaryChestPrefab;

        private Dictionary<ChestType, GameObject> chestPrefabs = new Dictionary<ChestType, GameObject>();

        public void Initialize(List<ChestScriptableObject> chests, PlayerController playerController)
        {
            InitializeChestPrefabs();
            chestController = new ChestController(chests, chestSlotsParent, chestPrefabs, playerController);
        }

        private void InitializeChestPrefabs()
        {
            if (commonChestPrefab != null)
                chestPrefabs[ChestType.COMMON] = commonChestPrefab;

            if (rareChestPrefab != null)
                chestPrefabs[ChestType.RARE] = rareChestPrefab;

            if (epicChestPrefab != null)
                chestPrefabs[ChestType.EPIC] = epicChestPrefab;

            if (legendaryChestPrefab != null)
                chestPrefabs[ChestType.LEGENDARY] = legendaryChestPrefab;
        }

        private void Update()
        {
            if (chestController != null)
                chestController.Update();
        }

        public void GenerateRandomChest()
        {
            if (chestController != null)
                chestController.GenerateRandomChest();
        }

        public void UndoGemSkip()
        {
            if (chestController != null)
                chestController.UndoGemSkip();
        }
    }
}