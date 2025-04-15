using UnityEngine;
using UnityEngine.UI;
using TMPro;
using ChestSystem.Chest.Data;
using ChestSystem.Chest.Core;
using ChestSystem.Utilities;
using System.Collections;

namespace ChestSystem.Chest.UI
{
    public class ChestView : MonoBehaviour
    {
        [SerializeField] private Image chestImage;
        [SerializeField] private TextMeshProUGUI timerText;
        [SerializeField] private TextMeshProUGUI statusText;
        [SerializeField] private Button chestButton;
        [SerializeField] private TextMeshProUGUI gemCostText;
        [SerializeField] private GameObject gemCostContainer;

        // Model
        private ChestModel model;

        // State Machine
        private ChestStateMachine stateMachine;

        // References
        private ChestController controller;
        private Coroutine unlockCoroutine;

        // Properties
        public ChestType ChestType => model?.ChestType ?? ChestType.COMMON;
        public ChestState CurrentState => model?.CurrentState ?? ChestState.LOCKED;

        private void Awake()
        {
            // State machine will be initialized in Initialize method
        }

        public void Initialize(ChestScriptableObject chestData, ChestController controller)
        {
            this.controller = controller;

            // Create the model
            model = new ChestModel(chestData);
            model.OnStateChanged += OnModelStateChanged;
            model.OnUnlockTimeUpdated += OnModelTimerUpdated;
            model.OnGemCostUpdated += OnModelGemCostUpdated;

            // Set visual elements
            this.name = chestData.chestType.ToString();
            if (chestImage != null && chestData.chestSprite != null)
                chestImage.sprite = chestData.chestSprite;

            // Set up the state machine
            stateMachine = new ChestStateMachine();
            stateMachine.Initialize(this, controller);

            // Set up the button
            chestButton.onClick.AddListener(OnChestClicked);

            // Initial visibility configurations
            if (gemCostContainer != null)
                gemCostContainer.SetActive(false);

            // Start with locked state
            stateMachine.ChangeState(ChestState.LOCKED);
        }

        private void OnChestClicked()
        {
            Debug.Log($"Chest clicked: {ChestType}");
            stateMachine.HandleClick();
        }

        // Model Event Handlers
        private void OnModelStateChanged(ChestState newState)
        {
            // State changes are handled by the state machine
        }

        private void OnModelTimerUpdated(float remainingTime)
        {
            SetTimerText(model.GetFormattedTimeRemaining());
        }

        private void OnModelGemCostUpdated(int gemCost)
        {
            UpdateGemCostDisplay(gemCost);
        }

        // UI Updates
        public void SetTimerText(string text)
        {
            if (timerText != null)
                timerText.text = text;
        }

        public void SetStatusText(string text)
        {
            if (statusText != null)
                statusText.text = text;
        }

        public void ShowGemCost()
        {
            if (gemCostContainer != null)
                gemCostContainer.SetActive(true);

            UpdateGemCostDisplay(model.CurrentGemCost);
        }

        public void HideGemCost()
        {
            if (gemCostContainer != null)
                gemCostContainer.SetActive(false);
        }

        private void UpdateGemCostDisplay(int gemCost)
        {
            if (gemCostText != null)
                gemCostText.text = $"Cost: {gemCost}";
        }

        // State Machine Methods
        public void StartUnlockingProcess()
        {
            if (unlockCoroutine != null)
                StopCoroutine(unlockCoroutine);

            unlockCoroutine = StartCoroutine(UnlockingProcess());
        }

        public IEnumerator UnlockingProcess()
        {
            controller.SetUnlockingChest(this);

            while (model.RemainingUnlockTime > 0)
            {
                yield return new WaitForSeconds(1f);
                model.DecrementTimer(1f);
            }

            controller.ChestUnlockCompleted(this);
            stateMachine.ChangeState(ChestState.UNLOCKED);
        }

        public void CompleteUnlocking()
        {
            if (unlockCoroutine != null)
                StopCoroutine(unlockCoroutine);

            model.UpdateUnlockTime(0);
            controller.ChestUnlockCompleted(this);
            stateMachine.ChangeState(ChestState.UNLOCKED);
        }

        public void CollectRewards()
        {
            int coinsAwarded = 0;
            int gemsAwarded = 0;

            controller.CollectChest(this, out coinsAwarded, out gemsAwarded);
        }

        public void OnReturnToPool()
        {
            if (unlockCoroutine != null)
                StopCoroutine(unlockCoroutine);

            chestButton.onClick.RemoveAllListeners();

            // Clean up model event subscriptions
            if (model != null)
            {
                model.OnStateChanged -= OnModelStateChanged;
                model.OnUnlockTimeUpdated -= OnModelTimerUpdated;
                model.OnGemCostUpdated -= OnModelGemCostUpdated;
            }
        }

        // Getters
        public ChestModel GetModel() => model;
        public ChestScriptableObject GetChestData() => model.ChestData;
        public float GetRemainingUnlockTime() => model.RemainingUnlockTime;
        public int GetCurrentGemCost() => model.CurrentGemCost;
    }
}